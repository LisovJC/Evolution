using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        #region SelectTaskProperties
        private string _title = "none";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        private string _description = "none";

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private string _deadLine;

        public string DeadLine
        {
            get => _deadLine;
            set => Set(ref _deadLine, value);
        }


        private string _assigned = "none";

        public string Assigned
        {
            get => _assigned;
            set => Set(ref _assigned, value);
        }

        private string _creator = "none";

        public string Creator
        {
            get => _creator;
            set => Set(ref _creator, value);
        }

        private double _plannedTimeCosts = 0.0;

        public double PlannedTimeCosts
        {
            get => _plannedTimeCosts;
            set => Set(ref _plannedTimeCosts, value);
        }

        private string _otherCategory = "";

        public string OtherCategory
        {
            get => _otherCategory;
            set => Set(ref _otherCategory, value);
        }

        private string _categories;

        public string Categories
        {
            get => _categories;
            set => Set(ref _categories, value);
        }

        private string _dateCreate;

        public string DateCreate
        {
            get => _dateCreate;
            set => Set(ref _dateCreate, value);
        }        
        #endregion



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
                SrartPage();
            });
        }

        private void SrartPage()
        {
            Categories = String.Empty;
            HelperService.SelectTask = GlobalTasks[SelectedIndex];
            Title = GlobalTasks[SelectedIndex].Title;
            Assigned = GlobalTasks[SelectedIndex].Assigned;
            DeadLine = GlobalTasks[SelectedIndex].DeadLine;
            Description = GlobalTasks[SelectedIndex].Description;
            DateCreate = GlobalTasks[SelectedIndex].DateCreate;
            if(GlobalTasks[SelectedIndex].Categories != null)
            {
                for (int i = 0; i < GlobalTasks[SelectedIndex].Categories.Count; i++)
                {
                    if (i != GlobalTasks[SelectedIndex].Categories.Count - 1)
                    {
                        Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName + ", ";
                    }
                    else
                    {
                        Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName;
                    }
                }
            }
            
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
