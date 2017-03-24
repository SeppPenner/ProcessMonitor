using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Languages.Implementation;
using Languages.Interfaces;
using ProcessMonitor.UiThreadInvoke;

namespace ProcessMonitor
{
    public partial class Main : Form
    {
        private readonly BackgroundWorker _backgroundworkerRefresh = new BackgroundWorker();
        private readonly ILanguageManager _lm = new LanguageManager();
        private Language _lang;
        private PerformanceCounter _pccpu = new PerformanceCounter();
        private PerformanceCounter _pcram = new PerformanceCounter();
        private Series _seriesCpu = new Series();
        private Series _seriesRam = new Series();

        public Main()
        {
            InitializeComponent();
            Initialize();
        }


        private void InitializeLanguageManager()
        {
            _lm.SetCurrentLanguage("de-DE");
            _lm.OnLanguageChanged += OnLanguageChanged;
        }

        private void LoadLanguagesToCombo()
        {
            foreach (var lang in _lm.GetLanguages())
                comboBoxLanguage.Items.Add(lang.Name);
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lm.SetCurrentLanguageFromName(comboBoxLanguage.SelectedItem.ToString());
        }

        private void OnLanguageChanged(object sender, EventArgs eventArgs)
        {
            _lang = _lm.GetCurrentLanguage();
            labelProcesses.Text = _lang.GetWord("Processes");
            labelDelay.Text = _lang.GetWord("Delay");
            checkBoxLogToCSV.Text = _lang.GetWord("LogToCSV");
            buttonEndMeasuring.Text = _lang.GetWord("EndMeasuring");
            buttonSaveRAMImage.Text = _lang.GetWord("SaveRAMUsage");
            buttonSaveCPUImage.Text = _lang.GetWord("SaveCPUUsage");
            chartRAM.Titles.Clear();
            chartRAM.Titles.Add(_lang.GetWord("UsedRAM"));
            chartCPU.Titles.Clear();
            chartCPU.Titles.Add(_lang.GetWord("UsedCPU"));
        }

        private void Initialize()
        {
            //Language
            InitializeLanguageManager();
            LoadLanguagesToCombo();
            //Backgroundworker
            _backgroundworkerRefresh.WorkerSupportsCancellation = true;
            _backgroundworkerRefresh.DoWork += Refresh;
            //Chart for RAM usage
            chartRAM.Titles.Clear();
            chartRAM.Titles.Add(_lang.GetWord("UsedRAM"));
            //Chart for CPU usage
            chartCPU.Titles.Clear();
            chartCPU.Titles.Add(_lang.GetWord("UsedCPU"));
            //Series for RAM usage
            _seriesRam = chartRAM.Series.Add(_lang.GetWord("RAMUtilisation"));
            _seriesRam.Color = Color.Blue;
            _seriesRam.ChartType = SeriesChartType.Line;
            _seriesRam.IsVisibleInLegend = false;
            //Series for CPU usage
            _seriesCpu = chartCPU.Series.Add(_lang.GetWord("CPUUtilisation"));
            _seriesCpu.Color = Color.Red;
            _seriesCpu.ChartType = SeriesChartType.Line;
            _seriesCpu.IsVisibleInLegend = false;
            //Button end measuring
            buttonEndMeasuring.Enabled = false;
            //Timer
            timerRefresh.Start();
        }

        private void Refresh(object sender, DoWorkEventArgs e)
        {
            try
            {
                var worker = sender as BackgroundWorker;
                //RAM Counter
                _pcram = new PerformanceCounter
                {
                    CategoryName = "Process",
                    CounterName = "Working Set - Private",
                    InstanceName = GetInstanceNameForProcessId((int) GetCurrentSelectedItem().Value)
                };
                //CPU Counter
                _pccpu = new PerformanceCounter
                {
                    CategoryName = "Process",
                    CounterName = "% Processor Time",
                    InstanceName = GetInstanceNameForProcessId((int) GetCurrentSelectedItem().Value)
                };

                if (checkBoxLogToCSV.Checked)
                {
                    var location = Assembly.GetExecutingAssembly().Location;
                    if (location == null)
                        throw new ArgumentNullException(nameof(location));
                    var logDir = Path.Combine(Directory.GetParent(location).FullName, "log");
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);
                    //TextWriter RAM
                    var ramFile = Path.Combine(logDir,
                        "Processlog_RAM_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv");
                    TextWriter tswRam = new StreamWriter(ramFile, true);
                    tswRam.WriteLine(_lang.GetWord("CurrentProcess") + " | " + _lang.GetWord("ProcessId") + ";" +
                                     _lang.GetWord("SizeInMB"));
                    tswRam.WriteLine(GetCurrentSelectedItem().Text + ";MB");
                    //TextWriter CPU
                    var cpuFile = Path.Combine(logDir,
                        "Processlog_CPU_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv");
                    TextWriter tswCpu = new StreamWriter(cpuFile, true);
                    tswCpu.WriteLine(_lang.GetWord("CurrentProcess") + " | " + _lang.GetWord("ProcessId") + ";%");
                    tswCpu.WriteLine(GetCurrentSelectedItem().Text + ";%");
                    while (worker != null && !worker.CancellationPending)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            tswRam.Close();
                            tswCpu.Close();
                            break;
                        }
                        //RAM Counter update
                        double ramSize = _pcram.NextValue() / 1048576;
                        AddPointXyToSeries(_seriesRam, 0, ramSize);
                        tswRam.WriteLine(DateTime.Now.ToString("dd.MM.yyyy-hh:mm:ss") + ";" + ramSize);
                        Thread.Sleep(GetDelay());
                        //CPU Counter update
                        var processorUsage = _pccpu.NextValue() / Environment.ProcessorCount;
                        AddPointXyToSeries(_seriesCpu, 0, processorUsage);
                        tswCpu.WriteLine(DateTime.Now.ToString("dd.MM.yyyy-hh:mm:ss") + ";" + processorUsage);
                        Thread.Sleep(GetDelay());
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
                        //RAM Counter update
                        AddPointXyToSeries(_seriesRam, 0, _pcram.NextValue() / 1048576);
                        Thread.Sleep(GetDelay());
                        //CPU Counter update
                        AddPointXyToSeries(_seriesCpu, 0, _pccpu.NextValue() / Environment.ProcessorCount);
                    }
                }
                _pcram.Close();
                _pcram.Dispose();
                _pccpu.Close();
                _pccpu.Dispose();
            }
            catch (Exception ex)
            {
                buttonEndMeasuring_Click(sender, e);
                MessageBox.Show(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private TimeSpan GetDelay()
        {
            var tempValue = 0;
            this.UiThreadInvoke(() => { tempValue = Convert.ToInt32(numericUpDownDelay.Value); });
            return TimeSpan.FromSeconds(Convert.ToDouble(tempValue) / 1000);
        }

        private void AddPointXyToSeries(Series series, object xValue, object yValue)
        {
            this.UiThreadInvoke(() => { series.Points.AddXY(xValue, yValue); });
        }

        private ComboboxItem GetCurrentSelectedItem()
        {
            var tempItem = new ComboboxItem();
            this.UiThreadInvoke(() => { tempItem = (ComboboxItem) comboBoxProcesses.SelectedItem; });
            return tempItem;
        }

        private void comboBoxProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            _seriesCpu.Points.Clear();
            _seriesRam.Points.Clear();
            _backgroundworkerRefresh.RunWorkerAsync();
            comboBoxProcesses.Enabled = false;
            checkBoxLogToCSV.Enabled = false;
            buttonEndMeasuring.Enabled = true;
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
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
                    if (!IsIdInItems(p.Id))
                        comboBoxProcesses.Items.Add(comboBoxItem);
                }
                for (var i = comboBoxProcesses.Items.Count - 1; i >= 0; i--)
                {
                    var item = (ComboboxItem) comboBoxProcesses.Items[i];
                    if (allProcesses.SingleOrDefault(x => x.Id == (int) item.Value) == null)
                        comboBoxProcesses.Items.RemoveAt(i);
                }
            }
            catch (
                Exception ex)
            {
                buttonEndMeasuring_Click(sender, e);
                MessageBox.Show(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsIdInItems(int compareId)
        {
            foreach (ComboboxItem item in comboBoxProcesses.Items)
                if ((int) item.Value == compareId)
                    return true;
            return false;
        }

        private void buttonSaveCPUImage_Click(object sender, EventArgs e)
        {
            _backgroundworkerRefresh.CancelAsync();
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"Jpeg|*.jpg|Png|*.png|Bitmap|*.bmp|Tiff|*.tif|Gif|*.gif",
                Title = _lang.GetWord("SaveImageAs")
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Equals("")) return;
            var fs = (FileStream) saveFileDialog.OpenFile();
            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    chartCPU.SaveImage(fs, ChartImageFormat.Jpeg);
                    break;

                case 2:
                    chartCPU.SaveImage(fs, ChartImageFormat.Png);
                    break;

                case 3:
                    chartCPU.SaveImage(fs, ChartImageFormat.Bmp);
                    break;
                case 4:
                    chartCPU.SaveImage(fs, ChartImageFormat.Tiff);
                    break;
                case 5:
                    chartCPU.SaveImage(fs, ChartImageFormat.Gif);
                    break;
            }
            fs.Close();
        }

        private void buttonSaveRAMImage_Click(object sender, EventArgs e)
        {
            _backgroundworkerRefresh.CancelAsync();
            var saveFileDialog = new SaveFileDialog
            {
                Filter = @"Jpeg|*.jpg|Png|*.png|Bitmap|*.bmp|Tiff|*.tif|Gif|*.gif",
                Title = _lang.GetWord("SaveImageAs")
            };
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName.Equals("")) return;
            var fs = (FileStream) saveFileDialog.OpenFile();
            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    chartRAM.SaveImage(fs, ChartImageFormat.Jpeg);
                    break;

                case 2:
                    chartRAM.SaveImage(fs, ChartImageFormat.Png);
                    break;

                case 3:
                    chartRAM.SaveImage(fs, ChartImageFormat.Bmp);
                    break;
                case 4:
                    chartRAM.SaveImage(fs, ChartImageFormat.Tiff);
                    break;
                case 5:
                    chartRAM.SaveImage(fs, ChartImageFormat.Gif);
                    break;
            }
            fs.Close();
        }

        private void buttonEndMeasuring_Click(object sender, EventArgs e)
        {
            this.UiThreadInvoke(() =>
            {
                _backgroundworkerRefresh.CancelAsync();
                comboBoxProcesses.Enabled = true;
                checkBoxLogToCSV.Enabled = true;
                buttonEndMeasuring.Enabled = false;
            });
        }

        private string GetInstanceNameForProcessId(int processId)
        {
            var process = Process.GetProcessById(processId);
            var processName = Path.GetFileNameWithoutExtension(process.ProcessName);

            var cat = new PerformanceCounterCategory("Process");
            var instances = cat.GetInstanceNames()
                .Where(inst => inst.StartsWith(processName))
                .ToArray();

            foreach (var instance in instances)
                using (var cnt = new PerformanceCounter("Process",
                    "ID Process", instance, true))
                {
                    var val = (int) cnt.RawValue;
                    if (val == processId)
                        return instance;
                }
            return null;
        }
    }
}