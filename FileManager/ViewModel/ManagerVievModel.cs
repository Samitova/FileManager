using FileManager.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileManager.ViewModel
{
    internal class ManagerVievModel: ViewModelBase
    {        
        private PaneViewModel _currentPaneViewModel;

        public PaneViewModel LeftPaneViewModel { get; set; }
        public PaneViewModel RightPaneViewModel { get; set; }
        public PaneViewModel CurrentPaneViewModel
        {
            get { return _currentPaneViewModel; }
            set
            {
                _currentPaneViewModel = value;
                OnPropertyChanged("CurrentPaneViewModel");
            }
        }


        private readonly BackgroundWorker worker;
        
        public ICommand Details { get; set; }
        public ICommand CopyCommand { get; set; }
        private int currentProgress = 0;


        /// <summary>
        /// ctor
        /// </summary>
        public ManagerVievModel()
        {
            LeftPaneViewModel = new PaneViewModel();
            RightPaneViewModel = new PaneViewModel();
           
            CurrentPaneViewModel = LeftPaneViewModel;

            Details = new RelayCommand(GetDetails);
            CopyCommand = new AsyncRelayCommand(Copy, ProgChanged);
        }

        internal void GetDetails()
        {
            CurrentPaneViewModel.GetDetails();
        }

        internal void Copy(Action<int> progress)
        {
            CurrentPaneViewModel.Copy(progress);
        }

        public void ProgChanged(object sender, ProgressChangedEventArgs e)
        {
            currentProgress += e.ProgressPercentage;
        }

    }
}
