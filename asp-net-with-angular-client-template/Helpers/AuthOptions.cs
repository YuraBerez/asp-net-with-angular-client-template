using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace asp_net_with_angular_client_template.Helpers
{
    public static class AuthOptions
    {

        public const string ISSUER = "AppServer";
        public const string AUDIENCE = "AppClient";
        public const string KEY = "<Y0ur aPp kEy!>";
        public const int LIFETIME = 60 * 12; // livetime access token

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

