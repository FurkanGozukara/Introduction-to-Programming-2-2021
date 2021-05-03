using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lecture_9
{
    public static class AppInit
    {

        public enum enWhichSorting
        {
            [Description("Sort By Drug Name Ascending")]
            SortByDrugNameAsc,
            [Description("Sort By Drug Name Descending")]
            SortByDrugNameDesc,
            [Description("Sort By Drug Dose Ascending")]
            SortByDrugDoseAsc,
            [Description("Sort By Drug Dose Descending")]
            SortByDrugDoseDesc,
            [Description("Sort By Drug Id Ascending")]
            SortByDrugIdAsc,
            [Description("Sort By Drug Id Descending")]
            SortByDrugIdDesc
        }

        public class sortingOption
        {
            public enWhichSorting whichSort { get; set; }
            public string srDescription { get; set; }
        }

        private static List<sortingOption> lstSortingOptions;

        private static void initSortingOptions()
        {
            lstSortingOptions = new List<sortingOption>();

            foreach (enWhichSorting sort in Enum.GetValues(typeof(enWhichSorting)))
            {
                lstSortingOptions.Add(new sortingOption { srDescription= StringValueOfEnum(sort) , whichSort = sort });
            }
        }

        static string StringValueOfEnum(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }


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

            GlobalMethods.setDrugsPanelVisibility();
            initSortingOptions();

            main.cmbSortingDrugs.ItemsSource = lstSortingOptions;
            main.cmbSortingDrugs.DisplayMemberPath = "srDescription";
            main.cmbSortingDrugs.SelectedIndex = 4;
        }
    }
}
