namespace ProcessMonitor
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.comboBoxProcesses = new System.Windows.Forms.ComboBox();
            this.labelProcesses = new System.Windows.Forms.Label();
            this.chartRAM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartCPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.labelDelay = new System.Windows.Forms.Label();
            this.numericUpDownDelay = new System.Windows.Forms.NumericUpDown();
            this.buttonSaveCPUImage = new System.Windows.Forms.Button();
            this.buttonSaveRAMImage = new System.Windows.Forms.Button();
            this.buttonEndMeasuring = new System.Windows.Forms.Button();
            this.checkBoxLogToCSV = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMainSmall = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartRAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelMainSmall.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxProcesses
            // 
            this.comboBoxProcesses.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxProcesses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProcesses.FormattingEnabled = true;
            this.comboBoxProcesses.Location = new System.Drawing.Point(3, 38);
            this.comboBoxProcesses.Name = "comboBoxProcesses";
            this.comboBoxProcesses.Size = new System.Drawing.Size(275, 21);
            this.comboBoxProcesses.Sorted = true;
            this.comboBoxProcesses.TabIndex = 0;
            this.comboBoxProcesses.SelectedIndexChanged += new System.EventHandler(this.ComboBoxProcessesSelectedIndexChanged);
            // 
            // labelProcesses
            // 
            this.labelProcesses.AutoSize = true;
            this.labelProcesses.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelProcesses.Location = new System.Drawing.Point(3, 0);
            this.labelProcesses.Name = "labelProcesses";
            this.labelProcesses.Size = new System.Drawing.Size(86, 35);
            this.labelProcesses.TabIndex = 1;
            this.labelProcesses.Text = "Prozessauswahl:";
            // 
            // chartRAM
            // 
            this.chartRAM.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.chartRAM.ChartAreas.Add(chartArea1);
            this.chartRAM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartRAM.Location = new System.Drawing.Point(3, 73);
            this.chartRAM.Name = "chartRAM";
            this.chartRAM.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            this.chartRAM.Size = new System.Drawing.Size(1126, 168);
            this.chartRAM.TabIndex = 2;
            // 
            // chartCPU
            // 
            this.chartCPU.BorderlineColor = System.Drawing.Color.Black;
            chartArea2.Name = "ChartArea1";
            this.chartCPU.ChartAreas.Add(chartArea2);
            this.chartCPU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCPU.Location = new System.Drawing.Point(3, 247);
            this.chartCPU.Name = "chartCPU";
            this.chartCPU.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            this.chartCPU.Size = new System.Drawing.Size(1126, 168);
            this.chartCPU.TabIndex = 3;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 10;
            this.timerRefresh.Tick += new System.EventHandler(this.TimerRefreshTick);
            // 
            // labelDelay
            // 
            this.labelDelay.AutoSize = true;
            this.labelDelay.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelDelay.Location = new System.Drawing.Point(284, 0);
            this.labelDelay.Name = "labelDelay";
            this.labelDelay.Size = new System.Drawing.Size(103, 35);
            this.labelDelay.TabIndex = 4;
            this.labelDelay.Text = "Verzögerung (in ms):";
            // 
            // numericUpDownDelay
            // 
            this.numericUpDownDelay.Dock = System.Windows.Forms.DockStyle.Top;
            this.numericUpDownDelay.Location = new System.Drawing.Point(284, 38);
            this.numericUpDownDelay.Maximum = new decimal(new int[] {
            24000000,
            0,
            0,
            0});
            this.numericUpDownDelay.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownDelay.Name = "numericUpDownDelay";
            this.numericUpDownDelay.Size = new System.Drawing.Size(117, 20);
            this.numericUpDownDelay.TabIndex = 5;
            this.numericUpDownDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // buttonSaveCPUImage
            // 
            this.buttonSaveCPUImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSaveCPUImage.Location = new System.Drawing.Point(901, 38);
            this.buttonSaveCPUImage.Name = "buttonSaveCPUImage";
            this.buttonSaveCPUImage.Size = new System.Drawing.Size(222, 23);
            this.buttonSaveCPUImage.TabIndex = 6;
            this.buttonSaveCPUImage.Text = "CPU-Nutzung als Bild speichern";
            this.buttonSaveCPUImage.UseVisualStyleBackColor = true;
            this.buttonSaveCPUImage.Click += new System.EventHandler(this.ButtonSaveCpuImageClick);
            // 
            // buttonSaveRAMImage
            // 
            this.buttonSaveRAMImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSaveRAMImage.Location = new System.Drawing.Point(676, 38);
            this.buttonSaveRAMImage.Name = "buttonSaveRAMImage";
            this.buttonSaveRAMImage.Size = new System.Drawing.Size(219, 23);
            this.buttonSaveRAMImage.TabIndex = 7;
            this.buttonSaveRAMImage.Text = "RAM-Nutzung als Bild speichern";
            this.buttonSaveRAMImage.UseVisualStyleBackColor = true;
            this.buttonSaveRAMImage.Click += new System.EventHandler(this.ButtonSaveRamImageClick);
            // 
            // buttonEndMeasuring
            // 
            this.buttonEndMeasuring.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEndMeasuring.Location = new System.Drawing.Point(508, 38);
            this.buttonEndMeasuring.Name = "buttonEndMeasuring";
            this.buttonEndMeasuring.Size = new System.Drawing.Size(162, 23);
            this.buttonEndMeasuring.TabIndex = 8;
            this.buttonEndMeasuring.Text = "Messung beenden";
            this.buttonEndMeasuring.UseVisualStyleBackColor = true;
            this.buttonEndMeasuring.Click += new System.EventHandler(this.ButtonEndMeasuringClick);
            // 
            // checkBoxLogToCSV
            // 
            this.checkBoxLogToCSV.AutoSize = true;
            this.checkBoxLogToCSV.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxLogToCSV.Location = new System.Drawing.Point(407, 38);
            this.checkBoxLogToCSV.Name = "checkBoxLogToCSV";
            this.checkBoxLogToCSV.Size = new System.Drawing.Size(95, 17);
            this.checkBoxLogToCSV.TabIndex = 9;
            this.checkBoxLogToCSV.Text = "In CSV loggen";
            this.checkBoxLogToCSV.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.chartRAM, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.chartCPU, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelMainSmall, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1132, 418);
            this.tableLayoutPanelMain.TabIndex = 10;
            // 
            // tableLayoutPanelMainSmall
            // 
            this.tableLayoutPanelMainSmall.ColumnCount = 6;
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9F));
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMainSmall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMainSmall.Controls.Add(this.comboBoxProcesses, 0, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.buttonSaveCPUImage, 5, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.buttonSaveRAMImage, 4, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.buttonEndMeasuring, 3, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.checkBoxLogToCSV, 2, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.labelProcesses, 0, 0);
            this.tableLayoutPanelMainSmall.Controls.Add(this.numericUpDownDelay, 1, 1);
            this.tableLayoutPanelMainSmall.Controls.Add(this.labelDelay, 1, 0);
            this.tableLayoutPanelMainSmall.Controls.Add(this.comboBoxLanguage, 5, 0);
            this.tableLayoutPanelMainSmall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMainSmall.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelMainSmall.Name = "tableLayoutPanelMainSmall";
            this.tableLayoutPanelMainSmall.RowCount = 2;
            this.tableLayoutPanelMainSmall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMainSmall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelMainSmall.Size = new System.Drawing.Size(1126, 64);
            this.tableLayoutPanelMainSmall.TabIndex = 4;
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(901, 3);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(222, 21);
            this.comboBoxLanguage.TabIndex = 10;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.ComboBoxLanguageSelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 418);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1148, 457);
            this.Name = "Main";
            this.Text = "Process Monitor";
            ((System.ComponentModel.ISupportInitialize)(this.chartRAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDelay)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMainSmall.ResumeLayout(false);
            this.tableLayoutPanelMainSmall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxProcesses;
        private System.Windows.Forms.Label labelProcesses;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRAM;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCPU;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Label labelDelay;
        private System.Windows.Forms.NumericUpDown numericUpDownDelay;
        private System.Windows.Forms.Button buttonSaveCPUImage;
        private System.Windows.Forms.Button buttonSaveRAMImage;
        private System.Windows.Forms.Button buttonEndMeasuring;
        private System.Windows.Forms.CheckBox checkBoxLogToCSV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMainSmall;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
    }
}

