using PL.Engineer;
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
        public ManagerWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click on button Engineers opens the EngineerListWindow.
        /// </summary>
        private void BtnEngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        /// <summary>
        /// Click on button Tasks opens the TaskTableWindow.
        /// </summary>
        private void BtnTaskTable_Click(object sender, RoutedEventArgs e)
        {
            new TaskTableWindow().Show();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
