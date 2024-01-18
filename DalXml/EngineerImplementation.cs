namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// The function adds a new engineer to the xml of engineers.
    /// </summary>
    /// <param name="engineer">New engineer to add to the engineers xml</param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException">if the engineer is already exist in the engineers xml</exception>
    public int? Create(Engineer engineer)
    {
        XElement xEngineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);  //load the engineers xml to XElement

        List<Engineer>? engineersList = getEngineersList(xEngineerRoot);  //convert the XElement to list

        Engineer? searchSameEngineer = engineersList.Find(e => e.ID == engineer.ID);  //Checking if there is already an engineer with such an id in the list.
        if (searchSameEngineer != null)
        {
            throw new DalAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist");
        }
        else
        {
            //create a new engineer XElement
            XElement id = new XElement("id", engineer.ID);
            XElement fullName = new XElement("fullName", engineer.FullName);
            XElement email = new XElement("email", engineer.Email);
            XElement level = new XElement("level", engineer.Level);
            XElement cost = new XElement("cost", engineer.Cost);
            XElement isEmployed = new XElement("isEmployed", engineer.isEmployed);

            //add and save the new engineer XElement to the engineers xml file
            xEngineerRoot.Add(new XElement("engineer", id, fullName, email, level, cost, isEmployed));
            XMLTools.SaveListToXMLElement(xEngineerRoot, s_engineers_xml);
        }
        return engineer.ID;
    }

    /// <summary>
    /// The function delete the received engineer from the xml.
    /// </summary>
    /// <param name="id">The ID of the engineer we want to delete</param>
    /// <exception cref="DalDoesNotExistException">If the engineer you want to delete does not exist in the xml</exception>
    public void Delete(int id)
    {
        IEnumerable<Engineer?> xElement = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).Where(eng => eng.ID == id);

        //XElement xEngineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);  //load the engineers xml to XElement
        //XElement engineerElement;
        //try
        //{
        //    //Retrieving the engineer to delete from the XElement list
        //    engineerElement = (from e in xEngineerRoot.Elements()
        //                       where XMLTools.ToIntNullable(e, "id") == id
        //                       select e).FirstOrDefault()!;

        //    engineerElement!.Remove(); //remove the requested engineer
        //    XMLTools.SaveListToXMLElement(xEngineerRoot, s_engineers_xml);
        //}
        //catch
        //{
        //    throw new DalDoesNotExistException($"An object of type Engineer with ID={id} does not exist");
        //}
    }

    /// <summary>
    /// The function returns a reference to the engineer with the requested filter
    /// </summary>
    /// <param name="filter">delegate func that recieves Engineer and returns bool</param>
    /// <returns>reference to the engineer with the requested filter</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).FirstOrDefault(filter);
    }

    /// <summary>
    /// The function returns a copy of the engineer list with filter
    /// </summary>
    /// <returns>A copy of the engineer list</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer?, bool>? filter = null)
    {
        if (filter != null)
        {
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e)).Where(filter);
        }
        else
        {
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e => getEngineer(e));
        }

    }

    public void Update(Engineer item)
    {
        throw new NotImplementedException();
    }

    private List<Engineer>? getEngineersList(XElement xelement)
    {
        List<Engineer>? engineers;
        try
        {
            engineers = (from e in xelement.Elements()
                        select new Engineer()
                        {
                            ID = XMLTools.ToIntNullable(e, "id"),
                            FullName = e.Element("fullName").Value,
                            Email = e.Element("email").Value,
                            Level = XMLTools.ToEnumNullable<DO.EngineerExperience>(e, "level"),
                            Cost = XMLTools.ToDoubleNullable(e, "cost"),
                            isEmployed = Convert.ToBoolean(e.Element("isEmployed").Value)
                        }).ToList<Engineer>();
        }
        catch
        {
            engineers = null;
        }
        return engineers;
    }

    
    static Engineer getEngineer(XElement e)
    {
        return new Engineer()
        {
            ID = e.ToIntNullable("id") ?? throw new FormatException("can' convert id"),
            FullName = (string?)e.Element("fullName") ?? "",
            Email = (string?)e.Element("email") ?? null,
            Level = e.ToEnumNullable<DO.EngineerExperience>("level") ?? null,
            isEmployed = (bool?)e.Element("isEmployed") ?? true

        };
    }
}
