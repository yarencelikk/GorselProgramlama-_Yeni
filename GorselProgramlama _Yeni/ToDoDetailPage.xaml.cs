using GorselProgramlama_Yeni.Models;
using GorselProgramlama_Yeni.Services;
namespace GorselProgramlama_Yeni.Pages;

public partial class ToDoDetailPage : ContentPage
{
    private readonly FirebaseService _firebaseService = new();

    public ToDoDetailPage()
    {
        InitializeComponent();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(titleEntry.Text))
        {
            await DisplayAlert("Uyarý", "Baþlýk boþ olamaz", "Tamam");
            return;
        }

        var newItem = new ToDoItem
        {
            Title = titleEntry.Text,
            Detail = detailEntry.Text,
            Date = datePicker.Date.ToString("yyyy-MM-dd"),
            Time = timePicker.Time.ToString(@"hh\:mm"),
            IsDone = false
        };

        await _firebaseService.AddToDoAsync(newItem);
        await Shell.Current.GoToAsync("..");
    }
}
