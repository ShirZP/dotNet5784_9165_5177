using BO;
using PL.Engineer;
using PL.GanttChar;
using PL.Task;
using System;
using System.Collections.Generic;
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

namespace PL.Users
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public ManagerWindow()
        {
            this.Activate();
            
            InitializeComponent();
            SharedDependencyProperties.SetClock(this, s_bl.Clock);
            StartDatePicker.DisplayDateStart = SharedDependencyProperties.GetClock(this);
            ExecuteProjectByClock();
        }

        /// <summary>
        /// Click on button Handle Engineers opens the EngineerListWindow.
        /// </summary>
        private void BtnEngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();
        }

        //Click on button Init DB - initial the data base.
        private void BtnInitialization_Click(object sender, RoutedEventArgs e)
        {
            // Showing a MessageBox with Yes and No buttons and a question
            MessageBoxResult result = MessageBox.Show("Do you want to proceed initialization?", "Initialization Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Code to execute if the user clicks Yes
            if (result == MessageBoxResult.Yes)
            {
                // Initial the DB
                s_bl.initializationDB();
                this.Activate();
                MessageBoxResult messageBoxResult = MessageBox.Show("Initialization done successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Click on button Reset DB - reset the data base.
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // Showing a MessageBox with Yes and No buttons and a question
            MessageBoxResult result = MessageBox.Show("Do you want to proceed reset data?", "Reset Data Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Code to execute if the user clicks Yes
            if (result == MessageBoxResult.Yes)
            {
                // Initial the DB
                s_bl.resetDB();
                this.Activate();
                MessageBoxResult messageBoxResult = MessageBox.Show("Reset DB done successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnTaskTable_Click(object sender, RoutedEventArgs e)
        {
            new TaskTableWindow().ShowDialog();
        }


        #region Clock methods

        private void BtnAddHour_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.MoveClockHourForward());
            ExecuteProjectByClock();
        }

        private void BtnAddDay_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.MoveClockDayForward());
            ExecuteProjectByClock();
        }

        private void BtnAddYear_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.MoveClockYearForward());
            ExecuteProjectByClock();
        }

        private void BtnResetClock_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.initializeClock());
            ExecuteProjectByClock();
        }

        #endregion

        private void BtnGanttChart_Click(object sender, RoutedEventArgs e)
        {
                new GanttCharWindow().ShowDialog();
        }

        private void BtnExecuteProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to proceed execute project?", "Execution Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Code to execute if the user clicks Yes
            if (result == MessageBoxResult.Yes)
            {
                Button btnExecuteProject = sender as Button;
                btnExecuteProject.Visibility = Visibility.Collapsed;

                StartDateLabel.Visibility = Visibility.Visible;
                StartDatePicker.Visibility = Visibility.Visible;
                ChooseDateBtn.Visibility = Visibility.Visible;
            }
        }

        private void BtnChooseDate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure about the date you chose?", "Start Date Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Code to execute if the user clicks Yes
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //הסתרת הפקדים הלא נחוצים
                    StartDatePicker.Visibility = Visibility.Collapsed;
                    ChooseDateBtn.Visibility = Visibility.Collapsed;

                    s_bl.SetProjectStartDate((DateTime)StartDatePicker.SelectedDate!);
                    //קביעת הלוז אוטומטית
                    s_bl.Task.autoScheduledDate();
                    s_bl.SetProjectEndDate();

                    SharedDependencyProperties.SetProjectStartDate(this, s_bl.GetProjectStartDate()!.Value);
                    SharedDependencyProperties.SetProjectEndDate(this, s_bl.GetProjectEndDate()!.Value);


                    ExecuteProjectByClock();



                    EndDateViewLabel.Visibility = Visibility.Visible;
                    StartDateView.Visibility = Visibility.Visible;
                    EndDateView.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        /// <summary>
        /// Refresh the ManagerWindow
        /// </summary>
        private void RefreshWindow_Activated(object sender, EventArgs e)
        {
            SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());

            if (s_bl.GetProjectStartDate() != null)
            {
                SharedDependencyProperties.SetProjectStartDate(this, s_bl.GetProjectStartDate()!.Value);
                SharedDependencyProperties.SetProjectEndDate(this, s_bl.GetProjectEndDate()!.Value);
            }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            if (picker.SelectedDate < SharedDependencyProperties.GetClock(this)) // Assuming DateTime.Today is the minimum allowed date
            {
                MessageBox.Show("It is not possible to select a date earlier than now!");
                picker.SelectedDate = SharedDependencyProperties.GetClock(this); // Set to minimum allowed date or null
            }
        }

        private void ExecuteProjectByClock()
        {
            if(s_bl.GetProjectStartDate().HasValue && s_bl.GetProjectStatus() == BO.ProjectStatus.Planning)
            {
                if(SharedDependencyProperties.GetClock(this) >= s_bl.GetProjectStartDate())
                {
                    s_bl.changeStatusToExecution();
                    SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());

                }
            }
            else if(s_bl.GetProjectStatus() == BO.ProjectStatus.Execution )
            {
                if(SharedDependencyProperties.GetClock(this) < s_bl.GetProjectStartDate())
                {
                    s_bl.changeStatusToPlanning();
                    SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());
                }
            }
        }
    }
}
