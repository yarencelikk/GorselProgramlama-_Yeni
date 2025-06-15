namespace GorselProgramlama_Yeni.Models;

public class SehirHavaDurumu
{
    public string Name { get; set; }
    public string Source => $"https://www.mgm.gov.tr/sunum/tahmin-show-2.aspx?m={Name}&basla=1&bitir=5&rC=111&rZ=fff";
}
