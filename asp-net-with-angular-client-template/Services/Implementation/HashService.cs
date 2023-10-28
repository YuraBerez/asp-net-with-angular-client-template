using System;
using System.Security.Cryptography;
using System.Text;
using asp_net_with_angular_client_template.Services.Interface;

namespace asp_net_with_angular_client_template.Services.Implementation
{
	public class HashService: IHashService
	{
        public string GetHash(string inputString)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }
    }
}

