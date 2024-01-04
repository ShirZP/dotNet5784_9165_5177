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

            char choice;
          //  do 
         //   {
                try
                {
                    Console.WriteLine("");



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
               // choice = Console.ReadLine();
            //}
           // while (choice != '0' )
            
        }
        
        

    }

}
