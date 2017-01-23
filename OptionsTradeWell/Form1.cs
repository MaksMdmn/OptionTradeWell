using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.Properties;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell
{
    public partial class MainForm : Form, IMainForm
    {
        private static int VIEW_NUMBER_OF_IMPL_VOL_VALUES = Settings.Default.DisplayedPeriodOfImplVol;
        private double minValueY = Settings.Default.ChartsMinYValue;
        private double maxValueY = Settings.Default.ChartsMaxYValue;
        private double stepY = Settings.Default.ChartsStepYValue;

        private SortedDictionary<double, OptTableDataRow> rowMap;
        private List<string> columnsNames;
        private System.Timers.Timer statusBarReducingTimer;

        private DataTable optionsDataTable;

        private Series buyCallVolSeries;
        private Series sellCallVolSeries;
        private Series midCallVolSeries;
        private Series buyPutVolSeries;
        private Series sellPutVolSeries;
        private Series midPutVolSeries;
        private Series implVolCallSeries;
        private Series implVolPutSeries;

        public MainForm()
        {
            statusBarReducingTimer = new System.Timers.Timer();
            rowMap = new SortedDictionary<double, OptTableDataRow>();

            InitializeComponent();

            InitPrimarySettingsView();

            InitializeOptionsDataTable();

            SetupOptionDeskTableLayout();

            SetupChartsLayouts();

            StartUpdateTimer();

            toolStripPrBrConnection.Value = 100;
        }

        public void UpdatePrimaryViewData(List<double[]> tableDataList, int uniqueValueIndex)
        {
            if (tableDataList.Count == 0)
            {
                throw new IllegalViewDataException("data for display is incorrect or empty: " + tableDataList);
            }

            if (optionsDataTable.Rows.Count != 0)
            {
                optionsDataTable.Clear();
                rowMap.Clear();
            }

            for (int i = 0; i < tableDataList.Count; i++)
            {
                //Create and fulfill row in options table and projected that data to table and charts
                OptTableDataRow tempRow = new OptTableDataRow(i, uniqueValueIndex, tableDataList[i]);
                rowMap.Add(tempRow.UniqueValue, tempRow);
                optionsDataTable.Rows.Add();

                FulfilOptionsDataTableRow(tempRow);
            }
        }


        public void UpdateRowInViewDataMap(double[] updatedData, int uniqueValueIndex)
        {

            double tempKey = updatedData[uniqueValueIndex];
            if (this.rowMap.Count == 0)
            {
                throw new IllegalViewDataException("data for display is incorrect or empty: " + this.rowMap);
            }

            if (this.rowMap.ContainsKey(tempKey))
            {
                OptTableDataRow tempRow = this.rowMap[tempKey];
                tempRow.DataArr = updatedData;
                FulfilOptionsDataTableRow(tempRow);
            }
            else
            {
                throw new IllegalViewDataException("row with such a unique value does not exist in table: " + tempKey);
            }
        }

        public void UpdateFuturesData(string[] data)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                this.lblSpotPrice.Text = data[0];
                this.toolStripStLbAsset.Text = string.Format("{0} {1}", "underlying asset:", data[1]);
                this.toolStripStLbDaysToExp.Text = string.Format("{0} {1}", "opt. days left:", data[2]);
                this.toolStripStLbLastUpd.Text = string.Format("{0} {1:dd-MM-yyyy HH:mm:ss}", "last update:", DateTime.Now);
                this.toolStripPrBrConnection.Value = 100; //TODO
            }));

        }


        public void UpdateImplVolChartData(string[] data)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                if (implVolCallSeries.Points.Count > VIEW_NUMBER_OF_IMPL_VOL_VALUES)
                {
                    implVolCallSeries.Points.RemoveAt(0);
                    implVolPutSeries.Points.RemoveAt(0);
                }

                implVolCallSeries.Points.AddXY(data[0], Convert.ToDouble(data[1]));
                implVolPutSeries.Points.AddXY(data[0], Convert.ToDouble(data[2]));
            }));
        }

        public void InitPrimarySettingsView()
        {
            txBxAutoWrtTimeMs.Text = Settings.Default.AutoWrittingToFileMs.ToString();
            txBxCentralStrChangeTimeSec.Text = Settings.Default.MinActualStrikeUpdateTimeSec.ToString();
            txBxDaysInYear.Text = Settings.Default.DaysInYear.ToString();
            txBxFutTableName.Text = Settings.Default.FuturesTableName;
            txBxImplVolPeriodsDisplayedNumbers.Text = Settings.Default.DisplayedPeriodOfImplVol.ToString();
            txBxMaxVolValue.Text = Settings.Default.MaxValueOfImplVol.ToString();
            txBxMaxYValue.Text = Settings.Default.ChartsMaxYValue.ToString();
            txBxMinYValue.Text = Settings.Default.ChartsMinYValue.ToString();
            txBxNumberOfTrackOpt.Text = Settings.Default.NumberOfTrackingOptions.ToString();
            txBxOptTableName.Text = Settings.Default.OptionsTableName;
            txBxStepYValue.Text = Settings.Default.ChartsStepYValue.ToString();
            txBxServName.Text = Settings.Default.ServerName;
            txBxPathToVolFile.Text = Settings.Default.PathToVolatilityFile;
            txBxUniqueIndx.Text = Settings.Default.UniqueIndexInDdeDataArray.ToString();
            txBxRounding.Text = Settings.Default.RoundTo.ToString();
        }

        private void InitializeOptionsDataTable()
        {
            optionsDataTable = new DataTable();

            columnsNames = new List<string>()
            {
                "Bid call",
                "Ask call",
                "STRIKE",
                "Bid put",
                "Ask put",
                "BuyVol call",
                "SellVol call",
                "BuyVol put",
                "SellVol put",
                "Delta call",
                "Gamma call",
                "Vega call",
                "Theta call",
                "Delta put",
                "Gamma put",
                "Vega put",
                "Theta put"
            };

            for (int i = 0; i < columnsNames.Count; i++)
            {
                optionsDataTable.Columns.Add(columnsNames[i]);
            }

            dgvOptionDesk.DataSource = optionsDataTable;

        }

        private void SetupOptionDeskTableLayout()
        {
            for (int i = 0; i < columnsNames.Count; i++)
            {
                dgvOptionDesk.Columns[i].HeaderText = columnsNames[i];
                dgvOptionDesk.Columns[i].Width = 50;
                dgvOptionDesk.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvOptionDesk.Columns[i].ReadOnly = true;
                dgvOptionDesk.MultiSelect = false;
                dgvOptionDesk.Columns[i].Resizable = DataGridViewTriState.False;
            }

            dgvOptionDesk.Columns[0].DefaultCellStyle.BackColor = Color.Chartreuse;
            dgvOptionDesk.Columns[1].DefaultCellStyle.BackColor = Color.IndianRed;
            dgvOptionDesk.Columns[2].DefaultCellStyle.BackColor = Color.LightSlateGray;
            dgvOptionDesk.Columns[3].DefaultCellStyle.BackColor = Color.Chartreuse;
            dgvOptionDesk.Columns[4].DefaultCellStyle.BackColor = Color.IndianRed;
            dgvOptionDesk.Columns[5].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[6].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[7].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[8].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[9].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgvOptionDesk.Columns[10].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgvOptionDesk.Columns[11].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgvOptionDesk.Columns[12].DefaultCellStyle.BackColor = Color.BlueViolet;
            dgvOptionDesk.Columns[13].DefaultCellStyle.BackColor = Color.Chocolate;
            dgvOptionDesk.Columns[14].DefaultCellStyle.BackColor = Color.Chocolate;
            dgvOptionDesk.Columns[15].DefaultCellStyle.BackColor = Color.Chocolate;
            dgvOptionDesk.Columns[16].DefaultCellStyle.BackColor = Color.Chocolate;

            dgvOptionDesk.Columns[8].DisplayIndex = 16;
            dgvOptionDesk.Columns[7].DisplayIndex = 15;
            dgvOptionDesk.Columns[4].DisplayIndex = 10;
            dgvOptionDesk.Columns[3].DisplayIndex = 9;
            dgvOptionDesk.Columns[2].DisplayIndex = 8;
            dgvOptionDesk.Columns[1].DisplayIndex = 7;
            dgvOptionDesk.Columns[0].DisplayIndex = 6;

            dgvOptionDesk.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvOptionDesk.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvOptionDesk.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvOptionDesk.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvOptionDesk.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.MediumSlateBlue;
            dgvOptionDesk.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvOptionDesk.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            dgvOptionDesk.ScrollBars = ScrollBars.Vertical;
            dgvOptionDesk.Height = 315;
            dgvOptionDesk.Width = dgvOptionDesk.ColumnCount * 50 + 50 + 10;

            dgvOptionDesk.ScrollBars = ScrollBars.Vertical;

        }


        private void SetupChartsLayouts()
        {
            string toolTipFormat = "strike: #VALX{F2}\nvol: #VALY{F2}";
            Chart[] allCharts = new Chart[] { chrtImplVol, chrtCallVol, chrtPutVol };
            foreach (Chart chart in allCharts)
            {
                chart.BackColor = Color.SlateGray;
                chart.ChartAreas[0].BackColor = Color.SlateGray;

                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisX.MajorTickMark.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisX.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisX.ScaleBreakStyle.LineColor = Color.WhiteSmoke;

                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisY.MajorTickMark.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisY.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisY.ScaleBreakStyle.LineColor = Color.WhiteSmoke;
                chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00} %";
            }

            implVolCallSeries = new Series();
            implVolPutSeries = new Series();
            buyCallVolSeries = new Series();
            sellCallVolSeries = new Series();
            midCallVolSeries = new Series();
            buyPutVolSeries = new Series();
            sellPutVolSeries = new Series();
            midPutVolSeries = new Series();

            implVolCallSeries.ChartType = SeriesChartType.Spline;
            implVolCallSeries.Color = Color.LightGreen;
            implVolCallSeries.MarkerSize = 5;

            implVolPutSeries.ChartType = SeriesChartType.Spline;
            implVolPutSeries.Color = Color.LightCoral;
            implVolPutSeries.MarkerSize = 5;


            buyCallVolSeries.ChartType = SeriesChartType.Point;
            buyCallVolSeries.MarkerStyle = MarkerStyle.Circle;
            buyCallVolSeries.Color = Color.GreenYellow;
            buyCallVolSeries.MarkerSize = 10;
            buyCallVolSeries.ToolTip = toolTipFormat;

            sellCallVolSeries.ChartType = SeriesChartType.Point;
            sellCallVolSeries.MarkerStyle = MarkerStyle.Circle;
            sellCallVolSeries.Color = Color.OrangeRed;
            sellCallVolSeries.MarkerSize = 10;
            sellCallVolSeries.ToolTip = toolTipFormat;

            midCallVolSeries.ChartType = SeriesChartType.Spline;
            midCallVolSeries.Color = Color.Aquamarine;
            midCallVolSeries.MarkerSize = 5;

            buyPutVolSeries.ChartType = SeriesChartType.Point;
            buyPutVolSeries.MarkerStyle = MarkerStyle.Circle;
            buyPutVolSeries.Color = Color.GreenYellow;
            buyPutVolSeries.MarkerSize = 10;
            buyPutVolSeries.ToolTip = toolTipFormat;

            sellPutVolSeries.ChartType = SeriesChartType.Point;
            sellPutVolSeries.MarkerStyle = MarkerStyle.Circle;
            sellPutVolSeries.Color = Color.OrangeRed;
            sellPutVolSeries.MarkerSize = 10;
            sellPutVolSeries.ToolTip = toolTipFormat;

            midPutVolSeries.ChartType = SeriesChartType.Spline;
            midPutVolSeries.Color = Color.DarkRed;
            midPutVolSeries.MarkerSize = 5;

            chrtImplVol.Series.Add(implVolCallSeries);
            chrtImplVol.Series.Add(implVolPutSeries);

            chrtCallVol.Series.Add(buyCallVolSeries);
            chrtCallVol.Series.Add(sellCallVolSeries);
            chrtCallVol.Series.Add(midCallVolSeries);

            chrtPutVol.Series.Add(buyPutVolSeries);
            chrtPutVol.Series.Add(sellPutVolSeries);
            chrtPutVol.Series.Add(midPutVolSeries);

            buyCallVolSeries.XValueMember = "Strike";
            buyCallVolSeries.YValueMembers = "BuyCallVol";
            sellCallVolSeries.XValueMember = "Strike";
            sellCallVolSeries.YValueMembers = "SellCallVol";
            midCallVolSeries.XValueMember = "Strike";
            midCallVolSeries.YValueMembers = "MidCallVol";

            buyPutVolSeries.XValueMember = "Strike";
            buyPutVolSeries.YValueMembers = "BuyPutVol";
            sellPutVolSeries.XValueMember = "Strike";
            sellPutVolSeries.YValueMembers = "SellPutVol";
            midPutVolSeries.XValueMember = "Strike";
            midPutVolSeries.YValueMembers = "MidPutVol";

            chrtCallVol.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            chrtPutVol.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;

            chrtCallVol.DataSource = rowMap.Values;
            chrtPutVol.DataSource = rowMap.Values;

            chrtImplVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtImplVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtImplVol.ChartAreas[0].AxisY.Interval = stepY;

            chrtCallVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtCallVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtCallVol.ChartAreas[0].AxisY.Interval = stepY;

            chrtPutVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtPutVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtPutVol.ChartAreas[0].AxisY.Interval = stepY;

        }

        private void StartUpdateTimer()
        {
            statusBarReducingTimer.Elapsed += StatusBarReducingTimer_Elapsed;
            statusBarReducingTimer.Interval = 500;
            statusBarReducingTimer.Enabled = true;
        }

        private void StatusBarReducingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                int tempVal = toolStripPrBrConnection.Value;
                if (tempVal > 0)
                {
                    toolStripPrBrConnection.Value = tempVal - 2;

                    chrtCallVol.DataBind();
                    chrtPutVol.DataBind();
                }
            }));
        }


        private void FulfilOptionsDataTableRow(OptTableDataRow row)
        {
            for (int i = 0; i < row.DataArr.Length; i++)
            {
                if (i == row.buyVollCallIndex
                    || i == row.sellVollCallIndex
                    || i == row.buyVollPutIndex
                    || i == row.sellVollPutIndex)
                {
                    optionsDataTable.Rows[row.RowNumber][i] = row.DataArr[i] * 100 + "%";
                }
                else
                {
                    optionsDataTable.Rows[row.RowNumber][i] = row.DataArr[i];
                }
            }
        }

        private class OptTableDataRow
        {
            internal readonly int buyVollCallIndex = 5;
            internal readonly int sellVollCallIndex = 6;
            internal readonly int buyVollPutIndex = 7;
            internal readonly int sellVollPutIndex = 8;

            private double[] dataArr;

            public OptTableDataRow(int rowNumber, int uniqueValueInDataArrIndex, double[] dataArr)
            {
                RowNumber = rowNumber;
                UniqueValueInDataArrIndex = uniqueValueInDataArrIndex;
                DataArr = dataArr;
                SetValuesFromDataArr();
            }

            public int RowNumber
            {
                get; private set;
            }
            public int UniqueValueInDataArrIndex
            {
                get; private set;
            }
            public double[] DataArr
            {
                get { return dataArr; }
                set
                {
                    this.dataArr = value;
                    SetValuesFromDataArr();
                }
            }

            public double UniqueValue
            {
                get
                {
                    return DataArr[UniqueValueInDataArrIndex];
                }
            }

            public double Strike { get; set; }

            public double BuyCallVol { get; set; }

            public double SellCallVol { get; set; }

            public double MidCallVol { get; set; }

            public double BuyPutVol { get; set; }

            public double SellPutVol { get; set; }

            public double MidPutVol { get; set; }

            private void SetValuesFromDataArr()
            {
                Strike = DataArr[UniqueValueInDataArrIndex];
                BuyCallVol = DataArr[buyVollCallIndex] * 100.0;
                SellCallVol = DataArr[sellVollCallIndex] * 100.0;
                MidCallVol = ((DataArr[buyVollCallIndex] + DataArr[sellVollCallIndex]) / 2.0) * 100.0;
                BuyPutVol = DataArr[buyVollPutIndex] * 100.0;
                SellPutVol = DataArr[sellVollPutIndex] * 100.0;
                MidPutVol = ((DataArr[buyVollPutIndex] + DataArr[sellVollPutIndex]) / 2.0) * 100.0;
            }
        }

        private void btnSetDefaultSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {

        }
    }
}

