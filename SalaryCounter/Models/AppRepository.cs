using System.Diagnostics.Metrics;
using System.Reflection;

namespace SalaryCounter.Models;

public class AppRepository : IDisposable
{
    public readonly SQLiteConnection database;
    public AppRepository(string dbName)
    {
       // var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        var dbPath = "C:\\Users\\anton\\Desktop\\dbSC3";
        database = new SQLiteConnection(dbPath);
        database.CreateTable<Employer>();
        database.CreateTable<EmployerTask>();
        database.CreateTable<EmployersList>();
    }
    //Задачи на экране
    public List<TaskVisible> GetTasks()
    {
        var tasks = new List<TaskVisible>();
        var itemTask = database.Table<EmployerTask>().ToList();
        foreach (var item in itemTask)
        {
            string temp = "";
            var employer = database.Table<EmployersList>().Where(x => x.IdTask == item.EmployersId).ToList();
            foreach (var item2 in employer)
            {
                string tempstr = database.Table<Employer>().Where(x => x.Id == item2.IdEmployer).First().LastName;
                temp = temp + tempstr + ",";
            }
            var task = new TaskVisible
            {
                Id = item.Id,
                Name = item.Name,
                Rate = item.Rate,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                ResultTime = item.ResultTime,
                Employers = temp
            };
            tasks.Add(task);
        }
        return tasks;
    }

    public int UpdateEmployerTask(int id,DateTime temp)
    {
        var findTask = FindTask(id);
        findTask.ResultTime = temp;
        return database.Update(findTask);
    }
    //Поиски
    public int FindLastEmployerTask()
    {
        if(database.Table<EmployerTask>().FirstOrDefault()!=null)
            return database.Table<EmployerTask>().Last().Id;
        return 1;
    }
    public EmployerTask FindTask(int id)
    {
        return database.Table<EmployerTask>().Where(x => x.Id == id).FirstOrDefault();
    }
    public List<Employer> FindEmployers(int id)
    {
        var result = new List<Employer>();
        var employers = database.Table<EmployersList>().Where(x => x.IdTask == id).ToList();
        foreach (var employer in employers)
        { 
            var temp = database.Table<Employer>().Where(x => x.Id == employer.IdEmployer).FirstOrDefault();
            result.Add(temp);
        }
        return result;
    }
    public Employer FindEmployer(int id)
    {
        var result = database.Table<Employer>().Where(x => x.Id == id).FirstOrDefault();
        return result;
    }
    public int FindEmployerId(string name)
    {
        var result = database.Table<Employer>().Where(x => x.LastName.Equals(name)).FirstOrDefault().Id;
        return result;
    }
    public string FindEmployerEfficiency(int id)
    {
        int time = 0;
        int counter = 0;
        string result = "";
        var employers = database.Table<EmployersList>().Where(x => x.IdEmployer == id).ToList();
        foreach (var employer in employers)
        {
            var task = database.Table<EmployerTask>().Where(x => x.Id == employer.IdTask).FirstOrDefault();
            if (task.ResultTime > DateTime.MinValue) 
            {
                counter++;
                time = time + task.ResultTime.Subtract(task.StartTime).Days;
                if (task.ResultTime.Subtract(task.StartTime).Days == 0)
                    time++;
            }
        }
        result = result + ((decimal)counter / (decimal)time * 100);
        return result;
    }

    //Задачи в бд
    public List<EmployerTask> GetEmployerTasks()
    {
        return database.Table<EmployerTask>().ToList();
    }
    public int CreateEmployerTask(EmployerTask employer)
    {
        return database.Insert(employer);
    }
    public int UpdateEmployerTask(EmployerTask employer)
    {
        return database.Update(employer);
    }
    
    public int DeleteEmployerTask(int id)
    {
        var findTask = FindTask(id);
        var tasks = database.Table<EmployersList>().Where(x => x.IdTask == id).ToList();
        foreach (var task in tasks)
        {
            database.Delete(task);
        }
        return database.Delete(findTask);
    }
    // Работники в задачах 
    public List<EmployersList> GetEmployersList()
    {
        return database.Table<EmployersList>().ToList();
    }
    public int CreateEmployersList(EmployersList employer)
    {
        return database.Insert(employer);
    }
    public int UpdateEmployersList(EmployersList employer)
    {
        return database.Update(employer);
    }
    public int DeleteEmployersList(EmployersList employer)
    {
        return database.Delete(employer);
    }
    //Работники
    public List<Employer> GetEmployers()
    {
        return database.Table<Employer>().ToList();
    }
    public int CreateEmployer(Employer employer)
    {
        return database.Insert(employer);
    }
    public int UpdateEmployer(Employer employer)
    {
        return database.Update(employer);
    }
    public int DeleteEmployer(Employer employer)
    {
        return database.Delete(employer);
    }
    public void Dispose()
    {
        database.Dispose();
    }
}
