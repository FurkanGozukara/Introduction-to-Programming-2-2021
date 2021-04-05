using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace lecture_6
{
    public static class RegisterOperations
    {
        public static void completeRegister(MainWindow main)
        {
            using (PMSContext context=new PMSContext())
            {
                TblUsers myUser = new TblUsers();
                myUser.Firstname = main.txtFirstName.Text;
                myUser.Lastname = main.txtLastName.Text;
                if(main.txtPw1.Password.ToString()!=main.txtPw2.Password.ToString())
                {
                    MessageBox.Show("Error! Entered passwords are not matching. Please re-type your password!");
                    return;
                }

                myUser.UserPw = GlobalMethods.returnUserPw(main.txtPw1.Password.ToString(), myUser.Firstname);
                if(main.cmbBoxUserRank.SelectedIndex<1)
                {
                    MessageBox.Show("Error! Please select your user rank / role first!");
                    return;
                }
                myUser.UserType = (main.cmbBoxUserRank.SelectedItem as TblUserTypes).UserTypeId;
                myUser.UserEmail = main.txtEmail.Text;
                myUser.RegisterIp = "123.32.12.53";
                try
                {
                    context.TblUsers.Add(myUser);
                    context.SaveChanges();
                }
                catch (Exception E )
                {
                    MessageBox.Show("An error has occured while registering. Error is: \n" + E.Message.ToString() + "\n\n" + E?.InnerException?.Message);
                    return;
                }

                MessageBox.Show("User has been succcesfully registered");
                //do the after register operations
                main.txtEmail.Text = "";
                main.txtFirstName.Text = "";
            }
        }
    }
}
