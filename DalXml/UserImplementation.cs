using DalApi;
using DO;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";

    public void Clear()
    {
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        UsersList.Clear();
        XMLTools.SaveListToXMLSerializer(UsersList, s_users_xml);
    }

    public int Create(User user)
    {
        //Deserialize
        List<DO.User> UsersList = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);

        if(UsersList.Find(u => u.ID == user.ID) != default(DO.User))
            throw new DalAlreadyExistsException($"An user with ID={user.ID} already exist");

        UsersList.Add(user);

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
}
