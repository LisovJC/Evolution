﻿using Evolution.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Model
{
    public class TaskModel : ObservableObject
    {
        public enum TypeTaskEdentity
        {
            local,
            global
        }

        private TypeTaskEdentity _typeTask = TypeTaskEdentity.local;

        public TypeTaskEdentity TypeTask
        {
            get => _typeTask;
            set => Set(ref _typeTask, value);
        }

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

        public enum Priority
        {
            P0,
            P1,
            P2,
            P3
        }

        private Priority _priority = Priority.P3;

        public Priority TaskPriority
        {
            get => _priority = Priority.P3;
            set => Set(ref _priority, value);
        }


        private string _assigned = "none";

        public string Assigned
        {
            get => _assigned;
            set => Set(ref _assigned, value);
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

        private List<Category> _categories;

        public List<Category> Categories
        {
            get => _categories;
            set => Set(ref _categories, value);
        }

        private DateTime _dateCreate;

        public DateTime DateCreate
        {
            get => _dateCreate;
            set => Set(ref _dateCreate, value);
        }

    }
}
