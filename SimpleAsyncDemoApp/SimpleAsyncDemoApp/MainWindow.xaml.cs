using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace SimpleAsyncDemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void executeSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var results = DemoMethods.RunDownloadSync();
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: { elapsedMs }";
        }

        private async void executeAsync_Click(object sender, RoutedEventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgressChanged;

            var watch = Stopwatch.StartNew();

            try
            {
                var results = await DemoMethods.RunDownloadAsync(progress, cts.Token);
                // PrintResults(results);
            }
            catch (OperationCanceledException)
            {
                resultsWindow.Text += $"The async download was cancelled. { Environment.NewLine }";
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: { elapsedMs }";
        }

        private void ReportProgressChanged(object sender, ProgressReportModel e)
        {
            dashboardProgress.Value = e.PrecentageComplete;
            PrintResults(e.SitesDownloaded);
        }

        private async void executeParallelAsync_Click(object sender, RoutedEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var results = await DemoMethods.RunDownloadPasrallelAsync();
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total execution time: { elapsedMs }";
        }

        private async void cancelOperation_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }

        private void PrintResults(List<WebsiteDataModel> results)
        {
            resultsWindow.Text = "";
            foreach (var item in results)
            {
                resultsWindow.Text += $"{ item.WebsiteUrl } download: {item.WebsiteData.Length} characters long.{ Environment.NewLine }";
            }
        }
    }
}
