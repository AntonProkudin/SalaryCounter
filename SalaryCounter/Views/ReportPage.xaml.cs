using Microsoft.Maui.Controls;

namespace SalaryCounter.Views;

public partial class ReportPage : ContentPage
{
    private readonly AppRepository _db;
    public static int HolderTaskId;
    public ReportPage(AppRepository db)
	{
        _db = db;
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        pickerCommand.Items.Add("Подсчёт эффективности работника");
        pickerCommand.Items.Add("Генерация по задаче");
        var pickerEmp = _db.GetEmployers();
        foreach (var item in pickerEmp)
        {
            pickerEmployer.Items.Add(item.LastName);
        }
        var pickerTas = _db.GetEmployerTasks();
        foreach (var item in pickerTas)
        {
            pickerTask.Items.Add(item.Id.ToString());
        }
    }
    private void Clicked(object sender, EventArgs e)
    {
        if (pickerCommand.SelectedItem.Equals("Подсчёт эффективности работника"))
        {
            int idEmployer = _db.FindEmployerId(pickerEmployer.SelectedItem.ToString());
            var Efficiency = _db.FindEmployerEfficiency(idEmployer);
            var Employer = _db.FindEmployer(idEmployer);
            var filePath = "C:\\Users\\anton\\Desktop\\otchet.csv";
            File.AppendAllText(filePath, "Имя;Фамилия;Эффективность\n");
            string str = $"{Employer.FirstName};{Employer.LastName};{Efficiency}\n";
            File.AppendAllText(filePath, str);
            AppShell.Current.DisplayAlert("Генерация", "Файл csv обновлен", "OK");
        }
        if (pickerCommand.SelectedItem.Equals("Генерация по задаче"))
        {
            HolderTaskId = Convert.ToInt32(pickerTask.SelectedItem);
            Shell.Current.GoToAsync($"CountSalaryTask");
        }
    }
   
}