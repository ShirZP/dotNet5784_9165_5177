using PL.Engineer;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void BtnInialization_Click(object sender, RoutedEventArgs e)
        {
            // Showing a MessageBox with Yes and No buttons and a question
            MessageBoxResult result = MessageBox.Show("Do you want to proceed initialization?", "Initialization Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Code to execute if the user clicks Yes
            if (result == MessageBoxResult.Yes)
            {
                // Initial the DB
                DalTest.Initialization.Do();
            }
        }
    }
}