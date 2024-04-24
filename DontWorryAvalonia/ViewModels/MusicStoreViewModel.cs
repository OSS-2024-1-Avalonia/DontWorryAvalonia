﻿using Avalonia.Controls.Documents;
using Avalonia.Markup.Xaml.MarkupExtensions;
using DontWorryAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DontWorryAvalonia.ViewModels
{
    /// <summary>
    /// ViewModelBase Added
    /// This adds the important extension method RaiseAndSetIfChanged to your view model, 
    /// and will allow you to give the properties there the ability to notify changes to the view.
    /// </summary>
    public class MusicStoreViewModel : ViewModelBase
    {
        private string? _searchText;
        private bool _isBusy;

        public string? SearchText
        {
            get => _searchText;
            // in order to implement the notification.
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        // a collection of album view models to represent the albums that the search might find,
        // and a property to hold an album if the user selects one.

        private AlbumViewModel? _selectedAlbum;

        // this is a collection is capable of notification, and it is provided by the .NET framework.
        public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

        public AlbumViewModel? SelectedAlbum
        {
            get => _selectedAlbum;
            set => this.RaiseAndSetIfChanged(ref _selectedAlbum, value);
        }

        // you will add some mock data directly to the view model.
        public MusicStoreViewModel()
        {
            this.WhenAnyValue(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(DoSearch!);
        }

        private async void DoSearch(string? s)
        {
            IsBusy = true;
            SearchResults.Clear();

            if (!string.IsNullOrWhiteSpace(s))
            {
                var albums = await Album.SearchAsync(s);

                foreach (var album in albums)
                {
                    var vm = new AlbumViewModel(album);
                    SearchResults.Add(vm);
                }
            }

            IsBusy = false;
        }
    }
}
