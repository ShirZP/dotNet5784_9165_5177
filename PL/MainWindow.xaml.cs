using PL.Engineer;
using PL.Task;
using PL.Users;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();
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

        
    }
}