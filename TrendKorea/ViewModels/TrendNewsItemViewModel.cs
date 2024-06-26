﻿using ReactiveUI;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.InteropServices;

namespace TrendKorea.ViewModels
{
    public class TrendNewsItemViewModel : ViewModelBase
    {
        private string _title;
        public string Title { get { return _title; } set { this.RaiseAndSetIfChanged(ref _title, value); } }

        private string _url;
        public string Url { get { return _url; } set { this.RaiseAndSetIfChanged(ref _url, value); } }

        private string _description;
        public string Description { get { return _description; } set { this.RaiseAndSetIfChanged(ref _description, value); } }

        public ReactiveCommand<string, Unit> OpenUrlCmd { get; }

        public TrendNewsItemViewModel()
        {
            OpenUrlCmd = ReactiveCommand.Create<string>(openBrowser);
        }

        private void openBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
            }
        }
    }
}
