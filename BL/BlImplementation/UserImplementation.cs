using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.User user)
    {
        try
        {
            DO.User newUser = new DO.User(user.ID, user.UserName, user.Password, (DO.UserPermissions)user.Permission);
            return _dal.User.Create(newUser);
        }
        catch (DO.DalAlreadyExistsException dalEx)
        {
            throw new BO.BlAlreadyExistsException($"An user with ID = {user.ID} already exist", dalEx);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException dalEx)
        {
            throw new BO.BlDoesNotExistException($"An user with ID = {id} does not exist", dalEx);
        }
    }

    
    public User Read(string userName, string password)
    {
        //Retrieving the User from the data layer
        DO.User? dalUser = _dal.User.Read(item => item.UserName == userName && item.Password == _dal.User.PasswordHash(password));

        if (dalUser == null)
            throw new BO.BlDoesNotExistException("Username or password is incorrect");

        return new BO.User(dalUser.ID, dalUser.UserName, dalUser.Password, (BO.UserPermissions)dalUser.Permission);
    }
}
