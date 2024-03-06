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
            SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());

            InitializeComponent();
            SharedDependencyProperties.SetClock(this, s_bl.Clock);
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
        }

        private void BtnAddDay_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.MoveClockDayForward());
        }

        private void BtnAddYear_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.MoveClockYearForward());
        }

        private void BtnResetClock_Click(object sender, RoutedEventArgs e)
        {
            SharedDependencyProperties.SetClock(this, s_bl.initializeClock());
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
                //הסתרת הפקדים הלא נחוצים
                StartDatePicker.Visibility = Visibility.Collapsed;
                ChooseDateBtn.Visibility = Visibility.Collapsed;

                s_bl.SetProjectStartDate((DateTime)StartDatePicker.SelectedDate!);
                //קביעת הלוז אוטומטית
                s_bl.Task.autoScheduledDate();
                s_bl.SetProjectEndDate();//TODO: לכתוב את הפונקציה לחישוב תאריך הסיום

                StartDateView.Text = s_bl.GetProjectStartDate().ToString();
                EndDateView.Text = s_bl.GetProjectEndDate().ToString();

                EndDateViewLabel.Visibility = Visibility.Visible;
                StartDateView.Visibility = Visibility.Visible;
                EndDateView.Visibility = Visibility.Visible;
                GanttChartBtn.Visibility = Visibility.Visible;
            }

        }
    }
}
