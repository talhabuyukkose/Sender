using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Core.Models
{
    public class SiteUser
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<IFormFile> FromFiles { get; set; }

    }
}
