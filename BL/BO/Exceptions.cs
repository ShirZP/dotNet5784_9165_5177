namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception  //אובייקט לא קיים
{
    public BlDoesNotExistException(string? message) : base(message) { }
}


[Serializable]
public class BlAlreadyExistsException : Exception  //אובייקט כבר קיים
{
    public BlAlreadyExistsException(string? message) : base(message) { }
}


[Serializable]
public class BlXMLFileLoadCreateException : Exception  //לא מצליח לטעון קובץ xml 
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
}
