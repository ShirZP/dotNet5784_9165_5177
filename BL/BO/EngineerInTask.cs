namespace BO;

public class EngineerInTask
{
    /// <summary>
    /// ID of the engineer 
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Name of the engineer
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// EngineerInTask Ctor with parameters.
    /// </summary>
    public EngineerInTask(int id, string name)
    {
        ID = id;
        Name = name;
    }

    public override string ToString() => Tools.ToStringProperty(this);
}


