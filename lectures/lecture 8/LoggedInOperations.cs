using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace lecture_6
{
    public static class LoggedInOperations
    {
        private static PMSContext _drugsData = new PMSContext();
        public static void refreshDrugsDataGrid()
        {
            _drugsData = new PMSContext();//this will refresh local data

            //this is 
            //_drugsData.TblDrugs.Select(pr => new { DrugId = pr.DrugId, DrugName = pr.DrugName, DrugDose = pr.DoseMg, DeleteButton = "Delete Drug" }).OrderBy(pr => pr.DrugId).Take(100).Load();//load requires using Microsoft.EntityFrameworkCore; reference

            //var bindingList = _drugsData.TblDrugs.Select(pr => new { DrugId = pr.DrugId, DrugName = pr.DrugName, DrugDose = pr.DoseMg, DeleteButton = "Delete Drug" }).OrderBy(pr => pr.DrugId).Take(100).ToList().ToBindingList();

            // this is way of getting anonymous type results from entity framework selection queries
            //var lstDrugs = _drugsData.TblDrugs.Select(pr => new { DrugId = pr.DrugId, DrugName = pr.DrugName, DrugDose = pr.DoseMg, DeleteButton = "Delete Drug" }).OrderBy(pr => pr.DrugId).Take(100).ToList();

            //foreach (var item in lstDrugs)
            //{
            //    _drugsData.TblDrugs.Local.Add(item);
            //}

            _drugsData.TblDrugs.OrderBy(pr => pr.DrugId).Take(100).Load();

            GlobalMethods.main.dataGridDrugs.ItemsSource = _drugsData.TblDrugs.Local.ToBindingList();

            GlobalMethods.main.dataGridDrugs.CurrentCellChanged += DataGridDrugs_CurrentCellChanged;

            for (int i = 0; i < GlobalMethods.main.dataGridDrugs.Columns.Count; i++)
            {
                GlobalMethods.main.dataGridDrugs.Columns[i].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                //sets each column to strech equally 
                if (GlobalMethods.main.dataGridDrugs.Columns[i].Header.ToString().ToLowerInvariant() == "tblprescriptions")
                {
                    GlobalMethods.main.dataGridDrugs.Columns[i].Visibility = System.Windows.Visibility.Hidden;
                }
            }



            GlobalMethods.main.dataGridDrugs.Items.Refresh();
        }

        private static void DataGridDrugs_CurrentCellChanged(object sender, EventArgs e)
        {
            string srMsg = $"New: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Count()}, Modified: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Count()}, Deleted: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).Count()} Drugs to be saved";
            setDrugScreenMsg(srMsg);
        }

        public static void saveDrugChanges()
        {
            _drugsData.SaveChanges();
            setDrugScreenMsg("Changes (update/delete/add) are saved at the database.");
        }

        private static void setDrugScreenMsg(string srMsg)
        {
            GlobalMethods.main.lblDrugScreen.Content = srMsg;
        }

        public static void deleteSelectedDrug()
        {
          
            var selectedItems = GlobalMethods.main.dataGridDrugs.SelectedItems.Cast<TblDrugs>().ToList();
            foreach (var vrDrug in selectedItems)
            {
                _drugsData.TblDrugs.Local.Remove(_drugsData.TblDrugs.Local.Where(YouCanSetAnyNameAsVariableNameYouWant => YouCanSetAnyNameAsVariableNameYouWant.DrugId == vrDrug.DrugId).FirstOrDefault());        
            }

            DataGridDrugs_CurrentCellChanged(null, null);
        }



    }
}
