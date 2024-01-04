namespace DalTest;
using DalApi;
using DO;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

public static class Initialization
{
    private const int MIN_ID = 200000000;
    private const int MAX_ID = 400000000;

    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();

    /// <summary>
    /// Initializing the task list with 20 tasks
    /// </summary>
    private static void createTasks()
    {

        for (int i = 1; i <= 20; i++)
        {
            //NickName
            string _nickName = $"Task number {i}";

            //Description
            string _description = $"Description {i}";

            //ScheduledDate
            DateTime start = DateTime.Now;
            int dayRange = s_rand.Next(365);

            DateTime _scheduledDate = start.AddDays(dayRange);

            //DeadlineDate
            DateTime _deadlineDate = _scheduledDate.AddDays(s_rand.Next(2, 30));

            //RequiredEffortTime
            int maxEffortTime = (_deadlineDate - _scheduledDate).Days;
            TimeSpan _requiredEffortTime = TimeSpan.FromDays(s_rand.Next(1, maxEffortTime));

            //StartDate
            int daysFromNowToDeadLine = (_deadlineDate - start).Days;
            DateTime _startDate = start.AddDays(s_rand.Next(daysFromNowToDeadLine));

            //FinalProduct
            string _finalProduct = $"FinalProduct {i}";
            
            //Remarks
            string _remarks = $"Remarks {i}";

            //EngineerId
            int? _engineerID = null;
            List<Engineer>? engineersList = s_dalEngineer!.ReadAll();
            if (engineersList != null)
            {
                Engineer e = engineersList[i % 6];  //choose an engineer from the list

                _engineerID = (i % 2) == 0 ? e.ID : null;
            }

            //Complexity
            int randComplexity = s_rand.Next(0, 5);
            DO.EngineerExperience _complexity = ((EngineerExperience)randComplexity);

            //IsMileStone
            bool _isMileStone = (i % 9) == 0 ? true : false;

            Task newTask = new(0, _nickName, _description, _scheduledDate, _startDate, _requiredEffortTime, null, _finalProduct, _remarks, _engineerID, _complexity, _deadlineDate, _isMileStone);
            s_dalTask!.Create(newTask);
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
            while (s_dalEngineer!.Read(_id) != null);

            //email
            string _email = _name.Trim() + "@gmail.com";

            //level
            int randLevel = s_rand.Next(0, 5);
            DO.EngineerExperience _level = ((EngineerExperience)randLevel);
  
            //cost
            double _cost = s_rand.Next(30, 80);


            Engineer newEngineer = new(_id, _name, _email, _level, _cost);
            s_dalEngineer!.Create(newEngineer);
        }
    }

    /// <summary>
    /// Initializing the dependency list with 40 Dependencies
    /// </summary>
    private static void createDependencies()
    {
        int _dependentTaskID;
        int _dependensOnTaskID;
        List<Task>? tasksList = s_dalTask!.ReadAll();


        for (int i = 0; i < 40;i++)
        {
            if (tasksList != null )
            {
                //DependentTask
                Task t = tasksList[s_rand.Next(0, 20)];  //choose a Task from the list
                _dependentTaskID = t.ID;
                DateTime? dependentTaskScheduledDate = t.ScheduledDate;

                //DependensOnTask
                do   //If the scheduled task start date of the dependent task is before the task it depends on, repeat the loop
                {
                    t = tasksList[s_rand.Next(0, 20)];
                } while (dependentTaskScheduledDate <= t.ScheduledDate);

                _dependensOnTaskID = t.ID;

                Dependency newDependency = new Dependency(0, _dependentTaskID, _dependensOnTaskID);
                s_dalDependency!.Create(newDependency);
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
    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DALTASK can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DALENGINEER can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DALDEPENDENCY can not be null!");
    }
}
