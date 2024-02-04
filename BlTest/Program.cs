using DalApi;
using DalTest;

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
                        printEngineerSubMenu();
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
        Console.WriteLine("1 - Task\n" + "2 - Engineer\n" + "0 - Exit\n");
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
        Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "0 - Exit\n");
    }



    /// <summary>
    /// All the operation that we can do on task.
    /// </summary>
    private static void taskSubMenu()
    {
        //int choice, id;
        //Task? task;
        //printTaskSubMenu();
        //string? intString;   //string to convert to int
        //intString = Console.ReadLine()!;
        //int.TryParse(intString, out choice);

        //while (choice != 0)
        //{
        //    switch (choice)
        //    {
        //        case 1:  //Create
        //            task = inputCreateTask();
        //            s_dal!.Task.Create(task);
        //            break;

        //        case 2:  //Read
        //            Console.WriteLine("Enter Task ID to read:");
        //            intString = Console.ReadLine()!;
        //            int.TryParse(intString, out id);
        //            task = s_dal!.Task.Read(item => item.ID == id);
        //            if (task != null)
        //            {
        //                Console.WriteLine(task);
        //            }
        //            else
        //            {
        //                Console.WriteLine($"The Task With ID = {id} does not exist");
        //            }
        //            break;

        //        case 3:  //ReadAll
        //            IEnumerable<Task?> tasksList = s_dal!.Task.ReadAll();
        //            foreach (var t in tasksList)
        //            {
        //                Console.WriteLine(t);
        //            }
        //            break;

        //        case 4: //Update
        //            Console.WriteLine("Enter task ID to update:");
        //            intString = Console.ReadLine()!;
        //            int.TryParse(intString, out id);
        //            task = s_dal!.Task.Read(item => item.ID == id);
        //            if (task != null)
        //            {
        //                Console.WriteLine(task);   //Before the update prints the task to update.
        //                task = inputUpdateTask(task);
        //                s_dal!.Task.Update(task);
        //            }
        //            break;

        //        case 5:  //Delete
        //            Console.WriteLine("Enter Task ID to delete:");
        //            intString = Console.ReadLine()!;
        //            int.TryParse(intString, out id);
        //            s_dal!.Task.Delete(id);
        //            Console.WriteLine("Deleted successfully");
        //            break;

        //        case 0:
        //            break;

        //        default:
        //            break;
        //    }
        //    printTaskSubMenu();
        //    intString = Console.ReadLine()!;
        //    int.TryParse(intString, out choice);
        //}
    }
}
