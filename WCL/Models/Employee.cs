using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WCL.Models
{
    public class Employee
    {
        [Key]  
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
    }



}
