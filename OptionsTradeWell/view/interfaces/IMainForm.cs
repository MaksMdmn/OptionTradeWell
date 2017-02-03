using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        event EventHandler OnStartUp;
        event EventHandler OnSettingsInFormChanged;
        event EventHandler<PositionTableArgs> OnPosUpdateButtonClick;

        void UpdateViewData(List<double[]> tableDataList);
        void UpdateFuturesData(string[] data);
        void UpdatePositionTableData(List<string[]> tableDataList);
        void UpdatePositionChartData(List<double[]> tableDataList);
        void UpdateTotalInfoTable(double[] dataArr);
    }

    public class PositionTableArgs : EventArgs
    {
        public List<string[]> userArgs;

        public PositionTableArgs(List<string[]> userArgs)
        {
            this.userArgs = userArgs;
        }
    } 
}