using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        event EventHandler OnStartUp;
        event EventHandler OnSettingsInFormChanged;

        void UpdateViewData(List<double[]> tableDataList);
        void UpdateFuturesData(string[] data);
    }
}