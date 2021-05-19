using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LinkedListExample
{
    class Program
    { 
        static void Main(string[] args)
        {
            string[] source = File.ReadAllLines(@"E:\test\CandidateTest_CSharp\CandidateTest_ManifestExample.csv");
            XElement xml = new XElement("Root",
                from str in source.Skip(1)
                let fields = str.Split(',')
                select new XElement("Order",
                 new XElement("OrderNo", fields[0]),
                 new XElement("ConsignmentNo", fields[1]),
                 new XElement("ParcelCode", fields[2]),
                 new XElement("ConsigneeName", fields[3]),
                 new XElement("FullAddress",
                     new XElement("Address1", fields[4]),
                     new XElement("Address2", fields[5]),
                     new XElement("City", fields[6]),
                     new XElement("State", fields[7]),
                     new XElement("Country", fields[8])
                    ),
              
                     new XElement("ItemQuantity", fields[9]),
                     new XElement("ItemValue", fields[10]),
                     new XElement("ItemWeight", fields[11]),
                     new XElement("ItemDesc", fields[12]),
                     new XElement("ItemCurrency", fields[13]==""? "GBP" : fields[13])
                    
                   )
                );
            XDocument doc = XDocument.Parse(xml.ToString());
            var groups = doc.Descendants("Order")
                  .GroupBy(x => (string)x.Element("OrderNo"))
                   .Select(gr => new
                   {
                       key = gr.Key,
                       tot = gr.Sum(q => (decimal?)q.Element("ItemValue")* (decimal?)q.Element("ItemQuantity"))
                   });



            XmlDocument result = new XmlDocument(); 
            foreach (var k in groups)
            {
                doc.Root.Descendants("Order").FirstOrDefault(x => (string?)x.Element("OrderNo") == k.key)
                                            .Add(new XElement("total", k.tot));
              
            }

            doc.Save(@"E:\test\CandidateTest_CSharp\out.xml");

        }
       
    }
 
}