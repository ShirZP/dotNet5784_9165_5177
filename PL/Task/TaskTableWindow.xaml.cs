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
using System.Collections;

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
            var button = sender as Button;
            BO.Task? task = button!.DataContext as BO.Task;
            if (task != null)
            {
                new TaskWindow(task.ID).ShowDialog();
            }


        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Task? task = (sender as DataGrid)?.SelectedItem as BO.Task;
            if (task != null)
            {
                new TaskWindow(task.ID).Show();
            }
        }

        private void filterChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox FirstComboBox)
            {
                if(SubcategoryFilter_CB != null)
                     SubcategoryFilter_CB.Items.Clear();

                    string selectedContent = FirstComboBox.SelectedValue.ToString();

                    // Temporarily store the enum values
                    Array? enumValues = null;

                    switch (selectedContent)
                    {
                        case "Status":
                            enumValues = Enum.GetValues(typeof(BO.Status));
                            break;

                        case "AssignedEngineer":
                            enumValues = (from engineer in s_bl.Engineer.ReadAll()
                                          select engineer.FullName).ToArray();
                            break;

                        case "Complexity":
                            enumValues = Enum.GetValues(typeof(BO.EngineerExperience));
                            break;

                        case "All":

                            break;
                    }

                    // Populate the SecondComboBox with the enum values
                    if (enumValues != null)
                    {
                        foreach (var value in enumValues)
                        {
                            SubcategoryFilter_CB.Items.Add(new ComboBoxItem { Content = value.ToString() });
                        }
                    }
                

            }
        }




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
