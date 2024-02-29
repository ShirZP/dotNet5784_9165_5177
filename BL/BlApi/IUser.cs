namespace BlApi;

public interface IUser
{
    int Create(BO.User user);
    BO.User Read(string userName, string password);
    void Delete(int id);
}
