using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSChatter.Models;

namespace RSChatter.Controllers
{
    public class FifoChatController : Controller
    {
         static List<Adviser> Advisers = new List<Adviser>()
        {
            new Adviser(){Login="jan.kowalski", AdvisorType = AdvisorType.BusinessAdvisor, Name = "Jan Kowalski", AdvisorName = "Doradca Biznesowy"},
            new Adviser(){Login="piotr.tomasz", AdvisorType = AdvisorType.BusinessAdvisor, Name = "Piotr Tomasz", AdvisorName = "Doradca Biznesowy"},

            new Adviser(){Login="kasia.tusk", AdvisorType = AdvisorType.IndividualAdvisor, Name = "Katarzyna Tusk",AdvisorName = "Doradca indywidualny"},
            new Adviser(){Login="katarzyna.kowalska", AdvisorType = AdvisorType.IndividualAdvisor, Name = "Katarzyna Kowalska", AdvisorName = "Doradca indywidualny"},
            new Adviser(){Login="leszek.cichosz", AdvisorType = AdvisorType.IndividualAdvisor, Name = "Leszek Cichosz", AdvisorName = "Doradca indywidualny"},
            new Adviser(){Login="wladek.misztela", AdvisorType = AdvisorType.IndividualAdvisor, Name = "Władek Misztela", AdvisorName = "Doradca indywidualny"},
            new Adviser(){Login="danuta.nowak", AdvisorType = AdvisorType.IndividualAdvisor, Name = "Danuta Nowak", AdvisorName = "Doradca indywidualny"},


            new Adviser(){Login="adam.nowak", AdvisorType = AdvisorType.CreditAdvisor, Name = "Adam Nowak", AdvisorName = "Doradca Biznesowy"},
            new Adviser(){Login="krystian.nowak", AdvisorType = AdvisorType.CreditAdvisor, Name = "Krystian Nowak", AdvisorName = "Doradca Biznesowy"},

            new Adviser(){Login="dariusz.korpas", AdvisorType = AdvisorType.MortgageAdvisor, Name = "Dariusz Korpas", AdvisorName = "Doradca Hipoteczny"},
            new Adviser(){Login="wiktor.korpas", AdvisorType = AdvisorType.MortgageAdvisor, Name = "Wiktor Korpas", AdvisorName = "Doradca Hipoteczny"},

            new Adviser(){Login="pawel.pyrski", AdvisorType = AdvisorType.TechnicalAdvisor, Name = "Paweł Pyrski", AdvisorName = "Doradca Techniczny"},
            new Adviser(){Login="wiktoria.cudak", AdvisorType = AdvisorType.TechnicalAdvisor, Name = "Wiktoria Cudak", AdvisorName = "Doradca Techniczny"},

        };

        private static Queue<SimpleChatUser> Queue = new Queue<SimpleChatUser>();

        public ActionResult Index()
        {
            return View();
        }


        // GET: SimpleChat
        public ActionResult Chat(SimpleChatUser user)
        {
            var model = new SimpleChatModel();
            model.User = user;

            var advisers = Advisers.Where(t => t.IsBusy == false).ToList();
            var index = new Random().Next(1, advisers.Count+1)-1;

            if (advisers.Count > 0)
            {
                if (Queue.Count>0)
                {
                    var queueUser = Queue.Peek();
                    if (queueUser.Type == model.Adviser.AdvisorType && queueUser.Name==user.Name)
                    {
                        model.Adviser = advisers[index];
                        model.Adviser.IsBusy = true;
                        Queue.Dequeue();
                    }
                }
                else
                {
                    model.Adviser = advisers[index];
                    model.Adviser.IsBusy = true;
                }


            }
            else
            {
                Queue.Enqueue(user);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EndChat(string login)
        {
            var adviser = Advisers.FirstOrDefault(t => t.Login == login);

            if (adviser != null)
            {
                adviser.IsBusy = false;
            }

            return Json(Url.Action("Index", "FifoChat"));
        }
    }
}