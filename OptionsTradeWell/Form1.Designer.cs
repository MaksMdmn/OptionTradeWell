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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.chrtPutVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrtCallVol = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.txBxInfo = new System.Windows.Forms.TextBox();
            this.chrtPos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnUpdatePos = new System.Windows.Forms.Button();
            this.txBxTickersPos = new System.Windows.Forms.TextBox();
            this.dgvPosParts = new System.Windows.Forms.DataGridView();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblDaysToExp = new System.Windows.Forms.Label();
            this.lblSpotPrice = new System.Windows.Forms.Label();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.btnStart = new System.Windows.Forms.Button();
            this.dgvOptionDesk = new System.Windows.Forms.DataGridView();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txBxStrikesNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSetDefaultSettings = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.txBxStepYValue = new System.Windows.Forms.TextBox();
            this.txBxMaxYValue = new System.Windows.Forms.TextBox();
            this.txBxMinYValue = new System.Windows.Forms.TextBox();
            this.txBxCentralStrChangeTimeSec = new System.Windows.Forms.TextBox();
            this.txBxMaxVolValue = new System.Windows.Forms.TextBox();
            this.txBxRounding = new System.Windows.Forms.TextBox();
            this.txBxDaysInYear = new System.Windows.Forms.TextBox();
            this.txBxNumberOfTrackOpt = new System.Windows.Forms.TextBox();
            this.txBxUniqueIndx = new System.Windows.Forms.TextBox();
            this.txBxOptTableName = new System.Windows.Forms.TextBox();
            this.txBxServName = new System.Windows.Forms.TextBox();
            this.txBxFutTableName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripPrBrConnection = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStLbAsset = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStLbLastUpd = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtPutVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrtCallVol)).BeginInit();
            this.tabMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrtPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPosParts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.chrtPutVol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chrtPutVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtPutVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtPutVol.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.chrtPutVol.ChartAreas.Add(chartArea1);
            this.chrtPutVol.Location = new System.Drawing.Point(665, 6);
            this.chrtPutVol.MaximumSize = new System.Drawing.Size(647, 642);
            this.chrtPutVol.MinimumSize = new System.Drawing.Size(647, 642);
            this.chrtPutVol.Name = "chrtPutVol";
            this.chrtPutVol.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
            this.chrtPutVol.Size = new System.Drawing.Size(647, 642);
            this.chrtPutVol.TabIndex = 6;
            this.chrtPutVol.Text = "chart3";
            // 
            // chrtCallVol
            // 
            this.chrtCallVol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chrtCallVol.BackColor = System.Drawing.Color.Transparent;
            this.chrtCallVol.BorderlineColor = System.Drawing.Color.Black;
            this.chrtCallVol.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.Name = "ChartArea1";
            this.chrtCallVol.ChartAreas.Add(chartArea2);
            this.chrtCallVol.Location = new System.Drawing.Point(12, 6);
            this.chrtCallVol.MaximumSize = new System.Drawing.Size(647, 642);
            this.chrtCallVol.MinimumSize = new System.Drawing.Size(647, 642);
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
            this.tabMain.Controls.Add(this.txBxInfo);
            this.tabMain.Controls.Add(this.chrtPos);
            this.tabMain.Controls.Add(this.btnUpdatePos);
            this.tabMain.Controls.Add(this.txBxTickersPos);
            this.tabMain.Controls.Add(this.dgvPosParts);
            this.tabMain.Controls.Add(this.label21);
            this.tabMain.Controls.Add(this.label20);
            this.tabMain.Controls.Add(this.lblDaysToExp);
            this.tabMain.Controls.Add(this.lblSpotPrice);
            this.tabMain.Controls.Add(this.statusStrip2);
            this.tabMain.Controls.Add(this.btnStart);
            this.tabMain.Controls.Add(this.dgvOptionDesk);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(1318, 679);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "MainPage";
            // 
            // txBxInfo
            // 
            this.txBxInfo.Location = new System.Drawing.Point(950, 175);
            this.txBxInfo.Multiline = true;
            this.txBxInfo.Name = "txBxInfo";
            this.txBxInfo.Size = new System.Drawing.Size(362, 146);
            this.txBxInfo.TabIndex = 19;
            // 
            // chrtPos
            // 
            chartArea3.Name = "ChartArea1";
            this.chrtPos.ChartAreas.Add(chartArea3);
            this.chrtPos.Location = new System.Drawing.Point(754, 336);
            this.chrtPos.Name = "chrtPos";
            this.chrtPos.Size = new System.Drawing.Size(558, 315);
            this.chrtPos.TabIndex = 18;
            this.chrtPos.Text = "chart1";
            // 
            // btnUpdatePos
            // 
            this.btnUpdatePos.Location = new System.Drawing.Point(677, 336);
            this.btnUpdatePos.Name = "btnUpdatePos";
            this.btnUpdatePos.Size = new System.Drawing.Size(71, 42);
            this.btnUpdatePos.TabIndex = 17;
            this.btnUpdatePos.Text = "button1";
            this.btnUpdatePos.UseVisualStyleBackColor = true;
            this.btnUpdatePos.Click += new System.EventHandler(this.btnUpdatePos_Click);
            // 
            // txBxTickersPos
            // 
            this.txBxTickersPos.Location = new System.Drawing.Point(677, 384);
            this.txBxTickersPos.Multiline = true;
            this.txBxTickersPos.Name = "txBxTickersPos";
            this.txBxTickersPos.Size = new System.Drawing.Size(71, 267);
            this.txBxTickersPos.TabIndex = 16;
            // 
            // dgvPosParts
            // 
            this.dgvPosParts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPosParts.Location = new System.Drawing.Point(34, 336);
            this.dgvPosParts.Name = "dgvPosParts";
            this.dgvPosParts.Size = new System.Drawing.Size(637, 315);
            this.dgvPosParts.TabIndex = 15;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label21.Location = new System.Drawing.Point(1056, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(100, 20);
            this.label21.TabIndex = 14;
            this.label21.Text = "days left:";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label20.Location = new System.Drawing.Point(950, 10);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 20);
            this.label20.TabIndex = 13;
            this.label20.Text = "spot price:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDaysToExp
            // 
            this.lblDaysToExp.BackColor = System.Drawing.Color.Gray;
            this.lblDaysToExp.Font = new System.Drawing.Font("Times New Roman", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDaysToExp.ForeColor = System.Drawing.Color.SkyBlue;
            this.lblDaysToExp.Location = new System.Drawing.Point(1056, 30);
            this.lblDaysToExp.Name = "lblDaysToExp";
            this.lblDaysToExp.Size = new System.Drawing.Size(100, 42);
            this.lblDaysToExp.TabIndex = 12;
            this.lblDaysToExp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpotPrice
            // 
            this.lblSpotPrice.BackColor = System.Drawing.Color.Gray;
            this.lblSpotPrice.Font = new System.Drawing.Font("Times New Roman", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpotPrice.ForeColor = System.Drawing.Color.SkyBlue;
            this.lblSpotPrice.Location = new System.Drawing.Point(950, 30);
            this.lblSpotPrice.Name = "lblSpotPrice";
            this.lblSpotPrice.Size = new System.Drawing.Size(100, 42);
            this.lblSpotPrice.TabIndex = 11;
            this.lblSpotPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStrip2
            // 
            this.statusStrip2.Location = new System.Drawing.Point(3, 654);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(1312, 22);
            this.statusStrip2.TabIndex = 9;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnStart.FlatAppearance.BorderSize = 5;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStart.Location = new System.Drawing.Point(1186, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(126, 66);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Let\'s do it, buddy.";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
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
            this.tabPage1.Controls.Add(this.txBxStrikesNumber);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.btnSetDefaultSettings);
            this.tabPage1.Controls.Add(this.btnSaveSettings);
            this.tabPage1.Controls.Add(this.txBxStepYValue);
            this.tabPage1.Controls.Add(this.txBxMaxYValue);
            this.tabPage1.Controls.Add(this.txBxMinYValue);
            this.tabPage1.Controls.Add(this.txBxCentralStrChangeTimeSec);
            this.tabPage1.Controls.Add(this.txBxMaxVolValue);
            this.tabPage1.Controls.Add(this.txBxRounding);
            this.tabPage1.Controls.Add(this.txBxDaysInYear);
            this.tabPage1.Controls.Add(this.txBxNumberOfTrackOpt);
            this.tabPage1.Controls.Add(this.txBxUniqueIndx);
            this.tabPage1.Controls.Add(this.txBxOptTableName);
            this.tabPage1.Controls.Add(this.txBxServName);
            this.tabPage1.Controls.Add(this.txBxFutTableName);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.label17);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1318, 679);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txBxStrikesNumber
            // 
            this.txBxStrikesNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxStrikesNumber.Location = new System.Drawing.Point(162, 163);
            this.txBxStrikesNumber.Name = "txBxStrikesNumber";
            this.txBxStrikesNumber.Size = new System.Drawing.Size(100, 20);
            this.txBxStrikesNumber.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 15);
            this.label7.TabIndex = 35;
            this.label7.Text = "OptionDeskStrikesNumber";
            // 
            // btnSetDefaultSettings
            // 
            this.btnSetDefaultSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSetDefaultSettings.Location = new System.Drawing.Point(781, 155);
            this.btnSetDefaultSettings.Name = "btnSetDefaultSettings";
            this.btnSetDefaultSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSetDefaultSettings.TabIndex = 34;
            this.btnSetDefaultSettings.Text = "Set Default";
            this.btnSetDefaultSettings.UseVisualStyleBackColor = true;
            this.btnSetDefaultSettings.Click += new System.EventHandler(this.btnSetDefaultSettings_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveSettings.Location = new System.Drawing.Point(862, 155);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 33;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // txBxStepYValue
            // 
            this.txBxStepYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxStepYValue.Location = new System.Drawing.Point(831, 124);
            this.txBxStepYValue.Name = "txBxStepYValue";
            this.txBxStepYValue.Size = new System.Drawing.Size(100, 20);
            this.txBxStepYValue.TabIndex = 32;
            // 
            // txBxMaxYValue
            // 
            this.txBxMaxYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxMaxYValue.Location = new System.Drawing.Point(831, 99);
            this.txBxMaxYValue.Name = "txBxMaxYValue";
            this.txBxMaxYValue.Size = new System.Drawing.Size(100, 20);
            this.txBxMaxYValue.TabIndex = 31;
            // 
            // txBxMinYValue
            // 
            this.txBxMinYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxMinYValue.Location = new System.Drawing.Point(831, 68);
            this.txBxMinYValue.Name = "txBxMinYValue";
            this.txBxMinYValue.Size = new System.Drawing.Size(100, 20);
            this.txBxMinYValue.TabIndex = 30;
            // 
            // txBxCentralStrChangeTimeSec
            // 
            this.txBxCentralStrChangeTimeSec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxCentralStrChangeTimeSec.Location = new System.Drawing.Point(831, 39);
            this.txBxCentralStrChangeTimeSec.Name = "txBxCentralStrChangeTimeSec";
            this.txBxCentralStrChangeTimeSec.Size = new System.Drawing.Size(100, 20);
            this.txBxCentralStrChangeTimeSec.TabIndex = 28;
            // 
            // txBxMaxVolValue
            // 
            this.txBxMaxVolValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxMaxVolValue.Location = new System.Drawing.Point(493, 129);
            this.txBxMaxVolValue.Name = "txBxMaxVolValue";
            this.txBxMaxVolValue.Size = new System.Drawing.Size(100, 20);
            this.txBxMaxVolValue.TabIndex = 27;
            // 
            // txBxRounding
            // 
            this.txBxRounding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxRounding.Location = new System.Drawing.Point(493, 98);
            this.txBxRounding.Name = "txBxRounding";
            this.txBxRounding.Size = new System.Drawing.Size(100, 20);
            this.txBxRounding.TabIndex = 26;
            // 
            // txBxDaysInYear
            // 
            this.txBxDaysInYear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxDaysInYear.Location = new System.Drawing.Point(493, 68);
            this.txBxDaysInYear.Name = "txBxDaysInYear";
            this.txBxDaysInYear.Size = new System.Drawing.Size(100, 20);
            this.txBxDaysInYear.TabIndex = 25;
            // 
            // txBxNumberOfTrackOpt
            // 
            this.txBxNumberOfTrackOpt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxNumberOfTrackOpt.Location = new System.Drawing.Point(493, 41);
            this.txBxNumberOfTrackOpt.Name = "txBxNumberOfTrackOpt";
            this.txBxNumberOfTrackOpt.Size = new System.Drawing.Size(100, 20);
            this.txBxNumberOfTrackOpt.TabIndex = 24;
            // 
            // txBxUniqueIndx
            // 
            this.txBxUniqueIndx.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxUniqueIndx.Location = new System.Drawing.Point(162, 129);
            this.txBxUniqueIndx.Name = "txBxUniqueIndx";
            this.txBxUniqueIndx.Size = new System.Drawing.Size(100, 20);
            this.txBxUniqueIndx.TabIndex = 21;
            // 
            // txBxOptTableName
            // 
            this.txBxOptTableName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxOptTableName.Location = new System.Drawing.Point(162, 98);
            this.txBxOptTableName.Name = "txBxOptTableName";
            this.txBxOptTableName.Size = new System.Drawing.Size(100, 20);
            this.txBxOptTableName.TabIndex = 20;
            // 
            // txBxServName
            // 
            this.txBxServName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxServName.Location = new System.Drawing.Point(162, 44);
            this.txBxServName.Name = "txBxServName";
            this.txBxServName.Size = new System.Drawing.Size(100, 20);
            this.txBxServName.TabIndex = 19;
            // 
            // txBxFutTableName
            // 
            this.txBxFutTableName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txBxFutTableName.Location = new System.Drawing.Point(162, 71);
            this.txBxFutTableName.Name = "txBxFutTableName";
            this.txBxFutTableName.Size = new System.Drawing.Size(100, 20);
            this.txBxFutTableName.TabIndex = 18;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(675, 68);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(150, 15);
            this.label18.TabIndex = 17;
            this.label18.Text = "GeneralChartMinYValue";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(675, 99);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(150, 15);
            this.label17.TabIndex = 16;
            this.label17.Text = "GeneralChartMaxYValue";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(675, 129);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(150, 15);
            this.label16.TabIndex = 15;
            this.label16.Text = "GeneralChartStepYValue";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(42, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(150, 15);
            this.label15.TabIndex = 14;
            this.label15.Text = "QUIK CONNECTION";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(337, 98);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(150, 15);
            this.label13.TabIndex = 12;
            this.label13.Text = "RoundingInOptionsCalcs";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(337, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(150, 15);
            this.label12.TabIndex = 11;
            this.label12.Text = "MaxValueOfVolatility";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(337, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 15);
            this.label11.TabIndex = 10;
            this.label11.Text = "DaysInYear";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(368, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 15);
            this.label10.TabIndex = 9;
            this.label10.Text = "OPTIONS FEATURES";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(675, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "CentralStrikeChange(min sec.)";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(706, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "CHARTS FEATURES";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "UniqueIndexInDdeDataArray";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(337, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "NumberOfTrackingOptions";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "OptionsTableName";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "FuturesTableName";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ServerName";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPrBrConnection,
            this.toolStripStLbAsset,
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
            // toolStripStLbLastUpd
            // 
            this.toolStripStLbLastUpd.Margin = new System.Windows.Forms.Padding(0, 3, 70, 2);
            this.toolStripStLbLastUpd.Name = "toolStripStLbLastUpd";
            this.toolStripStLbLastUpd.Size = new System.Drawing.Size(118, 17);
            this.toolStripStLbLastUpd.Text = "toolStripStatusLabel1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
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
            ((System.ComponentModel.ISupportInitialize)(this.chrtPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPosParts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOptionDesk)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridView dgvOptionDesk;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtPutVol;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtCallVol;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStLbAsset;
        private System.Windows.Forms.ToolStripProgressBar toolStripPrBrConnection;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.Label lblSpotPrice;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStLbLastUpd;
        private System.Windows.Forms.TextBox txBxStepYValue;
        private System.Windows.Forms.TextBox txBxMaxYValue;
        private System.Windows.Forms.TextBox txBxMinYValue;
        private System.Windows.Forms.TextBox txBxCentralStrChangeTimeSec;
        private System.Windows.Forms.TextBox txBxMaxVolValue;
        private System.Windows.Forms.TextBox txBxRounding;
        private System.Windows.Forms.TextBox txBxDaysInYear;
        private System.Windows.Forms.TextBox txBxNumberOfTrackOpt;
        private System.Windows.Forms.TextBox txBxUniqueIndx;
        private System.Windows.Forms.TextBox txBxOptTableName;
        private System.Windows.Forms.TextBox txBxServName;
        private System.Windows.Forms.TextBox txBxFutTableName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetDefaultSettings;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblDaysToExp;
        private System.Windows.Forms.TextBox txBxStrikesNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txBxInfo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrtPos;
        private System.Windows.Forms.Button btnUpdatePos;
        private System.Windows.Forms.TextBox txBxTickersPos;
        private System.Windows.Forms.DataGridView dgvPosParts;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

