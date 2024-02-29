﻿using PL.Engineer;
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

        public static readonly DependencyProperty ClockProperty = DependencyProperty.Register(
                                                                                        "Clock",
                                                                                        typeof(DateTime),
                                                                                        typeof(ManagerWindow),
                                                                                        new PropertyMetadata(null));
        public DateTime Clock
        {
            get { return (DateTime)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value); }
        }

        public ManagerWindow()
        {
            InitializeComponent();
            Clock = s_bl.Clock;
        }

        /// <summary>
        /// Click on button Handle Engineers opens the EngineerListWindow.
        /// </summary>
        private void BtnEngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
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
            new TaskTableWindow().Show();
        }

        private void BtnTLogIn_Click(object sender, RoutedEventArgs e)
        {
            new LogInWindow().Show();
        }


        #region Clock methods

        private void BtnAddHour_Click(object sender, RoutedEventArgs e)
        {
            Clock = s_bl.MoveClockHourForward();
        }

        private void BtnAddDay_Click(object sender, RoutedEventArgs e)
        {
            Clock = s_bl.MoveClockDayForward();
        }

        private void BtnAddYear_Click(object sender, RoutedEventArgs e)
        {
            Clock = s_bl.MoveClockYearForward();
        }

        private void BtnResetClock_Click(object sender, RoutedEventArgs e)
        {
            Clock = s_bl.initializeClock();
        }

        #endregion
    }
}
