using DO;

namespace BO;

public class User
{
    public int ID { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public BO.UserPermissions Permission {  get; set; }

    public User(int id, string userName, string password, BO.UserPermissions permission)
    {
        ID = id;
        UserName = userName;
        Password = password;
        Permission = permission;
    }
}
