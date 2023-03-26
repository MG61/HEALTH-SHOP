using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kurs7PM.API.Models
{
    public class TestVaid
    {
        public bool ValidatePassword(string password)
        {
            var input = password;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Поле не должно быть пустым!");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=[{]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
