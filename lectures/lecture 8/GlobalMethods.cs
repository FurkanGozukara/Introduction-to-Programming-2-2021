using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using lecture_6.CustomClasses;

namespace lecture_6
{
    public static class GlobalMethods
    {
        public static MainWindow main;
        private static DateTime _appRunDate = DateTimeHelper.ServerTime;

        public static void startInfoOperations()
        {
            main.btnLogout.Visibility = System.Windows.Visibility.Hidden;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(updateInfoLabel);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private static void updateInfoLabel(object sender, EventArgs e)
        {
            if (LoginOperations.loggedUser == null)
            {
                main.lblLoginStatus.Dispatcher.BeginInvoke(() =>
                {
                    main.lblLoginStatus.Content = "Not Logged In. Application Run Time: " + (DateTimeHelper.ServerTime - _appRunDate).TotalSeconds.ToString("N0") + " Seconds";
                });
                return;
            }

            main.lblLoginStatus.Dispatcher.BeginInvoke(() =>
            {
                main.lblLoginStatus.Content = $"Logged User: {LoginOperations.loggedUser.Firstname} {LoginOperations.loggedUser.Lastname}, Session Run Time: " + (DateTimeHelper.ServerTime -LoginOperations.dtLoginTime).TotalSeconds.ToString("N0") + " Seconds";
            });
        }

        private static string ComputeSha256Hash(this string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string returnUserPw(string srUserPw, string srRandomString)
        {
            //no rainbow tables will be useful with salted encyrpted password
            return ComputeSha256Hash(srRandomString + srUserPw);
        }

        public static string returnUserIp()
        {
            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //if (!string.IsNullOrEmpty(ipAddress))
            //{
            //    string[] addresses = ipAddress.Split(',');
            //    if (addresses.Length != 0)
            //    {
            //        return addresses[0];
            //    }
            //}

            //return context.Request.ServerVariables["REMOTE_ADDR"];

            return "152.34.64.123";
        }

        public static DateTime ServerDate(this DateTime dt)
        {
            return DateTime.UtcNow;
        }

        public static void changeLoginStatus()
        {
            main.tabLogin.IsEnabled = !main.tabLogin.IsEnabled;

            main.tabRegister.IsEnabled = !main.tabRegister.IsEnabled; 
            
            main.btnLogin.IsEnabled = !main.btnLogin.IsEnabled;

            switch (main.btnLogout.Visibility)
            {
                case System.Windows.Visibility.Visible:
                    main.btnLogout.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case System.Windows.Visibility.Hidden:
                    main.btnLogout.Visibility = System.Windows.Visibility.Visible;
                    break;
                case System.Windows.Visibility.Collapsed:
                    break;
                default:
                    break;
            }
        }

        public static BindingList<T> ToBindingList<T>(this IList<T> source)
        {
            return new BindingList<T>(source);
        }
    }

    static public class DateTimeHelper
    {
        public static DateTime ServerTime
        {
            get { return DateTime.UtcNow; }
        }
    }


}
