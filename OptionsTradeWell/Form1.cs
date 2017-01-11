using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OptionsTradeWell.model.exceptions;
using OptionsTradeWell.view;
using OptionsTradeWell.view.interfaces;

namespace OptionsTradeWell
{
    public partial class MainForm : Form, IMainForm
    {
        private static int VIEW_NUMBER_OF_IMPL_VOL_VALUES = 300;

        private Dictionary<double, OptionsDataRow> rowMap;
        private DataTable optionsDataTable;
        private List<string> columnsNames;

        public MainForm()
        {
            rowMap = new Dictionary<double, OptionsDataRow>();

            InitializeComponent();

            InitializeOptionDeskTable();
            FulfilByTestData();
            //SetupLayouts();

        }

        private void InitializeOptionDeskTable()
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

            SetupOptionDeskTableLayout();
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
            dgvOptionDesk.Height = 415;
            dgvOptionDesk.Width = dgvOptionDesk.ColumnCount * 50 + 50 + 10;
        }


        private void SetupLayouts()
        {
            SetupOptionDeskTableLayout();
            SetupImplVolChartLayout();
            SetupCallVolChartLayout();
            SetupPutVolChartLayout();
            //FulfilByTestData();
        }


        private void SetupImplVolChartLayout()
        {
        }
        private void SetupCallVolChartLayout()
        {
        }
        private void SetupPutVolChartLayout()
        {
        }

        private void FulfilByTestData()
        {
            List<double[]> myList = new List<double[]>();

            double[] myTestData1 = new double[17];
            Random r = new Random();




            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (j == 2)
                    {
                        myTestData1[2] = i;
                    }
                    else
                    {
                        myTestData1[j] = (j+1) * r.Next(5, 20);
                    }
                }
                myList.Add(myTestData1);
                myTestData1 = new double[17];

            }

            ItinializePrimaryViewData(myList, 2);

            //double[] chartData1 = { 32.12, 43.11, 3.42, 12, 54, 110, 85.2, 25.3 };
            //double[] chartData2 = { 2.12, 48.11, 13.42, 18, 24, 10, 5.2, 25.3 };

            //chrtCallVol.Series.Add(new Series());
            //chrtCallVol.Series.Add(new Series());
            //chrtPutVol.Series.Add(new Series());
            //chrtPutVol.Series.Add(new Series());

            //Random rnd = new Random();
            //for (int i = 0; i < chartData1.Length; i++)
            //{
            //    chrtImplVol.Series[0].Points.AddXY(i, chartData1[i]);
            //    chrtImplVol.Series[0].Points.AddXY(i, chartData1[i]);

            //    chrtCallVol.Series[0].Points.AddXY(i, chartData1[i]);
            //    chrtCallVol.Series[1].Points.AddXY(i, chartData1[i] * rnd.Next(0, 5));

            //    chrtPutVol.Series[0].Points.AddXY(i, chartData1[i]);
            //    chrtPutVol.Series[1].Points.AddXY(i, chartData1[i] * rnd.Next(0, 5));
            //}

            toolStripStLbAsset.Text = "BR-02.17";
            toolStripStLbDaysToExp.Text = "9";

            toolStripPrBrConnection.Value = 100;


        }

        public void ItinializePrimaryViewData(List<double[]> tableDataList, int uniqueValueIndex)
        {
            if (tableDataList.Count == 0)
            {
                throw new IllegalViewDataException("data for display is incorrect or empty: " + tableDataList);
            }


            Series buyCallVolSeries = new Series();
            Series sellCallVolSeries = new Series();
            Series midCallVolSeries = new Series();

            Series buyPutVolSeries = new Series();
            Series sellPutVolSeries = new Series();
            Series midPutVolSeries = new Series();

            buyCallVolSeries.ChartType = SeriesChartType.Point;
            buyCallVolSeries.Color = Color.OrangeRed;

            sellCallVolSeries.ChartType = SeriesChartType.Point;
            sellCallVolSeries.Color = Color.Green;

            midCallVolSeries.ChartType = SeriesChartType.Line;
            midCallVolSeries.Color = Color.DarkSlateGray;

            buyPutVolSeries.ChartType = SeriesChartType.Point;
            buyPutVolSeries.Color = Color.OrangeRed;

            sellPutVolSeries.ChartType = SeriesChartType.Point;
            sellPutVolSeries.Color = Color.Green;

            midPutVolSeries.ChartType = SeriesChartType.Line;
            midPutVolSeries.Color = Color.DarkRed;

            for (int i = 0; i < tableDataList.Count; i++)
            {
                //Create and fulfill row in options table
                OptionsDataRow tempRow = new OptionsDataRow(i, uniqueValueIndex, tableDataList[i]);
                rowMap.Add(tempRow.GetUniqueValue(), tempRow);
                optionsDataTable.Rows.Add();
                FulfilOptionsDataTableRow(tempRow);


                //use the same data for chart view

                //implied volatility chart
                HistoryImplVolData();

                //call volatility chart
                buyCallVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetBuyVolCall());
                sellCallVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetSellVolCall());
                midCallVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetMidVolCall());

                //put volatility chart
                buyPutVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetBuyVolPut());
                sellPutVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetSellVolPut());
                midPutVolSeries.Points.AddXY(tempRow.GetUniqueValue(), tempRow.GetMidVolPut());
            }

            chrtCallVol.Series.Add(buyCallVolSeries);
            chrtCallVol.Series.Add(sellCallVolSeries);
            chrtCallVol.Series.Add(midCallVolSeries);
            chrtPutVol.Series.Add(buyPutVolSeries);
            chrtPutVol.Series.Add(sellPutVolSeries);
            chrtPutVol.Series.Add(midPutVolSeries);
        }

        private void HistoryImplVolData()
        {
            //throw new NotImplementedException();
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
                OptionsDataRow tempRow = this.rowMap[tempKey];
                tempRow.DataArr = updatedData;
                FulfilOptionsDataTableRow(tempRow);


                //charts add too!!!
            }
            else
            {
                throw new IllegalViewDataException("row with such a unique value does not exist in table: " + tempKey);
            }
        }

        private void FulfilOptionsDataTableRow(OptionsDataRow row)
        {
            for (int i = 0; i < row.GetDataArrLength(); i++)
            {
                optionsDataTable.Rows[row.RowNumber][i] = row.DataArr[i];
            }
        }


        private class OptionsDataRow
        {
            private static int BUY_VOLL_CALL_INDEX = 6;
            private static int SELL_VOLL_CALL_INDEX = 7;
            private static int BUY_VOLL_PUT_INDEX = 8;
            private static int SELL_VOLL_PUT_INDEX = 9;

            public OptionsDataRow(int rowNumber, int uniqueValueInDataArrIndex, double[] dataArr)
            {
                RowNumber = rowNumber;
                UniqueValueInDataArrIndex = uniqueValueInDataArrIndex;
                DataArr = dataArr;
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
                get; set;
            }

            public double GetUniqueValue()
            {
                return DataArr[UniqueValueInDataArrIndex];
            }

            public int GetDataArrLength()
            {
                return DataArr.Length;
            }

            public double GetBuyVolCall()
            {
                return DataArr[BUY_VOLL_CALL_INDEX];
            }

            public double GetSellVolCall()
            {
                return DataArr[SELL_VOLL_CALL_INDEX];
            }

            public double GetBuyVolPut()
            {
                return DataArr[BUY_VOLL_PUT_INDEX];
            }

            public double GetSellVolPut()
            {
                return DataArr[SELL_VOLL_PUT_INDEX];
            }

            public double GetMidVolCall()
            {
                return (GetBuyVolCall() + GetSellVolCall()) / 2;
            }

            public double GetMidVolPut()
            {
                return (GetBuyVolPut() + GetSellVolPut()) / 2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chrtImplVol.Series[0].Points.Clear();
            double[] d = new[] { 66, 48.11, 13.42, 112, 24, 10, 52, 25.3 };
            Random r = new Random();

            for (int i = 0; i < d.Length; i++)
            {
                chrtImplVol.Series[0].Points.AddXY(i * r.Next(1, 3), d[i] * r.Next(5, 10));
            }
        }
    }


}
