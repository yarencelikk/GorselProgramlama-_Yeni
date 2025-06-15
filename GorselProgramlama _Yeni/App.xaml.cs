using GorselProgramlama_Yeni.Pages; // LoginPage, MainPage burada ise
using GorselProgramlama_Yeni;       // AppShell burada ise

namespace GorselProgramlama_Yeni
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            Application.Current.Dispatcher.Dispatch(async () =>
            {
                await Shell.Current.GoToAsync("LoginPage");
            });
        }
    }
}
