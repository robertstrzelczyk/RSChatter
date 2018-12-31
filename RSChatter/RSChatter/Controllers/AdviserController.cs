using RSChatter.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RSChatter.Controllers
{
    public class AdviserController : Controller
    {
        // GET: Adviser
        public ActionResult Index()
        {
            var clients = ChatHub.SignalRClients;
            return View(clients);
        }
    }
}