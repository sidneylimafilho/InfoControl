using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vivina.Erp.SystemFramework;
using InfoControl.Web.Services;
using System.Web.Mvc;
using InfoControl.Web.Mail;
using System.Collections;

namespace Vivina.Erp.WebUI.InfoControl.Host
{
    public class HostController : DataServiceBase
    {

        [JavaScriptSerializer]
        public ActionResult SendEmail(Hashtable Params, Hashtable FormData)
        {

            Postman.Send("mailer@vivina.com.br", Params["receiver"].ToString(), Params["subject"].ToString(), Params["message"].ToString(), null);

            //"Orçamento solicitado no site " + company.LegalEntityProfile.Website,
            //mailBody, new[] { addressFileToAttach });
            return View();
        }



    }
}