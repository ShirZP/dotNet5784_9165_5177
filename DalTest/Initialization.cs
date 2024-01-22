namespace DalTest;
using DalApi;
using DO;
using Microsoft.VisualBasic;
using System;
using System.Data.Common;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class Initialization
{
    private const int MIN_ID = 200000000;
    private const int MAX_ID = 400000000;

    private static IDal? s_dal; //stage 2


    private static readonly Random s_rand = new();

    /// <summary>
    /// Initializing the task list with 20 tasks
    /// </summary>
    private static void createTasks()
    {
        string[] tastsNickNames =
        {
            "Algorithm development",
            "Software design and testing",
            "Hardware prototyping",
            "Embedded systems programming",
            "Cybersecurity implementation",
            "Cloud computing optimization",
            "Data analysis",
            "VR/AR development",
            "UX/UI improvement",
            "Automation and robotics",
            "Firmware development",
            "Wireless technology integration",
            "IoT device creation",
            "Big data processing",
            "Quality assurance",
            "DevOps and CI/CD",
            "Cyber-physical systems",
            "Blockchain solutions",
            "AI integration",
            "Research and development"
        };

        
        foreach (var taskNickName in tastsNickNames)
        {
            
            //NickName
            string _nickName = taskNickName;

            //Description
            string _description = $"Description - {taskNickName}";
            
            //RequiredEffortTime
            TimeSpan _requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, 5));
            
            //FinalProduct
            string _finalProduct = $"FinalProduct - {taskNickName}";
            
            //Remarks
            string _remarks = $"Remarks - {taskNickName}";

            //Complexity
            int randComplexity = s_rand.Next(0, 5);
            DO.EngineerExperience _complexity = ((EngineerExperience)randComplexity);


            Task newTask = new(0, _nickName, _description, null, null, _requiredEffortTime, null, _finalProduct, _remarks, null, _complexity, null, null);
            s_dal!.Task.Create(newTask);
        }

    }

    /// <summary>
    /// Initializing the engineer list with 6 engineers
    /// </summary>
    private static void createEngineers()
    {
        string[] engineerNames =
        {
            "Dani Levi", "Eli Amar", "Yair Cohen",
            "Ariela Levin", "Dina Klein", "Shira Israelof"
        };

        foreach (var _name in engineerNames)
        {
            //id
            int _id;
            do
                _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dal!.Engineer.Read(item => item.ID == _id) != null);

            //email
            string _email = _name.Replace(" ", "") + "@gmail.com";
           
            //level
            int randLevel = s_rand.Next(0, 5);
            DO.EngineerExperience _level = ((EngineerExperience)randLevel);
  
            //cost
            double _cost = s_rand.Next(30, 80);


            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dal!.Engineer.Create(newEngineer);
        }
    }

    /// <summary>
    /// Initializing the dependency list with 40 Dependencies
    /// </summary>
    private static void createDependencies()
    {
        int _dependentTaskID;
        int _dependensOnTaskID;
        Dependency newDependency;

        List<Task?> tasksList = s_dal!.Task.ReadAll().ToList<Task?>();
        if (tasksList != null)
        {
            //19 dependencies
            for (int i = 19; i > 0; i--)
            {

                _dependentTaskID = tasksList[i].ID;
                _dependensOnTaskID = tasksList[i - 1].ID;

                newDependency = new Dependency(0, _dependentTaskID, _dependensOnTaskID);
                s_dal!.Dependency.Create(newDependency);

            }

            //more 9 dependencies
            for (int i = 19; i > 1; i -= 2)
            {
                _dependentTaskID = tasksList[i].ID;
                _dependensOnTaskID = tasksList[i - 2].ID;

                newDependency = new Dependency(0, _dependentTaskID, _dependensOnTaskID);
                s_dal!.Dependency.Create(newDependency);
            }

            //more 13 dependencies
            //Task number 14 depends on all the tasks before it (the dependency on 13 was already created in the first loop)
            for (int i = 12; i >= 0; i--)
            {
                _dependentTaskID = tasksList[14].ID;
                _dependensOnTaskID = tasksList[i].ID;

                newDependency = new Dependency(0, _dependentTaskID, _dependensOnTaskID);
                s_dal!.Dependency.Create(newDependency);
            }

        }
    }

    /// <summary>
    /// Initializes the 3 lists of 3 entities respectively. 
    /// </summary>
    /// <param name="dalTask">An interface type to a task entity</param>
    /// <param name="dalEngineer">An interface type to a task engineer</param>
    /// <param name="dalDependency">An interface type to a task dependency</param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        //s_dal = DalApi.Factory.Get; //stage 4

        s_dal!.Task.Clear();
        s_dal!.Engineer.Clear();
        s_dal!.Dependency.Clear();

        createEngineers();
        createTasks();
        createDependencies();
    }
}
