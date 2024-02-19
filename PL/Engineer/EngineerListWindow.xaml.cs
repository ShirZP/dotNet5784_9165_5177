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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty EngineerListProperty = DependencyProperty.Register(
                                                                                           "EngineerList",
                                                                                           typeof(IEnumerable<BO.Engineer>),
                                                                                           typeof(EngineerListWindow),
                                                                                           new PropertyMetadata(null));
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;


        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public EngineerListWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Combo box ExperienceSelector to engineer.
        /// </summary>
        private void cbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ? s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!;

        }

        /// <summary>
        /// Click on Button AddNewEngineer open the EngineerWindow.
        /// </summary>
        private void BtnAddNewEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        /// <summary>
        /// Double click on engineer in the list open the EngineerWindow to update the engineer details.
        /// </summary>
        private void ListViewUpdateEngineer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineer != null)
            {
                new EngineerWindow(engineer.ID).ShowDialog();
            }
        }

        /// <summary>
        /// Refresh the EngineerListWindow
        /// </summary>
        private void RefreshWindow_Activated(object sender, EventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ? s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!;

        }
    }
}
