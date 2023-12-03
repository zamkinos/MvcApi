using MvcApi.Islemler;
using MvcApi.Nesneler;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MvcApi.Controllers
{
    public class GirisController : ApiController
    {

        [HttpPost]
        public IHttpActionResult DogrulaKullanici(KeyValuePair<string, string> kullaniciBilgileri)
        {
            return Json(new GirisIslemleri().DogrulaKullanici(kullaniciBilgileri));
        }

        [HttpGet]
        public IHttpActionResult GetirAktifKullanicilar(string Anahtar)
        {
            if (Anahtar.Equals("***Zamkinos***"))
            {
                if (SrvSabitler.OturumListesi == null)
                    return Json(new List<KeyValuePair<string, Kullanici>>());
                else
                    return Json(SrvSabitler.OturumListesi.Select(z => new List<string>() { z.Key }).ToList());
            }
            else
            {
                return null;
            }
        }
    }
}