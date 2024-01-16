namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception  //אובייקט לא קיים
{
    public DalDoesNotExistException(string? message) : base(message) { }
}


[Serializable]
public class DalAlreadyExistsException : Exception  //אובייקט כבר קיים
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}


[Serializable]
public class DalXMLFileLoadCreateException : Exception  //לא מצליח לטעון קובץ xml 
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}

