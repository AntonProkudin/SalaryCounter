using System.Text.RegularExpressions;

namespace SalaryCounter.Views;

public partial class EmployerTaskPage : ContentPage
{
    private readonly AppRepository _db;
    public EmployerTaskPage(AppRepository db)
	{
        _db = db;
        InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetTasks();
        pickerCommand.Items.Add("Добавить");
        pickerCommand.Items.Add("Удалить");
        pickerCommand.Items.Add("Изменить");
        pickerCommand.Items.Add("Отметить выполненным");
    }
    private void Clicked(object sender, EventArgs e)
    {
        if (pickerCommand.SelectedItem.Equals("Добавить"))
        {
            int idTask = _db.FindLastEmployerTask() + 1;
            string[] split = { "," };
            string[] employerStr = AddEmployer.Text.Split(split, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in employerStr)
            {
                var temp = new EmployersList()
                {
                    IdEmployer = Convert.ToInt32(str),
                    IdTask = idTask
                };
                _db.CreateEmployersList(temp);
            }
            var task = new EmployerTask()
            {
                Name = AddName.Text,
                Rate = Convert.ToDecimal(AddRate.Text),
                EmployersId = _db.FindLastEmployerTask() + 1,
                StartTime = DatePickerStart.Date,
                EndTime = DatePickerEnd.Date,
            };
            _db.CreateEmployerTask(task);
            GetTasks();
        }
        if (pickerCommand.SelectedItem.Equals("Удалить"))
        {
            if (collectionView.SelectedItem is null)
                return;

            var template = collectionView.SelectedItem as TaskVisible;
            if (template is null)
                return;
            _db.DeleteEmployerTask(template.Id);
            GetTasks();
        }
        if (pickerCommand.SelectedItem.Equals("Изменить"))
        {

        }
        if (pickerCommand.SelectedItem.Equals("Отметить выполненным"))
        {
            if (collectionView.SelectedItem is null)
                return;

            var template = collectionView.SelectedItem as TaskVisible;
            DateTime temp = DateTime.Now;
            if (template is null)
                return;
            _db.UpdateEmployerTask(template.Id, temp);
            GetTasks();
        }
    }

    private void GetTasks()
    {
        collectionView.ItemsSource = Enumerable.Reverse(_db.GetTasks()).ToList();
    }
}