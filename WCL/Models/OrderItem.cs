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
    public class OrderItem
    {
        #region Constructor
        public OrderItem() 
        {
            IsNullError = true;
        }
        #endregion

        #region Properties
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int GoodsId { get; set; }
        public Goods Goods { get; set; }
        public int Quantity { get; set; }
        #endregion

        #region Validation
        public bool IsNullError { get; set; }
        public string ValidateFields(string field)
        {
            string result = string.Empty;
            
            //switch (field)
            //{
            //    case nameof(password)    when string.IsNullOrEmpty(password):     result = "Требуется ввести пароль";  break;
            //}

            IsNullError = string.IsNullOrEmpty(result);
            return result;
        }

        #endregion
    }
}
