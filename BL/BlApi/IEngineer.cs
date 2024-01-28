namespace BlApi;

public interface IEngineer
{
    //TODO: לשאול האם BO/DO בשליחה
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);  //Reads all engineers by filter

    public BO.Engineer Read(int id); //Reads engineer by id

    public int Create(BO.Engineer engineer); //Add new engineer

    public void Update(BO.Engineer UpdatedEngineer); //Updates engineer

    public void Delete(int id);  //Deletes a engineer by its id
}
