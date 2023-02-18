using Evolution.Core;
using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static Evolution.Enums.Enums;

namespace Evolution.ViewModel.Pages
{
    internal class MyTasksViewModel : ObservableObject
    {

        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }

        #region Collections
        public List<TaskModel> LoadedCommonTasks { get; set; } = new();
        public ObservableCollectionEX<TaskModel> GlobalTasks { get; set; } = new();
        #endregion
        public MyTasksViewModel()
        {
            LoadDatas();
        }

        private void SrartPage()
        {
            while (true)
            {
                if ((HelperService.index != SelectedIndex))
                {
                    HelperService.index = SelectedIndex;
                    //Categories = String.Empty;
                    break;
                }

                if (SelectedIndex == -1)
                {
                    break;
                }
            }

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
                HelperService.index = 0;
            }
            HelperService.SelectTask = GlobalTasks[SelectedIndex];
            //Title = GlobalTasks[SelectedIndex].Title;
            //Assigned = GlobalTasks[SelectedIndex].Assigned;
            //DeadLine = GlobalTasks[SelectedIndex].DeadLine;
            //Description = GlobalTasks[SelectedIndex].Description;
            //DateCreate = GlobalTasks[SelectedIndex].DateCreate;

            //if (GlobalTasks[SelectedIndex].Categories != null)
            //{
            //    for (int i = 0; i < GlobalTasks[SelectedIndex].Categories.Count; i++)
            //    {
            //        if (i != GlobalTasks[SelectedIndex].Categories.Count - 1)
            //        {
            //            Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName + ", ";
            //        }
            //        else
            //        {
            //            Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName;
            //        }
            //    }
            //}

        }

        public async void LoadDatas()
        {
            if (HelperService.GlobalTasksInCash.Count == 0)
            {
                await Task.Run(() => LoadedCommonTasks = FireBaseService.GetDataFromDataBase<TaskModel>(TypeDatas.GlobalTasks, HelperService.CountTasksOfGlobalTaskList).Result);
                LoadCollection();
            }
            else
            {
                LoadCollection(true);
            }
        }

        public void LoadCollection(bool isDownloaded = false)
        {
            try
            {
                if (isDownloaded)
                {
                    GlobalTasks.Clear();
                    for (int i = 0; i < HelperService.GlobalTasksInCash.Count; i++)
                    {
                        GlobalTasks.Add(HelperService.GlobalTasksInCash[i]);
                    }
                    SrartPage();
                    return;
                }

                for (int i = 0; i < LoadedCommonTasks.Count; i++)
                {
                    GlobalTasks.Add(LoadedCommonTasks[i]);
                }
                SrartPage();
                HelperService.GlobalTasksInCash = GlobalTasks;
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
