using SportsIq.Models.Markets;
using System;
using System.Collections.Generic;

namespace SportsIq.Utilities
{
    public static class OddsConverter
    {
        public static Market FinishedOdds(Market market, bool live, double preVig, double liveVig)
        {
            double overRound = preVig;
            int marketRunnerListCount = market.MarketRunnerList.Count;

            if (live)
            {
                overRound = liveVig;
            }

            overRound += 0.015 * (marketRunnerListCount - 2);

            /* weight */
            if (marketRunnerListCount == 2)
            {
                market.Weight = Math.Abs(market.MarketRunnerList[0].Price - market.MarketRunnerList[1].Price);
            }

            if (marketRunnerListCount == 3)
            {
                market.Weight = Math.Abs(market.MarketRunnerList[0].Price + market.MarketRunnerList[1].Price - market.MarketRunnerList[2].Price);
            }
            
            //  overRound += 0.015 * (marketRunnerListCount - 2);

            List<MarketRunner> tempMarketRunnerList = new List<MarketRunner>();

            foreach (MarketRunner marketRunner in market.MarketRunnerList)
            {
                marketRunner.Probability = marketRunner.Price;
                MarketRunner tempMarketRunner = new MarketRunner
                {
                    Price = marketRunner.Price
                };

                tempMarketRunnerList.Add(tempMarketRunner);
            }

            // steps 1 & 2
            double sum = 0;

            foreach (MarketRunner marketRunner in tempMarketRunnerList)
            {
                marketRunner.Price = 1 / marketRunner.Price;
                sum += marketRunner.Price;
            }

            // step 3
            foreach (MarketRunner marketRunner in tempMarketRunnerList)
            {
                marketRunner.Ratio = marketRunner.Price / sum;
            }

            // step 4
            double uFactor;

            if (marketRunnerListCount == 2)
            {
                MarketRunner marketRunner = market.MarketRunnerList[0];

                if (marketRunner.Price > .5)
                {
                    //   uFactor = ((1 - marketRunner.P) / 0.5 + Math.Abs((0.5 - marketRunner.P) * (1 - marketRunner.P)) / 0.5) * .75 + .25;
                    uFactor = ((1 - marketRunner.Price) / 0.5 + Math.Abs((0.5 - marketRunner.Price) * (1 - marketRunner.Price)) / 0.5) * .75 + .25;

                }
                else
                {
                    uFactor = (marketRunner.Price / 0.5 + Math.Abs((0.5 - marketRunner.Price) * marketRunner.Price) / 0.5) * .75 + .25;

                    //  uFactor = (marketRunner.P / 0.5 + Math.Abs((0.5 - marketRunner.P) * marketRunner.P) / 0.5) * .75 + .25;
                }
            }
            else
            {
                // uFactor = 1.0;
                uFactor = 1 * .75 + 0.25;
            }

            double vFactor = (overRound - 1) * uFactor;

            // steps 6 & 7
            for (int n = 0; n < tempMarketRunnerList.Count; n++)
            {
                //market.MarketRunnerList[n].P = Math.Round(1 / (vFactor * tempMarketRunnerList[n].Ratio + market.MarketRunnerList[n].Probability), 2);
                market.MarketRunnerList[n].Price = Math.Round(1 / (vFactor * tempMarketRunnerList[n].Ratio + market.MarketRunnerList[n].Price), 2);

                if (double.IsNaN(market.MarketRunnerList[n].Price))
                {
                    market.MarketRunnerList[n].Price = 0;
                }
            }

            return market;
        }
    }
}
