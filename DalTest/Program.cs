namespace DalTest
{
    using Dal;
    using DalApi;
    using DO;
    
    internal class Program
    {
        //static readonly IDal s_dal = new DalList(); //stage 2
        static readonly IDal s_dal = new DalXml(); //stage 3


        static void Main(string[] args)
        {
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
                            dependencySubMenu();
                            break;
                        case 4:
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); 
                            if (ans == "Y") //stage 3
                            {

                                Initialization.Do(s_dal); //stage 2
                            }
                                
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
            Console.WriteLine("1 - Task\n" + "2 - Engineer\n" + "3 - Dependency\n" + "4 - Reset data\n" + "0 - Exit\n");
        }

        /// <summary>
        /// Print sub menu.
        /// </summary>
        /// <param name="entityName">The name of the entity to print its sub menu.</param>
        private static void printSubMenu(string entityName)
        {
            Console.WriteLine($"Select an action for the {entityName} entity:");
            Console.WriteLine("1 - Create\n" + "2 - Read\n" + "3 - ReadAll\n" + "4 - Update\n" + "5 - Delete\n" + "0 - Exit\n");
        }

        #region  entities CRUD
        /// <summary>
        /// All the operation that we can do on task (CRUD).
        /// </summary>
        private static void taskSubMenu()
        {
            int choice, id;
            Task? task;
            printSubMenu("Task");
            string? intString;   //string to convert to int
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);

            while (choice != 0)
            {
                switch (choice)
                {
                    case 1:  //Create

                        task = inputCreateTask();
                        s_dal!.Task.Create(task);  
                        break;

                    case 2:  //Read
                        Console.WriteLine("Enter Task ID to read:");
                        intString = Console.ReadLine()!;
                        int.TryParse(intString, out id);
                        task = s_dal!.Task.Read(item => item.ID == id);
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
                        IEnumerable<Task?> tasksList = s_dal!.Task.ReadAll();
                        foreach (var t in tasksList)
                        {
                            Console.WriteLine(t);
                        }
                        break;

                    case 4: //Update
                        Console.WriteLine("Enter task ID to update:");
                        intString = Console.ReadLine()!;
                        int.TryParse(intString, out id);
                        task = s_dal!.Task.Read(item => item.ID == id);
                        if (task != null)
                        {
                            Console.WriteLine(task);   //Before the update prints the task to update.
                            task = inputUpdateTask(task);
                            s_dal!.Task.Update(task);
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
                printSubMenu("Task");
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
            Engineer? engineer, newEngineer;
            printSubMenu("Engineer");
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);


            while (choice!=0) 
            {
                switch (choice)
                {
                    case 1:  //Create
                        newEngineer = inputCreateEngineer();
                        s_dal!.Engineer.Create(newEngineer);
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
                        foreach(var e in engineersList)
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

                            Console.WriteLine($"Engineer { id } update successfully");
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
        /// All the operation that we can do on dependency (CRUD).
        /// </summary>
        private static void dependencySubMenu()
        {
            int choice;
            int id;
            
            Dependency? dependency;
            printSubMenu("Dependency");
            string? intString;   //string to convert to int
            intString = Console.ReadLine()!;
            int.TryParse(intString, out choice);

            while (choice != 0)
            {
                switch (choice)
                {
                    case 1:  //Create
                        dependency = inputCreateDependency();

                        s_dal!.Dependency.Create(dependency);
                        break;

                    case 2: //Read
                        Console.WriteLine("Enter Dependency ID to read:");
                        intString = Console.ReadLine()!;
                        int.TryParse(intString, out id);
                        dependency = s_dal!.Dependency.Read(item => item.ID == id);
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
                        IEnumerable<Dependency?> dependenciesList = s_dal!.Dependency.ReadAll();
                        foreach (var d in dependenciesList)
                        {
                            Console.WriteLine(d);
                        }
                        break;

                    case 4:  //Update
                        Console.WriteLine("Enter Dependency ID to Update:");
                        intString = Console.ReadLine()!;
                        int.TryParse(intString, out id);

                        dependency = s_dal!.Dependency.Read(item => item.ID == id);
                        if (dependency != null)
                        {
                            Console.WriteLine(dependency);   //Before the update prints the dependency to update.

                            dependency = inputUpdateDependency(dependency);

                            s_dal!.Dependency.Update(dependency);
                            Console.WriteLine($"Dependency {id} update successfully");
                        }
                        else
                        {
                            Console.WriteLine($"The Dependency With ID = {id} does not exist");
                        }
                        
                        break;

                    case 5:  //Delete
                        Console.WriteLine("Enter Dependency ID to delete:");
                        intString = Console.ReadLine()!;
                        int.TryParse(intString, out id);
                        s_dal!.Dependency.Delete(id);
                        Console.WriteLine($"Dependency {id} deleted successfully");
                        break;

                    case 0:  //Exit
                        break;

                    default:
                        break;

                }
                printSubMenu("Dependency");
                intString = Console.ReadLine()!;
                int.TryParse(intString, out choice);
            }
        }
        #endregion

        #region  input methods
        /// <summary>
        /// Input fields of Dependency.
        /// </summary>
        /// <returns>Object type Dependency</returns>
        private static Dependency inputCreateDependency()
        {
            int dependentTaskID, DependensOnTaskID;
            string intString;

            //dependentTaskID
            Console.WriteLine("Enter the ID of the dependent task:");
            intString = Console.ReadLine()!;
            int.TryParse(intString, out dependentTaskID);

            //DependensOnTaskID
            Console.WriteLine($"Enter the ID of the task Which the task {dependentTaskID} depends on:");
            intString = Console.ReadLine()!;
            int.TryParse(intString, out DependensOnTaskID);

            return new Dependency(0, dependentTaskID, DependensOnTaskID);
        }

        /// <summary>
        /// update fields of Dependency.
        /// </summary>
        /// <param name="originalDependency">dependency to update.</param>
        /// <returns></returns>
        private static Dependency inputUpdateDependency(Dependency originalDependency)
        {
            int dependentTaskID, DependensOnTaskID;
            string intString;

            //dependentTaskID
            Console.WriteLine("Enter the ID of the dependent task:");
            intString = Console.ReadLine()!;
            if (intString == null || intString == "")
                dependentTaskID = originalDependency.DependentTask;
            else
                int.TryParse(intString, out dependentTaskID);

            //DependensOnTaskID
            Console.WriteLine($"Enter the ID of the task Which the task {dependentTaskID} depends on:");
            intString = Console.ReadLine()!;
            if (intString == null || intString == "")
                DependensOnTaskID = originalDependency.DependensOnTask;
            else
                int.TryParse(intString, out DependensOnTaskID);

            return new Dependency(originalDependency.ID, dependentTaskID, DependensOnTaskID);
        }

        /// <summary>
        /// Input fields of Engineer.
        /// </summary>
        /// <returns>Object type Engineer</returns>
        private static Engineer inputCreateEngineer()
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
             return new Engineer(id, nameEngineer, email, ((EngineerExperience)level), cost);

        }

        /// <summary>
        /// update fields of Engineer.
        /// </summary>
        /// <param name="originalEngineer">engineer to update</param>
        /// <returns>Object type Engineer</returns>
        private static Engineer inputUpdateEngineer(Engineer originalEngineer)
        {
            int? levelNum;
            double? cost;
            string? intString;    //string to convert to int
            string? nameEngineer, email;

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
                level = (EngineerExperience)levelNum!;
            }

            //cost
            Console.WriteLine("Enter engineer cost:");
            intString = Console.ReadLine()!;
            if (intString == null || intString == "")
                cost = originalEngineer.Cost;
            else
                cost = TryParseInt(intString);

            //update newEngineer
            return new Engineer(originalEngineer.ID, nameEngineer, email, level, cost);
        }

        /// <summary>
        /// Input fields of Task.
        /// </summary>
        /// <returns>Object type Task</returns>
        private static Task inputCreateTask()
        {
            string nickName, description;
            string? finalProduct, remarks;
            DateTime? scheduledDate, startDate, completeDate, deadlineDate;
            string? dateString, intString;
            TimeSpan? requiredEffortTime;
            int? engineerId, complexityNum;
            DO.EngineerExperience? complexity;

            //nickName
            Console.WriteLine("Enter task nickName:");
            nickName = Console.ReadLine()!;

            //description
            Console.WriteLine("Enter task description:");
            description = Console.ReadLine()!;

            //scheduledDate
            Console.WriteLine("Enter task scheduled date to start:");
            dateString = Console.ReadLine();
            scheduledDate = TryParseDateTime(dateString);

            //startDate
            Console.WriteLine("Enter task actual start date:");
            dateString = Console.ReadLine();
            startDate = TryParseDateTime(dateString);

            //requiredEffortTime
            Console.WriteLine("Enter task required effort time to complete:");
            dateString = Console.ReadLine();
            requiredEffortTime = TryParseTimeSpan(dateString);

            //completeDate
            Console.WriteLine("Enter task complete date:");
            dateString = Console.ReadLine();
            completeDate = TryParseDateTime(dateString);

            //finalProduct
            Console.WriteLine("Enter task finalProduct:");
            finalProduct = Console.ReadLine();

            //remarks
            Console.WriteLine("Enter task remarks:");
            remarks = Console.ReadLine();

            //engineerId
            Console.WriteLine("Enter the ID of the engineer responsible for carrying out the task:");
            intString = Console.ReadLine();
            engineerId = TryParseInt(intString);

            //complexity
            Console.WriteLine("Choose task complexity:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
            intString = Console.ReadLine();
            if (intString != null)
            {
                complexityNum = TryParseInt(intString);
                complexity = (EngineerExperience)complexityNum!;
            }
            else
            {
                complexity = null;
            }

            //deadlineDate
            Console.WriteLine("Enter task deadline date:");
            dateString = Console.ReadLine();
            deadlineDate = TryParseDateTime(dateString);

            return new Task(0, nickName, description, scheduledDate, startDate, requiredEffortTime, completeDate, finalProduct, remarks, engineerId, complexity, deadlineDate);
        }

        /// <summary>
        /// update fields of Task.
        /// </summary>
        /// <param name="originalTask">task to update.</param>
        /// <returns></returns>
        private static Task inputUpdateTask(Task originalTask)
        {
            string nickName, description;
            string? finalProduct, remarks;
            DateTime? scheduledDate, startDate, completeDate, deadlineDate;
            string? dateString, intString, timeString;
            TimeSpan? requiredEffortTime;
            int? engineerId, complexityNum;

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

            //scheduledDate
            Console.WriteLine("Enter task scheduled date to start:");
            dateString = Console.ReadLine();
            if (dateString == null || dateString == "")
                scheduledDate = originalTask.ScheduledDate;
            else
                scheduledDate = TryParseDateTime(dateString);

            //startDate
            Console.WriteLine("Enter task actual start date:");
            dateString = Console.ReadLine();
            if (dateString == null || dateString == "")
                startDate = originalTask.StartDate;
            else
                startDate = TryParseDateTime(dateString);

            //requiredEffortTime
            Console.WriteLine("Enter task required effort time to complete:");
            timeString = Console.ReadLine();
            if (timeString == null || timeString == "")
                requiredEffortTime = originalTask.RequiredEffortTime;
            else
                requiredEffortTime = TryParseTimeSpan(dateString);

            //completeDate
            Console.WriteLine("Enter task complete date:");
            dateString = Console.ReadLine();
            if (dateString == null || dateString == "")
                completeDate = originalTask.CompleteDate;
            else
                completeDate = TryParseDateTime(dateString);

            //finalProduct
            Console.WriteLine("Enter task finalProduct:");
            finalProduct = Console.ReadLine();
            if (finalProduct == null || finalProduct == "")
                finalProduct = originalTask.FinalProduct;

            //remarks
            Console.WriteLine("Enter task remarks:");
            remarks = Console.ReadLine();
            if (remarks == null || remarks == "")
                remarks = originalTask.Remarks;
            

            //engineerId
            Console.WriteLine("Enter the ID of the engineer responsible for carrying out the task:");
            intString = Console.ReadLine();
            if (intString == null || intString == "")
                engineerId = originalTask.EngineerId;
            else
                engineerId = TryParseInt(intString);

            //complexity
            Console.WriteLine("Choose task complexity:" + "0 - Beginner\n" + "1 - AdvancedBeginner\n" + "2 - Intermediate\n" + "3 - Advanced\n" + "4 - Expert\n");
            intString = Console.ReadLine();
            DO.EngineerExperience? complexity;
            if (intString == null || intString == "")
                complexity = originalTask.Complexity;
            else
            {
                complexityNum = TryParseInt(intString);
                complexity = (EngineerExperience)complexityNum!;
            }

            //deadlineDate
            Console.WriteLine("Enter task deadline date:");
            dateString = Console.ReadLine();
            if (dateString == null || dateString == "")
                deadlineDate = originalTask.DeadlineDate;
            else
                deadlineDate = TryParseDateTime(dateString);

            return new Task(originalTask.ID, nickName, description, scheduledDate, startDate, requiredEffortTime, completeDate, finalProduct, remarks, engineerId, complexity, deadlineDate);
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

    }

}
