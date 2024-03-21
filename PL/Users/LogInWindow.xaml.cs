using PL;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
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

        public SecureString SecurePassword { private get; set; }

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

        /// <summary>
        /// The function enters the SecurePassword variable the password entered in the PasswordBox control
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }
        }

        String SecureStringToString(SecureString value)
        {
            // Declare a pointer to store the address of the unmanaged string. Initialize it to zero (null pointer).
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                //Converting the secure string to an unmanaged string. 'valuePtr' now points to the start of this unmanaged string.
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                //Convert the unmanaged string (pointed to by 'valuePtr') back to a managed 'string'. This copies the data from unmanaged memory to managed memory, allowing us to work with it as a regular string in C#.
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                //The unmanaged memory allocated by 'SecureStringToGlobalAllocUnicode' is properly freed.
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.User user = s_bl.User.Read(UserLogIn.UserName, SecureStringToString(SecurePassword));

                switch (user.Permission)
                {
                    case BO.UserPermissions.Engineer:
                        new UserEngineerWindow(user.ID).Show();
                        break;

                    case BO.UserPermissions.Manager:
                        new ManagerWindow().Show();
                        break;
                }
            }
            catch(BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("It's your problem that you forgot the password!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}

