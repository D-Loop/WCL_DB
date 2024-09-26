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
    public class ProductStock
    {
        #region Constructor
        public ProductStock() 
        {
            IsNullError = true;
        }
        #endregion

        #region Properties
        public int ProductStockId { get; set; }
        public int ProductId { get; set; }
        public virtual Goods Goods { get; set; }
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public int StockQuantity { get; set; }
        #endregion

        #region Validation
        public bool IsNullError { get; set; }
        public string ValidateFields(string field)
        {
            string result = string.Empty;
            
            //switch (field)
            //{
            //    case nameof(RepeatedPassword) when password != RepeatedPassword: result = "Требуется ввести одинаковые";  break;
            //}

            IsNullError = string.IsNullOrEmpty(result);
            return result;
        }

        #endregion
    }
}
