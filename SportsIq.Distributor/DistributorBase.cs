using System;
using System.Collections.Generic;
using log4net;
using Newtonsoft.Json;
using SportsIq.Models.Markets;
using SportsIq.Pusher;
using SportsIq.Utilities;

namespace SportsIq.Distributor
{
    public abstract class DistributorBase
    {
        protected IPusherUtil PusherUtil;
        private static readonly double ProbabilityLowerLimit;
        protected static ILog Logger;

        static DistributorBase()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = LogManager.GetLogger(typeof(DistributorBase));
            ProbabilityLowerLimit = 0.035;
        }

        //----------------------------------------------------------------------------------

        protected static bool SelectInsideLimitsWithNonZeroTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName && 
                         market.Target.IsNotEqualToZero() && 
                         IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        protected static bool SelectOutsideLimitsWithNonZeroTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName &&
                         market.Target.IsNotEqualToZero() &&
                         !IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        //----------------------------------------------------------------------------------

        protected static bool SelectInsideLimitsWithZeroTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName && 
                         market.Target.IsEqualToZero() && 
                         IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        // todo why is this function unused? Error?
        public static bool SelectOutsideLimitsWithZeroTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName &&
                         market.Target.IsEqualToZero() &&
                         !IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        //----------------------------------------------------------------------------------

        protected static bool SelectInsideLimitsWithoutTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName && 
                         IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        protected static bool SelectOutsideLimitsWithoutTarget(Market market, string marketName)
        {
            double probability = market.MarketRunnerList[0].Probability;

            bool found = market.Tp == marketName &&
                         !IsBetweenLimits(probability, ProbabilityLowerLimit);

            return found;
        }

        //----------------------------------------------------------------------------------

        private static bool IsBetweenLimits(double value, double lowerLimit)
        {
            double upperLimit = 1 - lowerLimit;
            bool isBetweenLimits = lowerLimit < value && value < upperLimit;
            return isBetweenLimits;
        }

        //----------------------------------------------------------------------------------

        protected static Markets SetMarketActive(Markets markets)
        {
            foreach (Market market in markets.MarketList)
            {
                market.Active = true;
            }

            return markets;
        }

        protected static Markets SetMarketInactive(Markets markets)
        {
            foreach (Market market in markets.MarketList)
            {
                market.Active = false;
            }

            return markets;
        }

        //----------------------------------------------------------------------------------

        protected void SendOddsMessage(Markets markets, Guid gameId, List<Markets> formattedMarketList, string eventName)
        {
            if (markets.MarketList.Count == 0)
            {
               // Logger.Info($"No odds for {gameId}");
                return;
            }

            //Logger.Info($"Sending {markets.MarketList.Count} {markets.MarketList[0].Tp}");
            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}{markets.Id}{gameId}";

            // todo moved Add() before SendOddsMessage()
            formattedMarketList.Add(markets);
            PusherUtil.SendOddsMessage(message, eventName, key);
        }

        protected void SendOddsMessage2(Markets markets, Guid gameId, string eventName)
        {
            // send odds message without updating the formatted list

            if (markets.MarketList.Count == 0)
            {
              //  Logger.Info($"No odds for {gameId}");
                return;
            }

            string message = JsonConvert.SerializeObject(markets);
            string key = $"{markets.Period}{markets.Id}{gameId}_susp";

            PusherUtil.SendOddsMessage(message, eventName, key);
        }
    }
}
