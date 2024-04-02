namespace DalApi;
using DO;

public interface IUser
{
    int Create(User user); //Creates new entity object in DAL
    User? Read(Func<User, bool> filter); //Reads entity object by filter 
    void Delete(int id); //Deletes an object by its Id
    void Clear(); //Clear all the objects from the data
    string PasswordHash(string str); //Hash the password of the user
}
