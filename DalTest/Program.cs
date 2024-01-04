namespace DalTest
{
    using Dal;
    using DalApi;
    using DO;
    
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation(); //stage 1
        private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1

        static void Main(string[] args)
        {
            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);

            int choice;
            printMainMenu();
            choice = Console.Read();


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
                            dependencySubMenu();
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
                choice = Console.Read();

            } while (choice != 0);
            
        }
        
        private static void printMainMenu()
        {
            Console.WriteLine("Select an entity of your choice:");
            Console.WriteLine("1 - Task\n" + "2 - Engineer\n" + "3 - Dependency\n" + "0 - Exit\n");
        }

        private static void printSubMenu(string entityName)
        {
            Console.WriteLine($"Select an action for the {entityName} entity:");
            Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "0 - Exit\n");
        }

        private static void taskSubMenu()
        {
            int choice, id;
            printSubMenu("Task");
            choice = Console.Read();

            while (choice != 0)
            {
                switch (choice)
                {
                    case 1:  //Create
                        

                        break;

                    case 2:  //Read
                        Console.WriteLine("Enter Task ID to read:");
                        id = Console.Read();
                        Task? task = s_dalTask!.Read(id);
                        if (task != null)
                        {
                            Console.WriteLine(task);
                        }
                        else
                        {
                            Console.WriteLine($"The Task With ID = {id} does not exist");
                        }
                        break;

                    case 3:  //ReadAll
                        List<Task> tasksList = s_dalTask!.ReadAll();
                        foreach (Task t in tasksList)
                        {
                            Console.WriteLine(t);
                        }
                        break;

                    case 4:
                        
                        break;

                    case 5:  //Delete
                        Console.WriteLine("Enter Task ID to delete:");
                        id = Console.Read();
                        s_dalTask!.Delete(id);
                        Console.WriteLine("Deleted successfully");
                        break;

                    case 0:
                        break;

                    default:
                        break;
                }
                printSubMenu("Task");
                choice = Console.Read();
            }
        }

        private static void engineerSubMenu()
        {
            printSubMenu("Engineer");
        }

        private static void dependencySubMenu()
        {
            int choice;
            int id, dependentTaskID, DependensOnTaskID;
            Dependency? dependency;
            printSubMenu("Dependency");
            choice = Console.Read();

            while (choice != 0)
            {
                switch (choice)
                {
                    case 1:  //Create
                        Console.WriteLine("Enter the ID of the dependent task:");
                        dependentTaskID = Console.Read();

                        Console.WriteLine($"Enter the ID of the task Which the task {dependentTaskID} depends on:");
                        DependensOnTaskID = Console.Read();

                        dependency = new Dependency(0, dependentTaskID, DependensOnTaskID);
                        s_dalDependency!.Create(dependency);
                        break;

                    case 2: //Read
                        Console.WriteLine("Enter Dependency ID to read:");
                        id = Console.Read();
                        dependency = s_dalDependency!.Read(id);
                        if (dependency != null)
                        {
                            Console.WriteLine(dependency);
                        }
                        else
                        {
                            Console.WriteLine($"The Dependency With ID = {id} does not exist");
                        }
                        break;

                    case 3:  //ReadAll
                        List<Dependency> dependenciesList = s_dalDependency!.ReadAll();
                        foreach (Dependency d in dependenciesList)
                        {
                            Console.WriteLine(d);
                        }
                        break;

                    case 4:  //Update
                        Console.WriteLine("Enter Dependency ID to Update:");
                        id = Console.Read();

                        dependency = s_dalDependency!.Read(id);
                        if (dependency != null)
                        {
                            Console.WriteLine(dependency);

                            Console.WriteLine("Enter the ID of the dependent task:");
                            dependentTaskID = Console.Read();

                            Console.WriteLine($"Enter the ID of the task Which the task {dependentTaskID} depends on:");
                            DependensOnTaskID = Console.Read();

                            dependency = new Dependency(0, dependentTaskID, DependensOnTaskID);
                            s_dalDependency!.Update(dependency);
                            Console.WriteLine($"Dependency {id} update successfully");
                        }
                        else
                        {
                            Console.WriteLine($"The Dependency With ID = {id} does not exist");
                        }
                        
                        break;

                    case 5:  //Delete
                        Console.WriteLine("Enter Dependency ID to delete:");
                        id = Console.Read();
                        s_dalDependency!.Delete(id);
                        Console.WriteLine($"Dependency {id} deleted successfully");
                        break;

                    case 0:  //Exit
                        break;

                    default:
                        break;

                }
            }
        }

    }

}
