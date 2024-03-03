using PL.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL
{
    public static class SharedDependencyProperties
    {
        #region ProjectStatusProperty
        public static readonly DependencyProperty ProjectStatusProperty = DependencyProperty.RegisterAttached(
                                                                                            "ProjectStatus",
                                                                                            typeof(BO.ProjectStatus),
                                                                                            typeof(SharedDependencyProperties),
                                                                                            new PropertyMetadata(null));

        public static void SetProjectStatus(DependencyObject element, BO.ProjectStatus value)
        {
            element.SetValue(ProjectStatusProperty, value);
        }

        public static BO.ProjectStatus GetProjectStatus(DependencyObject element)
        {
            return (BO.ProjectStatus)element.GetValue(ProjectStatusProperty);
        }

        #endregion

        #region ClockProperty
        public static readonly DependencyProperty ClockProperty = DependencyProperty.RegisterAttached(
                                                                                    "Clock",
                                                                                    typeof(DateTime),
                                                                                    typeof(SharedDependencyProperties),
                                                                                    new PropertyMetadata(default(DateTime)));

        public static void SetClock(DependencyObject element, DateTime value)
        {
            element.SetValue(ClockProperty, value);
        }

        public static DateTime GetClock(DependencyObject element)
        {
            return (DateTime)element.GetValue(ClockProperty);
        }

        #endregion
    }
}
