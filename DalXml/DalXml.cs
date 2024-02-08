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

    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }
    public DO.ProjectStatus ProjectStatus { get; set; }

    private DalXml() { }

    public DateTime? getProjectStartDate()
    {
        return Config.ProjectStartDate;
    }

    public DateTime? getProjectEndDate()
    {
        return Config.ProjectEndDate;
    }

    public ProjectStatus getProjectStatus()
    {
        return Config.ProjectStatus;
    }

    /// <summary>
    /// The function updated the project start date in the data configuration xml file
    /// </summary>
    /// <param name="projectStartDate"></param>
    public void setProjectStartDate(DateTime projectStartDate)
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
    public void setProjectEndDate(DateTime projectEndDate)
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
    public void initializeProjectStatus()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

        //initialize status
        root.Element("status")!.SetValue(DO.ProjectStatus.Planning.ToString());

        //initialize project dates
        root.Element("ProjectStartDate")!.SetValue(null); //TODO: ???????NULL???????
        root.Element("ProjectEndDate")!.SetValue(null);

        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }

    /// <summary>
    /// The function change the status to status BuildingSchedule
    /// </summary>
    /// <exception cref="DalChangProjectStatusException">If you try to change from status Execution to status BuildingSchedule</exception>
    public void changeStatusToBuildingSchedule()
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
    public void changeStatusToExecution()
    {
        XElement root = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);
        if (root.Element("status")!.Value == DO.ProjectStatus.Planning.ToString())
        {
            throw new DalChangProjectStatusException("can't change status from planning to Execution");
        }
        root.Element("status")!.SetValue(DO.ProjectStatus.Execution.ToString());
        XMLTools.SaveListToXMLElement(root, Config.s_data_config_xml);
    }
}
