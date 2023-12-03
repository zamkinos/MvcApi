using MvcApi.Islemler;
using System.Threading.Tasks;
using System.Web.Http;

namespace MvcApi.Controllers
{
    public class AdakKurbanController : ApiController
    {

        [WebApiTokenKontrol]
        public IHttpActionResult AdakKurbanCesitleriniGetir()
        {
            return Json(new AdakKurbanIslemleri().GetirAdakKurbanCesit());
        }
    }
}