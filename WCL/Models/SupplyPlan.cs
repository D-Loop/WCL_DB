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
    public class SupplyPlan
    {
        public int SupplyPlanID { get; set; }
        public int ProductID { get; set; }
        public DateTime SupplyDate { get; set; }
        public int PlannedQuantity { get; set; }
        public int SupplierID { get; set; }

        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
    }




}
