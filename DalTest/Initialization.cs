namespace DalTest;
using DalApi;
using DO;

public static class Initialization
{
    private const int MIN_ID = 200000000;
    private const int MAX_ID = 400000000;
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();


    private static void createTasks()
    {
        
        
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

    private static void createDependencies()
    {


    }

}
