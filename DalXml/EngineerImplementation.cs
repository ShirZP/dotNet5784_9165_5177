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

        Engineer? e = Read(e => e.ID == engineer.ID); //Checking if there is already an engineer with such an id in the xml.

        if (e != default(Engineer?))
        {
            throw new DalAlreadyExistsException($"An object of type Engineer with ID={engineer.ID} already exist");
        }
        else
        {
            //add and save the new engineer XElement to the engineers xml file
            xEngineerRoot.Add(ToXElement(engineer));
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
        XElement xEngineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);  //load the engineers xml to XElement

        //Retrieving the engineer to delete from the XElement list
        XElement? XEngineer = (from e in xEngineerRoot.Elements()
                               where XMLTools.ToIntNullable(e, "id") == id
                               select e).FirstOrDefault()!;

        if(XEngineer != default(XElement?))
        {
            XEngineer!.Remove(); //remove the requested engineer
            XMLTools.SaveListToXMLElement(xEngineerRoot, s_engineers_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Engineer with ID={id} does not exist");
        }
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
    /// <summary>
    /// The function updates engineer details from the engineers xml 
    /// </summary>
    /// <param name="updateEngineer">The updated engineer</param>
    /// <exception cref="DalDoesNotExistException">If the engineer you want to update does not exist in the xml</exception>
    public void Update(Engineer updateEngineer)
    {
        XElement xEngineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);  //load the engineers xml to XElement

        //Retrieving the requested engineer for update.
        XElement? XEngineer = (from e in xEngineerRoot.Elements()
                               where (int)e.Element("id")! == updateEngineer.ID
                               select e).FirstOrDefault();

        if (XEngineer != default(XElement?))
        {
            XEngineer.Remove();
            xEngineerRoot.Add(ToXElement(updateEngineer));
            XMLTools.SaveListToXMLElement(xEngineerRoot, s_engineers_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"An object of type Engineer with ID={updateEngineer.ID} does not exist");
        }
    }

    /// <summary>
    /// The function receives an object of type Engineer and converts it to an object of type XElement and returns it.
    /// </summary>
    /// <param name="engineer">an object of type Engineer to convert</param>
    /// <returns>object type XElement</returns>
    private XElement ToXElement(Engineer engineer)
    {
        //create a new engineer XElement
        XElement id = new XElement("id", engineer.ID);
        XElement fullName = new XElement("fullName", engineer.FullName);
        XElement email = new XElement("email", engineer.Email);
        XElement level = new XElement("level", engineer.Level);
        XElement cost = new XElement("cost", engineer.Cost);
        XElement isEmployed = new XElement("isEmployed", engineer.isEmployed);

        return new XElement("engineer", id, fullName, email, level, cost, isEmployed);
    }

    /// <summary>
    /// The function receives an object of type XElement and converts it to an object of type Engineer and returns it.
    /// </summary>
    /// <param name="XEngineer">an object of type XElement to convert</param>
    /// <returns>object type engineer</returns>
    /// <exception cref="FormatException"></exception>
    static Engineer getEngineer(XElement XEngineer)
    {
        return new Engineer()
        {
            ID = XEngineer.ToIntNullable("id") ?? throw new FormatException("can't convert id"),
            FullName = (string?)XEngineer.Element("fullName") ?? "",
            Email = (string?)XEngineer.Element("email") ?? null,
            Level = XEngineer.ToEnumNullable<DO.EngineerExperience>("level") ?? null,
            isEmployed = (bool?)XEngineer.Element("isEmployed") ?? true
        };
    }

    /// <summary>
    /// The function clears all the engineers from the engineers xml.
    /// </summary>
    public void Clear()
    {
        XElement xEngineerRoot = XMLTools.LoadListFromXMLElement(s_engineers_xml);  //load the engineers xml to XElement
        xEngineerRoot.RemoveAll();
        XMLTools.SaveListToXMLElement(xEngineerRoot, s_engineers_xml);
    }

}
