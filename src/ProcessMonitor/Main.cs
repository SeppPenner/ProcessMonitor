// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ProcessMonitor;

/// <summary>
/// The main form.
/// </summary>
public partial class Main : Form
{
    /// <summary>
    /// The refresh background worker.
    /// </summary>
    private readonly BackgroundWorker backgroundWorkerRefresh = new();

    /// <summary>
    /// The language manager.
    /// </summary>
    private readonly ILanguageManager languageManager = new LanguageManager();

    /// <summary>
    /// The language.
    /// </summary>
    private ILanguage? language;

    /// <summary>
    /// The performance counter for the CPU usage.
    /// </summary>
    private PerformanceCounter performanceCounterCpu = new();

    /// <summary>
    /// The performance counter for the RAM usage.
    /// </summary>
    private PerformanceCounter performanceCounterRam = new();

    /// <summary>
    /// The series for the CPU usage.
    /// </summary>
    private Series seriesCpu = new();

    /// <summary>
    /// The series for the RAM usage.
    /// </summary>
    private Series seriesRam = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
        this.InitializeComponent();
        this.Initialize();
    }

    /// <summary>
    /// Gets the instance name for a process identifier.
    /// </summary>
    /// <param name="processId">The process identifier.</param>
    /// <returns>The instance name as <see cref="string"/>.</returns>
    private static string GetInstanceNameForProcessId(int processId)
    {
        var process = Process.GetProcessById(processId);
        var processName = Path.GetFileNameWithoutExtension(process.ProcessName);

        var cat = new PerformanceCounterCategory("Process");
        var instances = cat.GetInstanceNames()
            .Where(inst => inst.StartsWith(processName))
            .ToArray();

        foreach (var instance in instances)
        {
            using var counter = new PerformanceCounter("Process", "ID Process", instance, true);
            var val = (int)counter.RawValue;
            if (val == processId)
            {
                return instance;
            }
        }

        return string.Empty;
    }

    /// <summary>
    /// Initializes the language manager.
    /// </summary>
    private void InitializeLanguageManager()
    {
        this.languageManager.SetCurrentLanguage("de-DE");
        this.languageManager.OnLanguageChanged += this.OnLanguageChanged!;
    }

    /// <summary>
    /// Loads the languages to the combo box.
    /// </summary>
    private void LoadLanguagesToCombo()
    {
        foreach (var lang in this.languageManager.GetLanguages())
        {
            this.comboBoxLanguage.Items.Add(lang.Name);
        }

        this.comboBoxLanguage.SelectedIndex = 0;
    }

    /// <summary>
    /// Handles the selected language changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ComboBoxLanguageSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedItem = this.comboBoxLanguage.SelectedItem.ToString();

        if (string.IsNullOrWhiteSpace(selectedItem))
        {
            return;
        }

        this.languageManager.SetCurrentLanguageFromName(selectedItem);
    }

    /// <summary>
    /// Handles the language changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void OnLanguageChanged(object sender, EventArgs e)
    {
        this.language = this.languageManager.GetCurrentLanguage();
        this.labelProcesses.Text = this.language.GetWord("Processes");
        this.labelDelay.Text = this.language.GetWord("Delay");
        this.checkBoxLogToCSV.Text = this.language.GetWord("LogToCSV");
        this.buttonEndMeasuring.Text = this.language.GetWord("EndMeasuring");
        this.buttonSaveRAMImage.Text = this.language.GetWord("SaveRAMUsage");
        this.buttonSaveCPUImage.Text = this.language.GetWord("SaveCPUUsage");
        this.chartRAM.Titles.Clear();
        this.chartRAM.Titles.Add(this.language.GetWord("UsedRAM"));
        this.chartCPU.Titles.Clear();
        this.chartCPU.Titles.Add(this.language.GetWord("UsedCPU"));
    }

    /// <summary>
    /// Initializes the data.
    /// </summary>
    private void Initialize()
    {
        // Language
        this.InitializeLanguageManager();
        this.LoadLanguagesToCombo();

        if (this.language is null)
        {
            return;
        }

        // Background worker
        this.backgroundWorkerRefresh.WorkerSupportsCancellation = true;
        this.backgroundWorkerRefresh.DoWork += this.Refresh!;

        // Chart for RAM usage
        this.chartRAM.Titles.Clear();
        this.chartRAM.Titles.Add(this.language.GetWord("UsedRAM"));

        // Chart for CPU usage
        this.chartCPU.Titles.Clear();
        this.chartCPU.Titles.Add(this.language.GetWord("UsedCPU"));

        // Series for RAM usage
        this.seriesRam = this.chartRAM.Series.Add(this.language.GetWord("RAMUtilization"));
        this.seriesRam.Color = Color.Blue;
        this.seriesRam.ChartType = SeriesChartType.Line;
        this.seriesRam.IsVisibleInLegend = false;

        // Series for CPU usage
        this.seriesCpu = this.chartCPU.Series.Add(this.language.GetWord("CPUUtilization"));
        this.seriesCpu.Color = Color.Red;
        this.seriesCpu.ChartType = SeriesChartType.Line;
        this.seriesCpu.IsVisibleInLegend = false;

        // Button end measuring
        this.buttonEndMeasuring.Enabled = false;

        // Timer
        this.timerRefresh.Start();
    }

    /// <summary>
    /// Does a refresh.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void Refresh(object sender, DoWorkEventArgs e)
    {
        try
        {
            var worker = sender as BackgroundWorker;

            // RAM Counter
            this.performanceCounterRam = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "Working Set - Private",
                InstanceName = GetInstanceNameForProcessId((int)this.GetCurrentSelectedItem().Value)
            };

            // CPU Counter
            this.performanceCounterCpu = new PerformanceCounter
            {
                CategoryName = "Process",
                CounterName = "% Processor Time",
                InstanceName = GetInstanceNameForProcessId((int)this.GetCurrentSelectedItem().Value)
            };

            if (this.language is null)
            {
                return;
            }

            if (this.checkBoxLogToCSV.Checked)
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var logDirectory = Path.Combine(Directory.GetParent(location)?.FullName ?? string.Empty, "log");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // TextWriter RAM
                var ramFile = Path.Combine(
                    logDirectory,
                    "Processlog_RAM_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv");
                TextWriter tswRam = new StreamWriter(ramFile, true);
                tswRam.WriteLine(
                    this.language.GetWord("CurrentProcess") + " | " + this.language.GetWord("ProcessId") + ";"
                    + this.language.GetWord("SizeInMB"));
                tswRam.WriteLine(this.GetCurrentSelectedItem().Text + ";MB");

                // TextWriter CPU
                var cpuFile = Path.Combine(
                    logDirectory,
                    "Processlog_CPU_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv");
                TextWriter tswCpu = new StreamWriter(cpuFile, true);
                tswCpu.WriteLine(
                    this.language.GetWord("CurrentProcess") + " | " + this.language.GetWord("ProcessId") + ";%");
                tswCpu.WriteLine(this.GetCurrentSelectedItem().Text + ";%");
                while (worker != null && !worker.CancellationPending)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        tswRam.Close();
                        tswCpu.Close();
                        break;
                    }

                    // RAM Counter update
                    double ramSize = this.performanceCounterRam.NextValue() / 1048576;
                    this.AddPointXyToSeries(this.seriesRam, 0, ramSize);
                    tswRam.WriteLine(DateTime.Now.ToString("dd.MM.yyyy-hh:mm:ss") + ";" + ramSize);
                    Thread.Sleep(this.GetDelay());

                    // CPU Counter update
                    var processorUsage = this.performanceCounterCpu.NextValue() / Environment.ProcessorCount;
                    this.AddPointXyToSeries(this.seriesCpu, 0, processorUsage);
                    tswCpu.WriteLine(DateTime.Now.ToString("dd.MM.yyyy-hh:mm:ss") + ";" + processorUsage);
                    Thread.Sleep(this.GetDelay());
                }

                tswRam.Close();
                tswCpu.Close();
            }
            else
            {
                while (worker != null && !worker.CancellationPending)
                {
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    // RAM Counter update
                    this.AddPointXyToSeries(this.seriesRam, 0, this.performanceCounterRam.NextValue() / 1048576);
                    Thread.Sleep(this.GetDelay());

                    // CPU Counter update
                    this.AddPointXyToSeries(
                        this.seriesCpu,
                        0,
                        this.performanceCounterCpu.NextValue() / Environment.ProcessorCount);
                }
            }

            this.performanceCounterRam.Close();
            this.performanceCounterRam.Dispose();
            this.performanceCounterCpu.Close();
            this.performanceCounterCpu.Dispose();
        }
        catch (Exception ex)
        {
            this.ButtonEndMeasuringClick(sender, e);
            MessageBox.Show(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Gets the delay.
    /// </summary>
    /// <returns>The delay as <see cref="TimeSpan"/>.</returns>
    private TimeSpan GetDelay()
    {
        var tempValue = 0;
        this.UiThreadInvoke(() => { tempValue = Convert.ToInt32(this.numericUpDownDelay.Value); });
        return TimeSpan.FromSeconds(Convert.ToDouble(tempValue) / 1000);
    }

    /// <summary>
    /// Adds a point to the series.
    /// </summary>
    /// <param name="series">The series.</param>
    /// <param name="xValue">The x value.</param>
    /// <param name="yValue">The y value.</param>
    private void AddPointXyToSeries(Series series, object xValue, object yValue)
    {
        this.UiThreadInvoke(() => { series.Points.AddXY(xValue, yValue); });
    }

    /// <summary>
    /// Gets the current selected item.
    /// </summary>
    /// <returns>The <see cref="ComboboxItem"/>.</returns>
    private ComboboxItem GetCurrentSelectedItem()
    {
        var tempItem = new ComboboxItem();
        this.UiThreadInvoke(() => { tempItem = (ComboboxItem)this.comboBoxProcesses.SelectedItem; });
        return tempItem;
    }

    /// <summary>
    /// Handles the process selected index changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ComboBoxProcessesSelectedIndexChanged(object sender, EventArgs e)
    {
        this.seriesCpu.Points.Clear();
        this.seriesRam.Points.Clear();
        this.backgroundWorkerRefresh.RunWorkerAsync();
        this.comboBoxProcesses.Enabled = false;
        this.checkBoxLogToCSV.Enabled = false;
        this.buttonEndMeasuring.Enabled = true;
    }

    /// <summary>
    /// Runs the refresh method on the timer.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void TimerRefreshTick(object sender, EventArgs e)
    {
        try
        {
            var allProcesses = Process.GetProcesses();
            foreach (var p in allProcesses)
            {
                var comboBoxItem = new ComboboxItem
                {
                    Text = p.ProcessName + " | " + Convert.ToString(p.Id),
                    Value = p.Id
                };

                if (!this.IsIdInItems(p.Id))
                {
                    this.comboBoxProcesses.Items.Add(comboBoxItem);
                }
            }

            for (var i = this.comboBoxProcesses.Items.Count - 1; i >= 0; i--)
            {
                var item = (ComboboxItem)this.comboBoxProcesses.Items[i];

                if (allProcesses.SingleOrDefault(x => x.Id == (int)item.Value) == null)
                {
                    this.comboBoxProcesses.Items.RemoveAt(i);
                }
            }
        }
        catch (
            Exception ex)
        {
            this.ButtonEndMeasuringClick(sender, e);
            MessageBox.Show(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Checks whether the identifier is in the list of identifiers or not.
    /// </summary>
    /// <param name="comparisonId">The comparison identifier.</param>
    /// <returns><c>true</c> if the identifier was found, <c>false</c> else.</returns>
    private bool IsIdInItems(int comparisonId)
    {
        return this.comboBoxProcesses.Items.Cast<ComboboxItem>().Any(item => (int)item.Value == comparisonId);
    }

    /// <summary>
    /// Handles the button click to save the CPU image.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ButtonSaveCpuImageClick(object sender, EventArgs e)
    {
        this.backgroundWorkerRefresh.CancelAsync();

        if (this.language is null)
        {
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = @"Jpeg|*.jpg|Png|*.png|Bitmap|*.bmp|Tiff|*.tif|Gif|*.gif",
            Title = this.language.GetWord("SaveImageAs")
        };

        saveFileDialog.ShowDialog();

        if (saveFileDialog.FileName.Equals(string.Empty))
        {
            return;
        }

        var fs = (FileStream)saveFileDialog.OpenFile();

        switch (saveFileDialog.FilterIndex)
        {
            case 1:
                this.chartCPU.SaveImage(fs, ChartImageFormat.Jpeg);
                break;

            case 2:
                this.chartCPU.SaveImage(fs, ChartImageFormat.Png);
                break;

            case 3:
                this.chartCPU.SaveImage(fs, ChartImageFormat.Bmp);
                break;
            case 4:
                this.chartCPU.SaveImage(fs, ChartImageFormat.Tiff);
                break;
            case 5:
                this.chartCPU.SaveImage(fs, ChartImageFormat.Gif);
                break;
        }

        fs.Close();
    }

    /// <summary>
    /// Handles the button click to save the RAM image.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ButtonSaveRamImageClick(object sender, EventArgs e)
    {
        this.backgroundWorkerRefresh.CancelAsync();

        if (this.language is null)
        {
            return;
        }

        var saveFileDialog = new SaveFileDialog
        {
            Filter = @"Jpeg|*.jpg|Png|*.png|Bitmap|*.bmp|Tiff|*.tif|Gif|*.gif",
            Title = this.language.GetWord("SaveImageAs")
        };

        saveFileDialog.ShowDialog();

        if (saveFileDialog.FileName.Equals(string.Empty))
        {
            return;
        }

        var fs = (FileStream)saveFileDialog.OpenFile();

        switch (saveFileDialog.FilterIndex)
        {
            case 1:
                this.chartRAM.SaveImage(fs, ChartImageFormat.Jpeg);
                break;

            case 2:
                this.chartRAM.SaveImage(fs, ChartImageFormat.Png);
                break;

            case 3:
                this.chartRAM.SaveImage(fs, ChartImageFormat.Bmp);
                break;
            case 4:
                this.chartRAM.SaveImage(fs, ChartImageFormat.Tiff);
                break;
            case 5:
                this.chartRAM.SaveImage(fs, ChartImageFormat.Gif);
                break;
        }

        fs.Close();
    }

    /// <summary>
    /// Handles the button click to end the measuring.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void ButtonEndMeasuringClick(object sender, EventArgs e)
    {
        this.UiThreadInvoke(() =>
        {
            this.backgroundWorkerRefresh.CancelAsync();
            this.comboBoxProcesses.Enabled = true;
            this.checkBoxLogToCSV.Enabled = true;
            this.buttonEndMeasuring.Enabled = false;
        });
    }
}
