using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AkaProje
{
    public class Encryption
    {
        public static string GetHasedPassword(string sifre)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(sifre);
            byte[] inArray = HashAlgorithm.Create("SHA256").ComputeHash(bytes);
            string hashedSifre = Convert.ToBase64String(inArray);
            return hashedSifre;
        }
    }
}