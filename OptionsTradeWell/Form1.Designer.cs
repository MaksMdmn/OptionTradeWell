namespace OptionsTradeWell
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.txBxDaysToExp = new System.Windows.Forms.TextBox();
            this.txBxAssetName = new System.Windows.Forms.TextBox();
            this.prBrIsConnected = new System.Windows.Forms.ProgressBar();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chrtImplVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvOptionDesk = new System.Windows.Forms.DataGridView();
            this.chrtPutVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrtCallVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabs = new System.Windows.Forms.TabControl();
            this.BidCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Strike = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BidPut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AskPut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyVolCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellVolCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyVolPut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellVolPut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtImplVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtPutVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtCallVol)).BeginInit();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.dataGridView2);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1231, 701);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(21, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(370, 229);
            this.dataGridView2.TabIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.AutoScroll = true;
            this.tabMain.BackColor = System.Drawing.Color.Gainsboro;
            this.tabMain.Controls.Add(this.txBxDaysToExp);
            this.tabMain.Controls.Add(this.txBxAssetName);
            this.tabMain.Controls.Add(this.prBrIsConnected);
            this.tabMain.Controls.Add(this.button4);
            this.tabMain.Controls.Add(this.button3);
            this.tabMain.Controls.Add(this.button2);
            this.tabMain.Controls.Add(this.button1);
            this.tabMain.Controls.Add(this.chrtImplVol);
            this.tabMain.Controls.Add(this.dgvOptionDesk);
            this.tabMain.Controls.Add(this.chrtPutVol);
            this.tabMain.Controls.Add(this.chrtCallVol);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(1231, 701);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "MainPage";
            // 
            // txBxDaysToExp
            // 
            this.txBxDaysToExp.Location = new System.Drawing.Point(112, 10);
            this.txBxDaysToExp.Name = "txBxDaysToExp";
            this.txBxDaysToExp.ReadOnly = true;
            this.txBxDaysToExp.Size = new System.Drawing.Size(100, 20);
            this.txBxDaysToExp.TabIndex = 11;
            this.txBxDaysToExp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txBxAssetName
            // 
            this.txBxAssetName.Location = new System.Drawing.Point(6, 10);
            this.txBxAssetName.Name = "txBxAssetName";
            this.txBxAssetName.ReadOnly = true;
            this.txBxAssetName.Size = new System.Drawing.Size(100, 20);
            this.txBxAssetName.TabIndex = 10;
            this.txBxAssetName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // prBrIsConnected
            // 
            this.prBrIsConnected.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.prBrIsConnected.Location = new System.Drawing.Point(449, 10);
            this.prBrIsConnected.Name = "prBrIsConnected";
            this.prBrIsConnected.Size = new System.Drawing.Size(67, 23);
            this.prBrIsConnected.TabIndex = 9;
            // 
            // button4
            // 
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(522, 123);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(522, 94);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(522, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(522, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // chrtImplVol
            // 
            this.chrtImplVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chrtImplVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtImplVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtImplVol.BorderlineWidth = 5;
            chartArea1.Name = "ChartArea1";
            this.chrtImplVol.ChartAreas.Add(chartArea1);
            this.chrtImplVol.Location = new System.Drawing.Point(6, 506);
            this.chrtImplVol.Name = "chrtImplVol";
            this.chrtImplVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            this.chrtImplVol.Series.Add(series1);
            this.chrtImplVol.Size = new System.Drawing.Size(510, 189);
            this.chrtImplVol.TabIndex = 2;
            this.chrtImplVol.Text = "chart1";
            // 
            // dgvOptionDesk
            // 
            this.dgvOptionDesk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptionDesk.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BidCall,
            this.AskCall,
            this.Strike,
            this.BidPut,
            this.AskPut,
            this.BuyVolCall,
            this.SellVolCall,
            this.BuyVolPut,
            this.SellVolPut});
            this.dgvOptionDesk.Location = new System.Drawing.Point(6, 36);
            this.dgvOptionDesk.Name = "dgvOptionDesk";
            this.dgvOptionDesk.Size = new System.Drawing.Size(510, 449);
            this.dgvOptionDesk.TabIndex = 1;
            // 
            // chrtPutVol
            // 
            this.chrtPutVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chrtPutVol.BackColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chrtPutVol.ChartAreas.Add(chartArea2);
            this.chrtPutVol.Location = new System.Drawing.Point(603, 364);
            this.chrtPutVol.Name = "chrtPutVol";
            this.chrtPutVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Name = "Series1";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Name = "Series2";
            this.chrtPutVol.Series.Add(series2);
            this.chrtPutVol.Series.Add(series3);
            this.chrtPutVol.Size = new System.Drawing.Size(596, 331);
            this.chrtPutVol.TabIndex = 4;
            this.chrtPutVol.Text = "chart3";
            // 
            // chrtCallVol
            // 
            this.chrtCallVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chrtCallVol.BackColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.chrtCallVol.ChartAreas.Add(chartArea3);
            this.chrtCallVol.Location = new System.Drawing.Point(603, 10);
            this.chrtCallVol.Name = "chrtCallVol";
            this.chrtCallVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Name = "Series1";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Name = "Series2";
            this.chrtCallVol.Series.Add(series4);
            this.chrtCallVol.Series.Add(series5);
            this.chrtCallVol.Size = new System.Drawing.Size(596, 348);
            this.chrtCallVol.TabIndex = 3;
            this.chrtCallVol.Text = "chart2";
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabMain);
            this.tabs.Controls.Add(this.tabSettings);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1239, 727);
            this.tabs.TabIndex = 5;
            // 
            // BidCall
            // 
            this.BidCall.FillWeight = 50F;
            this.BidCall.HeaderText = "Column1";
            this.BidCall.Name = "BidCall";
            this.BidCall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BidCall.Width = 50;
            // 
            // AskCall
            // 
            this.AskCall.FillWeight = 50F;
            this.AskCall.HeaderText = "Column1";
            this.AskCall.Name = "AskCall";
            this.AskCall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AskCall.Width = 50;
            // 
            // Strike
            // 
            this.Strike.FillWeight = 50F;
            this.Strike.HeaderText = "Column1";
            this.Strike.Name = "Strike";
            this.Strike.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Strike.Width = 50;
            // 
            // BidPut
            // 
            this.BidPut.FillWeight = 50F;
            this.BidPut.HeaderText = "Column1";
            this.BidPut.Name = "BidPut";
            this.BidPut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BidPut.Width = 50;
            // 
            // AskPut
            // 
            this.AskPut.FillWeight = 50F;
            this.AskPut.HeaderText = "Column1";
            this.AskPut.Name = "AskPut";
            this.AskPut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.AskPut.Width = 50;
            // 
            // BuyVolCall
            // 
            this.BuyVolCall.FillWeight = 50F;
            this.BuyVolCall.HeaderText = "Column1";
            this.BuyVolCall.Name = "BuyVolCall";
            this.BuyVolCall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BuyVolCall.Width = 50;
            // 
            // SellVolCall
            // 
            this.SellVolCall.FillWeight = 50F;
            this.SellVolCall.HeaderText = "Column2";
            this.SellVolCall.Name = "SellVolCall";
            this.SellVolCall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SellVolCall.Width = 50;
            // 
            // BuyVolPut
            // 
            this.BuyVolPut.FillWeight = 50F;
            this.BuyVolPut.HeaderText = "Column3";
            this.BuyVolPut.Name = "BuyVolPut";
            this.BuyVolPut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BuyVolPut.Width = 50;
            // 
            // SellVolPut
            // 
            this.SellVolPut.FillWeight = 50F;
            this.SellVolPut.HeaderText = "Column4";
            this.SellVolPut.Name = "SellVolPut";
            this.SellVolPut.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SellVolPut.Width = 50;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 751);
            this.Controls.Add(this.tabs);
            this.Name = "MainForm";
            this.Text = "OptionTradeWell";
            this.tabSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtImplVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtPutVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtCallVol)).EndInit();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.ProgressBar prBrIsConnected;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtImplVol;
        private System.Windows.Forms.DataGridView dgvOptionDesk;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtPutVol;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtCallVol;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TextBox txBxDaysToExp;
        private System.Windows.Forms.TextBox txBxAssetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn Strike;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidPut;
        private System.Windows.Forms.DataGridViewTextBoxColumn AskPut;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyVolCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellVolCall;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyVolPut;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellVolPut;
    }
}

