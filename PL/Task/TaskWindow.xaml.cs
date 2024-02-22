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


        public List<TimeSpan> daysEffortTimeOptions = new List<TimeSpan>();

        


    public TaskWindow(int id = 0)
        {
            InitializeComponent();

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

            
            // Populate the list with TimeSpan values for each day
            for (int day = 1; day <= 30; day++)
            {
                TimeSpan dayTimeSpan = TimeSpan.FromDays(day);
                daysEffortTimeOptions.Add(dayTimeSpan);
            }

            this.DataContext = this;
        }


        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                datePicker.DisplayDateStart = s_bl.GetProjectStartDate();
                datePicker.DisplayDateEnd = s_bl.GetProjectEndDate();
            }
        }











    //    private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
    //    {
    //        if (sender is DatePicker datePicker)
    //        {
    //            // Validate the entered date
    //            if (!IsValidDate(datePicker.Text))
    //            {
    //                // Display error message
    //                FindTextBlock().Text = "Invalid date. Please enter a valid date.";
    //            }
    //            else
    //            {
    //                // Clear error message if the date is valid
    //                FindTextBlock().Text = string.Empty;
    //            }
    //        }
    //    }

    //    private bool IsValidDate(string input)
    //    {
    //        DateTime enteredDate;

    //        if(!DateTime.TryParse(input, out enteredDate))
    //        {
    //            return false;
    //        }
            
    //        DateTime? startDate = s_bl.GetProjectStartDate();
    //        DateTime? endDate = s_bl.GetProjectEndDate();

    //        // Check if the entered date is within the allowed range
    //       return (enteredDate < startDate || enteredDate > endDate) ?  true :  false;

    //    }

    //    private TextBlock FindTextBlock()
    //    {
    //        foreach (UIElement child in ((Grid)Content).Children)
    //        {
    //            if (child is TextBlock textBlock)
    //            {
    //                return textBlock;
    //            }
    //        }
    //        return null;
    //    }
    //}
}
