namespace BO;

public class Engineer
{
    /// <summary>
    /// The engineer's unique personal identity card (as in a national identity card)
    /// </summary>
    public int ID { get; init; }

    /// <summary>
    /// Engineer's first and last name
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// The email address of the engineer
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The level of expertise of the engineer
    /// </summary>
    public BO.EngineerExperience Level { get; set; }

    /// <summary>
    /// How much per hour does the engineer get
    /// </summary>
    public double? Cost { get; set; }

    /// <summary>
    /// Current task ID and nickname for the employee - if exist
    /// </summary>
    public TaskInEngineer? EngineerCurrentTask { get; set; }

    /// <summary>
    /// Engineer Ctor with parameters.
    /// </summary>
    public Engineer(int id, string fullName, string email, BO.EngineerExperience level, double? cost, TaskInEngineer? engineerCurrentTask)
    {
        ID = id;
        FullName = fullName;
        Email = email;
        Level = level;
        Cost = cost;
        EngineerCurrentTask = engineerCurrentTask;
    }


    public override string ToString() => Tools.ToStringProperty(this);
}
