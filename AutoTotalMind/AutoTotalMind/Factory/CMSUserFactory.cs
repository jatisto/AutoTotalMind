using System;
using System.Security.Cryptography;
using System.Text;
using AutoTotalMind.Models;

namespace AutoTotalMind.Factory
{
    public class CMSUserFactory : AutoFactory<CMSUser>
    {
        public CMSUser Login(string email, string password)
        {
            SHA512 pw = new SHA512Managed();
            pw.ComputeHash(Encoding.ASCII.GetBytes(password));

            string hashedPassword = BitConverter.ToString(pw.Hash).Replace("-", "").ToLower();

            CMSUser cMSUser = SqlQuery("SELECT * FROM CMSUser WHERE Email='" + email + "' AND Password='" + hashedPassword + "'");

            if (cMSUser.ID > 0)
            {
                return cMSUser;
            }
            else
            {
                return null;
            }
        }
    }
}