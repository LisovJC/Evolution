﻿using Evolution.Command;
using Evolution.Core;
using Evolution.Services;
using Evolution.Services.HelperServices;
using Evolution.Services.UserServices;
using Evolution.View.Pages;
using Evolution.View.Pages.SecondaryPages;
using Evolution.ViewModel.Pages;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Evolution.ViewModel.Windows
{
    internal class MainViewModel : ObservableObject
    {
        enum AppPages
        {
            Home,
            MyTasks,
            AvailableTasks,
            TasksInWork,
            Create,
            Storage
        }

        private Visibility _isHomeSelect = Visibility.Hidden;

        public Visibility isHomeSelect
        {
            get => _isHomeSelect;
            set => Set(ref _isHomeSelect, value);
        }

        private Visibility _isStorageSelect = Visibility.Hidden;

        public Visibility isStorageSelect
        {
            get => _isStorageSelect;
            set => Set(ref _isStorageSelect, value);
        }

        private Visibility _isAvailableTSelect = Visibility.Hidden;

        public Visibility isAvailableTSelect
        {
            get => _isAvailableTSelect;
            set => Set(ref _isAvailableTSelect, value);
        }

        private Visibility _isMyTSelect = Visibility.Hidden;

        public Visibility isMyTSelect
        {
            get => _isMyTSelect;
            set => Set(ref _isMyTSelect, value);
        }

        private Visibility _isTInWorkSelect = Visibility.Hidden;

        public Visibility isTInWorkSelect
        {
            get => _isTInWorkSelect;
            set => Set(ref _isTInWorkSelect, value);
        }

        private Visibility _isCreateSelect = Visibility.Hidden;

        public Visibility isCreateSelect
        {
            get => _isCreateSelect;
            set => Set(ref _isCreateSelect, value);
        }
        

        private Page _selectMainPage;
        public Page SelectMainPage
        {
            get => _selectMainPage;
            set => Set(ref _selectMainPage, value);
        }

        private Page _selectSecondaryPage;
        public Page SelectSecondaryPage
        {
            get => _selectSecondaryPage;
            set => Set(ref _selectSecondaryPage, value);
        }

        private string _currentUser;
        public string CurrentUser
        {
            get => _currentUser;
            set => Set(ref _currentUser, value);
        }

        public static MainViewModel Instance { get; set; } = new();


        public RelayCommand SelectHomeCommand { get; set; }
        public RelayCommand SelectAvailableTCommand { get; set; }
        public RelayCommand SelectMyTCommand { get; set; }
        public RelayCommand SelectTInWorkCommand { get; set; }
        public RelayCommand SelectCreateCommand { get; set; }
        public RelayCommand SelectStorageCommand { get; set; }
        public RelayCommand CloseAppCommand { get; set; }

        public static readonly string selectButtonColor = "#ff3c38";
        public static readonly string unSelectButtonColor = "#fefefe";
        public MainViewModel()
        {
            CurrentUser = HelperService.CurrentUser;

            GoSelectPage(AppPages.Home);

            SelectHomeCommand = new(o =>{GoSelectPage(AppPages.Home);});

            SelectAvailableTCommand = new(o => { GoSelectPage(AppPages.AvailableTasks); });

            SelectMyTCommand = new(o => { GoSelectPage(AppPages.MyTasks); });

            SelectTInWorkCommand = new(o => { GoSelectPage(AppPages.TasksInWork); });

            SelectCreateCommand = new(o => { GoSelectPage(AppPages.Create); });           

            SelectStorageCommand = new(o => { GoSelectPage(AppPages.Storage); });

            CloseAppCommand = new(o => { Application.Current.Shutdown(); });
        }

        private void GoSelectPage(AppPages page)
        {
           
            if(page == AppPages.Home)
            {
                isHomeSelect = Visibility.Visible;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isTInWorkSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                SelectMainPage = new HomePage();
                SelectSecondaryPage = new SecondaryHomePage();
            }

            if (page == AppPages.AvailableTasks)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Visible;
                isMyTSelect = Visibility.Hidden;
                isTInWorkSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                SelectMainPage = new AvailableTaskPage();
                SelectSecondaryPage = new SAvailablePage();
            }

            if (page == AppPages.MyTasks)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Visible;
                isTInWorkSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                SelectMainPage = new MyTasksPage();
                SelectSecondaryPage = new SecondaryHomePage();
            }

            if (page == AppPages.TasksInWork) //TODO: Убрать!
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isTInWorkSelect = Visibility.Visible;
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                SelectMainPage = new TasksInWorkPage();
                SelectSecondaryPage = new SecondaryHomePage();
            }

            if (page == AppPages.Create)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isTInWorkSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Visible;                
                isStorageSelect = Visibility.Hidden;
                SelectMainPage = new CreatePage();
                SelectSecondaryPage = new SecondaryHomePage();
            }                     

            if (page == AppPages.Storage)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isTInWorkSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;                
                isStorageSelect = Visibility.Visible;
                SelectMainPage = new StoragePage();
                SelectSecondaryPage = new SecondaryHomePage();
            }
        }
    }
}
