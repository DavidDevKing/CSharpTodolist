namespace TD
{
    public class TDList(string name, TDTask? parentTask = null)
    {
        private string name = name.Trim();
        public readonly TDTask? ParentTask = parentTask;
        public string Name
        {
            get { return name; }
        }

        public readonly List<TDTask> Tasks = [];

        public bool IsComplete
        {
            get
            {
                if (Tasks.Count == 0) return false;
                foreach (TDTask task in Tasks)
                {
                    if (!task.IsComplete) return false;
                }
                return true;
            }
        }

        private int idCounter = 0;


        public bool Rename(string name)
        {
            name = name.Trim();
            if (name == "") return false;
            this.name = name.Trim();
            ParentTask?.Rename(name, false);
            return true;
        }


        public bool AddTask(string name, bool isList = false)
        {
            name = name.Trim();
            if (name == "") return false;
            TDTask task = new(name, idCounter, this);
            Tasks.Add(task);
            idCounter++;
            if (isList) task.InitList();
            return true;
        }

        public bool RenameTask(int taskID, string name)
        {
            TDTask? task = GetTaskByID(taskID, out _);
            if (task == null) return false;
            task.Rename(name);
            return true;
        }

        public bool CompleteTask(int taskID)
        {
            TDTask? task = GetTaskByID(taskID, out _);
            if (task == null) return false;
            task.Complete();
            return true;
        }

        public void CompleteAll()
        {
            foreach (TDTask task in Tasks) task.Complete();
        }

        public void UnCompleteAll()
        {
            foreach (TDTask task in Tasks) task.UnComplete();
        }


        public bool UnCompleteTask(int taskID)
        {
            TDTask? task = GetTaskByID(taskID, out _);
            if (task == null) return false;
            task.UnComplete();
            return true;
        }

        public bool RemoveTask(int taskID)
        {
            TDTask? task = GetTaskByID(taskID, out _);
            if (task == null) return false;
            Tasks.Remove(task);
            return true;

        }

        public TDTask? GetTaskByID(int taskID, out int index)
        {
            index = 0;
            for (int i = 0; i < this.Tasks.Count; i++)
            {
                TDTask curTask = Tasks[i];
                if (curTask.ID == taskID)
                {
                    index = i;
                    return curTask;
                }
            }
            return null;
        }



    }
    public class TDTask(string name, int id, TDList parent)
    {
        public readonly int ID = id;
        public TDList Parent = parent;
        public string name = name;
        public TDList? list = null;
        private bool completed = false;

        public bool IsComplete
        {
            get
            {
                if (list == null) return completed;
                return list.IsComplete;
            }
        }
        

        public void Rename(string name, bool renameList = true)
        {
            this.name = name;
            if (list != null && renameList) list.Rename(name);
        }


        public void Complete()
        {
            completed = true;
            list?.CompleteAll();
        }
        public void UnComplete()
        {
            completed = false;
            list?.UnCompleteAll();
        }

        public void InitList()
        {
            list = new(name, this);
        }

    }
}