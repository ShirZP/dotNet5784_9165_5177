using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class UserImplementation : IUser
{
    /// <summary>
    /// The function clears all the users from the list.
    /// </summary>
    public void Clear()
    {
        foreach (DO.User User in DataSource.Users)
        {
            if (User.Permission == UserPermissions.Engineer)
            {
                DataSource.Users.Remove(User);
            }
        }
       
    }

    /// <summary>
    /// The function adds a new user to the list of users.
    /// </summary>
    /// <param name="user">New user to add to the usersList</param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException">if the user is already exist in the user list</exception>
    public int Create(User user)
    {
        //Checking if there is already an user with such an id in the list.
        if (DataSource.Users.Find(u => u.ID == user.ID) != default(User))
        {
            throw new DalAlreadyExistsException($"An user with ID={user.ID} already exist");
        }
        else
        {
            DataSource.Users.Add(user);
        }

        return user.ID;
    }

    /// <summary>
    /// The function delete the received user.
    /// </summary>
    /// <param name="id">The ID of the user we want to delete</param>
    /// <exception cref="DalDoesNotExistException">If the user you want to delete does not exist in the list</exception>
    public void Delete(int id)
    {
        User? user = DataSource.Users.Find(u => u.ID == id);

        if (user != null)
        {
            DataSource.Users.Remove(user);
        }
        else
        {
            throw new DalDoesNotExistException($"An user with ID={id} does not exist");
        }
    }

    public string PasswordHash(string str)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The function returns a reference to the user with the requested filter
    /// </summary>
    /// <param name="filter">delegate func that receives user and returns bool</param>
    /// <returns>reference to the user with the requested filter</returns>
    public User? Read(Func<User, bool> filter)
    {
        return DataSource.Users.FirstOrDefault(filter);
    }
}
