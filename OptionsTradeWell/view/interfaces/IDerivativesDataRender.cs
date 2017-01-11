using OptionsTradeWell.model;

namespace OptionsTradeWell.view.interfaces
{
    public interface IDerivativesDataRender
    {
        double[] GetRenderDataFromFutures(Futures futures);
        double[] GetRenderDataFromOption(Option option);
        double[] GetRenderDataFromOptionPair(Option call, Option put);
    }
}