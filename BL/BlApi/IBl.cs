namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }

    //public BO.ProjectStatus GetProjectStatus();

    //public IMilestone Milestone { get; }   //TODO: 
}
