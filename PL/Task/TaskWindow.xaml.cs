using BO;
using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        public static readonly DependencyProperty daysEffortTimeOptionsProperty = DependencyProperty.Register(
                                                                                   "daysEffortTimeOptions",
                                                                                   typeof(List<TimeSpan>),
                                                                                   typeof(TaskWindow),
                                                                                   new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }



        public List<TimeSpan> daysEffortTimeOptions
        {
            get { return (List<TimeSpan>)GetValue(daysEffortTimeOptionsProperty); }
            set { SetValue(daysEffortTimeOptionsProperty, value); }
        }
        public TimeSpan daysEffortTime;



        public static readonly DependencyProperty AssignedEngineerProperty = DependencyProperty.Register(
                                                                                                "AssignedEngineer",
                                                                                                typeof(IEnumerable<string>),
                                                                                                typeof(EngineerWindow),
                                                                                                new PropertyMetadata(null));


        //The list of all the engineers
        private IEnumerable<BO.Engineer> assignedEngineer = new List<BO.Engineer>();

        //The list of all the names of the engineers
        public List<string> AssignedEngineer
        {
            get { return (List<string>)GetValue(AssignedEngineerProperty); }
            set { SetValue(AssignedEngineerProperty, value); }
        }

        //The name selected in the ComboBox of Assigned Engineer
        private string _selectedEngineerName = "None";
        public string SelectedEngineerName
        {
            get => _selectedEngineerName;
            set
            {
                _selectedEngineerName = value;
                UpdateCurrentEngineerCurrentTask(_selectedEngineerName);
            }
        }

        /// <summary>
        /// Updating the EngineerCurrentTask field of the engineer according to the name of the task
        /// </summary>
        /// <param name="selectedTaskName">The name selected in the ComboBox of EngineerCurrentTask</param>
        private void UpdateCurrentEngineerCurrentTask(string selectedEngineerName)
        {
            if (CurrentTask != null && !string.IsNullOrEmpty(selectedEngineerName) && selectedEngineerName != "None")
            {
                BO.Engineer? engineerInTask = (from t in assignedEngineer
                                               where t.FullName == selectedEngineerName
                                               select t).FirstOrDefault();
                if (engineerInTask != null)
                {
                    BO.EngineerInTask engineerInTaskObj = new EngineerInTask(engineerInTask.ID, engineerInTask.FullName);
                    CurrentTask.AssignedEngineer = engineerInTaskObj;
                }
            }
        }


        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            InitializePopup();

            //According to the id we will update CurrentEngineer. If id ==0 - an empty engineer will be opened to be added. Otherwise we will pull out the engineer and open a window for updating
            if (id == 0)
            {
                List<BO.TaskInList> dependencies = new List<BO.TaskInList>();
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


            daysEffortTimeOptions = new List<TimeSpan>();
            // Populate the list with TimeSpan values for each day
            for (int day = 1; day <= 30; day++)
            {
                TimeSpan dayTimeSpan = TimeSpan.FromDays(day);
                daysEffortTimeOptions.Add(dayTimeSpan);
            }

            LoadAllEngineers();

            this.DataContext = this;
        }

        private void LoadAllEngineers()
        {
            assignedEngineer = s_bl.Engineer.ReadAll();

            AssignedEngineer = (from engineer in assignedEngineer
                                select engineer.FullName).ToList();
            AssignedEngineer.Add("None");
        }


        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                datePicker.DisplayDateStart = s_bl.GetProjectStartDate();
                datePicker.DisplayDateEnd = s_bl.GetProjectEndDate();
            }
        }


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
                        int id = s_bl.Task.Create(CurrentTask);
                        messageBoxResult = MessageBox.Show($"Task {id} added Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        s_bl.Task.Update(CurrentTask);
                        messageBoxResult = MessageBox.Show($"Task {CurrentTask.ID} updated Successfully!", "Happy Message :)", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private Popup popup;
        private void InitializePopup()
        {
            popup = new Popup
            {
                Width = 200,
                Height = 100,
                Placement = PlacementMode.Bottom,
                // This Popup should not have a visible border or background in this example.
                // You can customize the Popup's appearance by modifying its Child.
                Child = new TextBlock
                {
                    Text = "Hello, I'm a Popup!",
                    Background = Brushes.White,
                    Padding = new Thickness(10)
                }
            };
        }

        private void BtnListBoxPopup_Click(object sender, RoutedEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.IsOpen = false;
            }
            else
            {
                // Set the placement target of the popup to the button that was clicked.
                popup.PlacementTarget = sender as UIElement;
                popup.IsOpen = true;
            }
        }

    }
}
