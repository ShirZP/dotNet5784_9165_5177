using BO;
using DalApi;
using DalTest;
using System.Collections.Generic;
using System.Collections.Specialized;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)"); 
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y") 
        {
            DalTest.Initialization.Do(); 
        }

        int choice;
        string? intString;  //string to convert to int
        printMainMenu();
        intString = Console.ReadLine()!;
        int.TryParse(intString, out choice);

        do
        {
            try
            {
                switch (choice)
                {
                    case 1:
                        taskSubMenu();
                        break;
                    case 2:
                        engineerSubMenu();
                        break;
                    case 3:
                        changeProjectStatus();
                        break;
                    case 0:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            printMainMenu();
            intString = Console.ReadLine()!;   //string to convert to int
            int.TryParse(intString, out choice);

        } while (choice != 0);
    }


    /// <summary>
    /// Print main menu.
    /// </summary>
    private static void printMainMenu()
    {
        Console.WriteLine("Select an entity of your choice:");
        Console.WriteLine("1 - Task\n" + "2 - Engineer\n" + "3 - Change status of project\n" + "0 - Exit\n");
    }

    /// <summary>
    /// Print task sub menu.
    /// </summary>
    private static void printTaskSubMenu()
    {
        Console.WriteLine($"Select an action for the task:");  
        Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "6 - Update the scheduled date\n" + "7 - SortByID\n" + "0 - Exit\n");
    }

    /// <summary>
    /// Print engineer sub menu.
    /// </summary>
    private static void printEngineerSubMenu()
    {
        Console.WriteLine($"Select an action for the engineer:"); 
        Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "6 - SortByName\n" + "0 - Exit\n");
    }



    /// <summary>
    /// All the operation that we can do on task.
    /// </summary>
    private static void taskSubMenu()
    {
        int choice, id;
        DateTime? newScheduledDate;
        string st;
        BO.Task? task;
        printTaskSubMenu();
        string? intString;   //string to convert to int
        intString = Console.ReadLine()!;
        int.TryParse(intString, out choice);

        while (choice != 0)
        {
            switch (choice)
            {
                case 1:  //Create
                    task = inputCreateTask();
                    s_bl!.Task.Create(task);
                    break;

                case 2:  //Read
                    Console.WriteLine("Enter Task ID to read:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    try
                    {
                        task = s_bl!.Task.Read(id);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;

                case 3:  //ReadAll
                    IEnumerable<TaskInList> tasksList = s_bl!.Task.ReadAll();
                    foreach (var t in tasksList)
                    {
                        Console.WriteLine(t);
                    }
                    break;

                case 4: //Update
                    Console.WriteLine("Enter task ID to update:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    try
                    {
                        task = s_bl!.Task.Read(id);
                        Console.WriteLine(task);   //Before the update prints the task to update.
                        task = inputUpdateTask(task);
                        s_bl!.Task.Update(task);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;

                case 5:  //Delete
                    Console.WriteLine("Enter Task ID to delete:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    s_bl!.Task.Delete(id);
                    Console.WriteLine("Deleted successfully");
                    break;

                case 6: //ScheduledDateUpdate
                    Console.WriteLine("Enter Task ID to update scheduled date:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);

                    Console.WriteLine("Enter date to update the scheduled date of the task:");
                    st = Console.ReadLine()!;
                    newScheduledDate = TryParseDateTime(st);
                    s_bl.Task.ScheduledDateUpdate(id, newScheduledDate, s_bl.GetProjectStatus());
                    break;

                case 7: //SortByID
                    IEnumerable<BO.TaskInList> tasksSortList = s_bl!.Task.SortByID();
                    foreach (var e in tasksSortList)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                    
                case 0:
                    break;

                default:
                    break;
            }
            printTaskSubMenu();
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);
        }
    }

    /// <summary>
    /// All the operation that we can do on engineer.
    /// </summary>
    private static void engineerSubMenu()
    {
        int choice, id;
        string? intString;    //string to convert to int
        BO.Engineer? engineer, newEngineer;
        printEngineerSubMenu();
        intString = Console.ReadLine()!;
        int.TryParse(intString, out choice);


        while (choice != 0)
        {
            switch (choice)
            {
                case 1:  //Create
                    newEngineer = inputCreateEngineer();
                    s_bl!.Engineer.Create(newEngineer);
                    break;

                case 2:  //Read
                    Console.WriteLine("Enter engineer ID:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    engineer = s_bl!.Engineer.Read(id);
                    if (engineer != null)
                    {
                        Console.WriteLine(engineer);
                    }
                    else
                    {
                        Console.WriteLine($"The engineer with ID = {id} does not exist");
                    }
                    break;

                case 3:  //ReadAll
                    IEnumerable<BO.Engineer?> engineersList = s_bl!.Engineer.ReadAll();
                    foreach (var e in engineersList)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 4:  //Update
                    Console.WriteLine("Enter engineer ID to update:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    engineer = s_bl!.Engineer.Read(id);
                    if (engineer != null)
                    {
                        Console.WriteLine(engineer);   //Before the update prints the engineer to update.

                        newEngineer = inputUpdateEngineer(engineer);
                        s_bl!.Engineer.Update(newEngineer);

                        Console.WriteLine($"Engineer {id} update successfully");
                    }
                    else
                    {
                        Console.WriteLine($"The engineer with ID = {id} does not exist");
                    }
                    break;

                case 5:  //Delete
                    Console.WriteLine("Enter engineer ID to read:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    s_bl!.Engineer.Delete(id);
                    Console.WriteLine($"Engineer {id} deleted successfully");
                    break;

                case 6: //SortByName
                    IEnumerable<BO.Engineer?> engineersSortList = s_bl!.Engineer.SortByName();
                    foreach (var e in engineersSortList)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 0:
                    break;

                default:
                    break;
            }

            printEngineerSubMenu();
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);
        }

    }

    #region input methods

    /// <summary>
    /// Input fields of Engineer.
    /// </summary>
    /// <returns>Object type Engineer</returns>
    private static BO.Engineer inputCreateEngineer()
    {
        int id, level, cost;
        string? intString;    //string to convert to int
        string? nameEngineer, email;

        //ID
        Console.WriteLine("Enter engineer ID:");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out id);

        //nameEngineer
        Console.WriteLine("Enter engineer full name:");
        nameEngineer = Console.ReadLine();

        //email
        Console.WriteLine("Enter engineer email:");
        email = Console.ReadLine();

        //level
        Console.WriteLine("Choose engineer level:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out level);

        //cost
        Console.WriteLine("Enter engineer cost:");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out cost);

        //create newEngineer
        return new BO.Engineer(id, nameEngineer, email, ((DO.EngineerExperience)level), cost, null);

    }

    /// <summary>
    /// update fields of Engineer.
    /// </summary>
    /// <param name="originalEngineer">engineer to update</param>
    /// <returns>Object type Engineer</returns>
    private static BO.Engineer inputUpdateEngineer(BO.Engineer originalEngineer)
    {
        int currentTaskID;   //TODO: nullable
        int? levelNum;
        double? cost;
        string? intString;    //string to convert to int
        string? nameEngineer, email, currentTaskNickName;

        //nameEngineer
        Console.WriteLine("Enter engineer full name:");
        nameEngineer = Console.ReadLine();
        if (nameEngineer == null || nameEngineer == "")
            nameEngineer = originalEngineer.FullName;

        //email
        Console.WriteLine("Enter engineer email:");
        email = Console.ReadLine();
        if (email == null || email == "")
            email = originalEngineer.Email;

        //level
        Console.WriteLine("Choose engineer level:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
        intString = Console.ReadLine()!;
        DO.EngineerExperience? level;
        if (intString == null || intString == "")
            level = originalEngineer.Level;
        else
        {
            levelNum = TryParseInt(intString);
            level = (DO.EngineerExperience)levelNum!;
        }

        //cost
        Console.WriteLine("Enter engineer cost:");
        intString = Console.ReadLine()!;
        if (intString == null || intString == "")
            cost = originalEngineer.Cost;
        else
            cost = TryParseInt(intString);

        //EngineerCurrentTask
        Console.WriteLine("Enter engineer current task ID:");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out currentTaskID);
        //currentTaskID = TryParseInt(intString);

        Console.WriteLine("Enter engineer current task nick name:");
        currentTaskNickName = Console.ReadLine()!;


        //update newEngineer
        return new BO.Engineer(originalEngineer.ID, nameEngineer, email, level, cost, new BO.TaskInEngineer(currentTaskID, currentTaskNickName));  
    }

    /// <summary>
    /// Input fields of Task.
    /// </summary>
    /// <returns>Object type Task</returns>
    private static BO.Task inputCreateTask()
    {
        string nickName, description;
        string? finalProduct, remarks;
        string? dateString, intString;
        TimeSpan? requiredEffortTime;
        int? complexityNum;
        DO.EngineerExperience? complexity;

        //nickName
        Console.WriteLine("Enter task nickName:");
        nickName = Console.ReadLine()!;

        //description
        Console.WriteLine("Enter task description:");
        description = Console.ReadLine()!;

        //requiredEffortTime
        Console.WriteLine("Enter task required effort time to complete:");
        dateString = Console.ReadLine();
        requiredEffortTime = TryParseTimeSpan(dateString);

        //finalProduct
        Console.WriteLine("Enter task finalProduct:");
        finalProduct = Console.ReadLine();

        //remarks
        Console.WriteLine("Enter task remarks:");
        remarks = Console.ReadLine();

        //complexity
        Console.WriteLine("Choose task complexity:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
        intString = Console.ReadLine();
        if (intString != null)
        {
            complexityNum = TryParseInt(intString);
            complexity = (DO.EngineerExperience)complexityNum!;
        }
        else
        {
            complexity = null;
        }

        List < TaskInList > dependencies = new List<TaskInList> ();
        return new BO.Task(0, nickName, description, BO.Status.New, dependencies, null, null, null, null, null, requiredEffortTime, finalProduct, remarks, null, complexity);
    }

    /// <summary>
    /// update fields of Task.
    /// </summary>
    /// <param name="originalTask">task to update.</param>
    /// <returns></returns>
    private static BO.Task inputUpdateTask(BO.Task originalTask)
    {
        string nickName, description;
        string? deliverables, remarks;
        DateTime? scheduledDate;
        string? dateString, intString, timeString;
        TimeSpan? requiredEffortTime;
        int? complexityNum, statusNum;
        int assignedEngineerID;
        string assignedEngineerNickName;

        //nickName
        Console.WriteLine("Enter task nickName:");
        nickName = Console.ReadLine()!;
        if (nickName == null || nickName == "")
            nickName = originalTask.NickName;

        //description
        Console.WriteLine("Enter task description:");
        description = Console.ReadLine()!;
        if (description == null || description == "")
            description = originalTask.Description;

        //Status
        Console.WriteLine("Choose task status:" + "0 - New\n" + "1 - Active\n" + "2 - Complete\n");
        intString = Console.ReadLine();
        BO.Status taskStatus;
        if (intString == null || intString == "")
            taskStatus = originalTask.Status;
        else
        {
            statusNum = TryParseInt(intString);
            taskStatus = (BO.Status)statusNum!;
        }


        //Dependencies
        int idDependency;
        List<TaskInList> dependenciesList = new List<TaskInList>();

        Console.WriteLine("Insert dependencies ID, to stop enter 0");
        intString = Console.ReadLine();
        int.TryParse(intString, out idDependency);
        while (idDependency != 0)
        {
            BO.Task task = s_bl.Task.Read(idDependency);
            dependenciesList.Add(new TaskInList(task.ID, task.Description,task.NickName ,task.Status));

            Console.WriteLine("Insert dependencies ID:");
            intString = Console.ReadLine();
            int.TryParse(intString, out idDependency);
        }

        //scheduledDate
        Console.WriteLine("Enter task scheduled date to start:");
        dateString = Console.ReadLine();
        if (dateString == null || dateString == "")
            scheduledDate = originalTask.ScheduledDate;
        else
            scheduledDate = TryParseDateTime(dateString);

        //requiredEffortTime
        Console.WriteLine("Enter task required effort time to complete:");
        timeString = Console.ReadLine();
        if (timeString == null || timeString == "")
            requiredEffortTime = originalTask.RequiredEffortTime;
        else
            requiredEffortTime = TryParseTimeSpan(dateString);

        //Deliverables
        Console.WriteLine("Enter task deliverables:");
        deliverables = Console.ReadLine();
        if (deliverables == null || deliverables == "")
            deliverables = originalTask.Deliverables;

        //remarks
        Console.WriteLine("Enter task remarks:");
        remarks = Console.ReadLine();
        if (remarks == null || remarks == "")
            remarks = originalTask.Remarks;

        //AssignedEngineer
        Console.WriteLine("Enter the ID of the engineer responsible for carrying out the task:");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out assignedEngineerID);

        Console.WriteLine("Enter the nick name of the engineer responsible for carrying out the task:");
        assignedEngineerNickName = Console.ReadLine()!;
      

        //complexity
        Console.WriteLine("Choose task complexity:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
        intString = Console.ReadLine();
        DO.EngineerExperience? complexity;
        if (intString == null || intString == "")
            complexity = originalTask.Complexity;
        else
        {
            complexityNum = TryParseInt(intString);
            complexity = (DO.EngineerExperience)complexityNum!;
        }
        

        return new BO.Task(originalTask.ID,
                            nickName,
                            description,
                            taskStatus, 
                            dependenciesList, 
                            scheduledDate,
                            null, null, null, null, 
                            requiredEffortTime, 
                            deliverables, 
                            remarks, 
                            new BO.EngineerInTask(assignedEngineerID, assignedEngineerNickName), 
                            complexity);
    }

    #endregion

    #region   TryParse methods
    /// <summary>
    /// The function accepts a string and returns it in dateTime format if successful, otherwise it returns null.
    /// </summary>
    /// <param name="text">Sring to convert to dateTime.</param>
    /// <returns></returns>
    public static DateTime? TryParseDateTime(string? text)
    {
        DateTime date;
        return DateTime.TryParse(text, out date) ? date : (DateTime?)null;
    }

    /// <summary>
    /// The function accepts a string and returns it in timeSpan format if successful, otherwise it returns null.
    /// </summary>
    /// <param name="text">Sring to convert to timeSpan.</param>
    /// <returns></returns>
    public static TimeSpan? TryParseTimeSpan(string? text)
    {
        TimeSpan date;
        return TimeSpan.TryParse(text, out date) ? date : (TimeSpan?)null;
    }

    /// <summary>
    /// The function accepts a string and returns it in int format if successful, otherwise it returns null.
    /// </summary>
    /// <param name="text">Sring to convert to int.</param>
    /// <returns></returns>
    public static int? TryParseInt(string? text)
    {
        int num;
        return int.TryParse(text, out num) ? num : (int?)null;
    }

    #endregion


    /// <summary>
    /// The function change the status of the project.
    /// </summary>
    /// <exception cref="FormatException">Wrong input</exception>
    public static void changeProjectStatus()
    {
        Console.Write("Would you like to change the status of the project? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
        {
            BO.ProjectStatus currentProjectStatus = s_bl.GetProjectStatus();

            BO.ProjectStatus calculateProjectStatus = s_bl.CalculateProjectStatus();

            switch (currentProjectStatus)
            {
                case BO.ProjectStatus.Planning:

                    Console.WriteLine("Enter a start date to the project");
                    ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

                    //convert start date type string to DateTime type
                    DateTime startDate;
                    DateTime.TryParse(ans, out startDate);

                    s_bl.SetProjectStartDate(startDate);

                    Console.WriteLine("Do you want to enter a end date to the project");
                    ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                    if (ans == "Y")
                    {
                        Console.WriteLine("Enter a end date to the project");
                        ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

                        //convert end date type string to DateTime type
                        DateTime endDate;
                        DateTime.TryParse(ans, out endDate);

                        s_bl.SetProjectEndDate(endDate);
                    }

                    //change the status of the project from Planing to BuildingSchedule
                    s_bl.changeStatusToBuildingSchedule();
                    Console.WriteLine("The status of the project update to BuildingSchedule");

                    break;

                case BO.ProjectStatus.BuildingSchedule:

                    if (calculateProjectStatus == BO.ProjectStatus.BuildingSchedule)
                    {
                        Console.WriteLine("can't change to the next stage - Execution stage!");
                        break;
                    }

                    if (calculateProjectStatus == BO.ProjectStatus.Execution)
                    {
                        s_bl.changeStatusToExecution();
                        Console.WriteLine("The status of the project update to Execution");
                    }

                    break;

                case BO.ProjectStatus.Execution:

                    Console.WriteLine("Can't change the status of the project!");

                    break;

            }
        }

    }
}
