using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace GorselProgramlama_Yeni.Pages
{
    public partial class LoginPage : ContentPage
    {
        private const string ApiKey = "AIzaSyBKXv1zhSzl0YAEequZV9rvfGnVHEmk9OA";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "L�tfen e-posta ve �ifre alanlar�n� doldurun.";
                ErrorLabel.IsVisible = true;
                return;
            }

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}";

            var data = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Giri� Ba�ar�l�", "Ho� geldiniz!", "Tamam");
                ErrorLabel.IsVisible = false;
                await Shell.Current.GoToAsync("MainPage");
            }
            else
            {
                if (result.Contains("EMAIL_NOT_FOUND"))
                    ErrorLabel.Text = "Bu e-posta adresine ait bir hesap bulunamad�.";
                else if (result.Contains("INVALID_PASSWORD"))
                    ErrorLabel.Text = "�ifre yanl��. L�tfen tekrar deneyin.";
                else if (result.Contains("INVALID_LOGIN_CREDENTIALS"))
                    ErrorLabel.Text = "Ge�ersiz giri� bilgileri. E-posta veya �ifre hatal� olabilir.";
                else
                    ErrorLabel.Text = "Giri� yap�lamad�. L�tfen bilgilerinizi kontrol edin.";

                ErrorLabel.IsVisible = true;
            }
        }



        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var fullName = FullNameEntry.Text?.Trim();
            var email = RegisterEmailEntry.Text?.Trim();
            var password = RegisterPasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorLabel.Text = "L�tfen t�m alanlar� doldurun.";
                ErrorLabel.IsVisible = true;
                return;
            }

            var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}";

            var data = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Ba�ar�l�", $"Kay�t tamamland�, Ho� geldin {fullName}", "Tamam");
                RegisterSection.IsVisible = false;
                LoginSection.IsVisible = true;
                ErrorLabel.IsVisible = false;
            }
            else
            {
                if (result.Contains("WEAK_PASSWORD"))
                    ErrorLabel.Text = "�ifreniz en az 6 karakter olmal�d�r.";
                else if (result.Contains("EMAIL_EXISTS"))
                    ErrorLabel.Text = "Bu e-posta zaten kay�tl�.";
                else
                    ErrorLabel.Text = "Kay�t ba�ar�s�z. L�tfen tekrar deneyin.";

                ErrorLabel.IsVisible = true;
            }
        }



        private void OnShowRegisterTapped(object sender, EventArgs e)
        {
            LoginSection.IsVisible = false;
            RegisterSection.IsVisible = true;
            // Hatalar� temizle
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
        }

        private void OnHideRegisterTapped(object sender, EventArgs e)
        {
            RegisterSection.IsVisible = false;
            LoginSection.IsVisible = true;
            // Hatalar� temizle
            ErrorLabel.IsVisible = false;
            ErrorLabel.Text = string.Empty;
        }

    }
}
