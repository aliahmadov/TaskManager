using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.ViewModels
{
    public class DetailedProcessViewModel:BaseViewModel
    {
		private Process process;

		public Process Process
		{
			get { return process; }
			set { process = value; OnPropertyChanged(); }
		}

	}
}
