using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Business;
using DataAccess.Models;

namespace TestControls
{
    [ViewType(typeof(PersonCreateView))]
    public class PersonCreateVM : ViewModelBase<Person>
    {
        public bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        public PersonCreateVM(Person model = null)
        {
            SubmitCommand = new RelayCommand(SubmitExecute);

            if (model == null)
            {
                ViewTitle = "افزودن شخص";
                Model = new Person();
            }
            else
            {
                isEdit = true;
                ViewTitle = "ویرایش شخص";
                Model = model;
            }
        }

        private void SubmitExecute()
        {
            Model.UserId = 1;
            Model.CreateDate = DateTime.Now;
            Model.RecordStatusId = 1;

            using (var uow = new AccountingUow())
            {
                BusinessResult<bool> result = null;

                if (isEdit)
                    result = uow.PersonRepository.Update(Model);
                else
                    result = uow.PersonRepository.Add(Model);

                uow.SaveChange();
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
                NavigationService.GoBack();
            }
        }
    }
}