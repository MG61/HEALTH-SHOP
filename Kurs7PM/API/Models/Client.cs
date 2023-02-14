using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs7PM.API.Models
{
    public class Client
    {
        public int ID_client { get; set; }

        public string Логин { get; set; }

        public string Пароль { get; set; }

        public string Фамилия { get; set; }

        public string Имя { get; set; }

        public string Отчество { get; set; }
    }
}
