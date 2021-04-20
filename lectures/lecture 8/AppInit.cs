using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_6
{
    public static class AppInit
    {
        public static void initApp(MainWindow main)
        {
            ObservableCollection<TblUserTypes> userRanks = new ObservableCollection<TblUserTypes>();
            userRanks.Add(new TblUserTypes() { UserTypeId = 0, UserTypeName = "Please Pick User Type" });
            using (PMSContext context = new PMSContext())
            {
                var vrUserTypes = context.TblUserTypes;
                foreach (var item in vrUserTypes)
                {
                    userRanks.Add(item);
                }
            }

            main.cmbBoxUserRank.ItemsSource = userRanks;
            main.cmbBoxUserRank.DisplayMemberPath = "UserTypeName";
            main.cmbBoxUserRank.SelectedIndex = 0;
        }
    }
}
