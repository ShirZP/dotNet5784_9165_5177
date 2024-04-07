using PL.Engineer;
using PL.PO;
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

namespace PL.GanttChar
{
    /// <summary>
    /// Interaction logic for GanttChar.xaml
    /// </summary>
    public partial class GanttCharWindow : Window
    {
        #region DependencyProperties
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty GanttTasksListProperty = DependencyProperty.Register("GanttTasksList",
                                                                                                        typeof(List<TaskGantt>),
                                                                                                        typeof(GanttCharWindow),
                                                                                                        new PropertyMetadata(null));

        public List<TaskGantt> GanttTasksList
        {
            get { return (List<TaskGantt>)GetValue(GanttTasksListProperty); }
            set { SetValue(GanttTasksListProperty, value); }
        }



        public static readonly DependencyProperty GanttDatesListProperty = DependencyProperty.Register("GanttDatesList",
                                                                                                       typeof(List<DateGantt>),
                                                                                                       typeof(GanttCharWindow),
                                                                                                       new PropertyMetadata(null));

        public List<DateGantt> GanttDatesList
        {
            get { return (List<DateGantt>)GetValue(GanttDatesListProperty); }
            set { SetValue(GanttDatesListProperty, value); }
        }

        #endregion

        public GanttCharWindow()
        {
            InitializeComponent();

            loadGanttDatesList();

            //Creating a list of TaskGantt tasks and sorting them by ID
            GanttTasksList = (from task in s_bl.Task.SortByID()
                              select convertTaskToGanttTask(task)).ToList();

            this.DataContext = this;
        
        }

        #region Methods

        /// <summary>
        /// The function returns a list of all dates from the beginning to the end of the project
        /// </summary>
        private void loadGanttDatesList()
        {
            GanttDatesList = new List<DateGantt>();

            for (DateTime date = s_bl.GetProjectStartDate().Value; date.Date <= s_bl.GetProjectEndDate()!.Value.Date; date = date.AddDays(1))
            {
                GanttDatesList.Add(new DateGantt() { Date = date });
            }
        }

        /// <summary>
        /// The function accepts a BO.Task task and returns a TaskGantt task.
        /// </summary>
        private TaskGantt convertTaskToGanttTask(BO.Task task)
        {
            int dateDurationSize = 70;

            DateTime? projectStartDate = s_bl.GetProjectStartDate()!.Value.Date;
            DateTime? projectEndDate = s_bl.GetProjectEndDate()!.Value.Date;

            GanttTaskStatus ganttTaskStatus = (task.Status == BO.Status.Active && task.ForecastDate!.Value.Date < s_bl.GetClock().Date) ? GanttTaskStatus.Delayed : (GanttTaskStatus)task.Status;

            //The list of dependencies for the ToolTip in the Gantt
            List<string> dependenciesName = (from d in  task.Dependencies
                                              select d.NickName).ToList();
            dependenciesName.Insert(0, "Task Dependencies:");


            int duration = (int)task.RequiredEffortTime!.Value.Days + 1;
            double timeFromStart = (task.ScheduledDate - projectStartDate)!.Value.TotalDays;

            double timeToEnd = (projectEndDate - projectStartDate)!.Value.TotalDays - timeFromStart - duration + 2;

            return new TaskGantt(){TaskID = task.ID,
                                   TaskName = task.NickName,
                                   TaskStatus = ganttTaskStatus,
                                   DependenciesName = dependenciesName,
                                   Duration = duration * dateDurationSize + duration + 1,
                                   TimeFromStart = timeFromStart * dateDurationSize + timeFromStart + 1,
                                   TimeToEnd = timeToEnd * dateDurationSize + timeToEnd + 1};
           
        
        }

        #endregion

        /// <summary>
        /// The function close the window.
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }
    }
}
