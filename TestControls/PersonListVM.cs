using Infra.Wpf.Mvvm;
using System.Collections.ObjectModel;
using System;
using DataAccess.Models;

namespace TestControls
{
    [ViewType(typeof(PersonListView))]
    public class PersonListVM : ViewModelBase<Person>
    {
        public RelayCommand<string> GetAllCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand CreateCommand { get; set; }

        public RelayCommand<Person> EditCommand { get; set; }

        public PersonListVM()
        {
            ViewTitle = "لیست اشخاص";
            GetAllCommand = new RelayCommand<string>(GetAllExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            CreateCommand = new RelayCommand(CreateExecute);
            EditCommand = new RelayCommand<Person>(EditExecute);
        }

        private void EditExecute(Person obj)
        {
            NavigationService.NavigateTo(new PersonCreateVM(obj));
        }

        private void CreateExecute()
        {
            NavigationService.NavigateTo(new PersonCreateVM(null));
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(string.Empty);
        }

        private void GetAllExecute(string predicate)
        {
            using (var uow = new AccountingUow())
            {
                var result = uow.PersonRepository.GetAll(predicate: predicate);

                if (result.Exception == null)
                    ItemsSource = new ObservableCollection<Person>(result.Data);
                else
                    Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            }
        }
    }
}
