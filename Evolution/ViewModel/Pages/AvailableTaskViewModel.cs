using Evolution.Core;
using Evolution.Model;
using Evolution.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.ViewModel.Pages
{
    internal class AvailableTaskViewModel
    {
        public ObservableCollection<TaskModel> tests { get; set; } = new();
        public AvailableTaskViewModel()
        {
            tests.Add(new TaskModel
            {
                Title = "Lisov",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "QWqwerqweqweqweqwe",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "weeeeF",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "eF",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "eF",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "eF",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "eF",
                Description = "GGG"
            });
            tests.Add(new TaskModel
            {
                Title = "eF",
                Description = "GGG"
            });            
        }
    }
}
