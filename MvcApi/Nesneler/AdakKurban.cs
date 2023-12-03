namespace MvcApi.Nesneler
{
    public class AdakKurban
    {
        public int Id { get; set; }
        public AdakKurbanType Tur { get; set; }
        public decimal Agirlik { get; set; }
    }

    public enum AdakKurbanType
    {
        Koyun = 1,
        Koc = 2,
        Dana = 3,
        Deve = 4
    }
}