using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.HelperServices;
using Evolution.Services.TaskServices;
using Evolution.Services.UserServices;
using Evolution.ViewModel.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace Evolution.ViewModel.Pages
{
    internal class AvailableTaskViewModel:ObservableObject
    {
        public List<TaskModel> LoadedCommonTasks { get; set; } = new();
        public ObservableCollectionEX<TaskModel> GlobalTasks { get; set; } = new();

        private int _progress = 0;

        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }



        private Visibility _progressVisibility = Visibility.Visible;

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;
            set => Set(ref _progressVisibility, value);
        }

        public RelayCommand OpenFullInformationOfTask { get; set; }
        public AvailableTaskViewModel()
        {
            ProgressChange();
            LoadDatas();

            OpenFullInformationOfTask = new(o =>
            {
                HelperService.SelectTask = GlobalTasks[SelectedIndex];
                //MainViewModel.Instance.SelectSecondaryPage = new SAvailablePage();
            });
        }

        public async void LoadDatas()
        {
            if(HelperService.GlobalTasksInCash.Count == 0)
            {
                await Task.Run(() => LoadedCommonTasks = FireBaseService.GetDataFromDataBase<TaskModel>("GlobalTask/Tasks").Result);
                LoadCollection();
            }
            else
            {
                LoadCollection(true);
            }      
        }

        public void LoadCollection(bool isDownloaded = false)
        {
            if(isDownloaded)
            {
                for (int i = 0; i < HelperService.GlobalTasksInCash.Count; i++)
                {
                    GlobalTasks.Add(HelperService.GlobalTasksInCash[i]);
                }
            }

            for (int i = 0; i < LoadedCommonTasks.Count; i++)
            {
                GlobalTasks.Add(LoadedCommonTasks[i]);
            }
            HelperService.GlobalTasksInCash = GlobalTasks;
        }

        public async void ProgressChange()
        {
            bool isPositive = true;
            await Task.Run(() =>
            {
                Random rnd = new();
                while (true)
                {
                    if (GlobalTasks.Count > 0)
                    {
                        ProgressVisibility = Visibility.Hidden;
                        break;
                    }
                    Progress += 1;
                    if (Progress > 99)
                    {
                        Progress = 1;
                    }
                    Thread.Sleep(50);
                }
            });
        }
    }
}
