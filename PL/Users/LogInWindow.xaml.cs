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
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty UserLogInProperty = DependencyProperty.Register(
                                                                                        "UserLogIn",
                                                                                        typeof(BO.User),
                                                                                        typeof(LogInWindow),
                                                                                        new PropertyMetadata(null));

        public BO.User UserLogIn
        {
            get { return (BO.User)GetValue(UserLogInProperty); }
            set { SetValue(UserLogInProperty, value); }
        }

        public LogInWindow()
        {
            InitializeComponent();
            UserLogIn = new BO.User(0, "", "", BO.UserPermissions.Engineer);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
       

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;    
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.User user = s_bl.User.Read(UserLogIn.UserName, UserLogIn.Password);

                switch (user.Permission)
                {
                    case BO.UserPermissions.Engineer:
                        this.Close();
                        //new windowE(user.ID).Show();
                        break;

                    case BO.UserPermissions.Manager:
                        this.Close();
                        new ManagerWindow().Show();
                        break;
                }
            }
            catch(BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
                UserLogIn.UserName = "";
                UserLogIn.Password = "";
            }
        }
        
    }
}
