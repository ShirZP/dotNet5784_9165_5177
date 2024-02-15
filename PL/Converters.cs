﻿using System.Globalization;
using System.Windows.Data;
using System.Collections;
using System.Windows;


namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //TODO: להוסיף בדיקה למספר רץ - ואז תמיד לא ניתן לעריכה
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//TODO: תיבת בחירה למשימה נוכחית של מהנדס
//internal class EngineerTasksCollection : IEnumerable
//{
//    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

//    static readonly IEnumerable<BO.TaskInList> s_t = (s_bl.Task.ReadAll(item=> item.AssignedEngineer.ID == (?)  as IEnumerable<BO.EngineerExperience>)!;

//    public IEnumerator GetEnumerator() => s_t.GetEnumerator();
//}

