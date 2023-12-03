using MvcApi.Controllers;
using MvcApi.Nesneler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace MvcApi.Islemler
{
    public class AdakKurbanIslemleri
    {
        [WebApiTokenKontrolAttribute]
        [HttpPost]
        public DtoTemel GetirAdakKurbanCesit()
        {
            DtoTemel dtoSonuc = new DtoTemel() { IseHataVar = false, HataAciklama = ""};
            try
            {
                // Burası veritabanından gelecek...
                List<AdakKurban> lst = new List<AdakKurban>() {
                    new AdakKurban(){Id = 1, Agirlik = 50, Tur = AdakKurbanType.Koyun},
                    new AdakKurban(){Id = 2, Agirlik = 250, Tur = AdakKurbanType.Dana},
                    new AdakKurban(){Id = 3, Agirlik = 350, Tur = AdakKurbanType.Deve}
                };
                dtoSonuc.ToplamKayitSayisi = lst.Count;
                dtoSonuc.StrJsonSonuc = JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                // Hatayi loglamalisin...
                dtoSonuc.ToplamKayitSayisi = 0;
                dtoSonuc.HataAciklama = ex.Message;
                dtoSonuc.IseHataVar = true;
            }
            return dtoSonuc;
        }
    }
}