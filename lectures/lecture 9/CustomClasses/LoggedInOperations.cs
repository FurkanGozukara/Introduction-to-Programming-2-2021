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
using System.IO;
using static lecture_9.AppInit;

namespace lecture_9
{
    public static class LoggedInOperations
    {
        private static PMSContext _drugsData = new PMSContext();
        public static void refreshDrugsDataGrid()
        {
            if (GlobalMethods.isA_DoctorLoggedIn() == false)
                return;

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

            switch (((sortingOption)GlobalMethods.main.cmbSortingDrugs.SelectedItem).whichSort)
            {
                case enWhichSorting.SortByDrugNameAsc:
                    _drugsData.TblDrugs.OrderBy(pr => pr.DrugName).Take(100).Load();
                    break;
                case enWhichSorting.SortByDrugNameDesc:
                    _drugsData.TblDrugs.OrderByDescending(pr => pr.DrugName).Take(100).Load();
                    break;
                case enWhichSorting.SortByDrugDoseAsc:
                    _drugsData.TblDrugs.OrderBy(pr => pr.DoseMg).Take(100).Load();
                    break;
                case enWhichSorting.SortByDrugDoseDesc:
                    _drugsData.TblDrugs.OrderByDescending(pr => pr.DoseMg).Take(100).Load();
                    break;
                case enWhichSorting.SortByDrugIdAsc:
                    _drugsData.TblDrugs.OrderBy(pr => pr.DrugId).Take(100).Load();
                    break;
                case enWhichSorting.SortByDrugIdDesc:
                    _drugsData.TblDrugs.OrderByDescending(pr => pr.DrugId).Take(100).Load();
                    break;
                default:
                    break;
            }



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

                if (GlobalMethods.main.dataGridDrugs.Columns[i].Header.ToString().ToLowerInvariant() == "drugid")
                {
                    GlobalMethods.main.dataGridDrugs.Columns[i].IsReadOnly = true;
                }
            }



            GlobalMethods.main.dataGridDrugs.Items.Refresh();
        }

        internal static void loadReadyDrugData()
        {
            Task.Factory.StartNew(() =>
            {
                using (PMSContext myContext = new PMSContext())
                {
                    myContext.TblDrugs.Load();

                    int irAddedCounter = 0, irProcessedCounter = 0;
                    foreach (var vrLine in File.ReadLines("StaticData\\pddf_2021_04_13.csv"))
                    {
                        irProcessedCounter++;
                        TblDrugs myDrug = new TblDrugs();

                        string srDrugName = vrLine.Split(",")[9];
                        myDrug.DrugName = srDrugName;
                        var vrSplitVals = srDrugName.Split(" ");

                        for (int i = 0; i < vrSplitVals.Length; i++)
                        {
                            if (vrSplitVals[i] == "MG")
                            {
                                decimal dcTry = 0;
                                if (decimal.TryParse(vrSplitVals[i - 1], out dcTry))
                                {
                                    myDrug.DoseMg = dcTry;
                                }
                                break;
                            }
                        }

                        bool blAdded = false;

                        if (myDrug.DoseMg != 0)
                        {
                            var vrSelect = myContext.TblDrugs.Local.Where(pr => pr.DrugName == myDrug.DrugName && pr.DoseMg == myDrug.DoseMg).FirstOrDefault();
                            if (vrSelect == null)
                            {
                                //vrSelect = myContext.TblDrugs.Local.Where(pr => pr.DrugName == myDrug.DrugName && pr.DoseMg == myDrug.DoseMg).FirstOrDefault();
                                //if (vrSelect == null)
                                //{
                                myContext.TblDrugs.Add(myDrug);
                                myContext.TblDrugs.Local.Add(myDrug);
                                blAdded = true;
                                irAddedCounter++;
                                //}

                            }

                        }

                        //if (!blAdded)
                        //    continue;



                        if (irProcessedCounter % 100 == 0)
                        {
                            myContext.SaveChanges();
                            setDrugScreenMsg($"so far inserted drugs count: { irAddedCounter.ToString("N0")} , processed: {irProcessedCounter.ToString("N0")}");
                        }
                    }
                }
            });

        }

        private static void DataGridDrugs_CurrentCellChanged(object sender, EventArgs e)
        {
            string srMsg = $"New: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).Count()}, Modified: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Count()}, Deleted: {_drugsData.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).Count()} Drugs to be saved";
            setDrugScreenMsg(srMsg);
        }

        public static void saveDrugChanges()
        {
            if (GlobalMethods.isA_DoctorLoggedIn() == false)
                return;
            _drugsData.SaveChanges();
            setDrugScreenMsg("Changes (update/delete/add) are saved at the database.");
        }

        private static void setDrugScreenMsg(string srMsg)
        {
            GlobalMethods.main.lblDrugScreen.Dispatcher.BeginInvoke((Action)(() =>
            {
                GlobalMethods.main.lblDrugScreen.Content = srMsg;
            }));
        }

        public static void deleteSelectedDrug()
        {
            if (GlobalMethods.isA_DoctorLoggedIn() == false)
                return;
            var selectedItems = GlobalMethods.main.dataGridDrugs.SelectedItems.Cast<TblDrugs>().ToList();
            foreach (var vrDrug in selectedItems)
            {
                _drugsData.TblDrugs.Local.Remove(_drugsData.TblDrugs.Local.Where(YouCanSetAnyNameAsVariableNameYouWant => YouCanSetAnyNameAsVariableNameYouWant.DrugId == vrDrug.DrugId).FirstOrDefault());
            }

            DataGridDrugs_CurrentCellChanged(null, null);
        }



    }
}
