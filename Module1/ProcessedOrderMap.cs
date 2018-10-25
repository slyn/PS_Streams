using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace DataProcessor
{
    public class ProcessedOrderMap : ClassMap<ProcessedOrder>
    {
        public ProcessedOrderMap()
        {
            AutoMap(); // eşleştirlebilenler eşleştirilir

            Map(m => m.Customer).Name("CustomerNumber");
            Map(m => m.Amount).Name("Quantity");
            // Quantity Romen rakamı > int çevir
            //Map(m => m.Amount).Name("Quantity").TypeConverter<RomanTypeConverter>();


        }
    }
}
