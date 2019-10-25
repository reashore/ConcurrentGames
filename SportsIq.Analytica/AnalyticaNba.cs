using ADE;
using SportsIq.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using SportsIq.Models.GamesOld.Nba;

namespace SportsIq.Analytica
{
    public interface IAnalyticaNba
    {
        void CalculateNba(NbaGame nbaGame);
        void Close();
    }

    public class AnalyticaNba : AnalyticaBase, IAnalyticaNba
    {
        #region NBA Methods

        public void CalculateNba(NbaGame nbaGame)
        {
            Dictionary<string, Dictionary<string, double>> modelData = nbaGame.ModelData;
            LoadTables(modelData);
            SetSecondFoulPossession(nbaGame);

            string saveFileName = GetSaveFullFileName(nbaGame);
            SaveModel(saveFileName);
            //LogArrayValues();
        }

        private void LoadTables(IReadOnlyDictionary<string, Dictionary<string, double>> modelData)
        {
            Dictionary<string, string[]> tableIndicesDictionary = new Dictionary<string, string[]>
            {
                {NbaModelDataKeys.Inmlf, new[] {"iGP", "iBL", "iGL2"}},                            // Market Odds
                // todo GSC dictionary is missing
                {NbaModelDataKeys.Gsc, new[] {"iTVH", "iMTR", "iQT", "iPLY"}},                     // Game Score
                {NbaModelDataKeys.Ttm, new[] {"iGP", "iMTR"}},                                     // Team Totals by Metric
                {NbaModelDataKeys.Posc, new[] {"iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS"}},      // Standard Deviation Player versus Team by Metric
                {NbaModelDataKeys.Potm, new[] {"iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS"}},      // Standard Deviation Player versus Team by Metric
                {NbaModelDataKeys.Pop, new[]  {"iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS"}},      // Standard Deviation Player versus Team by Metric
                {NbaModelDataKeys.Psco, new[] {"iTVH", "iMTR", "iQT", "iGT", "iPLY", "iTS"}},      // Standard Deviation Player versus Team by Metric
                {NbaModelDataKeys.Sdvtm, new[] {"iTVH", "iMTR", "iPLY"}},                          // Standard Deviation Player versus Team by Metric
                // todo SDOM has 6 dimensions in code and 3 in model
                {NbaModelDataKeys.Sdom, new[] { "iTVH", "iMTR", "iPLY" }}                          // Standard Deviation Player versus Team by Metric
            };

            LoadTables(tableIndicesDictionary, modelData);
        }

        private void SetSecondFoulPossession(NbaGame nbaGame)
        {
            CATable caTable = GetDefTable("EGT");

            string[] indexArray = { "S" };
            int seconds = nbaGame.NbaScore.Seconds;

            if (!caTable.SetDataByLabels(seconds, indexArray))
            {
                string message = $"SetDataByLabels() failed: seconds = {seconds}, indexArray1 = {ConvertIndexArrayToString(indexArray)}";
                Logger.Error(message);
                throw new Exception(message);
            }

            indexArray = new[] { "F" };
            int foul = nbaGame.Foul;

            if (!caTable.SetDataByLabels(foul, indexArray))
            {
                string message = $"SetDataByLabels() failed: foul = {foul}, indexArray2 = {ConvertIndexArrayToString(indexArray)}";
                throw new Exception(message);
            }

            indexArray = new[] { "P" };
            string possession = $"{nbaGame.Possession}";
            Logger.Info($"possession = {possession}, indexArray3 = {ConvertIndexArrayToString(indexArray)}");

            // todo bug here
            if (!caTable.SetDataByLabels(possession, indexArray))
            {
                throw new Exception($"SetDataByLabels() failed: possession = {possession}, indexArray3 = {ConvertIndexArrayToString(indexArray)}");
            }

            if (!caTable.Update())
            {
                throw new Exception("Update() failed");
            }
        }

        //private void LogArrayValues()
        //{
        //    CATable caTable = GetDefTable("OPP");
        //    string[] indexArray = { "iMTR", "iTVH", "iPLY", "iAlt", "iGL2" };

        //    // todo fails here
        //    if (!caTable.SetIndexOrder(indexArray))
        //    {
        //        string message = $"SetIndexOrder() failed: indexArray = {ConvertIndexArrayToString(indexArray)}";
        //        throw new Exception(message);
        //    }

        //    dynamic safeArray = caTable.GetSafeArray();
        //    Array array = (Array)safeArray;
        //    WriteLine(array);

        //    caTable = GetDefTable("FP");
        //    // todo missing SetIndexOrder()?
        //    safeArray = caTable.GetSafeArray();
        //    array = (Array)safeArray;
        //    WriteLine(array);
        //}

        private static string GetSaveFullFileName(NbaGame nbaGame)
        {
            const string dateFormat = "yyyymmd";
            string dateTimeString = DateTime.Now.ToString(dateFormat);
            string saveFileName = $"{nbaGame.Away}_{nbaGame.Home}_{nbaGame.GameId}_{dateTimeString}.ana";

            // todo directory is wrong for some reason?
            string baseDirectory = Utils.GetBaseDirectory();
            string saveFullFileName = Path.Combine(baseDirectory, "AnalyticaSavedModels", saveFileName);
            Logger.Debug($"saveFullFileName = {saveFullFileName}");

            return saveFullFileName;
        }

        #endregion
    }
}
