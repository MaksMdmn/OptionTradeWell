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
        public static double minValueY = Settings.Default.ChartsMinYValue;
        public static double maxValueY = Settings.Default.ChartsMaxYValue;
        public static double stepY = Settings.Default.ChartsStepYValue;

        private static int NUMBER_OF_POSDATATABLE_ROWS_HARDCORE = 30;

        public event EventHandler OnStartUp;
        public event EventHandler OnSettingsInFormChanged;
        public event EventHandler<PositionTableArgs> OnPosUpdateButtonClick;

        private object threadLock = new object();

        private SortedDictionary<double, OptionsTableRow> rowMap;
        private System.Timers.Timer statusBarReducingTimer;

        private DataTable optionsDataTable;
        private DataTable posDataTable;

        private string[] totalInfoNames;

        private Series buyCallVolSeries;
        private Series sellCallVolSeries;
        private Series midCallVolSeries;
        private Series buyPutVolSeries;
        private Series sellPutVolSeries;
        private Series midPutVolSeries;
        private Series curPosSeries;
        private Series expirPosSeries;

        public MainForm()
        {
            statusBarReducingTimer = new System.Timers.Timer();
            rowMap = new SortedDictionary<double, OptionsTableRow>();

            InitializeComponent();

            InitPrimarySettingsView();

            InitializeOptionsDataTable();

            InitializePosDataTable();

            InitializeTotalInfoTable();

            SetupChartsLayouts();

            StartUpdateTimer();

            toolStripPrBrConnection.Value = 100;
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
                this.lblDaysToExp.Text = data[2];
                this.toolStripStLbLastUpd.Text = string.Format("{0} {1:dd-MM-yyyy HH:mm:ss}", "last update:", DateTime.Now);
                this.toolStripPrBrConnection.Value = 100; //TODO
            }));

        }

        public void InitPrimarySettingsView()
        {
            txBxCentralStrChangeTimeSec.Text = Settings.Default.MinActualStrikeUpdateTimeSec.ToString();
            txBxDaysInYear.Text = Settings.Default.DaysInYear.ToString();
            txBxFutTableName.Text = Settings.Default.FuturesTableName;
            txBxMaxVolValue.Text = Settings.Default.MaxValueOfImplVol.ToString();
            txBxMaxYValue.Text = Settings.Default.ChartsMaxYValue.ToString();
            txBxMinYValue.Text = Settings.Default.ChartsMinYValue.ToString();
            txBxNumberOfTrackOpt.Text = Settings.Default.NumberOfTrackingOptions.ToString();
            txBxOptTableName.Text = Settings.Default.OptionsTableName;
            txBxStepYValue.Text = Settings.Default.ChartsStepYValue.ToString();
            txBxServName.Text = Settings.Default.ServerName;
            txBxUniqueIndx.Text = Settings.Default.UniqueIndexInDdeDataArray.ToString();
            txBxRounding.Text = Settings.Default.RoundTo.ToString();
            txBxStrikesNumber.Text = Settings.Default.OptDeskStrikesNumber.ToString();
        }
        public void UpdateViewData(List<double[]> tableDataList)
        {
            lock (threadLock)
            {
                if (rowMap.Count == 0)
                {
                    FirstCreationOfDataMap(tableDataList);
                }
                else
                {
                    UsualUpdateDataInRowMap(tableDataList);
                }
            }
        }

        public void UpdatePositionTableData(List<string[]> tableDataList)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                for (int i = 0; i < tableDataList.Count; i++)
                {
                    StringArrToPosTable(tableDataList[i], i);
                }
            }));
        }

        public void UpdatePositionChartData(List<double[]> tableDataList)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                curPosSeries.Points.Clear();
                expirPosSeries.Points.Clear();
                foreach (double[] dataArr in tableDataList)
                {
                    curPosSeries.Points.AddXY(dataArr[0], dataArr[1]);
                    expirPosSeries.Points.AddXY(dataArr[0], dataArr[2]);
                }
            }));
        }


        public void UpdateTotalInfoTable(double[] dataArr)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                for (int i = 0; i < dataArr.Length; i++)
                {
                    dgvTotalInfo[0, i + 1].Value = totalInfoNames[i] + " " + dataArr[i];
                    if (dataArr[i] >= 0)
                    {
                        dgvTotalInfo[0, i + 1].Style.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        dgvTotalInfo[0, i + 1].Style.BackColor = Color.LightCoral;
                    }
                }
            }));
        }

        private void FirstCreationOfDataMap(List<double[]> tableDataList)
        {
            for (int i = 0; i < tableDataList.Count; i++)
            {
                OptionsTableRow row = new OptionsTableRow(tableDataList[i]);
                row.IndexInTable = GetDataRowIndex(optionsDataTable.Rows.Add());
                rowMap.Add(row.UniqueValue, row);
                RowDataArrToOptionsTable(row);
            }
        }

        private void UsualUpdateDataInRowMap(List<double[]> tableDataList)
        {
            foreach (double[] dataArr in tableDataList)
            {
                OptionsTableRow row = new OptionsTableRow(dataArr);
                double minKey = rowMap.Keys.Min();
                double maxKey = rowMap.Keys.Max();

                if (dataArr[Settings.Default.UniqueIndexInDdeDataArray] < minKey)
                {
                    AddFirstRemoveLastRow(row);
                }
                else if (dataArr[Settings.Default.UniqueIndexInDdeDataArray] > maxKey)
                {
                    AddLastRemoveFirstRow(row);
                }
                else
                {
                    ChangeExistingRow(row);
                }
            }
        }

        private void AddFirstRemoveLastRow(OptionsTableRow row)
        {
            DataRow tempDataRow = optionsDataTable.NewRow();

            row.IndexInTable = 0;

            optionsDataTable.Rows.InsertAt(tempDataRow, 0);
            optionsDataTable.Rows.RemoveAt(optionsDataTable.Rows.Count - 1);

            rowMap.Remove(rowMap.Keys.Max());
            rowMap.Add(row.UniqueValue, row);

            RowDataArrToOptionsTable(row);
        }

        private void AddLastRemoveFirstRow(OptionsTableRow row)
        {
            DataRow tempDataRow = optionsDataTable.NewRow();

            row.IndexInTable = optionsDataTable.Rows.Count - 1;

            optionsDataTable.Rows.InsertAt(tempDataRow, optionsDataTable.Rows.Count);
            optionsDataTable.Rows.RemoveAt(0);

            rowMap.Remove(rowMap.Keys.Min());
            rowMap.Add(row.UniqueValue, row);

            RowDataArrToOptionsTable(row);
        }

        private void ChangeExistingRow(OptionsTableRow row)
        {
            OptionsTableRow actualRow = rowMap[row.UniqueValue];
            actualRow.DataArr = row.DataArr;
            RowDataArrToOptionsTable(actualRow);
        }

        private int GetDataRowIndex(DataRow row)
        {
            return optionsDataTable.Rows.IndexOf(row);
        }

        private void RowDataArrToOptionsTable(OptionsTableRow row)
        {
            for (int i = 0; i < row.DataArr.Length; i++)
            {
                if (i == OptionsTableRow.buyVollPutIndex
                    || i == OptionsTableRow.sellVollPutIndex
                    || i == OptionsTableRow.buyVollCallIndex
                    || i == OptionsTableRow.sellVollCallIndex)
                {
                    optionsDataTable.Rows[row.IndexInTable][i] = OptionsTableRow.GetPectentageViewOfVola(row.DataArr[i]);
                }
                else
                {
                    optionsDataTable.Rows[row.IndexInTable][i] = row.DataArr[i];
                }
            }
        }

        private void StringArrToPosTable(string[] strArr, int rowNumber)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                posDataTable.Rows[rowNumber][i] = strArr[i];
            }
        }

        private void InitializeOptionsDataTable()
        {
            optionsDataTable = new DataTable();

            List<string> columnsNames = new List<string>()
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

            //SETUP TABLE LAYOUT
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

            dgvOptionDesk.Height = 315;
            dgvOptionDesk.Width = dgvOptionDesk.ColumnCount * 50 + 50 + 10;

            dgvOptionDesk.ScrollBars = ScrollBars.Vertical;
        }

        private void InitializePosDataTable()
        {
            posDataTable = new DataTable();

            List<string> columnsNames = new List<string>()
            {
                "Type",
                "Strike",
                "Ent.Price",
                "Quantity",
                "Cur.Price",
                "Cur.Vol",
                "PnL usd",
                "PnL rub",
                "Delta",
                "Gamma",
                "Vega",
                "Theta",
            };

            for (int i = 0; i < columnsNames.Count; i++)
            {
                posDataTable.Columns.Add(columnsNames[i]);
            }

            //hardcore 30 row
            for (int i = 0; i < NUMBER_OF_POSDATATABLE_ROWS_HARDCORE; i++)
            {
                posDataTable.Rows.Add();
            }

            dgvPositions.DataSource = posDataTable;

            for (int i = 0; i < columnsNames.Count; i++)
            {
                dgvPositions.Columns[i].HeaderText = columnsNames[i];
                dgvPositions.Columns[i].Width = 60;
                dgvPositions.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvPositions.Columns[i].ReadOnly = false;
                dgvPositions.MultiSelect = false;
                dgvPositions.Columns[i].Resizable = DataGridViewTriState.False;
            }

            dgvPositions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvPositions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvPositions.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvPositions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvPositions.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Aquamarine;
            dgvPositions.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvPositions.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvPositions.AllowUserToDeleteRows = false;
            dgvPositions.AllowUserToAddRows = false;

            dgvPositions.Height = 310;
            dgvPositions.Width = 590;

            dgvPositions.ScrollBars = ScrollBars.Both;
        }

        private void InitializeTotalInfoTable()
        {

            dgvTotalInfo.Columns.Add("totalInfo", "");
            dgvTotalInfo.Columns[0].Width = 115;

            for (int i = 0; i < 12; i++)
            {
                dgvTotalInfo.Rows.Add();
            }

            dgvTotalInfo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvTotalInfo.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvTotalInfo.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvTotalInfo.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTotalInfo.RowHeadersVisible = false;
            dgvTotalInfo.ColumnHeadersVisible = false;
            dgvTotalInfo.GridColor = Color.WhiteSmoke;
            dgvTotalInfo.SelectionChanged += DgvTotalInfo_SelectionChanged;
            dgvTotalInfo.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvTotalInfo.ScrollBars = ScrollBars.None;

            totalInfoNames = new string[]
            {
                "rub_PnL:",
                "usd_PnL:",
                "fixPnL:",
                "delta:",
                "gamma:",
                "vega:",
                "theta:"
            };

            dgvTotalInfo[0, 0].Value = "TOTAL";
            dgvTotalInfo[0, 0].Style.BackColor = Color.LightGray;
            dgvTotalInfo[0, 0].Style.Font = new Font("Microsoft Sans Serif", 10.0f, FontStyle.Bold);
        }

        private void DgvTotalInfo_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTotalInfo.SelectedCells.Count > 0)
                dgvTotalInfo.ClearSelection();
        }

        private void SetupChartsLayouts()
        {
            string toolTipFormat = "strike: #VALX{F2}\nvol: #VALY{F2}";
            Chart[] allCharts = new Chart[] { chrtCallVol, chrtPutVol, chrtPos };
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
            }

            buyCallVolSeries = new Series();
            sellCallVolSeries = new Series();
            midCallVolSeries = new Series();
            buyPutVolSeries = new Series();
            sellPutVolSeries = new Series();
            midPutVolSeries = new Series();

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

            chrtCallVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtCallVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtCallVol.ChartAreas[0].AxisY.Interval = stepY;
            chrtCallVol.Titles.Add("CALLs");
            chrtCallVol.Titles[0].Alignment = ContentAlignment.TopCenter;
            chrtCallVol.Titles[0].ForeColor = Color.WhiteSmoke;
            chrtCallVol.Titles[0].Font = new Font("Microsoft Sans Serif", 10.0f);
            chrtCallVol.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00} %";

            chrtPutVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtPutVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtPutVol.ChartAreas[0].AxisY.Interval = stepY;
            chrtPutVol.Titles.Add("PUTs");
            chrtPutVol.Titles[0].Alignment = ContentAlignment.TopCenter;
            chrtPutVol.Titles[0].ForeColor = Color.WhiteSmoke;
            chrtPutVol.Titles[0].Font = new Font("Microsoft Sans Serif", 10.0f);
            chrtPutVol.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00} %";

            curPosSeries = new Series();
            expirPosSeries = new Series();

            curPosSeries.ChartType = SeriesChartType.Spline;
            curPosSeries.Color = Color.DarkOrchid;
            curPosSeries.BorderWidth = 3;

            expirPosSeries.ChartType = SeriesChartType.Line;
            expirPosSeries.Color = Color.Yellow;
            expirPosSeries.BorderWidth = 3;

            chrtPos.Series.Add(curPosSeries);
            chrtPos.Series.Add(expirPosSeries);

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
                    lock (threadLock)
                    {
                        chrtCallVol.DataBind();
                        chrtPutVol.DataBind();
                    }
                }
            }));
        }

        private void btnSetDefaultSettings_Click(object sender, EventArgs e)
        {
            InitPrimarySettingsView();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.MinActualStrikeUpdateTimeSec = Convert.ToInt32(txBxCentralStrChangeTimeSec.Text);
            Settings.Default.DaysInYear = Convert.ToDouble(txBxDaysInYear.Text);
            Settings.Default.FuturesTableName = txBxFutTableName.Text;
            Settings.Default.MaxValueOfImplVol = Convert.ToDouble(txBxMaxVolValue.Text);
            Settings.Default.ChartsMaxYValue = Convert.ToDouble(txBxMaxYValue.Text);
            Settings.Default.ChartsMinYValue = Convert.ToDouble(txBxMinYValue.Text);
            Settings.Default.NumberOfTrackingOptions = Convert.ToInt32(txBxNumberOfTrackOpt.Text);
            Settings.Default.OptionsTableName = txBxOptTableName.Text;
            Settings.Default.ChartsStepYValue = Convert.ToDouble(txBxStepYValue.Text);
            Settings.Default.ServerName = txBxServName.Text;
            Settings.Default.UniqueIndexInDdeDataArray = Convert.ToInt32(txBxUniqueIndx.Text);
            Settings.Default.RoundTo = Convert.ToInt32(txBxRounding.Text);
            Settings.Default.OptDeskStrikesNumber = Convert.ToInt32(txBxStrikesNumber.Text);
            Settings.Default.Save();

            if (OnSettingsInFormChanged != null)
            {
                OnSettingsInFormChanged(sender, e);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.BackColor = Color.Gray;
            btnStart.Text = "Starting...";

            if (OnStartUp != null)
            {
                OnStartUp(sender, e);
            }
            btnStart.ForeColor = Color.SkyBlue;
            btnStart.Text = "Started";

            btnStart.Enabled = false;
        }

        private void btnDeleteOne_Click(object sender, EventArgs e)
        {

        }

        private void btnCleanPos_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdatePos_Click(object sender, EventArgs e)
        {
            PositionTableArgs args = new PositionTableArgs(GetPosTableArgs());
            CleanPosTableData();

            if (OnPosUpdateButtonClick != null)
            {
                OnPosUpdateButtonClick(sender, args);
            }
        }

        private List<string[]> GetPosTableArgs()
        {
            List<string[]> result = new List<string[]>();

            for (int i = 0; i < NUMBER_OF_POSDATATABLE_ROWS_HARDCORE; i++)
            {
                string[] tempArgs = new string[4];
                tempArgs[0] = posDataTable.Rows[i]["Type"].ToString();
                tempArgs[1] = posDataTable.Rows[i]["Strike"].ToString();
                tempArgs[2] = posDataTable.Rows[i]["Ent.Price"].ToString();
                tempArgs[3] = posDataTable.Rows[i]["Quantity"].ToString();

                if (!String.IsNullOrEmpty(tempArgs[0]))
                {
                    result.Add(tempArgs);
                }
            }

            return result;
        }

        private void CleanPosTableData()
        {
            posDataTable.Rows.Clear();
            //hardcore 30 row
            for (int i = 0; i < NUMBER_OF_POSDATATABLE_ROWS_HARDCORE; i++)
            {
                posDataTable.Rows.Add();
            }
        }

        private class OptionsTableRow
        {
            internal static readonly int buyVollCallIndex = 5;
            internal static readonly int sellVollCallIndex = 6;
            internal static readonly int buyVollPutIndex = 7;
            internal static readonly int sellVollPutIndex = 8;

            private double[] dataArr;

            public OptionsTableRow(double[] dataArr)
            {
                UniqueValueInDataArrIndex = Settings.Default.UniqueIndexInDdeDataArray;
                DataArr = dataArr;
                SetValuesFromDataArr();
            }

            public int IndexInTable { get; set; }

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

            public static string GetPectentageViewOfVola(double vol)
            {
                return vol * 100 + "%";
            }

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
    }
}

