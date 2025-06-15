using System.Text.Json;
using System.Text.RegularExpressions;
using GorselProgramlama_Yeni.Models;

namespace GorselProgramlama_Yeni.Pages;

public partial class HaberlerPage : ContentPage
{
    private string aktifUrl = "https://www.trthaber.com/gundem_articles.rss";

    public HaberlerPage()
    {
        InitializeComponent();
        HaberleriYukle(aktifUrl);
    }

    private async void HaberleriYukle(string rssUrl)
    {
        try
        {
            string apiUrl = $"https://api.rss2json.com/v1/api.json?rss_url={rssUrl}";
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(apiUrl);

            using var doc = JsonDocument.Parse(json);
            var items = doc.RootElement.GetProperty("items");

            var haberler = new List<Haber>();
            foreach (var item in items.EnumerateArray())
            {
                var title = item.GetProperty("title").GetString();
                var description = item.GetProperty("description").GetString();
                var link = item.GetProperty("link").GetString();
                var pubDate = item.GetProperty("pubDate").GetString();
                var thumbnail = item.GetProperty("thumbnail").GetString();

                if (string.IsNullOrEmpty(thumbnail))
                {
                    thumbnail = Regex.Match(description ?? "", "<img.*?src=[\"'](.+?)[\"']").Groups[1].Value;
                }

                haberler.Add(new Haber
                {
                    title = title,
                    description = description,
                    link = link,
                    pubDate = pubDate,
                    thumbnail = thumbnail
                });
            }

            HaberListesi.ItemsSource = haberler;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Haberler yüklenemedi: " + ex.Message, "Tamam");
        }
    }

    private void KategoriSecildi(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string url)
        {
            aktifUrl = url;
            HaberleriYukle(aktifUrl);
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Haber secilenHaber)
        {
            await Navigation.PushAsync(new HaberDetayPage(secilenHaber));
        }

        ((CollectionView)sender).SelectedItem = null;
    }
}
