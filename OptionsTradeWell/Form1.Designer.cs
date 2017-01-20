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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.chrtPutVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrtCallVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.lblSpotPrice = new System.Windows.Forms.Label();
            this.chrtImplVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvOptionDesk = new System.Windows.Forms.DataGridView();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripPrBrConnection = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStLbAsset = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStLbDaysToExp = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStLbLastUpd = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtPutVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtCallVol)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtImplVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.chrtPutVol);
            this.tabSettings.Controls.Add(this.chrtCallVol);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1318, 679);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "ChartAnalyse";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // chrtPutVol
            // 
            this.chrtPutVol.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chrtPutVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtPutVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtPutVol.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chrtPutVol.ChartAreas.Add(chartArea1);
            this.chrtPutVol.Location = new System.Drawing.Point(665, 6);
            this.chrtPutVol.Name = "chrtPutVol";
            this.chrtPutVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
            this.chrtPutVol.Size = new System.Drawing.Size(647, 642);
            this.chrtPutVol.TabIndex = 6;
            this.chrtPutVol.Text = "chart3";
            // 
            // chrtCallVol
            // 
            this.chrtCallVol.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chrtCallVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtCallVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtCallVol.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.Name = "ChartArea1";
            this.chrtCallVol.ChartAreas.Add(chartArea2);
            this.chrtCallVol.Location = new System.Drawing.Point(12, 6);
            this.chrtCallVol.Name = "chrtCallVol";
            this.chrtCallVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            this.chrtCallVol.Size = new System.Drawing.Size(647, 642);
            this.chrtCallVol.TabIndex = 4;
            this.chrtCallVol.Text = "chart2";
            // 
            // tabMain
            // 
            this.tabMain.AutoScroll = true;
            this.tabMain.BackColor = System.Drawing.Color.Gainsboro;
            this.tabMain.Controls.Add(this.lblSpotPrice);
            this.tabMain.Controls.Add(this.chrtImplVol);
            this.tabMain.Controls.Add(this.statusStrip2);
            this.tabMain.Controls.Add(this.button4);
            this.tabMain.Controls.Add(this.button3);
            this.tabMain.Controls.Add(this.button2);
            this.tabMain.Controls.Add(this.button1);
            this.tabMain.Controls.Add(this.dgvOptionDesk);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(1318, 679);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "MainPage";
            // 
            // lblSpotPrice
            // 
            this.lblSpotPrice.BackColor = System.Drawing.Color.Gray;
            this.lblSpotPrice.Font = new System.Drawing.Font("Times New Roman", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpotPrice.ForeColor = System.Drawing.Color.Blue;
            this.lblSpotPrice.Location = new System.Drawing.Point(950, 138);
            this.lblSpotPrice.Name = "lblSpotPrice";
            this.lblSpotPrice.Size = new System.Drawing.Size(100, 42);
            this.lblSpotPrice.TabIndex = 11;
            this.lblSpotPrice.Text = "spotPrice";
            this.lblSpotPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chrtImplVol
            // 
            this.chrtImplVol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chrtImplVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtImplVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtImplVol.BorderlineWidth = 5;
            chartArea3.Name = "ChartArea1";
            this.chrtImplVol.ChartAreas.Add(chartArea3);
            this.chrtImplVol.Location = new System.Drawing.Point(34, 327);
            this.chrtImplVol.Name = "chrtImplVol";
            this.chrtImplVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            this.chrtImplVol.Series.Add(series1);
            this.chrtImplVol.Size = new System.Drawing.Size(910, 324);
            this.chrtImplVol.TabIndex = 10;
            this.chrtImplVol.Text = "chart1";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Location = new System.Drawing.Point(3, 654);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1312, 22);
            this.statusStrip2.TabIndex = 9;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // button4
            // 
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(950, 93);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(950, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(950, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(950, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvOptionDesk
            // 
            this.dgvOptionDesk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOptionDesk.Location = new System.Drawing.Point(34, 6);
            this.dgvOptionDesk.Name = "dgvOptionDesk";
            this.dgvOptionDesk.Size = new System.Drawing.Size(910, 315);
            this.dgvOptionDesk.TabIndex = 1;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabMain);
            this.tabs.Controls.Add(this.tabSettings);
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Location = new System.Drawing.Point(12, 12);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1326, 705);
            this.tabs.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1318, 679);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(93, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPrBrConnection,
            this.toolStripStLbAsset,
            this.toolStripStLbDaysToExp,
            this.toolStripStLbLastUpd});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1350, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripPrBrConnection
            // 
            this.toolStripPrBrConnection.Margin = new System.Windows.Forms.Padding(50, 3, 70, 2);
            this.toolStripPrBrConnection.Name = "toolStripPrBrConnection";
            this.toolStripPrBrConnection.Size = new System.Drawing.Size(100, 17);
            // 
            // toolStripStLbAsset
            // 
            this.toolStripStLbAsset.Margin = new System.Windows.Forms.Padding(0, 3, 70, 2);
            this.toolStripStLbAsset.Name = "toolStripStLbAsset";
            this.toolStripStLbAsset.Size = new System.Drawing.Size(118, 17);
            this.toolStripStLbAsset.Text = "toolStripStatusLabel1";
            // 
            // toolStripStLbDaysToExp
            // 
            this.toolStripStLbDaysToExp.Margin = new System.Windows.Forms.Padding(0, 3, 70, 2);
            this.toolStripStLbDaysToExp.Name = "toolStripStLbDaysToExp";
            this.toolStripStLbDaysToExp.Size = new System.Drawing.Size(118, 17);
            this.toolStripStLbDaysToExp.Text = "toolStripStatusLabel2";
            // 
            // toolStripStLbLastUpd
            // 
            this.toolStripStLbLastUpd.Margin = new System.Windows.Forms.Padding(0, 3, 70, 2);
            this.toolStripStLbLastUpd.Name = "toolStripStLbLastUpd";
            this.toolStripStLbLastUpd.Size = new System.Drawing.Size(118, 17);
            this.toolStripStLbLastUpd.Text = "toolStripStatusLabel1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabs);
            this.Name = "MainForm";
            this.Text = "OptionTradeWell";
            this.tabSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chrtPutVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtCallVol)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtImplVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvOptionDesk;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtPutVol;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtCallVol;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStLbAsset;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStLbDaysToExp;
        private System.Windows.Forms.ToolStripProgressBar toolStripPrBrConnection;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtImplVol;
        private System.Windows.Forms.Label lblSpotPrice;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStLbLastUpd;
    }
}

