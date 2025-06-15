using GorselProgramlama_Yeni.Pages;

namespace GorselProgramlama_Yeni
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute(nameof(ToDoDetailPage), typeof(ToDoDetailPage));

        }
    }
}
