using GorselProgramlama_Yeni.Models;
using System.Text.Json;

namespace GorselProgramlama_Yeni.Pages
{
    public partial class KurlarPage : ContentPage
    {
        public KurlarPage()
        {
            InitializeComponent();
            YukleKurlari();
        }

        private async void YukleKurlari()
        {
            try
            {
                LoadingIndicator.IsVisible = true;
                KurListesi.IsVisible = false;

                var httpClient = new HttpClient();
                var json = await httpClient.GetStringAsync("https://finans.truncgil.com/today.json");

                var rawData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                var liste = new List<Kur>();
                foreach (var item in rawData)
                {
                    if (item.Key == "Update_Date") continue;

                    var data = item.Value;
                    liste.Add(new Kur
                    {
                        isim = item.Key,
                        Alýþ = data.GetProperty("Alýþ").GetString(),
                        Satýþ = data.GetProperty("Satýþ").GetString(),
                        Deðiþim = data.GetProperty("Deðiþim").GetString()
                    });
                }

                KurListesi.ItemsSource = liste;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Kurlar yüklenemedi: " + ex.Message, "Tamam");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                KurListesi.IsVisible = true;
            }

        }
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            YukleKurlari(); // yenileme iþlemini tetikle
        }
    }
}
