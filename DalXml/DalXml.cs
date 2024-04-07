using DalApi;
using DO;
using System.Diagnostics;
using System.Xml.Linq;
namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IUser User => new UserImplementation();

    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public DO.ProjectStatus ProjectStatus { get; set; }
    public DateTime Clock { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private DalXml() { }

    public DateTime? GetProjectStartDate()
    {
        return Config.ProjectStartDate;
    }

    public DateTime? GetProjectEndDate()
    {
        return Config.ProjectEndDate;
    }

    public ProjectStatus GetProjectStatus()
    {
        return Config.ProjectStatus;
    }

    /// <summary>
    /// The function updated the project start date in the data configuration xml file
    /// </summary>
    /// <param name="projectStartDate"></param>
    public void SetProjectStartDate(DateTime projectStartDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        if (root.Element("ProjectStartDate") is null)
        {
            XElement pStartDate = new XElement("ProjectStartDate", projectStartDate);
            root.Add(pStartDate);
        }
        else
        {
            root.Element("ProjectStartDate")?.SetValue(projectStartDate.ToString());
        }
        
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }

    /// <summary>
    /// The function updated the project end date in the data configuration xml file
    /// </summary>
    /// <param name="projectEndDate"></param>
    public void SetProjectEndDate(DateTime projectEndDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        if (root.Element("ProjectEndDate") is null)
        {
            XElement pStartDate = new XElement("ProjectEndDate", projectEndDate);
            root.Add(pStartDate);
        }
        else
        {
            root.Element("ProjectEndDate")?.SetValue(projectEndDate.ToString());
        }

        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }


    /// <summary>
    /// The function initializes the status of the project to A and resets the start and end dates of the project
    /// </summary>
    public void InitializeProjectStatus()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

        //initialize status
        root.Element("ProjectStatus")!.SetValue(DO.ProjectStatus.Planning.ToString());

        //initialize project dates
        root.Element("ProjectStartDate")!.SetValue("");
        root.Element("ProjectEndDate")!.SetValue("");

        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }

    /// <summary>
    /// The function change the status to status Planning
    /// </summary>
    public void ChangeStatusToPlanning()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        root.Element("ProjectStatus")!.SetValue(DO.ProjectStatus.Planning.ToString());
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);

    }

    /// <summary>
    /// The function change the status to status BuildingSchedule
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status Execution to status BuildingSchedule</exception>
    public void ChangeStatusToBuildingSchedule()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        if(root.Element("ProjectStatus")!.Value == DO.ProjectStatus.Execution.ToString())
        {
            throw new DalChangProjectStatusException("can't change status from Execution to BuildingSchedule");
        }
        root.Element("ProjectStatus")!.SetValue(DO.ProjectStatus.BuildingSchedule.ToString());
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);

    }

    /// <summary>
    /// The function change the status to status Execution
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status planning to status Execution</exception>
    public void ChangeStatusToExecution()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        root.Element("ProjectStatus")!.SetValue(DO.ProjectStatus.Execution.ToString());
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }

    public void SetClock(DateTime clock)
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        root.Element("Clock")!.SetValue(clock.ToString());
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }

    public DateTime GetClock()
    {
        return Config.Clock;
    }

    public void InitializeClock()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

        //initialize Clock
        root.Element("Clock")!.SetValue(DateTime.Now);

        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }
}
