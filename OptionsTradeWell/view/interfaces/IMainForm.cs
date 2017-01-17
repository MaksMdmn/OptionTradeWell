using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        //event EventHandler Button1Click;
        //event EventHandler Button2Click;
        //event EventHandler Button3Click;

        void UpdatePrimaryViewData(List<double[]>tableDataList, int uniqueValueIndex);
        void UpdateRowInViewDataMap(double[] updatedData, int uniqueValueIndex);
        void UpdateFuturesData(string[] data);
    }
}