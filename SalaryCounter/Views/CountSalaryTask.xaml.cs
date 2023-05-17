using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;

namespace SalaryCounter.Views;

public partial class CountSalaryTask : ContentPage
{
    private readonly AppRepository _db;
    public CountSalaryTask(AppRepository db)
	{
        _db = db;
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetEmployers();
    }
    private void Clicked(object sender, EventArgs e)
    {
        GenerateCsvTask();
    }
    private void GenerateCsvTask()
    {
        decimal salary = 0;
        var task = new EmployerTask();
        task = _db.FindTask(Convert.ToInt32(ReportPage.HolderTaskId));
        var filePath = "C:\\Users\\anton\\Desktop\\otchet.csv";
        salary = task.EndTime.Subtract(task.StartTime).Days * 8 * task.Rate;
        File.AppendAllText(filePath, "Имя;Фамилия;Зарплата\n");
        foreach (var item in collectionView.ItemsSource)
        {
            if (item is SalaryEmployer collectionItem)
            {
                 string str = $"{collectionItem.FirstName};{collectionItem.LastName};{salary * Convert.ToInt32(collectionItem.EntrySalary)}\n";
                 File.AppendAllText(filePath, str);
                 Console.WriteLine();

                string entryText = collectionItem.EntrySalary;
            }
        }
        AppShell.Current.DisplayAlert("Генерация", "Файл csv обновлен", "OK");
        Shell.Current.SendBackButtonPressed();
    }
    private void GetEmployers()
    {
        var collection = new List<SalaryEmployer>();
        foreach (var item in _db.FindEmployers(ReportPage.HolderTaskId))
        {
            var temp = new SalaryEmployer();
            temp.FirstName = item.FirstName;
            temp.LastName = item.LastName;
            temp.EntrySalary = "";
            collection.Add(temp);
        }
        collectionView.ItemsSource = collection;
    }
}