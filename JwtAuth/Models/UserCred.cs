﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Models
{
    public class UserCred
    {
        public string username { set; get; }
        public string password { set; get; }
    }
    public class RefreshTokenCred
    {
        public string jwt_token { set; get; }
        public string refresh_token { set; get; }
    }
}
