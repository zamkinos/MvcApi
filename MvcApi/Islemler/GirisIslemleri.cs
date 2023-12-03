using MvcApi.Nesneler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;

namespace MvcApi.Islemler
{
    public class GirisIslemleri
    {
        [HttpPost]
        public DtoTemel DogrulaKullanici(KeyValuePair<string, string> kullaniciBilgileri)
        {
            DtoTemel dtoSonuc = new DtoTemel() { IseHataVar = false, HataAciklama = "", ToplamKayitSayisi = 1};
            try
            {
                List<SqlParameter> lstParametreler = new List<SqlParameter>() {
                    new SqlParameter() { ParameterName = "@KullaniciKodu", Value = kullaniciBilgileri.Key, Direction = ParameterDirection.Input, DbType = DbType.String },
                    new SqlParameter() { ParameterName = "@Sifre", Value = kullaniciBilgileri.Value, Direction = ParameterDirection.Input, DbType = DbType.String }
                };
                using (VeritabaniIslemleri vtIslem = new VeritabaniIslemleri())
                {
                    Kullanici objKullanici = new Kullanici();
                    DataTable dtKullanici = vtIslem.ReturnDataTable("PRC_L_GETIRKULLANICI", lstParametreler);
                    foreach (DataRow dr in dtKullanici.Rows)
                    {
                        objKullanici.FirmaKodu = "01";
                        objKullanici.KullaniciAdi = kullaniciBilgileri.Key;
                        objKullanici.Sifre = kullaniciBilgileri.Value;
                        objKullanici.SifreToken = tokenUret();
                    }
                    OturumEkle(objKullanici.KullaniciAdi, objKullanici);
                    dtoSonuc.StrJsonSonuc = JsonConvert.SerializeObject(objKullanici);

                    // Girişleri loglamakta fayda var.
                    lstParametreler = new List<SqlParameter>() {
                        new SqlParameter() { ParameterName = "@KullaniciKodu", Value = objKullanici.KullaniciAdi, Direction = ParameterDirection.Input, DbType = DbType.String },
                        new SqlParameter() { ParameterName = "@Anahtar", Value = objKullanici.SifreToken, Direction = ParameterDirection.Input, DbType = DbType.String },
                        new SqlParameter() { ParameterName = "@IpAdresi", Value = getirIpAdres(HttpContext.Current), Direction = ParameterDirection.Input, DbType = DbType.String }
                    };
                    vtIslem.ExecuteInsertUpdateDelete("PRC_I_APIGIRISLOGLA", lstParametreler);
                }
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

        private string getirIpAdres(HttpContext context)
        {
            string ipAdresi = context.Request.UserHostAddress;
            if (string.IsNullOrEmpty(ipAdresi))
            {
                ipAdresi = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipAdresi))
                {
                    ipAdresi = context.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            return string.IsNullOrEmpty(ipAdresi) ? "" : ipAdresi;
        }

        public void OturumEkle(string kullaniciKodu, Kullanici kullanici)
        {
            if (SrvSabitler.OturumListesi == null)
            {
                SrvSabitler.OturumListesi = new List<KeyValuePair<string, Kullanici>>();
            }
            KeyValuePair<string, Kullanici> tmpKvp = SrvSabitler.OturumListesi.Find(x => x.Key.Equals(kullaniciKodu));
            if (!string.IsNullOrEmpty(tmpKvp.Key))
            {
                SrvSabitler.OturumListesi.Remove(tmpKvp);
            }
            SrvSabitler.OturumListesi.Add(new KeyValuePair<string, Kullanici>(kullaniciKodu, kullanici));
        }

        public KeyValuePair<string, Kullanici> TokenDogrula(string token)
        {
            return SrvSabitler.OturumListesi.Find(x => x.Value.SifreToken.Equals(token));
        }

        private string tokenUret()
        {
            return Guid.NewGuid().ToString();
        }
    }
}