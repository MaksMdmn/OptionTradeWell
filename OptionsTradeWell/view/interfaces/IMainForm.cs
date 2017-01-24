using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        event EventHandler OnStartUp;
        event EventHandler OnSettingsInFormChanged;

        void UpdatePrimaryViewData(List<double[]> tableDataList, int uniqueValueIndex);
        void UpdateRowInViewDataMap(double[] updatedData, int uniqueValueIndex);
        void UpdateFuturesData(string[] data);
        void UpdateImplVolChartData(string[] data);
        void ReloadImplVolChartData(string[] data);
    }
}