using System.Collections.Generic;

namespace MvcApi.Nesneler
{
    public static class SrvSabitler
    {
        public static List<KeyValuePair<string, Kullanici>> OturumListesi { get; set; }
        public const string API_TOKEN_ADI = "Anahtar";
        public const string VERITABANI_BAGLANTI_CUMLESI = "server=1.1.1.1;database=MOBIL;uid=mobiluser;password=mobilpassword;";
    }
}