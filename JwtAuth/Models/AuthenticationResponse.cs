using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Models
{
    public class AuthenticationResponse
    {
        public string jwt_token { set; get; }
        public string refresh_token { set; get; }
        public string message { set; get; }
    }
}
