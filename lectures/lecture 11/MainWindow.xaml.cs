using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace lecture_11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainReference;
        private static ObservableCollection<string> ocItemsList = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            mainReference = this;
            ThreadPool.SetMaxThreads(100000, 100000);
            ThreadPool.SetMinThreads(100000, 100000);
        }

        private void btnSpawnTasks_Click(object sender, RoutedEventArgs e)
        {
            listBoxDisplay.ItemsSource = ocItemsList;

            addRandomNumberToCollection();

            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    loopRandomAdd();
                });
            }
        }

        private static void loopRandomAdd()
        {
            for (int i = 0; i < Int32.MaxValue; i++)
            {
                addRandomNumberToCollection();
                System.Threading.Thread.Sleep(1);
            }
        }

        static Random randGenerator = new Random();

        private static void addRandomNumberToCollection()
        {
            //mainReference.Dispatcher.BeginInvoke(
            //    new Action(
            //        delegate ()
            //            {
            //                ocItemsList.Insert(0, randGenerator.Next().ToString());//explicit conversation
            //            }));

            mainReference.Dispatcher.Invoke(
                 new Action(
                     delegate ()
                        {
                            ocItemsList.Insert(0, randGenerator.Next().ToString());//explicit conversation
                        }));
        }

        private static Dictionary<int, int> dicValues = new Dictionary<int, int>();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        static DateTime dtDispatcherStartDate = DateTime.Now;

        private void btnDictionaryExample_Click(object sender, RoutedEventArgs e)
        {
            //dontWaitTasksStart();

            Task.Factory.StartNew(() => { waitForTasksToStart(); });


        }

        private void dontWaitTasksStart()
        {
            Debug.WriteLine("btnDictionaryExample_Click: " + Thread.CurrentThread.ManagedThreadId);
            dtDispatcherStartDate = DateTime.Now;
            Task.Factory.StartNew(() => //starting dispatcher timer inside a task make no difference because we have to define the timer object in the main thread
            {
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            });

            Debug.WriteLine("starting tasks: " + DateTime.Now);

            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Debug.WriteLine("inside task factory in btnDictionaryExample_Click: " + Thread.CurrentThread.ManagedThreadId);
                    addValuesToDictionary();
                });
            }

            Debug.WriteLine("starting tasks command finished: " + DateTime.Now);
        }

        List<Task> lstTasks = new List<Task>();

        private void waitForTasksToStart()
        {
            Debug.WriteLine("btnDictionaryExample_Click: " + Thread.CurrentThread.ManagedThreadId);
            dtDispatcherStartDate = DateTime.Now;
            Task.Factory.StartNew(() => //starting dispatcher timer inside a task make no difference because we have to define the timer object in the main thread
            {
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            });

            Debug.WriteLine("starting tasks: " + DateTime.Now);



            for (int i = 0; i < 50; i++)
            {
                var vrTask = Task.Factory.StartNew(() =>
                  {
                      Debug.WriteLine("inside task factory in btnDictionaryExample_Click: " + Thread.CurrentThread.ManagedThreadId);
                      addValuesToDictionary();
                  });

                lstTasks.Add(vrTask);
            }



            //so if we want to wait until all tasks are started

            while (true)
            {
                System.Threading.Thread.Sleep(10);
                if (lstTasks.Where(pr => pr.Status == TaskStatus.WaitingToRun).Any())
                {
                    continue;
                }
                else
                    break;
            }


            Debug.WriteLine("starting tasks command finished: " + DateTime.Now);

            Task.WaitAll(lstTasks.ToArray());//this will never get passed until all tasks are finished

            Debug.WriteLine("all tasks are finished in " + (DateTime.Now - dtDispatcherStartDate).TotalSeconds.ToString("N2") + " seconds");
        }

        private static int _irStartedTaskCount = 0;

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //throw new Exception(""); //this causes entire application crash because dispatcher timer tick event is running under main window / main application thread / thread id : 1
            //Debug.WriteLine("DispatcherTimer_Tick: " + Thread.CurrentThread.ManagedThreadId);
            lblCounter.Content = dicValues.Count.ToString("N0") + " - running tasks count: " + lstTasks.Where(pr => pr.Status == TaskStatus.Running).Count<Task>() + " completed: " + lstTasks.Where(pr => pr.Status == TaskStatus.RanToCompletion).Count<Task>();

            //Debug.WriteLine("DispatcherTimer_Tick: " + Thread.CurrentThread.ManagedThreadId);
        }

        private static object _lockdicValues = new object();

        private static void addValuesToDictionary()
        {
            Interlocked.Increment(ref _irStartedTaskCount);

            Debug.WriteLine($"# of tasks: {_irStartedTaskCount} , addValuesToDictionary thread id: " + Thread.CurrentThread.ManagedThreadId + " , started after: " + (DateTime.Now - dtDispatcherStartDate).TotalSeconds.ToString("N2"));
            //throw new Exception(""); //this won't cause application to crash beucase only the sub thread / task will get crashed and terminated
            //you should never allow your main application to crash and handle unexpected sub thread crashes

            for (int i = 0; i < 100000000; i++)
            {
                var vrKey = randGenerator.Next(0, 1000000);
                lock (dicValues)//this ensures that no thread / task can get inside of this scope simultaneously
                                //when the object is locked by lets say thread / task A, the other competing tasks will wait until the holding task releases it
                                //locking can also causes dead lock and you have to be careful with locking
                                //lock (_lockdicValues) this is also another option
                {
                    //we are putting all modification to the object inside locked scope and ensure that no multiple tasks / threads can modify the object at the same time
                    if (dicValues.ContainsKey(vrKey))
                    {
                        dicValues[vrKey]++;
                    }
                    else
                    {
                        dicValues.Add(vrKey, 1);
                    }


                }
            }
        }

        private void btnFetchRandomValue_Click(object sender, RoutedEventArgs e)
        {
            var myRandomKey = randGenerator.Next(1, 1000000);

            //best way of calculating time / performance is using stopwatch

            Stopwatch watch = new Stopwatch();
            watch.Start();
            var vrValue = dicValues[myRandomKey];
            watch.Stop();
            lblRandomKeyValue.Content = $"key: {myRandomKey.ToString("N0")} - value: {vrValue.ToString("N0")} , - time to retreive key: {watch.ElapsedMilliseconds.ToString("N2")} miliseconds";
        }

    
    }
}
