using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HerosCompanyApi.Models
{
    public class AuthenticationConfigurations
    {
        public string AccessTokenSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }
}
