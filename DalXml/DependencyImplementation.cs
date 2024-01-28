using DalApi;
using DO;
using System.Data.Common;
namespace Dal;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    /// <summary>
    /// The function adds the new dependency to the xml of Dependencies with a unique ID for it.
    /// </summary>
    /// <param name="dependency">New dependency to add to the dependencies xml</param>
    /// <returns>The new dependency uniqe id</returns>
    public int Create(Dependency dependency)
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml); 

        int newId = Config.NextDependencyId;
        Dependency dependencyWithNewID = dependency with { ID = newId };
        DependenciesList.Add(dependencyWithNewID);

        //Serialize
        XMLTools.SaveListToXMLSerializer<Dependency>(DependenciesList, s_dependencies_xml);

        return newId;
    }

    /// <summary>
    /// The function deletes the received dependency from the Dependencies xml
    /// </summary>
    /// <param name="id">The ID of the dependency to delete</param>
    /// <exception cref="DalDoesNotExistException">If the dependncy does not exist in the Dependencies xml</exception>
    public void Delete(int id)
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        Dependency? dependency = DependenciesList.Find(d => d.ID == id);
        if (dependency != null)
        {
            DependenciesList.Remove(dependency);
            //Serialize
            XMLTools.SaveListToXMLSerializer<Dependency>(DependenciesList, s_dependencies_xml);
        }
        else
            throw new DalDoesNotExistException($"An object of type Dependency with ID={id} does Not exist");
    }

    /// <summary>
    /// The function returns a reference to the dependency with the requested filter
    /// </summary>
    /// <param name="filter">delegate func that recieves Dependency and returns bool</param>
    /// <returns>reference to the dependency with the requested filter</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        return DependenciesList.FirstOrDefault(filter);
    }

    /// <summary>
    /// The function returns a copy of dependencies list from the xml with filter
    /// </summary>
    /// <returns>A copy of dependencies list</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency?, bool>? filter = null)
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        if (filter == null)
            return DependenciesList.Select(item => item);
        else
            return DependenciesList.Where(filter);
    }

    /// <summary>
    /// The function updates a dependency from the dependencies xml
    /// </summary>
    /// <param name="dependency">The updated dependency</param>
    /// <exception cref="DalDoesNotExistException">If the dependency you want to update does not exist in the xml</exception>
    public void Update(Dependency dependency)
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);

        int dependencyID = dependency.ID;
        Dependency? oldDependency = DependenciesList.Find(t => t.ID == dependencyID);  //Search for the dependency to update in the list
        if (oldDependency != null)
        {
            DependenciesList.Remove(oldDependency);
            Dependency updateDependency = dependency with { ID = dependencyID };
            DependenciesList.Add(updateDependency);

            //Serialize
            XMLTools.SaveListToXMLSerializer<Dependency>(DependenciesList, s_dependencies_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Dependency with ID={dependency.ID} does Not exist");
        }
    }

    /// <summary>
    /// The function clears all the dependencies from the dependencies xml.
    /// </summary>
    public void Clear()
    {
        //Deserialize
        List<Dependency> DependenciesList = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        DependenciesList.Clear();
        //Serialize
        XMLTools.SaveListToXMLSerializer<Dependency>(DependenciesList, s_dependencies_xml);
    }
}
