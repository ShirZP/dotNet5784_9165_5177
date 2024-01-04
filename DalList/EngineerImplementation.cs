namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EngineerImplementation : IEngineer
{
    /// <summary>
    /// The function adds a new engineer to the list of engineers.
    /// </summary>
    /// <param name="engineer">New engineer to add to the engineersList</param>
    /// <returns>The id of the new engineer</returns>
    public int Create(Engineer engineer)
    {
        Engineer? searchSameEngineer = DataSource.Engineers.Find(e => e.ID == engineer.ID);  //Checking if there is already an engineer with such an id in the list.
        if (searchSameEngineer != null) 
        {
            throw new Exception($"An object of type Engineer with ID={engineer.ID} already exist");
        }
        else
        {
            DataSource.Engineers.Add(engineer);
        }
    
        return engineer.ID;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// The function returns a reference to the engineer with the requested ID
    /// </summary>
    /// <param name="id">the engineer id that we want to find</param>
    /// <returns>reference to the engineer with the requested ID</returns>
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find(e => e.ID == id);

        return engineer;
    }

    /// <summary>
    /// The function returns a copy of the engineer list
    /// </summary>
    /// <returns>A copy of the engineer list</returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    /// <summary>
    /// The function updates engineer details from the engineers list 
    /// </summary>
    /// <param name="engineer">The updated engineer</param>
    /// <exception cref="Exception">If the engineer you want to update does not exist in the list</exception>
    public void Update(Engineer engineer)
    {
        int engineerID = engineer.ID;
        Engineer? oldEngineer = DataSource.Engineers.Find(e => e.ID == engineerID);  //Search for the engineer to update in the list.
        if (oldEngineer != null)
        {
            DataSource.Engineers.Remove(oldEngineer);
            Engineer updateEngineer = engineer with { ID = engineerID };
            DataSource.Engineers.Add(updateEngineer);
        }
        else
        {
            throw new Exception($"An object of type Engineer with ID={engineer.ID} does not exist");
        }
    }
}

