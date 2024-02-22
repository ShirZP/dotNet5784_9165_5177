using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty TaskProperty = DependencyProperty.Register(
                                                                                        "CurrentTask",
                                                                                        typeof(BO.Task),
                                                                                        typeof(TaskWindow),
                                                                                        new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public TaskWindow(int id = 0)
        {
            InitializeComponent();

            //According to the id we will update CurrentEngineer. If id ==0 - an empty engineer will be opened to be added. Otherwise we will pull out the engineer and open a window for updating
            if (id == 0)
            {
                IEnumerable<BO.TaskInList> dependencies = new List<BO.TaskInList>();
                CurrentTask = new BO.Task(0, "", "", BO.Status.New, dependencies, null, null, null, null, null, null, null, null, BO.EngineerExperience.Beginner);
            }
            else
            {
                try
                {
                    CurrentTask = s_bl?.Task.Read(id)!;
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            this.DataContext = this;
        }
    }
}
