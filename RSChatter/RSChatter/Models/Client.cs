using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSChatter.Models
{
    public class Adviser
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }

        public string Login { get; set; }

        public AdvisorType AdvisorType { get; set; }

        public string AdvisorName { get; set; }

        public bool IsBusy { get; set; }
    }
}