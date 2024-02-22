using System;
using PL;
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
    /// Interaction logic for TaskTableWindow.xaml
    /// </summary>
    public partial class TaskTableWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty TasksListProperty = DependencyProperty.Register(
                                                                                         "TasksList",
                                                                                         typeof(IEnumerable<BO.Task>),
                                                                                         typeof(TaskTableWindow),
                                                                                         new PropertyMetadata(null));

        public IEnumerable<BO.Task> TasksList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }


        public TaskFieldsToFilter Category { get; set; } = TaskFieldsToFilter.All;


        public TaskTableWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Refresh the TaskTableWindow
        /// </summary>
        private void RefreshWindow_Activated(object sender, EventArgs e)
        {
            TasksList = s_bl?.Task.ReadAllFullTasksDetails()!;
        }

        private void PenButton_Click(object sender, RoutedEventArgs e)
        {
            //BO.Task? task = (sender as DataGrid)?.SelectedItem as BO.Task;
            //if (task != null)
            //{
                new TaskWindow().Show();
            //}

            //private void cbFilterSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
            //{
            //    if (Category == TaskFieldsToFilter.All)
            //        TasksList = s_bl?.Task.ReadAllFullTasksDetails()!;
            //    else if (Category == TaskFieldsToFilter.Status)
            //    {

            //    }
            //}
        }
    }
}
