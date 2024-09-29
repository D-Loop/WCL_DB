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
    public class SalesReport
    {
        [Key]  
        public int ReportID { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal TotalSales { get; set; }
        public int EmployeeID { get; set; }
    }



}
