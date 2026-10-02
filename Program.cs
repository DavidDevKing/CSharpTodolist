using TD;


namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the TodoListApp. Type :h for help.");
            TDList curList = new("Home");
            bool typedHelp = false;

            Dictionary<string, Func<string, bool>> CommandList = new()
            {
                // Adds a new task
                {":t", param => {return curList.AddTask(param);}},
                // Adds a new list
                {":l", param => {return curList.AddTask(param, true);}},
                // Renames the current list
                {":r", param => {return curList.Rename(param);}},
                // Displays the help menu
                {":h", param => {typedHelp = true; return true;}},
                {
                    // Deletes the task or list with the specified id
                    ":dt", param =>
                    {
                        if (!int.TryParse(param, out int id)) return false;
                        return curList.RemoveTask(id);
                    }
                },
                {
                    // Deletes the current list
                    ":dd", param =>
                    {
                        if (curList.ParentTask == null) return false;
                        int id = curList.ParentTask.ID;
                        curList = curList.ParentTask.Parent;
                        return curList.RemoveTask(id);
                    }
                },
                {
                    // Renames the task or list with the specified id
                    ":rt", param => 
                    {
                        string idStr = "";
                        for (int i = 0; i < param.Length; i++)
                        {
                            char c = param[i];
                            if (c == ' ') break;
                            idStr += c;
                        }
                        if (!int.TryParse(idStr, out int id)) return false;
                        string name = param[idStr.Length..];
                        name = name.Trim();
                        if (name == "") return false;
                        return curList.RenameTask(id, name);
                    }
                },
                {
                    // Completes the task or list with the specified ID
                    ":ct", param =>
                    {
                        if (!int.TryParse(param, out int id)) return false;
                        return curList.CompleteTask(id);
                    }
                },
                {
                    // UnCompletes the task with the selected ID
                    ":ut", param =>
                    {
                        if (!int.TryParse(param, out int id)) return false;
                        return curList.UnCompleteTask(id);
                    }
                },
                {
                    // Changes directory to using IDs
                    "cd", param => 
                    {
                        TDList targetList = curList;
                        string[] dirs = param.Split("/");
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            if (dirs[i] =="..")
                            {
                                if (targetList.ParentTask == null) return false;
                                targetList = targetList.ParentTask.Parent;
                                continue;
                            }
                            if (i == dirs.Length-1 && dirs[i] == "") break;
                            if (!int.TryParse(dirs[i], out int id)) return false;
                            TDTask? task = targetList.GetTaskByID(id, out _);
                            if (task == null) return false;
                            if (task.list == null) return false;
                            targetList = task.list;
                        }
                        curList = targetList;
                        return true;
                    }
                },
                {
                    // Quits the application
                    ":q", param =>
                    {
                        Environment.Exit(0);
                        return true;
                    }
                }

            };



            while (true)
            {
                if (typedHelp)
                {
                    Console.WriteLine("Command                         Function");
                    Console.WriteLine(":h                              Displays the help menu.");
                    Console.WriteLine(":t [name]                       Creates a new task with the specified name.");
                    Console.WriteLine(":l [name]                       Creates a new list with the specified name.");
                    Console.WriteLine(":r [name]                       Renames the current list to the specified name.");
                    Console.WriteLine(":rt [id] [name]                 Renames the task or list with the specified id to the specified name.");
                    Console.WriteLine(":ct [id]                        Completes the task or list with the specified id.");
                    Console.WriteLine(":ut [id]                        Uncompletes the task or list with the selected id.");
                    Console.WriteLine(":dt [id]                        Deletes the task or list with the specified id.");
                    Console.WriteLine(":dd                             Deletes the current list.");
                    Console.WriteLine("cd [directory]                  Changes directory of lists based on their id.");
                    Console.WriteLine(":q                              Quits the application");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Write(new string('\n', 50));
                    typedHelp = false;
                }
                Console.WriteLine(ListDirectory(curList));
                foreach (TDTask task in curList.Tasks) Console.WriteLine($"{task.ID}: {task.name}{(task.IsComplete ? " ✅️" : "")}{(task.list == null ? "" : " >")}");

                Console.Write("\n");

                Console.WriteLine("Enter a command: ");
                string? command = Console.ReadLine();
                if (command == null) break;
                Execute(command, CommandList);
                Console.Write(new string('\n',50));
            }
        }




        ///<summary>
        /// Executes a terminal command.
        /// </summary>
        /// <param name="command">The command to be executed.</param>
        /// <param name="commandList">The Dictionary contain all the possible commands, and their functions.</param>
        /// <returns>True if the command is successfully executed.</returns>
        static bool Execute(string command, Dictionary<string, Func<string, bool>> commandList)
        {
            command = command.Trim();
            if (command == "") return false;
            // Extract the command Key word
            string[] parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string keyword = parts[0];
            string param = parts.Length > 1 ? parts[1] : "";

            param = param.Trim();

            if (!commandList.TryGetValue(keyword, out Func<string, bool>? commandFunc)) return false;
            return commandFunc(param);
        }


        ///<summary>Finds the directory of the current list</summary>
        ///<param name="curList">The list to find the directory off</param>
        ///<returns>The directory of the current list as a string</returns>
        static string ListDirectory(TDList curlist)
        {
            string dir = "";
            TDList myCur = curlist;
            List<string> parentList = [];
            while (true)
            {
                parentList.Add(myCur.Name);
                if (myCur.ParentTask == null) break;
                myCur = myCur.ParentTask.Parent;
            }

            for (int i = parentList.Count - 1; i >= 0; i--)
            {
                string parentName = parentList[i];
                dir += parentName;
                if (i != 0) dir += " > ";
            }


            return dir;
        }
    }
}
