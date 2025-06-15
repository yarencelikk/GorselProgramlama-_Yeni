using System.Text.Json;
using GorselProgramlama_Yeni.Models;

namespace GorselProgramlama_Yeni.Pages;

public partial class HavaDurumuPage : ContentPage
{
    private List<string> sehirler = new();
    private readonly string jsonPath = Path.Combine(FileSystem.AppDataDirectory, "hava.json");

    public HavaDurumuPage()
    {
        InitializeComponent();
        YukleKayitliSehirler();
    }

    private void SehirEkle(object sender, EventArgs e)
    {
        string giris = sehirEntry.Text?.Trim();
        if (string.IsNullOrEmpty(giris)) return;

        string sehir = giris.ToUpperInvariant()
            .Replace("Ç", "C").Replace("Ð", "G").Replace("Ý", "I")
            .Replace("Ö", "O").Replace("Þ", "S").Replace("Ü", "U")
            .Replace("ç", "C").Replace("ð", "G").Replace("ý", "I")
            .Replace("ö", "O").Replace("þ", "S").Replace("ü", "U");

        if (sehirler.Contains(sehir)) return;

        sehirler.Add(sehir);
        SehirGorunumuEkle(sehir);
        KaydetSehirler();
        sehirEntry.Text = "";
    }

    private void SehirGorunumuEkle(string sehir)
    {
        var baslik = new Label
        {
            Text = sehir.ToLowerInvariant(),
            FontSize = 18,
            FontAttributes = FontAttributes.Bold
        };

        var resim = new Image
        {
            Source = new UriImageSource
            {
                Uri = new Uri($"https://www.mgm.gov.tr/sunum/tahmin-show-2.aspx?m={sehir}&basla=1&bitir=5&rC=111&rZ=fff"),
                CachingEnabled = false
            },
            Aspect = Aspect.AspectFit,
            HeightRequest = 100
        };

        var silBtn = new Button
        {
            Text = "Sil",
            BackgroundColor = Colors.Red,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End
        };

        var frame = new Frame
        {
            Content = new VerticalStackLayout
            {
                Children = { baslik, resim, silBtn }
            },
            BorderColor = Colors.Gray,
            CornerRadius = 10,
            Padding = 10
        };

        silBtn.Clicked += (s, e) =>
        {
            sehirler.Remove(sehir);
            havaDurumuStack.Children.Remove(frame);
            KaydetSehirler();
        };

        havaDurumuStack.Children.Add(frame);
    }

    private void KaydetSehirler()
    {
        File.WriteAllText(jsonPath, JsonSerializer.Serialize(sehirler));
    }

    private void YukleKayitliSehirler()
    {
        if (File.Exists(jsonPath))
        {
            var json = File.ReadAllText(jsonPath);
            var kayitli = JsonSerializer.Deserialize<List<string>>(json);
            if (kayitli != null)
            {
                foreach (var sehir in kayitli)
                {
                    sehirler.Add(sehir);
                    SehirGorunumuEkle(sehir);
                }
            }
        }
    }
}
