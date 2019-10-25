using ADE;
using SportsIq.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using log4net;
using static System.Convert;

namespace SportsIq.Analytica
{
    public abstract class AnalyticaBase : IDisposable
    {
        protected static readonly ILog Logger;
        public bool IsTeamMode { get; set; }            // switches between Team and Player mode
        private CAEngine _caEngine;

        static AnalyticaBase()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(AnalyticaBase));
        }

        protected AnalyticaBase()
        {
            string analyticaModelFullFileName = GetAnalyticaModelFullFileName();
            Open(analyticaModelFullFileName);
        }

        private string AnalyticaModelFullFileName { get; set; }
        private string ModelName { get; set; }

        #region Common Methods

        private string GetAnalyticaModelFullFileName()
        {
            string isTeamModeString = ConfigurationManager.AppSettings["isTeamMode"];
            IsTeamMode = ToBoolean(isTeamModeString);
            string analyticaModelFileNameKey = IsTeamMode ? "analyticaTeamModelFileName" : "analyticaPlayerModelFileName";
            string analyticaModelFileName = ConfigurationManager.AppSettings[analyticaModelFileNameKey];
            string baseDirectory = Utils.GetBaseDirectory();
            const string relativePath = "AnalyticaModels";
            string analyticaModelFullFileName = Path.Combine(baseDirectory, relativePath, analyticaModelFileName);
            return analyticaModelFullFileName;
        }

        private void Open(string analyticaModelFullFileName)
        {
            if (_caEngine != null)
            {
                Close();
            }

            if (!File.Exists(analyticaModelFullFileName))
            {
                string message = $"File not found, {analyticaModelFullFileName}";
                Logger.Error(message);
                throw new FileNotFoundException(message);
            }

            _caEngine = new CAEngine
            {
                MaxMemoryLimit = 0
            };

            string modelName = _caEngine.OpenModel(analyticaModelFullFileName);

            if (modelName == string.Empty)
            {
                string message = $"Could not open Analytica model: {analyticaModelFullFileName}";
                Logger.Error(message);
                throw new Exception(message);
            }

            ModelName = modelName;
            AnalyticaModelFullFileName = analyticaModelFullFileName;
        }

        private void Close()
        {
            if (_caEngine != null)
            {
                _caEngine.CloseModel();
                Marshal.ReleaseComObject(_caEngine);
                _caEngine = null;
            }

            ModelName = string.Empty;
            AnalyticaModelFullFileName = string.Empty;
        }

        public void InitializeTables(Dictionary<string, string[]> tableIndicesDictionary)
        {
            foreach (KeyValuePair<string, string[]> keyValuePair in tableIndicesDictionary)
            {
                string tableName = keyValuePair.Key;
                string[] indexArray = keyValuePair.Value;

                CATable caTable = GetDefTable(tableName);

                if (!caTable.SetIndexOrder(indexArray))
                {
                    const string message = "SetIndexOrder() failed";
                    Logger.Error(message);
                    throw new Exception(message);
                }

                const double initialValue = 0L;
                InitializeTable(caTable, initialValue);
            }
        }

        protected CATable GetDefTable(string tableName)
        {
            if (tableName.IsNullOrWhiteSpace())
            {
                string message = $"The tableName must not be null or whitespace, tableName = {tableName}";
                throw new ArgumentException(message, nameof(tableName));
            }

            CAObject caObject = _caEngine.GetObjectByName(tableName);

            if (caObject == null)
            {
                string message = $"{tableName} CAObject is null";
                Logger.Error(message);
                throw new Exception(message);
            }

            CATable caTable = caObject.DefTable();

            if (caTable == null)
            {
                const string message = "CATable is null";
                Logger.Error(message);
                throw new Exception(message);
            }

            return caTable;
        }

        private CATable GetResultTable(string tableName)
        {
            if (tableName.IsNullOrWhiteSpace())
            {
                const string message = "The tableName must not be null or whitespace";
                Logger.Error(message);
                throw new ArgumentException(message, nameof(tableName));
            }

            // todo how to check if table name exists in model?
            CAObject caObject = _caEngine.GetObjectByName(tableName);

            if (caObject == null)
            {
                const string message = "caObject is null : ";
                Logger.Error(message + tableName);
                throw new Exception(message);
            }

            CATable caTable = caObject.ResultTable();

            if (caTable == null)
            {
                const string message = "caTable is null";
                Logger.Error(message);
                throw new Exception(message);
            }

            return caTable;
        }

        protected void SetDescription(string objectName, double value)
        {
            CAObject caObject = _caEngine.GetObjectByName(objectName);

            if (!caObject.SetAttribute("Definition", value))
            //{
            //    Logger.Info($"Updated setting {objectName} {value}");
            //}
            //else
            {
                Logger.Info($"SetDescription() failed: {objectName} {value}");
            }
        }

        protected Array GetTableArray(string tableName, string[] indexArray)
        {
            CATable caTable = GetResultTable(tableName);

            if (!caTable.SetIndexOrder(indexArray))
            {
                const string message = "SetIndexOrder() failed";
                Logger.Error(message + " " + tableName);
                throw new Exception(message);
            }

            dynamic safeArray = caTable.GetSafeArray();
            Array array = (Array)safeArray;
            return array;
        }

        protected void LoadTables(Dictionary<string, string[]> tableIndicesDictionary, IReadOnlyDictionary<string, Dictionary<string, double>> modelData)
        {
            foreach (KeyValuePair<string, string[]> keyValuePair in tableIndicesDictionary)
            {
                string tableName = keyValuePair.Key;
                string[] indexArray = keyValuePair.Value;

                if (modelData.ContainsKey(tableName))
                {
                    CATable caTable = GetDefTable(tableName);
                    Dictionary<string, double> dictionary = modelData[tableName];
                    if (modelData[tableName] == null)
                    {
                        Logger.Info(tableName);
                    }
                    LoadTable(caTable, indexArray, dictionary,tableName);
                }
                else
                {
                    string message = $"ModelData does not contain dictionary {tableName}";
                    Logger.Error(message);
                    throw new Exception(message);
                }
            }
        }

        private void LoadTable(CATable caTable, string[] indexArray, Dictionary<string, double> dictionary, string tableName)
        {
            if (caTable == null)
            {
                string message = $"caTable name is null {tableName}";
                Logger.Error(message);
                throw new ArgumentNullException(nameof(caTable));
            }

            if (dictionary == null)
            {
                const string message = "dictionary is null";
                Logger.Error(message);
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!caTable.SetIndexOrder(indexArray))
            {
                string message = $"LoadTable() failed, SetIndexOrder() failed: indexArray = {ConvertIndexArrayToString(indexArray)}  {tableName}";
                Logger.Error(message);
                throw new Exception(message);
            }

            if (dictionary.Count == 0)
            {
                Logger.Error("LoadTable(): dictionary is empty");
                return;
            }

            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                const char comma = ',';
                string[] indexArray2 = key.Split(comma);

                if (!caTable.SetDataByLabels(value, indexArray2))
                {
                    string message = $"UpdateTable failed: SetDataByLabels() failed: indexArray = {ConvertIndexArrayToString(indexArray2)}, value = {value}, ErrorText = {_caEngine.ErrorText} {tableName}";
                    Logger.Error(message);
                    throw new Exception(message);
                }
            }

            if (!caTable.Update())
            {
                const string message = "Update() failed";
                Logger.Error(message);
                throw new Exception(message);
            }
        }

        protected void SaveModel(string saveFullFileName)
        {
            if (!_caEngine.SaveModel(saveFullFileName))
            {
                string message = $"SaveModel() failed: saveFullFileName = {saveFullFileName}";
                Logger.Error(message);
                throw new Exception(message);
            }
        }

        public Array LogTableValues(CATable caTable, string[] indexArray)
        {
            if (caTable == null)
            {
                const string message = "caTable is null";
                Logger.Error(message);
                throw new ArgumentNullException(nameof(caTable));
            }

            if (!caTable.SetIndexOrder(indexArray))
            {
                string message = $"SetIndexOrder() failed: indexArray = {ConvertIndexArrayToString(indexArray)}";
                Logger.Error(message);
                throw new Exception(message);
            }

            dynamic safeArray = caTable.GetSafeArray();
            Array array = (Array)safeArray;

            return array;
        }

        private static string ConvertIndexArrayToString(IEnumerable<string> indexArray)
        {
            string indexArrayString = "(";

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string index in indexArray)
            {
                indexArrayString += $"{index}, ";
            }

            int length = indexArrayString.Length;
            indexArrayString = indexArrayString.Substring(0, length - 2);
            indexArrayString += ")";

            return indexArrayString;
        }

        protected static double GetArrayValue(Array array, params int[] indices)
        {
            double value;

            try
            {
                object valueObject = array.GetValue(indices);
                value = ToDouble(valueObject);
            }
            catch (InvalidCastException)
            {
                value = 0d;
            }

            return value;
        }

        private void InitializeTable(CATable caTable, object initialValue)
        {
            if (caTable == null)
            {
                const string message = "caTable is null";
                Logger.Error(message);
                throw new ArgumentNullException(nameof(caTable));
            }

            dynamic safeArray = caTable.GetSafeArray();
            Array array = (Array)safeArray;
            int rank = array.Rank;

            switch (rank)
            {
                case 2:
                    InitializeTableOfRank2(caTable, array, initialValue);
                    break;

                case 3:
                    InitializeTableOfRank3(caTable, array, initialValue);
                    break;

                case 4:
                    InitializeTableOfRank4(caTable, array, initialValue);
                    break;

                case 5:
                    InitializeTableOfRank5(caTable, array, initialValue);
                    break;

                case 6:
                    InitializeTableOfRank6(caTable, array, initialValue);
                    break;

                default:
                    const string message = "Only tables of rank 2, 3, 4, 5, and 6 can be initialized";
                    Logger.Error(message);
                    throw new Exception(message);
            }

            if (!caTable.Update())
            {
                const string message = "CATable.Update() failed";
                Logger.Error(message);
                throw new Exception(message);
            }
        }

        private static void InitializeTableOfRank2(CATable caTable, Array array, object initialValue)
        {
            const int rank = 2;
            int[] indexArray = new int[rank];

            for (int n1 = 1; n1 < array.GetLength(0) + 1; n1++)
            {
                indexArray[0] = n1;

                for (int n2 = 1; n2 < array.GetLength(1) + 1; n2++)
                {
                    indexArray[1] = n2;

                    if (!caTable.SetDataByElements(initialValue, indexArray))
                    {
                        string message = $"aTable.SetDataByElements failed: indexArray = {indexArray} initialValue = {initialValue}";
                        Logger.Error(message);
                        throw new Exception(message);
                    }
                }
            }
        }

        private static void InitializeTableOfRank3(CATable caTable, Array array, object initialValue)
        {
            const int rank = 3;
            int[] indexArray = new int[rank];

            for (int n1 = 1; n1 < array.GetLength(0) + 1; n1++)
            {
                indexArray[0] = n1;

                for (int n2 = 1; n2 < array.GetLength(1) + 1; n2++)
                {
                    indexArray[1] = n2;

                    for (int n3 = 1; n3 < array.GetLength(2) + 1; n3++)
                    {
                        indexArray[2] = n3;

                        if (!caTable.SetDataByElements(initialValue, indexArray))
                        {
                            string message = $"aTable.SetDataByElements failed: indexArray = {indexArray} initialValue = {initialValue}";
                            Logger.Error(message);
                            throw new Exception(message);
                        }
                    }
                }
            }
        }

        private static void InitializeTableOfRank4(CATable caTable, Array array, object initialValue)
        {
            const int rank = 4;
            int[] indexArray = new int[rank];

            for (int n1 = 1; n1 < array.GetLength(0) + 1; n1++)
            {
                indexArray[0] = n1;

                for (int n2 = 1; n2 < array.GetLength(1) + 1; n2++)
                {
                    indexArray[1] = n2;

                    for (int n3 = 1; n3 < array.GetLength(2) + 1; n3++)
                    {
                        indexArray[2] = n3;

                        for (int n4 = 1; n4 < array.GetLength(3) + 1; n4++)
                        {
                            indexArray[3] = n4;

                            if (!caTable.SetDataByElements(initialValue, indexArray))
                            {
                                string message = $"aTable.SetDataByElements failed: indexArray = {indexArray} initialValue = {initialValue}";
                                Logger.Error(message);
                                throw new Exception(message);
                            }
                        }
                    }
                }
            }
        }

        private static void InitializeTableOfRank5(CATable caTable, Array array, object initialValue)
        {
            const int rank = 5;
            int[] indexArray = new int[rank];

            for (int n1 = 1; n1 < array.GetLength(0) + 1; n1++)
            {
                indexArray[0] = n1;

                for (int n2 = 1; n2 < array.GetLength(1) + 1; n2++)
                {
                    indexArray[1] = n2;

                    for (int n3 = 1; n3 < array.GetLength(2) + 1; n3++)
                    {
                        indexArray[2] = n3;

                        for (int n4 = 1; n4 < array.GetLength(3) + 1; n4++)
                        {
                            indexArray[3] = n4;

                            for (int n5 = 1; n5 < array.GetLength(4) + 1; n5++)
                            {
                                indexArray[4] = n5;

                                if (!caTable.SetDataByElements(initialValue, indexArray))
                                {
                                    string message = $"aTable.SetDataByElements failed: indexArray = {indexArray} initialValue = {initialValue}";
                                    Logger.Error(message);
                                    throw new Exception(message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void InitializeTableOfRank6(CATable caTable, Array array, object initialValue)
        {
            const int rank = 6;
            int[] indexArray = new int[rank];

            for (int n1 = 1; n1 < array.GetLength(0) + 1; n1++)
            {
                indexArray[0] = n1;

                for (int n2 = 1; n2 < array.GetLength(1) + 1; n2++)
                {
                    indexArray[1] = n2;

                    for (int n3 = 1; n3 < array.GetLength(2) + 1; n3++)
                    {
                        indexArray[2] = n3;

                        for (int n4 = 1; n4 < array.GetLength(3) + 1; n4++)
                        {
                            indexArray[3] = n4;

                            for (int n5 = 1; n5 < array.GetLength(4) + 1; n5++)
                            {
                                indexArray[4] = n5;

                                for (int n6 = 1; n6 < array.GetLength(5) + 1; n6++)
                                {
                                    indexArray[5] = n6;

                                    if (!caTable.SetDataByElements(initialValue, indexArray))
                                    {
                                        string message = $"aTable.SetDataByElements failed: indexArray = {indexArray} initialValue = {initialValue}";
                                        Logger.Error(message);
                                        throw new Exception(message);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region IDisposable

        private static void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                //CaEngine?.Dispose();
            }

            // Free native resources
            //CaEngine?.Dispose();
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AnalyticaBase()
        {
            Dispose(false);
        }

        #endregion
    }
}