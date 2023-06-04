using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order:EntityBase<Guid>
    {
        public int RequestedAmount { get; set; }  //bizden kazımamızı istenen ürün miktarı
        public int TotalFountAmount { get; set; }  //bizden 100 istendi ama toplamda 65 tane bulundu.
        public ProductCrawlType ProductCrawlType { get; set; }
        public ICollection<OrderEvent> OrderEvents { get; set; }  //Bot started, ...
        public ICollection<Product> Products { get; set; }
        
    }
}
