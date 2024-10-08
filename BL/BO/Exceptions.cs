﻿namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception  //אובייקט לא קיים
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }

}


[Serializable]
public class BlAlreadyExistsException : Exception  //אובייקט כבר קיים
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

}


[Serializable]
public class BlXMLFileLoadCreateException : Exception  //לא מצליח לטעון קובץ xml 
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException) : base(message, innerException) { }

}

[Serializable]
public class BlIntException : Exception  //בעיה עם מספר
{
    public BlIntException(string? message) : base(message) { }
}

[Serializable]
public class BlStringException : Exception  //בעיה עם מחרוזת
{
    public BlStringException(string? message) : base(message) { }
}


[Serializable]
public class BlEmptyEnumException : Exception  //problem with empty enum 
{
    public BlEmptyEnumException(string? message) : base(message) { }
}

[Serializable]
public class BlCompleteOrActiveTasksException : Exception  //רשום למשימה נוכחית או שכבר ביצע משימות
{
    public BlCompleteOrActiveTasksException(string? message) : base(message) { }
}


[Serializable]
public class BlThereIsADependencyOnTheTaskException : Exception  //רשום למשימה נוכחית או שכבר ביצע משימות
{
    public BlThereIsADependencyOnTheTaskException(string? message) : base(message) { }
}


[Serializable]
public class BlEngineerNotAssignedToTaskException : Exception  //מהנדס לא מוקצה למשימה 
{
    public BlEngineerNotAssignedToTaskException(string? message) : base(message) { }
}

[Serializable]
public class BlNullScheduledDateInDependensOnTaskException : Exception  //תאריך ההתחלה במתוכנן ריק במשימה שתלויים בה
{
    public BlNullScheduledDateInDependensOnTaskException(string? message) : base(message) { }
}

[Serializable]
public class BlDependentsTasksException : Exception //בעיה במשימות קודמות
{
    public BlDependentsTasksException(string? message) : base(message) { }
}

[Serializable]
public class BlInappropriateLevelException : Exception //רמה לא מתאימה
{
    public BlInappropriateLevelException(string? message) : base(message) { }
}

[Serializable]
public class BlProjectStatusException : Exception  //בעיה בסטטוס הפרויקט
{
    public BlProjectStatusException(string? message) : base(message) { }
}


[Serializable]
public class BlscheduledDateException : Exception  //בעיה בתאריך מתוכנן
{
    public BlscheduledDateException(string? message) : base(message) { }
}

[Serializable]
public class BlStatusException : Exception  //בעיה בסטטוס
{
    public BlStatusException(string? message) : base(message) { }
}

