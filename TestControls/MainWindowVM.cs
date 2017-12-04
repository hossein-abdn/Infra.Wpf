using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class MainWindowVM : ViewModelBase
    {
        #region Properties

        public RelayCommand PersonListViewCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        #endregion

        #region Methods

        public MainWindowVM()
        {
            PersonListViewCommand = new RelayCommand(PersonListViewExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
        }

        private void LoadedEventExecute()
        {
            NavigationService = new NavigationService();
        }

        private void PersonListViewExecute()
        {
            NavigationService.NavigateTo(new PersonListVM());
        }

        #endregion
    }
}
