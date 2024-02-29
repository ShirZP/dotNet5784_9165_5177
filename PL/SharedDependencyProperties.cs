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
    }
}
