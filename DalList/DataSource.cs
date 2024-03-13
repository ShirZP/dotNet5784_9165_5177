namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal static DateTime? projectStartDate = null;
        internal static DateTime? projectEndDate = null;
        internal static DO.ProjectStatus projectStatus = DO.ProjectStatus.Planning;
        internal static DateTime clock = DateTime.Now;

       //Unique ID number for a Task
       internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        //Unique ID number for a Dependency
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

    }

    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.User> Users { get; } = new();
}
