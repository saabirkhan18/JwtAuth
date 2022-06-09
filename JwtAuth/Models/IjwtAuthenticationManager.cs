using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuth.Models
{
    public interface IjwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
