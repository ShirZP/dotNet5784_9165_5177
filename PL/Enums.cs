using System.Collections;
using System.Linq;

namespace PL;


internal class ExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


internal class EngineerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)).Cast<BO.EngineerExperience>().Where(level => level != BO.EngineerExperience.All) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class TaskFieldsToFilterCollection : IEnumerable
{
    static readonly IEnumerable<TaskFieldsToFilter> s_enums = (Enum.GetValues(typeof(TaskFieldsToFilter)) as IEnumerable<TaskFieldsToFilter>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class TaskStatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums = (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

#region Enums
public enum TaskFieldsToFilter
{
    Status,
    AssignedEngineer,
    Complexity,
    All
}

public enum TaskStatusFilter
{
    New,
    Active,
    Complete,
    All
}

public enum GanttTaskStatus
{
    New,
    Active,
    Complete,
    Delayed
}

#endregion
