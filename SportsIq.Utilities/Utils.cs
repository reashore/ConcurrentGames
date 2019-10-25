using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using Newtonsoft.Json.Linq;
using static System.Convert;
using static System.Console;

namespace SportsIq.Utilities
{
    public static class Utils
    {
        private static readonly ILog Logger;

        static Utils()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(Utils));
        }

        #region Files and Streams

        public static string GetBaseDirectory()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string codeBase = assembly.CodeBase;
            string localPath = new Uri(codeBase).LocalPath;
            string baseDirectory = Path.GetDirectoryName(localPath);
            return baseDirectory;
        }

        public static string GetFullFileName(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            bool fileExists = fileInfo.Exists;

            if (!fileExists)
            {
                const string message = "File not found";
                throw new FileNotFoundException(message, fileName);
            }

            string directoryName = fileInfo.DirectoryName;

            if (directoryName == null)
            {
                const string message = "Directory name was null";
                throw new Exception(message);
            }

            string fullFileName = Path.Combine(directoryName, fileName);

            if (!File.Exists(fullFileName))
            {
                const string message = "File not found";
                throw new FileNotFoundException(message, fullFileName);
            }

            return fullFileName;
        }
        
        public static void DeleteLogFile(string logFileName)
        {
            string baseDirectory = GetBaseDirectory();
            string logFileFullName = Path.Combine(baseDirectory, logFileName);
            File.Delete(logFileFullName);
        }
        
        public static void DeleteAnalyticaSavedModelsFiles()
        {
            string baseDirectory = GetBaseDirectory();
            string analyticaSavedModelsDirectory = Path.Combine(baseDirectory, "AnalyticaSavedModels");
            List<string> fileNameList = new List<string>(Directory.GetFiles(analyticaSavedModelsDirectory));

            foreach (string fileName in fileNameList)
            {
                File.Delete(fileName);
            }
        }

        public static StreamWriter OpenStreamWriter()
        {
            const string fileName = "./EmptyRecordSets.txt";
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            return streamWriter;
            //const string content = "This is a line of text";
            //streamWriter.Write(content);
            //streamWriter.Close();
        }

        #endregion

        #region Comparing doubles

        public static bool IsEqualToZero(double value, double tolerance = .001)
        {
            return Math.Abs(value) < tolerance;
        }

        public static bool IsNotEqualToZero(double value, double tolerance = .001)
        {
            return !IsEqualToZero(value, tolerance);
        }

        public static bool AreEqual(double value1, double value2, double tolerance = .001)
        {
            return Math.Abs(value1 - value2) < tolerance;
        }

        public static bool AreNotEqual(double value1, double value2, double tolerance = .001)
        {
            return !AreEqual(value1, value2, tolerance);
        }

        #endregion

        #region Dictionary Helpers

        public static Dictionary<string, double> CopyDictionary(Dictionary<string, double> dictionary)
        {
            Dictionary<string, double> newDictionary = new Dictionary<string, double>();

            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                newDictionary[key] = value;
            }

            return newDictionary;
        }

        public static void CompareDictionaries(Dictionary<string, double> dictionary1, Dictionary<string, double> dictionary2)
        {
            foreach (KeyValuePair<string, double> keyValuePair in dictionary1)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                double value2 = dictionary2[key];

                if (!AreEqual(value, value2))
                {
                    throw new Exception("Dictionaries are not equal");
                }
            }
        }

        public static void PrintDictionary(Dictionary<string, double> dictionary)
        {
            WriteLine("Dictionary:");

            foreach (KeyValuePair<string, double> keyValuePair in dictionary)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                WriteLine($"[{key}] = {value}");
            }
        }

        public static Dictionary<string, double> MergeDictionaries(Dictionary<string, double> dictionary1, Dictionary<string, double> dictionary2)
        {
            Dictionary<string, double> mergedDictionary = new Dictionary<string, double>();

            foreach (KeyValuePair<string, double> keyValuePair in dictionary1)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                mergedDictionary[key] = value;
            }

            foreach (KeyValuePair<string, double> keyValuePair in dictionary2)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                mergedDictionary[key] = value;
            }

            return mergedDictionary;
        }

        public static Dictionary<string, double> AddToDictionary(Dictionary<string, double> mainDictionary, Dictionary<string, double> newDictonary)
        {
            //Dictionary<string, double> mergedDictionary = new Dictionary<string, double>();////

            foreach (KeyValuePair<string, double> keyValuePair in newDictonary)
            {
                string key = keyValuePair.Key;
                double value = keyValuePair.Value;

                mainDictionary[key] = value;
            }
            
            return mainDictionary;
        }


        public static Dictionary<string, double> MixDictionaries(Dictionary<string, double> dictionary1, Dictionary<string, double> dictionary2)
        {
            Dictionary<string, double> mergedDictionary = new Dictionary<string, double>();
            double total = 0;

            foreach (KeyValuePair<string, double> keyValuePair in dictionary1)
            {
                string key = keyValuePair.Key;

                if (dictionary2.ContainsKey(key))
                {
                    double value1 = keyValuePair.Value;
                    double value2 = dictionary2[key];
                    double sum = value1 + value2;
                    total += sum;
                }
            }

            foreach (KeyValuePair<string, double> keyValuePair in dictionary1)
            {
                string key = keyValuePair.Key;

                if (dictionary2.ContainsKey(key))
                {
                    double value1 = keyValuePair.Value;
                    double value2 = dictionary2[key];
                    double value = (value1 + value2) / total;
                    mergedDictionary[key] = value;
                }
            }

            return mergedDictionary;
        }


        #endregion

        #region Other

        public static Guid ParseGuid(string guidString)
        {
            bool success = Guid.TryParse(guidString, out Guid guid);

            if (success)
            {
                return guid;
            }

            return Guid.Empty;
        }

        public static bool IsValidJson(string jsonString)
        {
            // https://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net

            if (jsonString.IsNullOrWhiteSpace())
            {
                return false;
            }

            jsonString = jsonString.Trim();
            bool isObject = jsonString.StartsWith("{") && jsonString.EndsWith("}");
            bool isArray = jsonString.StartsWith("[") && jsonString.EndsWith("]");

            if (isObject || isArray)
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    JToken jToken = JToken.Parse(jsonString);
                    return true;
                }
                catch (Exception exception) 
                {
                    Logger.Error($"IsValidJson() exception = {exception}");
                    return false;
                }
            }

            return false;
        }

        public static int AdjustScope(int scope)
        {
            // scopes = {1, 3, 5, 10, 1000}

            if (1 <= scope && scope < 3)
            {
                scope = 1;
            }

            if (3 <= scope && scope < 5)
            {
                scope = 3;
            }

            if (5 <= scope && scope < 10)
            {
                scope = 5;
            }

            if (10 <= scope && scope < 1000)
            {
                scope = 10;
            }

            if (1000 <= scope)
            {
                scope = 1000;
            }

            return scope;
        }

        public static int ConvertPeriodToGameStringNHL(int period, string gameStr, int fullTime)
        {
            //  int fullTime = 2880;
            int periodTime = fullTime / 3;
            int spentTime = 0;
            string min;
            string sec;

            string game = gameStr.Replace(":", "");

            char[] characters = game.ToCharArray();

            switch (characters.Length)
            {
                case 4:
                    min = characters[0] + characters[1].ToString();
                    sec = characters[2] + characters[3].ToString();
                    break;

                case 3:
                    min = characters[0].ToString();
                    sec = characters[1] + characters[2].ToString();
                    break;

                case 2:
                    min = "0";
                    sec = characters[0] + characters[1].ToString();
                    break;

                case 1:
                    min = "0";
                    sec = characters[0].ToString();
                    break;

                default:
                    throw new Exception("String length should not be 0");
            }

            if (period > 1)
            {
                spentTime = (period - 1) * periodTime;
            }

            int seconds = periodTime - (ToInt32(min) * 60 + ToInt32(sec)) + spentTime;

            if (period > 3)
            {
                fullTime = 0;
                seconds = 0;
            }

            return fullTime - seconds;
        }


        public static int ConvertPeriodToGameString(int period, string gameStr, int fullTime)
        {
          //  int fullTime = 2880;
            int periodTime = fullTime / 4;
            int spentTime = 0;
            string min;
            string sec;

            string game = gameStr.Replace(":", "");

            char[] characters = game.ToCharArray();

            switch (characters.Length)
            {
                case 4:
                    min = characters[0] + characters[1].ToString();
                    sec = characters[2] + characters[3].ToString();
                    break;

                case 3:
                    min = characters[0].ToString();
                    sec = characters[1] + characters[2].ToString();
                    break;

                case 2:
                    min = "0";
                    sec = characters[0] + characters[1].ToString();
                    break;

                case 1:
                    min = "0";
                    sec = characters[0].ToString();
                    break;

                default:
                    throw new Exception("String length should not be 0");
            }

            if (period > 1)
            {
                spentTime = (period - 1) * periodTime;
            }

            int seconds = periodTime - (ToInt32(min) * 60 + ToInt32(sec)) + spentTime;

            if (period > 4)
            {
                fullTime = 0;
                seconds = 0;
            }

            return fullTime - seconds;
        }

        #endregion
    }
}
