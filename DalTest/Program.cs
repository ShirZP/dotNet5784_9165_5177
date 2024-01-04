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
            printSubMenu("Engineer");
        }

        private static void dependencySubMenu()
        {
            printSubMenu("Dependency");
        }

    }

}
