using OptionsTradeWell.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OptionsTradeWell
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupLayouts();
        }

        private void SetupLayouts()
        {
            SetupOptionDeskLayout();
            SetupImplVolChartLayout();
            SetupCallVolChartLayout();
            SetupPutVolChartLayout();
            FulfilByTestData();
        }

        private void SetupOptionDeskLayout()
        {
            dgvOptionDesk.ColumnCount = 9;
            dgvOptionDesk.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgvOptionDesk.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvOptionDesk.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvOptionDesk.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvOptionDesk.RowsDefaultCellStyle.SelectionBackColor = System.Drawing.Color.MediumOrchid;
            dgvOptionDesk.RowsDefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dgvOptionDesk.ScrollBars = ScrollBars.Vertical;
            dgvOptionDesk.Height = 20 * dgvOptionDesk.Rows[0].Height;
            dgvOptionDesk.Width = 9 * 50 + 50 + 10;

            dgvOptionDesk.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
            dgvOptionDesk.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            dgvOptionDesk.Columns[0].HeaderText = "Bid call";
            dgvOptionDesk.Columns[1].HeaderText = "Ask call";
            dgvOptionDesk.Columns[2].HeaderText = "STRIKE";
            dgvOptionDesk.Columns[3].HeaderText = "Bid put";
            dgvOptionDesk.Columns[4].HeaderText = "Ask put";
            dgvOptionDesk.Columns[5].HeaderText = "BuyVol call";
            dgvOptionDesk.Columns[6].HeaderText = "SellVol call";
            dgvOptionDesk.Columns[7].HeaderText = "BuyVol put";
            dgvOptionDesk.Columns[8].HeaderText = "SellVol put";

            dgvOptionDesk.Columns[0].Width = 50;
            dgvOptionDesk.Columns[1].Width = 50;
            dgvOptionDesk.Columns[2].Width = 50;
            dgvOptionDesk.Columns[3].Width = 50;
            dgvOptionDesk.Columns[4].Width = 50;
            dgvOptionDesk.Columns[5].Width = 50;
            dgvOptionDesk.Columns[6].Width = 50;
            dgvOptionDesk.Columns[7].Width = 50;
            dgvOptionDesk.Columns[8].Width = 50;

            dgvOptionDesk.Columns[0].DefaultCellStyle.BackColor = Color.Chartreuse;
            dgvOptionDesk.Columns[1].DefaultCellStyle.BackColor = Color.IndianRed;
            dgvOptionDesk.Columns[2].DefaultCellStyle.BackColor = Color.LightSlateGray;
            dgvOptionDesk.Columns[3].DefaultCellStyle.BackColor = Color.Chartreuse;
            dgvOptionDesk.Columns[4].DefaultCellStyle.BackColor = Color.IndianRed;
            dgvOptionDesk.Columns[5].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[6].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[7].DefaultCellStyle.BackColor = Color.MediumOrchid;
            dgvOptionDesk.Columns[8].DefaultCellStyle.BackColor = Color.MediumOrchid;

            dgvOptionDesk.Columns[0].DisplayIndex = 2;
            dgvOptionDesk.Columns[1].DisplayIndex = 3;
            dgvOptionDesk.Columns[2].DisplayIndex = 4;
            dgvOptionDesk.Columns[3].DisplayIndex = 5;
            dgvOptionDesk.Columns[4].DisplayIndex = 6;
            dgvOptionDesk.Columns[5].DisplayIndex = 0;
            dgvOptionDesk.Columns[6].DisplayIndex = 1;
            dgvOptionDesk.Columns[7].DisplayIndex = 7;
            dgvOptionDesk.Columns[8].DisplayIndex = 8;



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
            string[] optDeskData = { "33.22", "33.22", "33.22", "33.22", "33.22", "33.22", "33.22", "33.22", "33.22", };
            for (int i = 0; i < 40; i++)
            {
                dgvOptionDesk.Rows.Add(optDeskData);
            }



            double[] chartData1 = { 32.12, 43.11, 3.42, 12, 54, 110, 85.2, 25.3 };
            double[] chartData2 = { 2.12, 48.11, 13.42, 18, 24, 10, 5.2, 25.3 };

            Random rnd = new Random();
            for (int i = 0; i < chartData1.Length; i++)
            {
                chrtImplVol.Series[0].Points.AddXY(i, chartData1[i]);

                chrtCallVol.Series[0].Points.AddXY(i, chartData1[i]);
                chrtCallVol.Series[1].Points.AddXY(i, chartData1[i] * rnd.Next(0, 5));

                chrtPutVol.Series[0].Points.AddXY(i, chartData1[i]);
                chrtPutVol.Series[1].Points.AddXY(i, chartData1[i] * rnd.Next(0, 5));

            }


            txBxAssetName.Text = "BR-02.17";
            txBxDaysToExp.Text = "9";

            prBrIsConnected.Value = 100;

        }


    }
}
