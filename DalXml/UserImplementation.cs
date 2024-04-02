using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";

    public void Clear()
    {
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        List<DO.User> newList = (from user in UsersList
                                 where user.Permission == UserPermissions.Manager
                                 select user).ToList();

        XMLTools.SaveListToXMLSerializer(newList, s_users_xml);
    }
    public int Create(User user)
    {
        //Deserialize
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        if (UsersList.Find(u => u.ID == user.ID) != default(DO.User))
            throw new DalAlreadyExistsException($"An user with ID={user.ID} already exist");

        User HashUser = user with { Password = PasswordHash(user.Password) };
        UsersList.Add(HashUser);

        //Serialize
        XMLTools.SaveListToXMLSerializer<DO.User>(UsersList, s_users_xml);

        return user.ID;
    }

    public void Delete(int id)
    {
        //Deserialize
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        DO.User? user = UsersList.Find(u => u.ID == id);
        if (user != null)
        {
            UsersList.Remove(user);
            //Serialize
            XMLTools.SaveListToXMLSerializer<DO.User>(UsersList, s_users_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"An User with ID={id} does not exist");
        }
    }

    public User? Read(Func<User, bool> filter)
    {
        //Deserialize
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        return UsersList.FirstOrDefault(filter);
    }

    /// <summary>
    /// The function receives a string and hashes it
    /// </summary>
    /// <returns>hashed string</returns>
    public string PasswordHash(string str)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hashBytes); // Convert byte array directly to hex string
        }
    }
}
