using MvcApi.Islemler;
using MvcApi.Nesneler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MvcApi.Controllers
{
    public class WebApiTokenKontrolAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            GirisIslemleri objGirisIslem = new GirisIslemleri();
            IEnumerable<string> token = actionContext.Request.Headers.FirstOrDefault(s => s.Key == SrvSabitler.API_TOKEN_ADI).Value;
            KeyValuePair<string, Kullanici> kvp = objGirisIslem.TokenDogrula(token.FirstOrDefault());
            if (token == null || kvp.Key == null)
            {
                var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Eksik ya da hatalı API anahtarı!..");
                actionContext.Response = response;
            }
            else
            {
                // Burada da gelen talepler loglanmali.
            }
        }
    }
}