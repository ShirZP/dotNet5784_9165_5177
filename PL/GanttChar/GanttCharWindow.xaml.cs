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

            this.DataContext = this;
        
        }


        private TaskGantt convertTaskToGanttTask(BO.Task task)
        {
            int dateDurationSize = 70;

            DateTime? projectStartDate = s_bl.GetProjectStartDate();
            DateTime? projectEndDate = s_bl.GetProjectEndDate();

            DateTime? start = task.StartDate == null ? task.ScheduledDate.Value.Date : task.StartDate.Value.Date;
            DateTime? end = task.CompleteDate == null ? task.ForecastDate.Value.Date : task.CompleteDate.Value.Date;
           
            TimeSpan? effort;
            if(task.StartDate != null && task.CompleteDate != null)
            {
                effort = (task.CompleteDate - task.StartDate);
            }
            else if(task.StartDate != null && task.CompleteDate == null)
            {
                effort = (task.ForecastDate - task.StartDate);
            }
            else
            {
                effort = task.RequiredEffortTime;
            }


            int duration = (int)effort!.Value.Days + 1;
            double timeFromStart = (start - projectStartDate)!.Value.TotalDays;
            double timeToEnd = ((projectEndDate!.Value - end).Value.TotalDays == 0) ? 0 : (projectEndDate!.Value - end)!.Value.TotalDays + 1;

            return new TaskGantt(){taskID = task.ID,
                                   taskName = task.NickName, 
                                   taskStatus = task.Status,
                                   duration = duration * dateDurationSize + duration + 1,
                                   timeFromStart = timeFromStart * dateDurationSize + timeFromStart + 1,
                                   timeToEnd = timeToEnd * dateDurationSize + timeToEnd + 1};
           
        
        }

        private void loadGanttDatesList()
        {
             GanttDatesList = new List<DateGantt>();

            for (DateTime date = s_bl.GetProjectStartDate().Value; date.Date < s_bl.GetProjectEndDate()!.Value.Date; date = date.AddDays(1))
            {
                GanttDatesList.Add(new DateGantt() { Date = date });
            }
        }
    }
}
