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



        public GanttCharWindow()
        {
            InitializeComponent();

            loadGanttDatesList();

            GanttTasksList = (from task in s_bl.Task.ReadAllFullTasksDetails()
                              select convertTaskToGanttTask(task)).ToList();

            //GanttTasksList = new List<TaskGantt>()
            //{new TaskGantt() {taskID = 1,taskName = "T1",taskStatus = BO.Status.New, duration = 3*60, timeFromStart = 20,    timeToEnd = 7},
            //new TaskGantt() {taskID = 2,taskName = "T2",taskStatus = BO.Status.Active, duration = 5*60, timeFromStart = 60,    timeToEnd = 4},
            //new TaskGantt() { taskID = 3, taskName = "T3",taskStatus = BO.Status.Complete, duration = 2*60, timeFromStart = 10, timeToEnd = 13 }
            //};

            this.DataContext = this;
        
        }


        private TaskGantt convertTaskToGanttTask(BO.Task task)
        {
            int dateDurationSize = 70;

            DateTime? projectStartDate = s_bl.GetProjectStartDate();
            DateTime? projectEndDate = s_bl.GetProjectEndDate();
            int duration = (int)task.RequiredEffortTime!.Value.Days;
            int timeFromStart = (int)(task.ScheduledDate - projectStartDate)!.Value.TotalDays;
            int timeToEnd = (int)(projectEndDate - task.ForecastDate)!.Value.TotalDays;

            return new TaskGantt(){taskID = task.ID,taskName = task.NickName, taskStatus = task.Status, duration = duration * dateDurationSize,  timeFromStart = timeFromStart * dateDurationSize, timeToEnd = timeToEnd * dateDurationSize };
           
        
        }

        private void loadGanttDatesList()
        {
             GanttDatesList = new List<DateGantt>();

            for (DateTime date = s_bl.GetProjectStartDate().Value; date <= s_bl.GetProjectEndDate(); date = date.AddDays(1))
            {
                GanttDatesList.Add(new DateGantt() { Date = date });
            }
        }
    }
}
