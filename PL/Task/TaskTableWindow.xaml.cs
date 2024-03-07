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

        public static readonly DependencyProperty UserIDPermissionProperty = DependencyProperty.Register(
                                                                                        "UserIDPermission",
                                                                                        typeof(int),
                                                                                        typeof(TaskTableWindow),
                                                                                        new PropertyMetadata(null));

        public IEnumerable<BO.Task> TasksList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TasksListProperty); }
            set { SetValue(TasksListProperty, value); }
        }

        public int UserIDPermission
        {
            get { return (int)GetValue(UserIDPermissionProperty); }
            set { SetValue(UserIDPermissionProperty, value); }
        }

        public TaskFieldsToFilter Category { get; set; } = TaskFieldsToFilter.All;


        public TaskTableWindow(int id = 0)
        {
            InitializeComponent();
            SharedDependencyProperties.SetProjectStatus(this, s_bl.GetProjectStatus());
            SubcategoryFilter_CB.IsEnabled = false;
            UserIDPermission = id;
            this.DataContext = this;
        }

        /// <summary>
        /// Refresh the TaskTableWindow
        /// </summary>
        private void RefreshWindow_Activated(object sender, EventArgs e)
        {
            if(UserIDPermission == 0) 
            {
                TasksList = s_bl?.Task.ReadAllFullTasksDetails()!;
            }
            else
            {
                BO.Engineer engineerUser = s_bl.Engineer.Read(UserIDPermission);

                TasksList = (from task in s_bl?.Task.ReadAllFullTasksDetails()!
                             where task.AssignedEngineer == null && task.Complexity <= engineerUser.Level && taskDependenciesComplete(task)
                             select task).ToList(); 
            }
        }

        /// <summary>
        /// The function checks whether the task has completed dependencies.
        /// </summary>
        private bool taskDependenciesComplete(BO.Task task)
        {
            BO.TaskInList? notCompleteTask = (from dependency in task.Dependencies
                                       where dependency.Status != BO.Status.Complete
                                       select dependency).FirstOrDefault(); 
            if(notCompleteTask != null)
            {
                return false;   
            }

            return true;
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DgSelectTask_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (sender is DataGrid dataGrid && dataGrid.SelectedItem is BO.Task selectedTask)
                {
                    if (UserIDPermission == 0)
                    {
                        new TaskDetails(selectedTask.ID).ShowDialog();
                    }
                    else
                    {
                        BO.Engineer engineer = s_bl.Engineer.Read(UserIDPermission);
                        engineer.EngineerCurrentTask = new BO.TaskInEngineer(selectedTask.ID, selectedTask.NickName);
                        s_bl.Engineer.Update(engineer);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
