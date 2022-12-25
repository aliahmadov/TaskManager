using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TaskManager.Commands;
using TaskManager.Views;

namespace TaskManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {


        private ObservableCollection<Process> processes;

        public ObservableCollection<Process> Processes
        {
            get { return processes; }
            set { processes = value; OnPropertyChanged(); }
        }


        private Process process;

        public Process SelectedProcess
        {
            get { return process; }
            set { process = value; }
        }


        private int number;

        public int Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(); }
        }


        public RelayCommand SelectedProcessCommand { get; set; }

        private DispatcherTimer _dispatcherTimer;

        public RelayCommand CreateProcessCommand { get; set; }

        public RelayCommand EndProcessCommand { get; set; }


        private string enteredProcess;

        public string EnteredProcess
        {
            get { return enteredProcess; }
            set { enteredProcess = value; OnPropertyChanged(); }
        }

        private string searchText;

        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> blackProcessNames;

        public ObservableCollection<string> BlackProcessNames
        {
            get { return blackProcessNames; }
            set { blackProcessNames = value; OnPropertyChanged(); }
        }



        public RelayCommand TextChangedCommand { get; set; }


        public RelayCommand AddToBlackListCommand { get; set; }

        public RelayCommand ClearBlackListCommand { get; set; }

        private string blackProcess;

        public string BlackProcess
        {
            get { return blackProcess; }
            set { blackProcess = value; OnPropertyChanged(); }
        }


        public MainViewModel()
        {
            SearchText = "";
            EnteredProcess = "";
            BlackProcess = "";

            Processes = new ObservableCollection<Process>(Process.GetProcesses());
            BlackProcessNames = new ObservableCollection<string>();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispatcherTimer.Tick += TimerTick;
            _dispatcherTimer.Start();

            SelectedProcessCommand = new RelayCommand((a) =>
            {
                if (SelectedProcess != null)
                {

                    var view = new DetailedProcessInfo();
                    var viewModel = new DetailedProcessViewModel();
                    view.DataContext = viewModel;
                    viewModel.Process = SelectedProcess;
                    EnteredProcess = SelectedProcess.ProcessName;
                    view.ShowDialog();
                }
            });


            CreateProcessCommand = new RelayCommand(c =>
            {
                if (EnteredProcess.Count() != 0)
                {

                    var process = new Process();
                    process.StartInfo.FileName = $"{EnteredProcess}.exe";


                    if (BlackProcessNames.Any(p => p == EnteredProcess))
                    {
                        process.Start();
                        Thread.Sleep(2000);
                        process.Kill();

                    }
                    else
                        try
                        {
                            process.Start();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    EnteredProcess = "";

                }
            }, (d) =>
            {
                if (EnteredProcess.Count() != 0)
                {
                    return true;
                }

                return false;
            }
            );

            EndProcessCommand = new RelayCommand(c =>
            {
                if (EnteredProcess.Count() != 0)
                {
                    var process = Process.GetProcessesByName(EnteredProcess);

                    try
                    {
                        foreach (var item in process)
                        {
                            item.Kill();

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("System cannot end the specified process");
                    }
                    EnteredProcess = "";
                }
            }, (d =>
            {
                if (EnteredProcess.Count() != 0) return true;
                return false;
            }));


            TextChangedCommand = new RelayCommand(c =>
            {
                Processes = new ObservableCollection<Process>(Process.GetProcesses().Where(d => d.ProcessName.ToLower().StartsWith(SearchText)));

            });


            AddToBlackListCommand = new RelayCommand(c =>
            {

                BlackProcessNames.Add(BlackProcess);
                BlackProcess = "";

            }, (d) =>
            {
                if (BlackProcess.Count() != 0)
                {
                    return true;
                }
                return false;
            });

            ClearBlackListCommand = new RelayCommand(c =>
            {
                BlackProcessNames = new ObservableCollection<string>();
            });


        }

        private void TimerTick(object sender, EventArgs e)
        {
            Number++;
            if (SearchText.Count() == 0)
                Processes = new ObservableCollection<Process>(Process.GetProcesses());
            else
                Processes = new ObservableCollection<Process>(Process.GetProcesses().Where(d => d.ProcessName.ToLower().StartsWith(SearchText)));
        }
    }
}
