using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WCL.Models
{
    public class Order
    {
        #region Constructor
        public Order() 
        {
            IsNullError = true;
        }
        #endregion

        #region Properties
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        #endregion

        #region Validation
        public bool IsNullError { get; set; }
        public string ValidateFields(string field)
        {
            string result = string.Empty;
            
            //switch (field)
            //{
            //    case nameof(phone) when string.IsNullOrEmpty(phone): result = "Требуется ввести номер телефона"; break;
            //}

            IsNullError = string.IsNullOrEmpty(result);
            return result;
        }

        #endregion
    }
}
