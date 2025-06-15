using GorselProgramlama_Yeni.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace GorselProgramlama_Yeni;

public partial class HaberDetayPage : ContentPage
{
    private Haber haber;

    public HaberDetayPage(Haber secilenHaber)
    {
        InitializeComponent();
        haber = secilenHaber;

        BaslikLabel.Text = haber.title;
        TarihLabel.Text = haber.pubDate;
        HaberResim.Source = haber.thumbnail;

        _ = YukleTamIcerik();
    }

    private async Task YukleTamIcerik()
    {
        try
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(haber.link);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            // Makale içeriðini bul (TRT için özel class kontrolü)
            var detayDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class,'news-content')]");

            if (detayDiv != null)
            {
                // Kaynak ve Etiket içeriðini sil
                var silinecekNodlar = detayDiv.SelectNodes(".//div[contains(@class,'news-meta')] | .//div[contains(@class,'news-tags')]");
                if (silinecekNodlar != null)
                {
                    foreach (var node in silinecekNodlar)
                        node.Remove();
                }

                string tamMetin = detayDiv.InnerText.Trim();
                tamMetin = Regex.Replace(tamMetin, @"\s+", " "); // Fazla boþluklarý temizle
                AciklamaLabel.Text = tamMetin;
            }
            else
            {
                AciklamaLabel.Text = "Detay içeriði alýnamadý.";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Ýçerik alýnýrken hata oluþtu: {ex.Message}", "Tamam");
        }
    }

    private async void ShareClicked(object sender, EventArgs e)
    {
        await Share.RequestAsync(new ShareTextRequest
        {
            Uri = haber.link,
            Title = haber.title
        });
    }
}
