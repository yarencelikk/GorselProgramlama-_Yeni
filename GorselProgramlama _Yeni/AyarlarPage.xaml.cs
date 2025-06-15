using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace GorselProgramlama_Yeni.Pages
{
    public partial class AyarlarPage : ContentPage
    {
        public AyarlarPage()
        {
            InitializeComponent();

            // Tercihi yükle
            if (Preferences.ContainsKey("KoyuTema"))
            {
                bool isDark = Preferences.Get("KoyuTema", false);
                App.Current.UserAppTheme = isDark ? AppTheme.Dark : AppTheme.Light;
            }
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            bool isDarkMode = e.Value;

            App.Current.UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;

            // Tercihi kaydet
            Preferences.Set("KoyuTema", isDarkMode);
        }
    }
}
