using System;
using System.Collections.Generic;
using System.Data;
using OptionsTradeWell.model;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell.view
{
    public class DerivativesDataRender : IDerivativesDataRender
    {
        public double[] GetRenderDataFromFutures(Futures futures)
        {
            if (futures == null)
            {
                throw new RenderingDerivativesException("instrument is null, can't render it.");
            }

            double[] result = new double[]
            {
                futures.Position.GetMarketPriceToClose(futures.GetTradeBlotter()),
                0.0,
                Math.Round(futures.Position.CalcCurrentPnL(futures.GetTradeBlotter()), 2),
                Math.Round(futures.Position.CalcCurrentPnLInCurrency(futures.GetTradeBlotter(), futures.PriceStep, futures.PriceStepValue), 0),
                futures.Position.Quantity,
                0.0,
                0.0,
                0.0,
            };

            return result;
        }

        public double[] GetRenderDataFromOption(Option option)
        {
            if (option == null)
            {
                throw new RenderingDerivativesException("instrument is null, can't render it.");
            }

            double[] result = new double[]
            {
                option.Position.GetMarketPriceToClose(option.GetTradeBlotter()),
                Math.Round(option.ImplVol, 4) * 100,
                option.Position.CalcCurrentPnL(option.GetTradeBlotter()),
                Math.Round(option.Position.CalcCurrentPnLInCurrency(option.GetTradeBlotter(), option.PriceStep, option.PriceStepValue), 0),
                Math.Round(option.DependOnPosDelta(), 4),
                Math.Round(option.DependOnPosGamma(), 4),
                Math.Round(option.DependOnPosVega(), 4),
                Math.Round(option.DependOnPosTheta(), 4)
            };

            return result;

        }

        public double[] GetRenderDataFromOptionPair(Option call, Option put)
        {
            if (call == null || put == null)
            {
                throw new RenderingDerivativesException("instrument is null, can't render it.");
            }

            if (call.Strike != put.Strike)
            {
                throw new RenderingDerivativesException("Strikes of call and put options are not equal: " + call.ToString() + " " + put.ToString());
            }

            double[] result = new double[]
            {
                call.GetTradeBlotter().BidPrice,
                call.GetTradeBlotter().AskPrice,
                call.Strike,
                put.GetTradeBlotter().BidPrice,
                put.GetTradeBlotter().AskPrice,
                call.BuyVol,
                call.SellVol,
                put.BuyVol,
                put.SellVol,
                call.Delta,
                call.Gamma,
                call.Vega,
                call.Theta,
                put.Delta,
                put.Gamma,
                put.Vega,
                put.Theta
            };

            return result;
        }
    }
}