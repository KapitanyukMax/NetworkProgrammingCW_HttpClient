using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PropertyChanged;

namespace NetworkProgrammingCW_HttpClient
{
    [AddINotifyPropertyChangedInterface]
    internal class MainViewModel
    {
        private WebClient client = new WebClient();

        public int Height { get; set; } = 0;

        public int Width { get; set; } = 0;

        public string Category { get; set; } = string.Empty;

        public string Url => $"https://source.unsplash.com/random/{Width}x{Height}/?{Category}&1";

        public string FileDestination { get; set; } = string.Empty;

        private ObservableCollection<string> historyList = new ObservableCollection<string>();

        public IEnumerable HistoryList => historyList;

        public double DownloadPercentage { get; set; } = 0.0;

        private RelayCommand? downLoadFileCommand;

        public ICommand DownloadFileCommand => downLoadFileCommand ??
            (downLoadFileCommand = new RelayCommand(o => DownloadFileAsync(),
                                                    o => !client.IsBusy));

        private RelayCommand? chooseFileDestinationCommand;

        public ICommand ChooseFileDestinationCommand => chooseFileDestinationCommand ??
            (chooseFileDestinationCommand = new RelayCommand(o => ChooseFileDestination()));

        public MainViewModel()
        {
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadPercentage = e.ProgressPercentage;
        }

        private void ChooseFileDestination()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPEG file|*.jpg";

            if (dialog.ShowDialog() == true)
                FileDestination = dialog.FileName;
        }

        private void AddHistoryItem()
        {
            historyList.Add($"{DateTime.Now.ToShortTimeString()}: {FileDestination}");
        }

        private void ShowImage()
        {
            ImageWindow imageWindow = new ImageWindow(FileDestination);
            imageWindow.Show();
        }

        private async void DownloadFileAsync()
        {
            if (Width == 0 || Height == 0)
            {
                MessageBox.Show("Invalid image size!");
                return;
            }
            if (string.IsNullOrWhiteSpace(Category))
            {
                MessageBox.Show("Invalid category!");
                return;
            }
            if (string.IsNullOrWhiteSpace(FileDestination))
            {
                MessageBox.Show("Choose file destination!");
                return;
            }

            await client.DownloadFileTaskAsync(Url, FileDestination);
            AddHistoryItem();
            ShowImage();
        }
    }
}