using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSChatter.Models
{
    public class SimpleChatUser
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public AdvisorType Type { get; set; }
    }
}