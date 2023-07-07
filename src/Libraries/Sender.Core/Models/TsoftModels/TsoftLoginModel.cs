using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Models.T_SoftModels
{
    public class TsoftLoginModel
    {
        public List<Data> data { get; set; }
        public class Data
        {
            public string userId { get; set; }
            public string username { get; set; }
            public string token { get; set; }
            public string secretKey { get; set; }
            public string expirationTime { get; set; }
            public string limited { get; set; }
            public string type { get; set; }
            public string tableId { get; set; }
        }
    }
}
