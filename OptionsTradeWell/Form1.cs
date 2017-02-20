using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NLog;
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

        private static int NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE = 30;
        private static string SIMUL_POS_SAVE_FILE_NAME = "simulPosData.xml";
        private static string ACT_POS_SAVE_FILE_NAME = "actPosData.xml";
        private static Logger LOGGER = LogManager.GetCurrentClassLogger();

        public event EventHandler OnStartUpClick;
        public event EventHandler OnSettingsInFormChanged;
        public event EventHandler OnTotalResetPositionInfoClick;
        public event EventHandler<PositionTableArgs> OnPosUpdateButtonClick;
        public event EventHandler<PositionTableArgs> OnActPosUpdateButtonClick;
        public event EventHandler OnGetPosFromQuikClick;
        public event EventHandler<DeltaHedgeEventArgs> OnHandleHedgeClick;
        public event EventHandler<DeltaHedgeEventArgs> OnAutoHedgeClick;

        private bool isPosUpdating;
        private bool isLockingActPosTableEnabled;
        private bool isAutoHedgeEnabled;
        private bool isCheckBoxesLoaded;
        private DeltaHedgeEventArgs actualDeltaHedgeEventArgs;
        private PositionCloseConditionEventArgs actualPositionCloseConditionEventArgs;

        private SortedDictionary<double, OptionsTableRow> rowMap;

        private System.Timers.Timer statusBarReducingTimer;

        private DataTable optionsDataTable;
        private DataTable simulationPosDataTable;
        private DataTable actualPosDataTable;

        private string[] totalInfoNames;

        private Series buyCallVolSeries;
        private Series sellCallVolSeries;
        private Series midCallVolSeries;
        private Series buyPutVolSeries;
        private Series sellPutVolSeries;
        private Series midPutVolSeries;
        private Series curPosSeries;
        private Series expirPosSeries;
        private Series spotPriceSeries;
        private Series zeroSeries;

        public MainForm()
        {
            LOGGER.Info("Creation of MainForm...");
            statusBarReducingTimer = new System.Timers.Timer();
            rowMap = new SortedDictionary<double, OptionsTableRow>();
            isPosUpdating = false;
            isLockingActPosTableEnabled = false;
            isCheckBoxesLoaded = false;
            isAutoHedgeEnabled = false;

            InitializeComponent();

            LOGGER.Info("Components initialized");

            LoadPrimarySettings();

            LOGGER.Info("Primary settings initialized");

            InitializeOptionsDataTable();

            LOGGER.Info("OptionsDataTable initialized");

            InitializeSimulationPosDataTable();

            LOGGER.Info("SimulationPositionDataTable initialized");

            InitializeActualPosDataTable();

            LOGGER.Info("ActualPositionDataTable initialized");

            InitializeTotalInfoTable();

            LOGGER.Info("TotalInfoTable initialized");

            InitializeChartsLayouts();

            LOGGER.Info("Charts initialized");

            StartUpdateTimersEvents();

            toolStripPrBrConnection.Value = 100;

            dgvOptionDesk.DataError += DgvOptionDesk_DataError;
            dgvSimulPos.DataError += DgvSimulPosDataError;
            dgvTotalInfo.DataError += DgvTotalInfo_DataError;
            dgvActualPos.DataError += DgvActualPos_DataError;

            actualDeltaHedgeEventArgs = new DeltaHedgeEventArgs();
            actualPositionCloseConditionEventArgs = new PositionCloseConditionEventArgs();

            LoadPositionsCheckAndTextBoxesStatusFromSettings();

            LOGGER.Info("Check and Text Boxes values loaded");

            LOGGER.Info("MainForm created");
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
               this.toolStripStLbLastUpd.Text = string.Format("{0} {1:dd-MM-yyyy HH:mm:ss}", "last update:",
                   DateTime.Now);
               this.toolStripPrBrConnection.Value = 100;
           }));

        }

        public void UpdateViewData(List<double[]> tableDataList)
        {
            if (rowMap.Count == 0)
            {
                lock (rowMap)
                {
                    FirstCreationOfDataMap(tableDataList);
                }
            }
            else
            {
                UsualUpdateDataInRowMap(tableDataList);
            }
        }

        public void UpdateSimulationPositionTableData(List<string[]> tableDataList)
        {
            FulfilDataTable(simulationPosDataTable, tableDataList);
        }

        public void UpdateActualPositionTableData(List<string[]> tableDataList)
        {
            FulfilDataTable(actualPosDataTable, tableDataList);
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

               double expandVisibilityKoef = 1.5;
               double numberOfStepsAtChart = 10;

               double tempMinValY = 9999.99;
               double tempMaxValY = 0.0;
               double xVal = 0.0;
               double curPosVal = 0.0;
               double expPosVal = 0.0;

               double chartMin;
               double chartMax;
               double chartStep;

               double tempSpotPrice = 0.0;
               double notRoundedSpotPrice;
               if (Double.TryParse(lblSpotPrice.Text, out notRoundedSpotPrice))
               {
                   tempSpotPrice = Math.Round(notRoundedSpotPrice / Settings.Default.StrikeStep, 0) *
                                   Settings.Default.StrikeStep;
               }

               foreach (double[] dataArr in tableDataList)
               {
                   xVal = dataArr[0];
                   curPosVal = dataArr[1];
                   expPosVal = dataArr[2];

                   if (expPosVal > tempMaxValY)
                   {
                       tempMaxValY = expPosVal;
                   }

                   if (curPosVal > tempMaxValY)
                   {
                       tempMaxValY = curPosVal;
                   }

                   if (expPosVal < tempMinValY)
                   {
                       tempMinValY = expPosVal;
                   }

                   if (curPosVal < tempMinValY)
                   {
                       tempMinValY = curPosVal;
                   }
                   curPosSeries.Points.AddXY(xVal, curPosVal);
                   expirPosSeries.Points.AddXY(xVal, expPosVal);
                   zeroSeries.Points.Clear();
                   zeroSeries.Points.AddXY(xVal, 0.0);

                   if (tempSpotPrice > 0.0)
                   {
                       if (Math.Abs(tempSpotPrice - xVal) < 0.0001)
                       {
                           spotPriceSeries.Points.Clear();
                           spotPriceSeries.Points.AddXY(notRoundedSpotPrice, curPosVal);
                           spotPriceSeries.Points.AddXY(notRoundedSpotPrice, expPosVal);
                       }

                   }
               }

               chartMin = tempMinValY < 0 ? tempMinValY * expandVisibilityKoef : 0;
               chartMax = tempMaxValY < 0 ? 0 : tempMaxValY * expandVisibilityKoef;
               chartStep = Math.Round((tempMaxValY - tempMinValY) / numberOfStepsAtChart, 0);

               if (chartMin >= chartMax)
               {
                   chartMin = 0;
                   chartMax = 10;
                   chartStep = 1;
               }

               chrtPos.ChartAreas[0].AxisY.Minimum = chartMin;
               chrtPos.ChartAreas[0].AxisY.Maximum = chartMax;
               chrtPos.ChartAreas[0].AxisY.Interval = chartStep;

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

        public void UpdateMessageWindow(string message)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
           {
               StringBuilder sb = new StringBuilder();
               string headMessage = string.Format("{0:dd-MM-yyyy HH:mm:ss}", DateTime.Now);
               string oldMessage = txBxMsgInfo.Text;

               sb.AppendLine(headMessage);
               sb.AppendLine(message);
               sb.AppendLine(oldMessage);

               txBxMsgInfo.Text = sb.ToString();
           }));
        }

        private void FulfilDataTable(DataTable dataTable, List<string[]> tableDataList)
        {
            while (this.IsHandleCreated == false)
            {
            }

            this.BeginInvoke((Action)(() =>
            {
                for (int i = 0; i < tableDataList.Count; i++)
                {
                    StringArrToPosTable(tableDataList[i], i, dataTable);
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

        private void StringArrToPosTable(string[] strArr, int rowNumber, DataTable dataTable)
        {
            for (int i = 0; i < strArr.Length; i++)
            {
                dataTable.Rows[rowNumber][i] = strArr[i];
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

        private void InitializeSimulationPosDataTable()
        {
            simulationPosDataTable = new DataTable();
            simulationPosDataTable.TableName = "simulationPosDataTable";

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
                simulationPosDataTable.Columns.Add(columnsNames[i]);
            }

            if (File.Exists(SIMUL_POS_SAVE_FILE_NAME))
            {
                simulationPosDataTable.ReadXml(SIMUL_POS_SAVE_FILE_NAME);
            }
            else
            {
                //hardcore 30 row
                for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
                {
                    simulationPosDataTable.Rows.Add();
                }
            }


            dgvSimulPos.DataSource = simulationPosDataTable;

            for (int i = 0; i < columnsNames.Count; i++)
            {
                dgvSimulPos.Columns[i].HeaderText = columnsNames[i];
                dgvSimulPos.Columns[i].Width = 60;
                dgvSimulPos.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvSimulPos.Columns[i].ReadOnly = false;
                dgvSimulPos.Columns[i].Resizable = DataGridViewTriState.False;
            }
            dgvSimulPos.MultiSelect = true;
            dgvSimulPos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvSimulPos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvSimulPos.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvSimulPos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSimulPos.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Aquamarine;
            dgvSimulPos.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvSimulPos.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvSimulPos.AllowUserToDeleteRows = false;
            dgvSimulPos.AllowUserToAddRows = false;

            dgvSimulPos.Height = 310;
            dgvSimulPos.Width = 590;

            dgvSimulPos.ScrollBars = ScrollBars.Both;
        }

        private void InitializeActualPosDataTable()
        {
            actualPosDataTable = new DataTable();
            actualPosDataTable.TableName = "actualPosDataTable";

            List<string> columnsNames = new List<string>()
            {
                "Type",
                "Strike",
                "Ent.Price",
                "Quantity",
                "Cur.Price",
                "PnL usd",
                "PnL rub",
                "M.R.C.",
                "M.R.N.C.",
                "M.R.B."
            };

            for (int i = 0; i < columnsNames.Count; i++)
            {
                actualPosDataTable.Columns.Add(columnsNames[i]);
            }

            if (File.Exists(ACT_POS_SAVE_FILE_NAME))
            {
                actualPosDataTable.ReadXml(ACT_POS_SAVE_FILE_NAME);
            }
            else
            {
                //hardcore 30 row
                for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
                {
                    actualPosDataTable.Rows.Add();
                }
            }


            dgvActualPos.DataSource = actualPosDataTable;

            for (int i = 0; i < columnsNames.Count; i++)
            {
                dgvActualPos.Columns[i].HeaderText = columnsNames[i];
                dgvActualPos.Columns[i].Width = 60;
                dgvActualPos.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvActualPos.Columns[i].ReadOnly = false;
                dgvActualPos.Columns[i].Resizable = DataGridViewTriState.False;
            }
            dgvActualPos.MultiSelect = true;
            dgvActualPos.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvActualPos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvActualPos.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvActualPos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvActualPos.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.Aquamarine;
            dgvActualPos.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvActualPos.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvActualPos.AllowUserToDeleteRows = false;
            dgvActualPos.AllowUserToAddRows = false;

            dgvActualPos.Height = 200;
            dgvActualPos.Width = 660;

            dgvActualPos.ScrollBars = ScrollBars.Both;
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
                "theta:",
                "MY_GO:"
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

        private void InitializeChartsLayouts()
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
                chart.ChartAreas[0].AxisX.IsMarginVisible = false;

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
            chrtCallVol.ChartAreas[0].AxisX.Interval = Settings.Default.StrikeStep;

            chrtPutVol.ChartAreas[0].AxisY.Minimum = minValueY;
            chrtPutVol.ChartAreas[0].AxisY.Maximum = maxValueY;
            chrtPutVol.ChartAreas[0].AxisY.Interval = stepY;
            chrtPutVol.Titles.Add("PUTs");
            chrtPutVol.Titles[0].Alignment = ContentAlignment.TopCenter;
            chrtPutVol.Titles[0].ForeColor = Color.WhiteSmoke;
            chrtPutVol.Titles[0].Font = new Font("Microsoft Sans Serif", 10.0f);
            chrtPutVol.ChartAreas[0].AxisY.LabelStyle.Format = "{0.00} %";
            chrtPutVol.ChartAreas[0].AxisX.Interval = Settings.Default.StrikeStep;

            chrtPos.ChartAreas[0].AxisX.Interval = Settings.Default.StrikeStep * 2;
            chrtPos.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 8.0f);

            curPosSeries = new Series();
            expirPosSeries = new Series();
            spotPriceSeries = new Series();
            zeroSeries = new Series();

            curPosSeries.ChartType = SeriesChartType.Spline;
            curPosSeries.Color = Color.Aqua;
            curPosSeries.BorderWidth = 3;
            curPosSeries.SetCustomProperty("LineTension", "0");

            expirPosSeries.ChartType = SeriesChartType.Line;
            expirPosSeries.Color = Color.LimeGreen;
            expirPosSeries.BorderWidth = 3;

            zeroSeries.ChartType = SeriesChartType.Line;
            zeroSeries.Color = Color.Red;
            zeroSeries.BorderWidth = 1;

            spotPriceSeries.ChartType = SeriesChartType.Point;
            spotPriceSeries.Color = Color.Yellow;
            spotPriceSeries.BorderWidth = 10;
            spotPriceSeries.ToolTip = "price: #VALX{F2}\nPnL: #VALY{F2}";

            chrtPos.Series.Add(curPosSeries);
            chrtPos.Series.Add(expirPosSeries);
            chrtPos.Series.Add(spotPriceSeries);
            chrtPos.Series.Add(zeroSeries);
        }

        private void StartUpdateTimersEvents()
        {
            statusBarReducingTimer.Elapsed += StatusBarReducingTimer_Elapsed;
            statusBarReducingTimer.Interval = 1000;
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
               }

               chrtCallVol.DataBind();
               chrtPutVol.DataBind();

               if (isPosUpdating)
               {
                   PositionTableArgs args = new PositionTableArgs(GetPosTableArgs(simulationPosDataTable));
                   CleanDataTable(simulationPosDataTable);

                   if (OnPosUpdateButtonClick != null)
                   {
                       OnPosUpdateButtonClick(sender, args);
                   }
               }

           }));
        }

        private List<string[]> GetPosTableArgs(DataTable dataTable)
        {
            List<string[]> result = new List<string[]>();

            for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
            {
                string[] tempArgs = new string[4];
                tempArgs[0] = dataTable.Rows[i]["Type"].ToString().ToUpper();
                tempArgs[1] = dataTable.Rows[i]["Strike"].ToString();
                tempArgs[2] = dataTable.Rows[i]["Ent.Price"].ToString();
                tempArgs[3] = dataTable.Rows[i]["Quantity"].ToString();

                double checkIfDouble;
                int checkIfNotZero;
                UserPosTableTypes checkIfCorrectType;

                if (!String.IsNullOrEmpty(tempArgs[0])
                    && UserPosTableTypes.TryParse(tempArgs[0], out checkIfCorrectType)
                    && Double.TryParse(tempArgs[2], out checkIfDouble)
                    && Int32.TryParse(tempArgs[3], out checkIfNotZero)
                    && checkIfNotZero != 0)
                {

                    if (tempArgs[0].Equals(UserPosTableTypes.F))
                    {
                        tempArgs[1] = "";
                    }
                    result.Add(tempArgs);
                }
            }

            return result;
        }

        private void CleanDataTable(DataTable dataTable)
        {
            dataTable.Rows.Clear();
            //hardcore 30 row
            for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
            {
                dataTable.Rows.Add();
            }
        }

        private void CleanRowsInTable(DataTable dataTable, DataGridView dataGridView)
        {
            int selectedCellsCount = dataGridView.GetCellCount(DataGridViewElementStates.Selected);

            for (int i = 0; i < selectedCellsCount; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    dataTable.Rows[dataGridView.SelectedCells[i].RowIndex][j] = "";
                }
            }
        }

        private void LoadPrimarySettings()
        {
            txBxAccount.Text = Settings.Default.Account;
            txBxPosTableName.Text = Settings.Default.PositionTableName;
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
            txBxStrikesNumber.Text = Settings.Default.MaxOptionStrikeInQuikDesk.ToString();
            txBxStrStep.Text = Settings.Default.StrikeStep.ToString();
            txBxPathToQuik.Text = Settings.Default.PathToQuik.ToString();
        }

        private void SavePrimarySettings()
        {
            Settings.Default.Account = txBxAccount.Text;
            Settings.Default.PositionTableName = txBxPosTableName.Text;
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
            Settings.Default.MaxOptionStrikeInQuikDesk = Convert.ToInt32(txBxStrikesNumber.Text);
            Settings.Default.StrikeStep = Convert.ToDouble(txBxStrStep.Text);
            Settings.Default.PathToQuik = txBxPathToQuik.Text;
            Settings.Default.Save();
        }

        private void DgvOptionDesk_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DgvGeneralActionsIfDataErrorHappened(dgvOptionDesk, e, "Option Desk");
        }

        private void DgvActualPos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DgvGeneralActionsIfDataErrorHappened(dgvActualPos, e, "Actual Position");

        }

        private void DgvTotalInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DgvGeneralActionsIfDataErrorHappened(dgvTotalInfo, e, "Total Info");

        }

        private void DgvSimulPosDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DgvGeneralActionsIfDataErrorHappened(dgvSimulPos, e, "Positions in simulation");

        }

        private void DgvGeneralActionsIfDataErrorHappened(DataGridView dgv, DataGridViewDataErrorEventArgs e,
            string dgvName)
        {
            UpdateMessageWindow(e.Exception.StackTrace);
            UpdateMessageWindow(e.Exception.Message);

            LOGGER.Error("{0}: dataGridError event was activated: {1}, \n\r" +
                         "Could be helpful that data: \n\r ---rowMap---\n\r{2}\n\r" +
                         "---dgv---\n\r{3}",
                         dgvName,
                         e.Exception.ToString(),
                         RowMapToStringRepresentation(rowMap),
                         DataGrivViewToStringRepresentation(dgv));
        }

        private string RowMapToStringRepresentation(SortedDictionary<double, OptionsTableRow> map)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<double, OptionsTableRow> pair in map)
            {
                sb.Append("key: ");
                sb.Append(pair.Key);
                sb.Append(" value: ");
                sb.Append(pair.Value.ToString());
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string DataGrivViewToStringRepresentation(DataGridView dgv)
        {
            char delimiter = ';';
            StringBuilder sb = new StringBuilder();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    sb.Append(cell.Value);
                    sb.Append(delimiter);
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private void btnSetDefaultSettings_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnSetDefaultSettings_Click");
            LoadPrimarySettings();
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnSaveSettings_Click");
            SavePrimarySettings();

            if (OnSettingsInFormChanged != null)
            {
                OnSettingsInFormChanged(sender, e);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnStart_Click");

            btnStart.BackColor = Color.Gray;
            btnStart.Text = "Starting...";

            if (OnStartUpClick != null)
            {
                OnStartUpClick(sender, e);
            }
            btnStart.ForeColor = Color.SkyBlue;
            btnStart.Text = "Started";

            btnStart.Enabled = false;
        }

        private void btnUpdatePos_Click(object sender, EventArgs e)
        {
            isPosUpdating = !isPosUpdating;

            if (isPosUpdating)
            {
                btnUpdatePos.BackColor = Color.DarkSlateGray;
                btnUpdatePos.ForeColor = Color.WhiteSmoke;
                btnUpdatePos.Text = "updating...";

                for (int i = 0; i < dgvSimulPos.Rows.Count; i++)
                {
                    dgvSimulPos.Rows[i].ReadOnly = true;
                }

                simulationPosDataTable.WriteXml(SIMUL_POS_SAVE_FILE_NAME);
            }
            else
            {
                btnUpdatePos.BackColor = Color.Gainsboro;
                btnUpdatePos.ForeColor = Color.Black;
                btnUpdatePos.Text = "update";

                for (int i = 0; i < dgvSimulPos.Rows.Count; i++)
                {
                    dgvSimulPos.Rows[i].ReadOnly = false;
                }
            }

            btnCleanSelected.Enabled = !isPosUpdating;
            btnAddFromTable.Enabled = !isPosUpdating;
            btnPlusOneFut.Enabled = !isPosUpdating;
            btnMinusOneFut.Enabled = !isPosUpdating;
            btnRes.Enabled = !isPosUpdating;

            LOGGER.Info("btnUpdatePos_Click, enabled: {0}", isPosUpdating);
        }

        private void btnAddFromTable_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnAddFromTable_Click");

            int sellCallIndex = 0;
            int buyCallIndex = 1;
            int strikeIndex = 2;
            int sellPutIndex = 3;
            int buyPutIndex = 4;

            int rowIndex = dgvOptionDesk.SelectedCells[0].RowIndex;

            string type = null;
            string strike = null;
            string price = null;
            string number = null;

            strike = Convert.ToString(dgvOptionDesk[strikeIndex, rowIndex].Value);
            price = Convert.ToString(dgvOptionDesk.SelectedCells[0].Value);

            if (dgvOptionDesk.SelectedCells[0].ColumnIndex == sellCallIndex)
            {
                type = "C";
                number = "-1";
            }

            if (dgvOptionDesk.SelectedCells[0].ColumnIndex == buyCallIndex)
            {
                type = "C";
                number = "1";
            }

            if (dgvOptionDesk.SelectedCells[0].ColumnIndex == sellPutIndex)
            {
                type = "P";
                number = "-1";
            }

            if (dgvOptionDesk.SelectedCells[0].ColumnIndex == buyPutIndex)
            {
                type = "P";
                number = "1";
            }

            if (type == null)
            {
                return;
            }

            for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
            {
                if (Convert.ToString(dgvSimulPos[0, i].Value).Equals(""))
                {
                    dgvSimulPos[0, i].Value = type;
                    dgvSimulPos[1, i].Value = strike;
                    dgvSimulPos[2, i].Value = price;
                    dgvSimulPos[3, i].Value = number;
                    break;
                }
            }
        }

        private void btnCleanSelected_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnCleanSelected_Click");

            CleanRowsInTable(simulationPosDataTable, dgvSimulPos);
        }

        private void btnPlusOneFut_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnPlusOneFut_Click");

            for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
            {
                if (Convert.ToString(dgvSimulPos[0, i].Value).Equals(""))
                {
                    dgvSimulPos[0, i].Value = "F";
                    dgvSimulPos[1, i].Value = "";
                    dgvSimulPos[2, i].Value = lblSpotPrice.Text;
                    dgvSimulPos[3, i].Value = "1";
                    break;
                }
            }
        }

        private void btnMinusOneFut_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnMinusOneFut_Click");

            for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
            {
                if (Convert.ToString(dgvSimulPos[0, i].Value).Equals(""))
                {
                    dgvSimulPos[0, i].Value = "F";
                    dgvSimulPos[1, i].Value = "";
                    dgvSimulPos[2, i].Value = lblSpotPrice.Text;
                    dgvSimulPos[3, i].Value = "-1";
                    break;
                }
            }
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnRes_Click");

            if (OnTotalResetPositionInfoClick != null)
            {
                CleanDataTable(simulationPosDataTable);
                OnTotalResetPositionInfoClick(sender, e);
            }

            simulationPosDataTable.WriteXml(SIMUL_POS_SAVE_FILE_NAME);
        }


        private void btnGetPosFromQuik_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnGetPosFromQuik_Click");
            CleanDataTable(actualPosDataTable);

            if (OnGetPosFromQuikClick != null)
            {
                OnGetPosFromQuikClick(sender, e);
            }
        }

        private void btnSendToSimul_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnSendToSimul_Click");

            int startCl = 0;
            int endCl = 3;

            if (!isPosUpdating)
            {
                CleanDataTable(simulationPosDataTable);
                for (int i = 0; i < NUMBER_OF_ROWS_IN_ALL_POS_TABLES_HARDCORE; i++)
                {
                    for (int j = startCl; j <= endCl; j++)
                    {
                        simulationPosDataTable.Rows[i][j] = actualPosDataTable.Rows[i][j];
                    }
                }
            }
            else
            {
                MessageBox.Show("Simulation table is updating now. Please, turn off the update process and try again.");
            }
        }

        private void btnCleanActPosRows_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnCleanActPosRows_Click");

            CleanRowsInTable(actualPosDataTable, dgvActualPos);
        }

        private void btnOneTimUpdActPos_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnOneTimUpdActPos_Click");

            PositionTableArgs args = new PositionTableArgs(GetPosTableArgs(actualPosDataTable));
            CleanDataTable(actualPosDataTable);

            if (OnActPosUpdateButtonClick != null)
            {
                OnActPosUpdateButtonClick(sender, args);
            }

            actualPosDataTable.WriteXml(ACT_POS_SAVE_FILE_NAME);
        }

        private void btnLockPosTable_Click(object sender, EventArgs e)
        {
            isLockingActPosTableEnabled = !isLockingActPosTableEnabled;
            LOGGER.Info("btnLockPosTable_Click: {0}", isLockingActPosTableEnabled);

            if (isLockingActPosTableEnabled)
            {
                btnLockPosTable.BackColor = Color.Gray;
                btnLockPosTable.ForeColor = Color.WhiteSmoke;
                btnLockPosTable.Text = "LOCKED";

                for (int i = 0; i < dgvActualPos.Rows.Count; i++)
                {
                    dgvActualPos.Rows[i].ReadOnly = true;
                }

                btnGetPosFromQuik.Enabled = false;
                btnCleanActPosRows.Enabled = false;
            }
            else
            {
                btnLockPosTable.BackColor = Color.Transparent;
                btnLockPosTable.ForeColor = Color.Black;
                btnLockPosTable.Text = "UNLOCKED";

                for (int i = 0; i < dgvActualPos.Rows.Count; i++)
                {
                    dgvActualPos.Rows[i].ReadOnly = false;
                }

                btnGetPosFromQuik.Enabled = true;
                btnCleanActPosRows.Enabled = true;
            }
        }

        private void btnAutoHedge_Click(object sender, EventArgs e)
        {
            isAutoHedgeEnabled = !isAutoHedgeEnabled;
            LOGGER.Info("btnAutoHedge_Click: {0}", isAutoHedgeEnabled);

            if (isAutoHedgeEnabled)
            {
                btnAutoHedge.BackColor = Color.Gray;
                btnAutoHedge.ForeColor = Color.WhiteSmoke;
                btnAutoHedge.Text = "Auto Hedge/Close \r\nWORKING...";

                btnLockPosTable.PerformClick();
                btnLockPosTable.Enabled = false;
                btnHandleHedge.Enabled = false;

                txBxMaxFutQ.Enabled = false;
                txBxDeltaStep.Enabled = false;
                txBxPriceHedgeLvls.Enabled = false;
                txBxClosePosIfPosForClosing.Enabled = false;
                txBxClosePosIfTrackInstr.Enabled = false;
                txBxClosePosIfSign.Enabled = false;
                txBxClosePosIfPrice.Enabled = false;
                txBxClosePosIfPnL.Enabled = false;

                chkBxMaxFutQ.Enabled = false;
                chkBxDeltaStep.Enabled = false;
                chkBxPriceHedgeLvls.Enabled = false;
                chkBxCloseIfPrice.Enabled = false;
                chkBxCloseIfPnL.Enabled = false;
                chkBxClosePosIf.Enabled = false;

                if (OnAutoHedgeClick != null)
                {
                    OnAutoHedgeClick(sender, actualDeltaHedgeEventArgs);
                }
            }
            else
            {
                btnAutoHedge.BackColor = Color.Transparent;
                btnAutoHedge.ForeColor = Color.Black;
                btnAutoHedge.Text = "Auto Hedge/Close \r\nSTOPPED";

                txBxMaxFutQ.Enabled = true;
                txBxDeltaStep.Enabled = true;
                txBxPriceHedgeLvls.Enabled = true;
                txBxClosePosIfPosForClosing.Enabled = true;
                txBxClosePosIfTrackInstr.Enabled = true;
                txBxClosePosIfSign.Enabled = true;
                txBxClosePosIfPrice.Enabled = true;
                txBxClosePosIfPnL.Enabled = true;

                chkBxMaxFutQ.Enabled = true;
                chkBxDeltaStep.Enabled = true;
                chkBxPriceHedgeLvls.Enabled = true;
                chkBxCloseIfPrice.Enabled = true;
                chkBxCloseIfPnL.Enabled = true;
                chkBxClosePosIf.Enabled = true;

                btnHandleHedge.Enabled = true;
                btnLockPosTable.Enabled = true;
                btnLockPosTable.PerformClick();
            }
        }

        private void btnHandleHedge_Click(object sender, EventArgs e)
        {
            LOGGER.Info("btnHandleHedge_Click");
            if (OnHandleHedgeClick != null)
            {
                OnHandleHedgeClick(sender, actualDeltaHedgeEventArgs);
            }
        }

        private void chkBxMaxFutQ_CheckedChanged(object sender, EventArgs e)
        {
            int val;

            if (chkBxMaxFutQ.Checked == true)
            {
                if (!Int32.TryParse(txBxMaxFutQ.Text, out val) || val <= 0)
                {
                    MessageBox.Show("this value must be > 0, integer type.");
                    chkBxMaxFutQ.Checked = false;
                    txBxMaxFutQ.Text = "";
                }
                else
                {
                    actualDeltaHedgeEventArgs.MaxFutQ = val;
                    txBxMaxFutQ.Enabled = false;
                }
            }
            else
            {
                txBxMaxFutQ.Enabled = true;
            }

            SavePositionsCheckAndTextBoxesStatusToSettings();
        }

        private void chkBxDeltaStep_CheckedChanged(object sender, EventArgs e)
        {
            double val;

            if (chkBxDeltaStep.Checked == true)
            {
                if (!Double.TryParse(txBxDeltaStep.Text, out val) || val < 1)
                {
                    MessageBox.Show("this value must be >= 1, double type.");
                    chkBxDeltaStep.Checked = false;
                    txBxDeltaStep.Text = "";
                }
                else
                {
                    actualDeltaHedgeEventArgs.DeltaStep = val;
                    txBxDeltaStep.Enabled = false;
                    chkBxPriceHedgeLvls.Checked = false;
                }
            }
            else
            {
                txBxDeltaStep.Enabled = true;
            }

            SavePositionsCheckAndTextBoxesStatusToSettings();
        }

        private void chkBxPriceHedgeLvls_CheckedChanged(object sender, EventArgs e)
        {
            List<double> vals = new List<double>();

            if (chkBxPriceHedgeLvls.Checked == true)
            {
                string[] tempStrArr = txBxPriceHedgeLvls.Text.Split(new char[] { '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (tempStrArr.Length == 0)
                {
                    MessageBox.Show("all values must be > 0, double type.");
                    chkBxPriceHedgeLvls.Checked = false;
                    txBxPriceHedgeLvls.Text = "";

                    SavePositionsCheckAndTextBoxesStatusToSettings();

                    return;
                }

                foreach (string str in tempStrArr)
                {
                    double val;
                    if (!Double.TryParse(str, out val) || val <= 0)
                    {
                        MessageBox.Show("all values must be > 0, double type.");
                        chkBxPriceHedgeLvls.Checked = false;
                        txBxPriceHedgeLvls.Text = "";

                        SavePositionsCheckAndTextBoxesStatusToSettings();

                        return;
                    }
                    else
                    {
                        vals.Add(val);
                    }
                }
                actualDeltaHedgeEventArgs.HedgeLevels = vals;
                txBxPriceHedgeLvls.Enabled = false;
                chkBxDeltaStep.Checked = false;
            }
            else
            {
                txBxPriceHedgeLvls.Enabled = true;
            }

            SavePositionsCheckAndTextBoxesStatusToSettings();
        }

        private void chkBxClosePosIf_CheckedChanged(object sender, EventArgs e)
        {
            double price;
            bool isCorrectFiled = true;

            if (chkBxClosePosIf.Checked == true)
            {
                if (String.IsNullOrEmpty(txBxClosePosIfPosForClosing.Text))
                {
                    isCorrectFiled = false;
                    txBxClosePosIfPosForClosing.Text = "";
                }


                if (String.IsNullOrEmpty(txBxClosePosIfTrackInstr.Text))
                {
                    isCorrectFiled = false;
                    txBxClosePosIfTrackInstr.Text = "";
                }

                if (chkBxCloseIfPrice.Checked == false
                    && chkBxCloseIfPnL.Checked == false)
                {
                    isCorrectFiled = false;
                }


                if (isCorrectFiled == false)
                {
                    MessageBox.Show(
                        "All fields must be filled, positions and instrument are string" + "\r\n"
                        + "one of conditions (by price or pnl) must be choosen");
                    chkBxClosePosIf.Checked = false;

                    SavePositionsCheckAndTextBoxesStatusToSettings();

                    return;
                }

                actualPositionCloseConditionEventArgs.ClosingPositions =
                    new List<string>(txBxClosePosIfPosForClosing.Text.Split(new char[] { '\r', '\n' },
                        StringSplitOptions.RemoveEmptyEntries));
                actualPositionCloseConditionEventArgs.TrackingInstr = txBxClosePosIfTrackInstr.Text;
                actualPositionCloseConditionEventArgs.SignCondition = txBxClosePosIfSign.Text;

                txBxClosePosIfPosForClosing.Enabled = false;
                txBxClosePosIfTrackInstr.Enabled = false;
                txBxClosePosIfPrice.Enabled = false;
                txBxClosePosIfPrice.Enabled = false;
                txBxClosePosIfSign.Enabled = false;
                txBxClosePosIfPnL.Enabled = false;

            }
            else
            {
                txBxClosePosIfPosForClosing.Enabled = true;
                txBxClosePosIfTrackInstr.Enabled = true;
                txBxClosePosIfSign.Enabled = true;
                txBxClosePosIfPrice.Enabled = true;
                txBxClosePosIfPrice.Enabled = true;
                txBxClosePosIfSign.Enabled = true;
                txBxClosePosIfPnL.Enabled = true;
            }

            SavePositionsCheckAndTextBoxesStatusToSettings();
        }

        private void chkBxCloseIfPrice_CheckedChanged(object sender, EventArgs e)
        {
            double val;

            if (chkBxCloseIfPrice.Checked == true)
            {
                if (!Double.TryParse(txBxClosePosIfPrice.Text, out val) || val <= 0)
                {
                    MessageBox.Show("this value must be > 0, double type.");
                    chkBxCloseIfPrice.Checked = false;
                    txBxClosePosIfPrice.Text = "";

                    SavePositionsCheckAndTextBoxesStatusToSettings();

                    return;

                }

                if (String.IsNullOrEmpty(txBxClosePosIfSign.Text)
                    || (!txBxClosePosIfSign.Text.Equals("<") && !txBxClosePosIfSign.Text.Equals(">")))
                {
                    MessageBox.Show("possible conditions: < or >");
                    txBxClosePosIfSign.Text = "";

                    SavePositionsCheckAndTextBoxesStatusToSettings();

                    return;
                }

                else
                {
                    actualPositionCloseConditionEventArgs.PriceCondition = val;
                    actualPositionCloseConditionEventArgs.PnLCondition = 0.0;
                    chkBxCloseIfPnL.Checked = false;
                }

                SavePositionsCheckAndTextBoxesStatusToSettings();
            }

        }

        private void chkBxCloseIfPnL_CheckedChanged(object sender, EventArgs e)
        {
            double val;

            if (chkBxCloseIfPnL.Checked == true)
            {
                if (!Double.TryParse(txBxClosePosIfPnL.Text, out val) || val <= 0)
                {
                    MessageBox.Show("this value must be > 0, double type.");
                    chkBxCloseIfPnL.Checked = false;
                    txBxClosePosIfPnL.Text = "";
                }
                else
                {
                    actualPositionCloseConditionEventArgs.PriceCondition = 0.0;
                    actualPositionCloseConditionEventArgs.PnLCondition = val;
                    chkBxCloseIfPrice.Checked = false;
                }
            }

            SavePositionsCheckAndTextBoxesStatusToSettings();
        }

        private void SavePositionsCheckAndTextBoxesStatusToSettings()
        {
            if (isCheckBoxesLoaded)
            {
                Settings.Default.checkBoxMaxFutQ = chkBxMaxFutQ.Checked;
                Settings.Default.checkBoxDeltaStep = chkBxDeltaStep.Checked;
                Settings.Default.checkBoxPriceLevels = chkBxPriceHedgeLvls.Checked;
                Settings.Default.checkBosClosePos = chkBxClosePosIf.Checked;
                Settings.Default.checkBoxPriceCon = chkBxCloseIfPrice.Checked;
                Settings.Default.checkBoxPnLCon = chkBxCloseIfPnL.Checked;
                Settings.Default.checkBoxMaxFutQValue = txBxMaxFutQ.Text;
                Settings.Default.checkBoxDeltaStepValue = txBxDeltaStep.Text;
                Settings.Default.checkBoxPriceLevelsValue = txBxPriceHedgeLvls.Text;
                Settings.Default.checkBosCloseWhatPosWillBeCloseValue = txBxClosePosIfPosForClosing.Text;
                Settings.Default.checkBosCloseTrackingInstrValue = txBxClosePosIfTrackInstr.Text;
                Settings.Default.checkBoxPricePriceConSignValue = txBxClosePosIfSign.Text;
                Settings.Default.checkBoxPricePriceConValue = txBxClosePosIfPrice.Text;
                Settings.Default.checkBoxPnLValue = txBxClosePosIfPnL.Text;
                Settings.Default.Save();
            }
        }

        private void LoadPositionsCheckAndTextBoxesStatusFromSettings()
        {
            txBxMaxFutQ.Text = Settings.Default.checkBoxMaxFutQValue;
            txBxDeltaStep.Text = Settings.Default.checkBoxDeltaStepValue;
            txBxPriceHedgeLvls.Text = Settings.Default.checkBoxPriceLevelsValue;
            txBxClosePosIfPosForClosing.Text = Settings.Default.checkBosCloseWhatPosWillBeCloseValue;
            txBxClosePosIfTrackInstr.Text = Settings.Default.checkBosCloseTrackingInstrValue;
            txBxClosePosIfSign.Text = Settings.Default.checkBoxPricePriceConSignValue;
            txBxClosePosIfPrice.Text = Settings.Default.checkBoxPricePriceConValue;
            txBxClosePosIfPnL.Text = Settings.Default.checkBoxPnLValue;

            chkBxMaxFutQ.Checked = Settings.Default.checkBoxMaxFutQ;
            chkBxDeltaStep.Checked = Settings.Default.checkBoxDeltaStep;
            chkBxPriceHedgeLvls.Checked = Settings.Default.checkBoxPriceLevels;
            chkBxCloseIfPrice.Checked = Settings.Default.checkBoxPriceCon;
            chkBxCloseIfPnL.Checked = Settings.Default.checkBoxPnLCon;
            chkBxClosePosIf.Checked = Settings.Default.checkBosClosePos;

            isCheckBoxesLoaded = true;
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

            public int UniqueValueInDataArrIndex { get; private set; }

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
                get { return DataArr[UniqueValueInDataArrIndex]; }
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

            public override string ToString()
            {
                return $"{nameof(IndexInTable)}: {IndexInTable}, {nameof(UniqueValueInDataArrIndex)}: {UniqueValueInDataArrIndex}, {nameof(DataArr)}: {String.Join(";", DataArr)}, {nameof(UniqueValue)}: {UniqueValue}, {nameof(Strike)}: {Strike}, {nameof(BuyCallVol)}: {BuyCallVol}, {nameof(SellCallVol)}: {SellCallVol}, {nameof(MidCallVol)}: {MidCallVol}, {nameof(BuyPutVol)}: {BuyPutVol}, {nameof(SellPutVol)}: {SellPutVol}, {nameof(MidPutVol)}: {MidPutVol}";
            }
        }
    }
}

