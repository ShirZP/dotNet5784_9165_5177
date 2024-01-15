﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// The function adds a new engineer to the list of engineers.
    /// </summary>
    /// <param name="engineer">New engineer to add to the engineersList</param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException">if the engineer is already exist in the engineers list</exception>
    public int Create(Engineer engineer)
    {
        Engineer? searchSameEngineer = DataSource.Engineers.Find(e => e.ID == engineer.ID);  //Checking if there is already an engineer with such an id in the list.
        if (searchSameEngineer != null) 
        {
            throw new DalAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist");
        }
        else
        {
            DataSource.Engineers.Add(engineer);
        }
    
        return engineer.ID;
    }

    /// <summary>
    /// The function change the status of the recieved engineer to unemployed.
    /// </summary>
    /// <param name="id">The ID of the engineer we want to delete(change to unemployed)</param>
    /// <exception cref="DalDoesNotExistException">If the engineer you want to delete does not exist in the list</exception>
    public void Delete(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find(e => e.ID == id);

        if (engineer != null) 
        {
            Engineer unemployedEngineer = engineer with { isEmployed = false};
            DataSource.Engineers.Remove(engineer);
            DataSource.Engineers.Add(unemployedEngineer);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Engineer with ID={id} does not exist");
        }

    }

    /// <summary>
    /// The function returns a reference to the engineer with the requested ID
    /// </summary>
    /// <param name="id">the engineer id that we want to find</param>
    /// <returns>reference to the engineer with the requested ID</returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.ID == id);
    }

    /// <summary>
    /// The function returns a copy of the engineer list
    /// </summary>
    /// <returns>A copy of the engineer list</returns>
    public IEnumerable<Engineer?> ReadAll()
    {
        return DataSource.Engineers.Select(item => item);
    }

    /// <summary>
    /// The function updates engineer details from the engineers list 
    /// </summary>
    /// <param name="engineer">The updated engineer</param>
    /// <exception cref="DalDoesNotExistException">If the engineer you want to update does not exist in the list</exception>
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
            throw new DalDoesNotExistException($"An object of type Engineer with ID={engineer.ID} does not exist");
        }
    }
}

