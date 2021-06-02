using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static lecture14.MainWindow;

namespace lecture14
{
    /// <summary>
    /// Interaction logic for DisplayList.xaml
    /// </summary>
    public partial class DisplayList : Window
    {
        private static ObservableCollection<string> listofUrls = new ObservableCollection<string>();

        DispatcherTimer updateListTimer = new DispatcherTimer();

        public DisplayList()
        {
            InitializeComponent();
            updateListTimer.Tick += UpdateListTimer_Tick;
            updateListTimer.Interval = new TimeSpan(0, 0, 1);
            updateListTimer.Start();
            lstListBoxUrls.ItemsSource = listofUrls;
        }

        private void UpdateListTimer_Tick(object sender, EventArgs e)
        {
            listofUrls.Clear();
            string[] arrayTemp;
            lock (lock_of_hash_sets)
            {
                switch (selectedList)
                {
                    case enWhichHashSet.hsWaitingToBeCrawlingUrls:
                    default:
                        arrayTemp = new string[hsWaitingToBeCrawlingUrls.Count];
                        hsWaitingToBeCrawlingUrls.CopyTo(arrayTemp);
                        break;
                    case enWhichHashSet.hsCrawledUrls:
                        arrayTemp = new string[hsCrawledUrls.Count];
                        hsCrawledUrls.CopyTo(arrayTemp);
                        break;
                    case enWhichHashSet.hsCurrentlyCrawling:
                        arrayTemp = new string[hsCurrentlyCrawling.Count];
                        hsCurrentlyCrawling.CopyTo(arrayTemp);
                        break;
                    case enWhichHashSet.hsFailedUrls:
                        arrayTemp = new string[hsFailedUrls.Count];
                        hsFailedUrls.CopyTo(arrayTemp);
                        break;
                }                
            }

            foreach (var item in arrayTemp)
            {
                listofUrls.Add(item);
            }
        }
    }
}
