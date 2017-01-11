using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        //event EventHandler Button1Click;
        //event EventHandler Button2Click;
        //event EventHandler Button3Click;

        void ItinializePrimaryViewData(List<double[]>tableDataList, int uniqueValueIndex);
        void UpdateRowInViewDataMap(double[] updatedData, int uniqueValueIndex);
    }
}