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
            throw new System.NotImplementedException();
        }

        public double[] GetRenderDataFromOption(Option option)
        {
            throw new System.NotImplementedException();
        }

        public double[] GetRenderDataFromOptionPair(Option call, Option put)
        {
            if (call.Strike != put.Strike)
            {
                throw new WrongOptionsPairException("Strikes of call and put options are not equal: " + call.ToString() + " " + put.ToString());
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