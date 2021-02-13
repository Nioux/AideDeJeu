﻿using AideDeJeu.Repositories;
using AideDeJeu.ViewModels.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AideDeJeu.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string Version
        {
            get
            {
                return AppInfo.VersionString; // DependencyService.Get<INativeAPI>().GetVersion();
            }
        }
        public string AppName
        {
            get
            {
                return AppInfo.Name; // DependencyService.Get<INativeAPI>().GetVersion();
            }
        }

        public static OSAppTheme OSAppTheme
        {
            get
            {
                return Application.Current.Properties.ContainsKey("OSAppTheme") ? (OSAppTheme)(int)Application.Current.Properties["OSAppTheme"] : OSAppTheme.Unspecified;
            }
            set
            {
                Application.Current.Properties["OSAppTheme"] = (int)value;
                App.Current.UserAppTheme = BaseViewModel.OSAppTheme;
            }
        }

        public MainViewModel Main
        {
            get
            {
                return DependencyService.Get<MainViewModel>();
            }
        }
        public BookmarksRepository Bookmarks
        {
            get
            {
                return DependencyService.Get<BookmarksRepository>();
            }
        }
        public StoreViewModel Store
        {
            get
            {
                return DependencyService.Get<StoreViewModel>();
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
