using GorselProgramlama_Yeni.Models;
using GorselProgramlama_Yeni.Services;
using System.Collections.ObjectModel;

namespace GorselProgramlama_Yeni.Pages;

public partial class ToDoPage : ContentPage
{
    private readonly FirebaseService _firebaseService = new();
    private ObservableCollection<ToDoItem> _items = new();

    public ToDoPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        var items = await _firebaseService.GetToDoItemsAsync();
        _items = new ObservableCollection<ToDoItem>(items);
        toDoCollection.ItemsSource = _items;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.CommandParameter as ToDoItem;

        if (item == null)
            return;

        var confirm = await DisplayAlert("Sil", "Görevi silmek istiyor musunuz?", "Evet", "Hayýr");
        if (confirm)
        {
            await _firebaseService.DeleteToDoAsync(item.Id);
            _items.Remove(item);
        }
    }
    private async void OnCheckboxChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var item = (ToDoItem)checkbox.BindingContext;

        if (item != null)
        {
            item.IsDone = e.Value;
            await _firebaseService.UpdateToDoAsync(item); // Firebase'e güncelle
        }
    }


    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ToDoDetailPage));
    }
}
