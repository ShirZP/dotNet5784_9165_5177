﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Threading.Tasks;


internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// The function adds the new dependency to the list of Dependencies with a unique ID for it.
    /// </summary>
    /// <param name="dependency">New dependency to add to the DependenciesList</param>
    /// <returns>The new dependency uniqe id</returns>
    public int Create(Dependency dependency)
    {
        int newId = DataSource.Config.NextDependencyId;

        Dependency dependencyWithNewID = dependency with { ID = newId };
        DataSource.Dependencies.Add(dependencyWithNewID);

        return newId;
    }

    /// <summary>
    /// The function deletes the received dependency from the Dependencies list
    /// </summary>
    /// <param name="id">The ID of the dependency to delete</param>
    /// <exception cref="DalDoesNotExistException">If the dependncy does not exist in the Dependencies list</exception>
    public void Delete(int id)
    {
        Dependency? dependency = DataSource.Dependencies.Find(d => d.ID == id);
        if (dependency != null)
        {
            DataSource.Dependencies.Remove(dependency);
        }
        else
            throw new DalDoesNotExistException($"An object of type Dependency with ID={id} does Not exist");
    }

    /// <summary>
    /// The function returns a reference to the dependency with the requested filter
    /// </summary>
    /// <param name="filter">delegate func that recieves Dependency and returns bool</param>
    /// <returns>reference to the dependency with the requested filter</returns>
    public Dependency? Read(Func<Dependency, bool> filter) //stage 2
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    /// <summary>
    /// The function returns a copy of the dependency list with filter
    /// </summary>
    /// <returns>A copy of the dependency list</returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }


    /// <summary>
    /// The function updates a dependency from the dependencies list
    /// </summary>
    /// <param name="dependency">The updated dependency</param>
    /// <exception cref="DalDoesNotExistException">If the dependency you want to update does not exist in the list</exception>
    public void Update(Dependency dependency)
    {
        int dependencyID = dependency.ID;
        Dependency? oldDependency = DataSource.Dependencies.Find(t => t.ID == dependencyID);  //Search for the dependency to update in the list
        if (oldDependency != null)
        {
            DataSource.Dependencies.Remove(oldDependency);
            Dependency updateDependency = dependency with { ID = dependencyID };
            DataSource.Dependencies.Add(updateDependency);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Dependency with ID={dependency.ID} does Not exist");
        }
    }

    /// <summary>
    /// The function clears all the dependencies from the list.
    /// </summary>
    public void Clear()
    {
        DataSource.Dependencies.Clear();
    }
}
