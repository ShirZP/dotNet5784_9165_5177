using BO;
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
using System.Collections;
using System.ComponentModel;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty EngineerProperty = DependencyProperty.Register(
                                                                                        "CurrentEngineer",
                                                                                        typeof(BO.Engineer),
                                                                                        typeof(EngineerWindow),
                                                                                        new PropertyMetadata(null));


        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerTasksNameProperty = DependencyProperty.Register(
                                                                                                "engineerTasksName",
                                                                                                typeof(IEnumerable<string>),
                                                                                                typeof(EngineerWindow),
                                                                                                new PropertyMetadata(null));


        //The list of all the tasks that the engineer is registered for
        private IEnumerable<BO.TaskInList> engineerTasksInList = new List<BO.TaskInList>();

        //The list of all the names of the tasks that the engineer is registered on
        public List<string> engineerTasksName
        {
            get { return (List<string>)GetValue(EngineerTasksNameProperty); }
            set { SetValue(EngineerTasksNameProperty, value); }
        }

        //The name selected in the ComboBox of EngineerCurrentTask
        private string _selectedTaskName = "None";
        public string SelectedTaskName
        {
            get => _selectedTaskName;
            set
            {
                _selectedTaskName = value;
                UpdateCurrentEngineerCurrentTask(_selectedTaskName);
            }
        }

        /// <summary>
        /// Updating the EngineerCurrentTask field of the engineer according to the name of the task
        /// </summary>
        /// <param name="selectedTaskName">The name selected in the ComboBox of EngineerCurrentTask</param>
        private void UpdateCurrentEngineerCurrentTask(string selectedTaskName)
        {
            if (CurrentEngineer != null && !string.IsNullOrEmpty(selectedTaskName) && selectedTaskName != "None")
            {
                BO.TaskInList? taskInList = (from t in engineerTasksInList
                                             where t.NickName == selectedTaskName
                                             select t).FirstOrDefault();
                if (taskInList != null)
                {
                    BO.TaskInEngineer taskInEngineer = new TaskInEngineer(taskInList.ID, taskInList.NickName);
                    CurrentEngineer.EngineerCurrentTask = taskInEngineer;
                }
            }
        }


        public EngineerWindow(int id = 0)
        {
            InitializeComponent();

            //According to the id we will update CurrentEngineer. If id == 0 - an empty engineer will be opened to be added. Otherwise we will pull out the engineer and open a window for updating
            if (id == 0)
            {
                CurrentEngineer = new BO.Engineer(0, "", "", BO.EngineerExperience.Beginner, null, null);
            }
            else
            {
                try
                {
                    CurrentEngineer = s_bl?.Engineer.Read(id)!;
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            LoadAllTasksInList();
            if(CurrentEngineer.EngineerCurrentTask != null)
            {
                SelectedTaskName = CurrentEngineer.EngineerCurrentTask.NickName;

            }
            
            this.DataContext = this;
        }

        /// <summary>
        /// Updating the engineer's task list
        /// </summary>
        private void LoadAllTasksInList()
        {
            engineerTasksInList = s_bl.Task.ReadAll(item => item.AssignedEngineer != null &&
                                                            item.AssignedEngineer.ID == CurrentEngineer.ID &&
                                                            item.Status != BO.Status.Complete);

            engineerTasksName = (from task in engineerTasksInList
                                 select task.NickName).ToList();
            engineerTasksName.Add("None");
        }

        /// <summary>
        /// Add/update engineer
        /// </summary>
        private void BtnAddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button != null)
            {
                try
                {
                    MessageBoxResult messageBoxResult;

                    if (button.Content.ToString() == "Add")
                    {
                        s_bl.Engineer.Create(CurrentEngineer);
                        messageBoxResult = MessageBox.Show($"Engineer {CurrentEngineer.ID} added Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        s_bl.Engineer.Update(CurrentEngineer);
                        messageBoxResult = MessageBox.Show($"Engineer {CurrentEngineer.ID} updated Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// The function allows you to type only numbers.
        /// </summary>
        private void CheckValidInt(object sender, TextCompositionEventArgs e)
        {
            if (!(int.TryParse(e.Text, out _)))
            {
                e.Handled = true;
            }
        }

        
    }
}
