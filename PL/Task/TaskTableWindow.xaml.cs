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
            SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());
            SubcategoryFilter_CB.IsEnabled = false;
            this.DataContext = this;
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
            
            if(SubcategoryFilter_CB != null)
                SubcategoryFilter_CB.Items.Clear();

            string selectedContent = MainCategoryFilter_CB.SelectedValue.ToString();

            // Temporarily store the values
            Array? values = null;

            switch (selectedContent)
            {
                case "Status":
                    SubcategoryFilter_CB.IsEnabled = true;
                    values = Enum.GetValues(typeof(TaskStatusFilter));
                    break;

                case "AssignedEngineer":
                    SubcategoryFilter_CB.IsEnabled = true;
                    values = (from engineer in s_bl.Engineer.ReadAll()
                                    select engineer.FullName).ToArray();

                    values = addNewItemToArray(values, "All");
                    break;

                case "Complexity":
                    SubcategoryFilter_CB.IsEnabled = true;
                    values = Enum.GetValues(typeof(BO.EngineerExperience));
                    break;

                case "All":
                    if (SubcategoryFilter_CB != null)
                        SubcategoryFilter_CB.IsEnabled = false;
                    break;
            }

            // Populate the SecondComboBox with the enum values
            if (values != null)
            {
                foreach (var value in values)
                {
                    SubcategoryFilter_CB.Items.Add(new ComboBoxItem { Content = value.ToString() });
                }

                SubcategoryFilter_CB.SelectedIndex = values.Length - 1;
            }
                          
        }

        private Array addNewItemToArray(Array arr, string item)
        {
            string[] newArr = new string[arr.Length + 1];

            // Copy values from the original array to the new array
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = (String)arr.GetValue(i);
            }

            // Add the new value to the end of the new array
            newArr[newArr.Length - 1] = item;

            return newArr;
        }

        private void filterTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string subFieldFilter = (SubcategoryFilter_CB.SelectedValue as ComboBoxItem)?.Content?.ToString();

            if (subFieldFilter != null)
            {
                if (subFieldFilter == "All")
                {
                    TasksList = s_bl?.Task.ReadAllFullTasksDetails();
                }
                else
                {

                    switch (Category)
                    {
                        case TaskFieldsToFilter.Status:
                            TasksList = s_bl?.Task.ReadAllFullTasksDetails(item => item.Status == (BO.Status)Enum.Parse(typeof(BO.Status), subFieldFilter))!;
                            break;

                        case TaskFieldsToFilter.AssignedEngineer:
                            TasksList = s_bl?.Task.ReadAllFullTasksDetails(item => item.AssignedEngineer != null && item.AssignedEngineer.Name == subFieldFilter)!;
                            break;

                        case TaskFieldsToFilter.Complexity:
                            TasksList = s_bl?.Task.ReadAllFullTasksDetails(item => item.Complexity == (BO.EngineerExperience)Enum.Parse(typeof(BO.EngineerExperience), subFieldFilter))!;
                            break;

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
