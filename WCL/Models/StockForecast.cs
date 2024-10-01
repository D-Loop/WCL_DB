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
    public class StockForecast
    {
        public int StockForecastID { get; set; }
        public int ProductID { get; set; }
        public DateTime ForecastDate { get; set; }
        public int PredictedQuantity { get; set; }

        public Product Product { get; set; }
    }


}
