using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Models.TsoftModels
{
    public class TsoftBaseModel2
    {
        public bool success { get; set; }
        public List<Message> message { get; set; }
        public object summary { get; set; }

        public class Message
        {
            public int? type { get; set; }
            public string? code { get; set; }
            public int? index { get; set; }
            public string? subid { get; set; }
            public List<string> text { get; set; }
            public object[] errorField { get; set; }
        }
    }
}