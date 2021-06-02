using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using HtmlAgilityPack;

namespace lecture14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow main;
        public static enWhichHashSet selectedList;
        private bool blDontCrawlExternalUrls = true;

        public enum enWhichHashSet
        {
            Select_Which_HashSet,
            hsWaitingToBeCrawlingUrls,
            hsCrawledUrls,
            hsCurrentlyCrawling,
            hsFailedUrls
        }

        public MainWindow()
        {
            InitializeComponent();
            ThreadPool.SetMaxThreads(100000, 100000);
            ThreadPool.SetMinThreads(100000, 100000);
            main = this;
            cmbWhichHashSet.ItemsSource = Enum.GetValues(typeof(enWhichHashSet));
        }

        private void btnMainProgramExceptionTest_Click(object sender, RoutedEventArgs e)
        {
            Convert.ToInt32("2133a");
        }

        private void btnDispatcherExceptionTest_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                Convert.ToInt32("2133a");
            }));
        }

        private void btnDispatcherExceptionInsideATask_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                this.Dispatcher.BeginInvoke((Action)(() =>//starting this under a task doesnt make any difference because the dispatcher is still executing under main thread. when you use BeginInvoke instead of Invoke, it executes whenever it is possible not immediately 
                {
                    Convert.ToInt32("2133a");
                }));

            });
        }

        private void btnTestUnhandledExceptionInsideATask_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Convert.ToInt32("2133a");
            });
        }

        private static HashSet<string> hsAllDiscoveredUrls = new HashSet<string>();
        public static HashSet<string> hsWaitingToBeCrawlingUrls = new HashSet<string>();
        public static HashSet<string> hsCrawledUrls = new HashSet<string>();
        public static HashSet<string> hsCurrentlyCrawling = new HashSet<string>();
        public static HashSet<string> hsFailedUrls = new HashSet<string>();
        public static object lock_of_hash_sets = new object();

        DispatcherTimer crawlingCheckTimer = new DispatcherTimer();

        int irHowManyConcurrentTaskToStart = 10;

        public static List<Task> lstOfActiveCrawlingTasks = new List<Task>();
        private DateTime dtCrawlingStarted = DateTime.Now;

        private void btnStartCrawling_Click(object sender, RoutedEventArgs e)
        {
            hsWaitingToBeCrawlingUrls.Add(txtRootUrl.Text);
            crawlingCheckTimer.Tick += CrawlingCheckTimer_Tick;
            crawlingCheckTimer.Interval = new TimeSpan(0, 0, 1);//here we are basically doing a polling
            crawlingCheckTimer.Start();
        }

        //dispatcher timer tick is still running inside main thread
        private void CrawlingCheckTimer_Tick(object sender, EventArgs e)
        {
            lock (lock_of_hash_sets)
            {
                lstOfActiveCrawlingTasks = lstOfActiveCrawlingTasks.Where(
                 pr => pr.Status != TaskStatus.RanToCompletion
                 &&
                 pr.Status != TaskStatus.Faulted
                 &&
                 pr.Status != TaskStatus.Canceled).ToList();

                Dispatcher.BeginInvoke((Action)(() =>
                {

                    lblStatus.Content = $@"
                    Number of total tasks: {lstOfActiveCrawlingTasks.Count}
                    Number of running tasks: {lstOfActiveCrawlingTasks.Select(pr => pr.Status == TaskStatus.Running).ToList().Count}
                    Waiting to be crawled urls count: {hsWaitingToBeCrawlingUrls.Count.ToString("N0")}
                    Currently Crawling urls count: {hsCurrentlyCrawling.Count}
                    Crawling completed urls count: {hsCrawledUrls.Count.ToString("N0")}
                    Failed urls count: {hsFailedUrls.Count.ToString("N0")}
                    Crawling speed per minute: { (Convert.ToDouble(hsCrawledUrls.Count) / (DateTime.Now - dtCrawlingStarted).TotalMinutes).ToString("N2") }
                    Discovered urls speed per minute: { (Convert.ToDouble(hsAllDiscoveredUrls.Count) / (DateTime.Now - dtCrawlingStarted).TotalMinutes).ToString("N2") }";

                }));

                if (lstOfActiveCrawlingTasks.Count < irHowManyConcurrentTaskToStart)
                {
                    for (int i = 0;
                        i < (irHowManyConcurrentTaskToStart - lstOfActiveCrawlingTasks.Count); i++)
                    {
                        string srLastUrl = "";
                        foreach (var vrUrl in hsWaitingToBeCrawlingUrls)
                        {
                            if (!hsCurrentlyCrawling.Contains(vrUrl))
                            {
                                srLastUrl = vrUrl;
                                Task vrTask = Task.Factory.StartNew(() => { startCrawlingProcess(vrUrl); }).ContinueWith(t =>
                                {
                                    App.writeMessage(t.Exception, "TaskException");
                                },
                                TaskContinuationOptions.OnlyOnFaulted);

                                lstOfActiveCrawlingTasks.Add(vrTask);
                                hsCurrentlyCrawling.Add(vrUrl);
                                break;
                            }
                        }
                        hsWaitingToBeCrawlingUrls.Remove(srLastUrl);
                    }
                }
            }
        }

        private void startCrawlingProcess(string srUrl)
        {
            HtmlDocument hdDoc = new HtmlDocument();
            var baseUri = new Uri(srUrl);
            List<string> lstDiscoveredUrls = new List<string>();
            try
            {
                HtmlWeb web = new HtmlWeb();
                hdDoc = web.Load(srUrl);

                var vrNodes = hdDoc.DocumentNode.SelectNodes("//a[@href]");

                // extracting all links
                if (vrNodes != null)
                    foreach (HtmlNode link in vrNodes)//xpath notation
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        //this is used to convert from relative path to absolute path
                        var absoluteUri = new Uri(baseUri, att.Value.ToString().Split('#').FirstOrDefault());

                        if (!absoluteUri.ToString().StartsWith("http://") && !absoluteUri.ToString().StartsWith("https://"))
                            continue;

                        var vrExternal = Uri.Compare(baseUri, absoluteUri, UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture);

                        if (blDontCrawlExternalUrls == true && vrExternal != 0)
                            continue;

                        lstDiscoveredUrls.Add(absoluteUri.ToString());
                    }

                lstDiscoveredUrls = lstDiscoveredUrls.Distinct().ToList();

                lock (lock_of_hash_sets)
                {
                    foreach (var vrUrl in lstDiscoveredUrls)
                    {
                        hsAllDiscoveredUrls.Add(vrUrl);

                        if (hsCrawledUrls.Contains(vrUrl) == false)
                        {
                            if (hsCurrentlyCrawling.Contains(vrUrl) == false)
                            {
                                if (hsWaitingToBeCrawlingUrls.Contains(vrUrl) == false)
                                {
                                    if (hsFailedUrls.Contains(vrUrl) == false)
                                    {
                                        hsWaitingToBeCrawlingUrls.Add(vrUrl);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                App.writeMessage(E, "startCrawlingProcess");
                lock (lock_of_hash_sets)
                {
                    hsFailedUrls.Add(srUrl);
                }
            }

            lock (lock_of_hash_sets)
            {
                hsCrawledUrls.Add(srUrl);
                hsCurrentlyCrawling.Remove(srUrl);
            }
        }

        private void btnStartWaitingToBeCrawled_Click(object sender, RoutedEventArgs e)
        {
            selectedList = (enWhichHashSet)cmbWhichHashSet.SelectedItem;
            DisplayList _display = new DisplayList();
            _display.Show();
        }

        private void btnSetCrawlingTaskCount_Click(object sender, RoutedEventArgs e)
        {
            irHowManyConcurrentTaskToStart = Convert.ToInt32(txtNewTaskCount.Text);
        }
    }
}
