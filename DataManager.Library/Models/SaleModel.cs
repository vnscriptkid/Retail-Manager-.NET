using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Library.Models
{
    public class SaleModel
    {
        public List<SaleLineModel> SaleDetails { get; set; } = new List<SaleLineModel>();
    }
}
