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
    /// Interaction logic for TaskDetails.xaml
    /// </summary>
    public partial class TaskDetails : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty TaskViewProperty = DependencyProperty.Register(
                                                                                       "TaskView",
                                                                                        typeof(BO.Task),
                                                                                        typeof(TaskDetails),
                                                                                        new PropertyMetadata(null));

        public BO.Task TaskView
        {
            get { return (BO.Task)GetValue(TaskViewProperty); }
            set { SetValue(TaskViewProperty, value); }
        }

        public TaskDetails(int id)
        {
            InitializeComponent();
            try
            {
                TaskView = s_bl.Task.Read(id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR :(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
