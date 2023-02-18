using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kurs7PM.API.Models
{
    internal class Provider
    {
        public int ID_provider { get; set; }

        public string Логин { get; set; }
        
        public string Пароль { get; set; }
        
        public string Фамилия { get; set; }
        
        public string Имя { get; set; }
        
        public string Отчество { get; set; }
        
        public string Склад { get; set; }
    }
}
