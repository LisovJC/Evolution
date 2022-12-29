using Evolution.Core;
using Evolution.Model;
using Evolution.Services;
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
        public ObservableCollection<TaskModel> LoadedCommonTasks { get; set; } = new();
        public ObservableCollectionEX<TaskModel> CommonTasks { get; set; } = new();
        public AvailableTaskViewModel()
        {
            LoadDatas();
        }

        public async void LoadDatas()
        {
            await Task.Run(() => LoadedCommonTasks = TaskService.GetCommonTasks().Result);
            LoadCollection();
        }

        public void LoadCollection()
        {
            for (int i = 0; i < LoadedCommonTasks.Count; i++)
            {
                CommonTasks.Add(LoadedCommonTasks[i]);
            }
        }
    }
}
