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
            printSubMenu("Task"); 
        }

        private static void engineerSubMenu()
        {
            int choice, id, level, cost;
            string? nameEngineer, email;
            Engineer? engineer, newEngineer;
            printSubMenu("Engineer");
            choice = Console.Read();


            while(choice!=0) 
            {
                switch (choice)
                {
                    case 1:  //Create
                        Console.WriteLine("Enter engineer ID:");
                        id = Console.Read();

                        Console.WriteLine("Enter engineer full name:");
                        nameEngineer = Console.ReadLine();

                        Console.WriteLine("Enter engineer email:");
                        email = Console.ReadLine();
                        Console.WriteLine("Choose engineer level:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
                        level = Console.Read();

                        Console.WriteLine("Enter engineer cost:");
                        cost = Console.Read();

                        newEngineer = new Engineer(id, nameEngineer, email, ((EngineerExperience)level), cost);
                        s_dalEngineer!.Create(newEngineer);

                        break;

                    case 2:  //Read
                        Console.WriteLine("Enter engineer ID:");
                        id = Console.Read();
                        engineer = s_dalEngineer!.Read(id);
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
                        List<Engineer> engineersList = s_dalEngineer!.ReadAll();
                        foreach(var e in engineersList)
                        {
                            Console.WriteLine(e);    
                        }
                        break;

                    case 4:  //Update
                        Console.WriteLine("Enter engineer ID to update:");
                        id = Console.Read();
                        engineer = s_dalEngineer!.Read(id);
                        if (engineer != null)
                        {
                            Console.WriteLine(engineer);

                            Console.WriteLine("Enter engineer full name:");
                            nameEngineer = Console.ReadLine();

                            Console.WriteLine("Enter engineer email:");
                            email = Console.ReadLine();

                            Console.WriteLine("Choose engineer level:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
                            level = Console.Read();

                            Console.WriteLine("Enter engineer cost:");
                            cost = Console.Read();

                            newEngineer = new Engineer(id, nameEngineer, email, ((EngineerExperience)level), cost);
                            s_dalEngineer!.Update(newEngineer);

                            Console.WriteLine($"Engineer { id } update successfully");
                        }
                        else
                        {
                            Console.WriteLine($"The engineer with ID = {id} does not exist");
                        }
                        break;

                    case 5:  //Delete
                        Console.WriteLine("Enter engineer ID to read:");
                        id = Console.Read();
                        s_dalEngineer!.Delete(id);
                        Console.WriteLine($"Engineer {id} deleted successfully");
                        break;

                    case 0:
                        break;

                    default:
                        break;
                }

                printSubMenu("Engineer");
                choice = Console.Read();
            }

        }

        private static void dependencySubMenu()
        {
            printSubMenu("Dependency");
        }

    }

}
