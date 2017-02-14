using System;
using System.Collections.Generic;

namespace OptionsTradeWell.view.interfaces
{
    public interface IMainForm
    {
        event EventHandler OnSettingsInFormChanged;
        event EventHandler OnStartUpClick;
        event EventHandler OnTotalResetPositionInfoClick;
        //not implemented yet.
        event EventHandler OnGetPosFromQuikClick;
        event EventHandler<DeltaHedgeEventArgs> OnHandleHedgeClick;
        event EventHandler<DeltaHedgeEventArgs> OnAutoHedgeClick;

        //not implemented yet.
        event EventHandler<PositionTableArgs> OnPosUpdateButtonClick;
        event EventHandler<PositionTableArgs> OnActPosUpdateButtonClick;


        void UpdateViewData(List<double[]> tableDataList);
        void UpdateFuturesData(string[] data);
        void UpdatePositionTableData(List<string[]> tableDataList);
        void UpdateActualPositionTableData(List<string[]> tableDataList);
        void UpdatePositionChartData(List<double[]> tableDataList);
        void UpdateTotalInfoTable(double[] dataArr);
        void UpdateMessageWindow(string message);
    }

    public class PositionTableArgs : EventArgs
    {
        public List<string[]> userArgs;

        public PositionTableArgs(List<string[]> userArgs)
        {
            this.userArgs = userArgs;
        }
    }

    public class DeltaHedgeEventArgs : EventArgs
    {
        public DeltaHedgeEventArgs()
        {
        }

        public int MaxFutQ { get; set; }

        public double DeltaStep { get; set; }

        public List<double> HedgeLevels { get; set; }
    }

    public class PositionCloseConditionEventArgs : EventArgs
    {
        public List<string> ClosingPositions { get; set; }

        public string TrackingInstr { get; set; }

        public string SignCondition { get; set; }

        public double PriceCondition { get; set; }
        public double PnLCondition { get; set; }
    }
}