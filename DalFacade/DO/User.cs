namespace DO;
/// <summary>
/// User entity to enter the system
/// </summary>
/// <param name="UserName">User name</param>
/// <param name="Password">User password</param>
/// <param name="permission">User permission to log in</param>
public record User
(
    int ID,
    string UserName,
    string Password,
    DO.UserPermissions Permission
)
{
    public User() : this(0, "", "", DO.UserPermissions.Engineer) { }
}


