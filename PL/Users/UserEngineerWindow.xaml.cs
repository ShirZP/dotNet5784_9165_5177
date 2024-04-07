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
            ReadUser(id);
        }

        /// <summary>
        /// Opening the engineer's current task view window
        /// </summary>
        private void BtnCurrentTaskView_Click(object sender, RoutedEventArgs e)
        {
            new TaskDetails(EngineerUser.EngineerCurrentTask!.ID).ShowDialog();
        }


        /// <summary>
        /// Clicking on completing a task or selecting a task
        /// </summary>
        private void BtnSelectOrDoneTask_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                try
                {
                    MessageBoxResult messageBoxResult;

                    if (button.Content.ToString() == "Select Task")
                    {
                        //Opening the task table window according to the engineer's permission
                        new TaskTableWindow(EngineerUser.ID).ShowDialog();
                        EngineerUser = s_bl.Engineer.Read(EngineerUser.ID);
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Do you want to mark the task as complete?", "Initialization Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if(result == MessageBoxResult.Yes) 
                        {
                            //update the task's status to complete.
                            BO.Task task = s_bl.Task.Read(EngineerUser.EngineerCurrentTask!.ID);
                            task.Status = BO.Status.Complete;
                            s_bl.Task.Update(task);

                            //update the current task of the engineer to be empty.  
                            s_bl.Engineer.Update(EngineerUser);
                            EngineerUser = s_bl.Engineer.Read(EngineerUser.ID);
                        }
                       
                    }

                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        ///  The function re-extracts the engineer.
        /// </summary>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ReadUser(EngineerUser.ID);
        }

        /// <summary>
        /// The function receives an engineer id and read it into EngineerUser.
        /// </summary>
        /// <param name="id">User id</param>
        private void ReadUser(int id)
        {
            try
            {
                EngineerUser = s_bl.Engineer.Read(id);
                SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
