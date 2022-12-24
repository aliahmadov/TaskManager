using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Commands;
using TaskManager.Views;

namespace TaskManager.ViewModels
{
    public class MainViewModel:BaseViewModel
    {


		private ObservableCollection<Process> processes;

        public ObservableCollection<Process> Processes
		{
			get { return processes; }
			set { processes = value; }
		}


		private Process process;

		public Process SelectedProcess
		{
			get { return process; }
			set { process = value; }
		}

		public RelayCommand SelectedProcessCommand { get; set; }

		public MainViewModel()
		{
			Processes = new ObservableCollection<Process>(Process.GetProcesses());

			SelectedProcessCommand = new RelayCommand((a) =>
			{
				var view = new DetailedProcessInfo();
				var viewModel = new DetailedProcessViewModel();
				view.DataContext=viewModel;
				viewModel.Process = SelectedProcess;

				view.ShowDialog();
			});

            var p = Process.GetCurrentProcess();			
		
		}





	}
}
