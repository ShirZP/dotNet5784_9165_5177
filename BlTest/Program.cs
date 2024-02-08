using BO;
using DalApi;
using DalTest;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "6 - Update the scheduled date\n" + "0 - Exit\n");
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
                    s_dal!.Task.Delete(id);
                    Console.WriteLine("Deleted successfully");
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
    /// All the operation that we can do on engineer (CRUD).
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
                    engineer = s_dal!.Engineer.Read(item => item.ID == id);
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
                    IEnumerable<Engineer?> engineersList = s_dal!.Engineer.ReadAll();
                    foreach (var e in engineersList)
                    {
                        Console.WriteLine(e);
                    }
                    break;

                case 4:  //Update
                    Console.WriteLine("Enter engineer ID to update:");
                    intString = Console.ReadLine()!;
                    int.TryParse(intString, out id);
                    engineer = s_dal!.Engineer.Read(item => item.ID == id);
                    if (engineer != null)
                    {
                        Console.WriteLine(engineer);   //Before the update prints the engineer to update.

                        newEngineer = inputUpdateEngineer(engineer);
                        s_dal!.Engineer.Update(newEngineer);

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
                    s_dal!.Engineer.Delete(id);
                    Console.WriteLine($"Engineer {id} deleted successfully");
                    break;

                case 0:
                    break;

                default:
                    break;
            }

            printSubMenu("Engineer");
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);
        }

    }


    /// <summary>
    /// Input fields of Engineer.
    /// </summary>
    /// <returns>Object type Engineer</returns>
    private static BO.Engineer inputCreateEngineer()
    {
        int id, level, cost, currentTaskId;
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

        //EngineerCurrentTask
        Console.WriteLine("Enter engineer current task:");
        Console.WriteLine("Enter current task ID:");
        intString = Console.ReadLine()!;
        int.TryParse(intString, out currentTaskId);


        //create newEngineer
        return new BO.Engineer(id, nameEngineer, email, ((BO.EngineerExperience)level), cost, null);

    }



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

                    if(calculateProjectStatus == BO.ProjectStatus.BuildingSchedule)
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
}
