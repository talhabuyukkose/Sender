using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Models.SenderModels
{
    public class SenderBaseModel
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public Summary summary { get; set; }

        public class Summary
        {
            public int? totalRecordCount { get; set; }
            public string? primaryKey { get; set; }
        }
    }
}
