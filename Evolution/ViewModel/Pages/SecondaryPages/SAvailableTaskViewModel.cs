using Evolution.Core;
using Evolution.Model;
using Evolution.Services.HelperServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;

namespace Evolution.ViewModel.Pages.SecondaryPages
{
    internal class SAvailableTaskViewModel : ObservableObject
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        public TaskModel SelectTask { get; set; } = new();
        public SAvailableTaskViewModel()
        {
            try
            {
                SelectTask = HelperService.SelectTask;
                Title = SelectTask.Title;
                Debug.WriteLine(Title);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
