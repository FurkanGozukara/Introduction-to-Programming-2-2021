using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lecture_9.CustomClasses
{
    public static class LoginOperations
    {
        public static TblUsers loggedUser = null;
        public static DateTime dtLoginTime;

        public static void loginTry(MainWindow main)
        {
            using (PMSContext context = new PMSContext())
            {
                var vrIpTestResult = context.TblLoginTry.Where(pr => pr.TriedIp == GlobalMethods.returnUserIp() & pr.DateOfTry.AddMinutes(15) > DateTimeHelper.ServerTime).Count();

                if (vrIpTestResult >= 5)
                {
                    MessageBox.Show("Error! You have so many times tried to login withing 15 minutes. Try again later!");
                    return;
                }

                var vrUserEmail = context.TblUsers.FirstOrDefault(pr => pr.UserEmail == main.txtLoginEmail.Text);

                if (vrUserEmail == null)
                {
                    increaseLoginTry();
                    MessageBox.Show("Error! No such user is found!");
                    return;
                }

                if (vrUserEmail.UserPw != GlobalMethods.returnUserPw(main.txtLoginPassword.Password.ToString(), vrUserEmail.SaltOfPw))
                {
                    increaseLoginTry();
                    MessageBox.Show("Error! The entered password is incorrect");
                    return;
                }

                loggedUser = vrUserEmail;
                loggedUser.UserPw = null;//you can nullfiy the sensitive information so they wont remain in ram memory
                dtLoginTime = DateTimeHelper.ServerTime;

                GlobalMethods.changeLoginStatus();
                if (loggedUser.UserType == 2)
                    GlobalMethods.main.tabDrugs.IsSelected = true;
                MessageBox.Show("You have successfully logged-in");
            }
        }

        private static void increaseLoginTry()
        {
            using (PMSContext context = new PMSContext())
            {
                TblLoginTry tempTry = new TblLoginTry();
                tempTry.TriedIp = GlobalMethods.returnUserIp();
                tempTry.DateOfTry = DateTimeHelper.ServerTime;
                context.TblLoginTry.Add(tempTry);
                context.SaveChanges();
            }
        }

        public static void tryLogout()
        {
            loggedUser = null;
            GlobalMethods.changeLoginStatus();
            GlobalMethods.main.tabLogin.IsSelected = true;           
            MessageBox.Show("You have successfully logged-out");

        }
    }
}
