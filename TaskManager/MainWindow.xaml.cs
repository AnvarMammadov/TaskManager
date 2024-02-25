using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> processNames;
        private ObservableCollection<string> blackListedProcesses;
        private DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Tick += DispatcherTimer_Tick;

            processNames = new ObservableCollection<string>();
            blackListedProcesses = new ObservableCollection<string>();

            processListBox.ItemsSource = processNames;
            blackListListBox.ItemsSource = blackListedProcesses;

            dispatcherTimer.Start();

            UpdateProcesses();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateProcesses();
        }

        private void UpdateProcesses()
        {
            processNames.Clear();

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                processNames.Add(process.ProcessName);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string processName = tbProcessName.Text;

            try
            {
                Process.Start(processName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateProcesses();
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            string processName = tbProcessName.Text;

            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    processes[0].Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateProcesses();
        }

        private void processListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProcess = processListBox.SelectedItem as string;

            tbProcessName.Text = selectedProcess;
        }

        private void btnAddBlackBox_Click(object sender, RoutedEventArgs e)
        {

            string blackListProcess = tbBlackList.Text;

            try
            {
                if (blackListedProcesses.Any(p => p.Equals(blackListProcess)))
                {
                    MessageBox.Show("Bu süreç çalıştırılamaz!", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; 
                }

                blackListedProcesses.Add(blackListProcess);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UpdateProcesses();
        }
    }
}
