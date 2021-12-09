using Octokit;
using PropertyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DS2_META
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DS2Hook Hook => viewModel.Hook;

        Timer updateTimer = new Timer();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Location = settings.WindowLocation;
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version= fileVersionInfo.ProductVersion;

            Title = "DS2 META " + version;
            //EnableTabs(false);
            //InitAllTabs();

            updateTimer.Interval = 16;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Enabled = true;

            return;

            try
            {
                GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("DS-Gadget-for-Remastest"));
                Release release = await gitHubClient.Repository.Release.GetLatest("Nordgaren", "DS-Gadget");
                Version gitVersion = Version.Parse(release.TagName.ToLower().Replace("v", ""));
                Version exeVersion = Version.Parse(version);
                if (gitVersion > exeVersion) //Compare latest version to current version
                {
                    labelCheckVersion.Visibility= Visibility.Hidden;
                    link.NavigateUri = new Uri(release.HtmlUrl);
                    llbNewVersion.Visibility = Visibility.Visible;
                }
                else if (gitVersion == exeVersion)
                {
                    labelCheckVersion.Content = "App up to date";
                }
                else
                {
                    labelCheckVersion.Content = "App version unreleased. Be wary of bugs!";
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is ApiException || ex is ArgumentException)
            {
                labelCheckVersion.Content = "Current app version unknown";
            }
            catch (Exception ex)
            {
                labelCheckVersion.Content = "Something is very broke, contact DS Gadget repo owner";
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Hook.Loaded)
            {
                Hook.UpdateProperties();

            }
        }

        private void link_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}
