namespace BO;

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
public class BlPositiveIntException : Exception  //בעיה עם מספר חיובי
{
    public BlPositiveIntException(string? message) : base(message) { }
}

[Serializable]
public class BlEmptyStringException : Exception  //בעיה עם מחרוזת ריקה
{
    public BlEmptyStringException(string? message) : base(message) { }
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

