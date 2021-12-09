using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class CreateUserDomainModel
    {
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }
        public string password 
        {
            get { return "Password123"; }
        }
        private string _connection = "Username-Password-Authentication";
        public string connection 
        {
            get { return _connection; }
        }
        //private string PasswordGenerator() 
        //{
        //    int length = 10;
        //    string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        //    Random random = new Random();
        //    // Select one random character at a time from the string  
        //    // and create an array of chars  
        //    char[] chars = new char[length];
        //    for (int i = 0; i < length; i++)
        //    {
        //        chars[i] = validChars[random.Next(0, validChars.Length)];
        //    }
        //    return new string(chars);
        //}
    }
}
