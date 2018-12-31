using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSChatter.Models.QnAMaker;

namespace RSChatter.Models.QnAMaker
{
    public class Answers
    {
        public List<string> Questions { get; set; }

        public string Answer { get; set; }

        public double Score { get; set; }

        public int Id { get; set; }

        public string Source { get; set; }

        public List<MetaData> Metadata { get; set; }
    }
}