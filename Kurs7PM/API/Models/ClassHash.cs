using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kurs7PM.API.Models
{
    public class ClassHash
    {
        public bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

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
                ErrorMessage = "Пароль должен содержать по крайней мере один символ нижнего регистра";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере один символ верхнего регистра";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Пароль должен быть не меньше 8 и не больше 15 символов";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Пароль должен содержать по крайней мере одну цифру";
                return false;
            }

            else
            {
                return true;
            }
        }
    }
}
