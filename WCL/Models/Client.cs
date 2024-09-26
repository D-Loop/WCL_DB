using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WCL.Models
{
    public class Client
    {
        [Key] // Уникальный идентификатор клиента
        public int ClientId { get; set; }

        [Required] // Имя клиента, обязательное поле
        [MaxLength(100)] // Ограничение на длину имени
        public string Name { get; set; }

        [MaxLength(150)] // Электронная почта клиента (необязательное поле)
        public string Email { get; set; }

        [MaxLength(15)] // Номер телефона клиента (необязательное поле)
        public string Phone { get; set; }

        [MaxLength(250)] // Адрес клиента (необязательное поле)
        public string Address { get; set; }

        // Связь с таблицей заказов (один клиент может иметь несколько заказов)
        public virtual ICollection<Order> Orders { get; set; }
    }
}
