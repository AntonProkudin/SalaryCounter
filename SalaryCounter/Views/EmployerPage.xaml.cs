namespace SalaryCounter.Views;

public partial class EmployerPage : ContentPage
{
    private readonly AppRepository _db;
    public EmployerPage(AppRepository db)
	{
        _db = db;
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetEmployers();
        pickerCommand.Items.Add("Добавить");
        pickerCommand.Items.Add("Удалить");
        pickerCommand.Items.Add("Изменить");
    }
    private void Clicked(object sender, EventArgs e)
    {
        if (pickerCommand.SelectedItem.Equals("Добавить"))
        {
            var employer = new Employer()
            {
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text
            };
            _db.CreateEmployer(employer);
            GetEmployers();
        }
        if (pickerCommand.SelectedItem.Equals("Изменить"))
        {
            if (collectionView.SelectedItem is null)
                return;

            var employer = collectionView.SelectedItem as Employer;
            if (employer is null)
                return;

            employer.FirstName = FirstNameEntry.Text;
            employer.LastName = LastNameEntry.Text;
            _db.UpdateEmployer(employer);
            GetEmployers();
        }
        if (pickerCommand.SelectedItem.Equals("Удалить"))
        {
            if (collectionView.SelectedItem is null)
                return;

            var employer = collectionView.SelectedItem as Employer;
            if (employer is null)
                return;

            _db.DeleteEmployer(employer);
            GetEmployers();
        }
    }
    private void GetEmployers()
    {
        collectionView.ItemsSource = _db.GetEmployers();
    }
}