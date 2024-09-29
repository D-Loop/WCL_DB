using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace WCL.Models
{
    public class WarehouseStock
    {
        [Key]  
        public int StockID { get; set; }
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
