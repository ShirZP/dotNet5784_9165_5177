using BO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PL.Users
{
    /// <summary>
    /// Interaction logic for UserEngineerWindow.xaml
    /// </summary>
    public partial class UserEngineerWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty EngineerUserProperty = DependencyProperty.Register(
                                                                                        "EngineerUser",
                                                                                        typeof(BO.Engineer),
                                                                                        typeof(UserEngineerWindow),
                                                                                        new PropertyMetadata(null));

        public BO.Engineer EngineerUser
        {
            get { return (BO.Engineer)GetValue(EngineerUserProperty); }
            set { SetValue(EngineerUserProperty, value); }
        }
        public UserEngineerWindow(int id)
        {
            InitializeComponent();
            this.DataContext = this;

            try 
            {
                EngineerUser = s_bl.Engineer.Read(id);
                SharedDependencyProperties.SetClock(this, s_bl.Clock);
                SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void BtnCurrentTaskView_Click(object sender, RoutedEventArgs e)
        {
            new TaskDetails(EngineerUser.EngineerCurrentTask!.ID).ShowDialog();
            EngineerUser = s_bl.Engineer.Read(EngineerUser.ID);
        }

    }
}
