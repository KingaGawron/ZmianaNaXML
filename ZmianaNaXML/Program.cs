using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace ZmianaNaXML
{
    public class DniPlanu
    {
        public string Pracownik;

        public string Data;

        public string Definicja;

        public string OdGodziny;

        public string Czas;
        public DniPlanu(string pracownik, string data, string definicja, string odGodziny, string czas)
        {
            Pracownik = pracownik;
            Data = data;
            Definicja = definicja;
            OdGodziny = odGodziny;
            Czas = czas;
        }
        class Program
        {
            static void Main(string[] args)
            {

                string[] csvLines = File.ReadAllLines(@"NaXML.csv");
                List<DniPlanu> dzienplanu = new List<DniPlanu>();

                string[] dateData = csvLines[2].Split(";");
                var names = new List<string>();
                for (int i = 1; i < dateData.Length; i++)
                {
                    names.Add(dateData[i]);
                }
                for (int i = 3; i < csvLines.Length; i++)
                {
                    string[] rowData = csvLines[i].Split(";");

                    for (int j = 1; j < rowData.Length - 2; j++)
                    {

                        dzienplanu.Add(new DniPlanu(rowData[0], names[j - 1], "Praca", rowData[j], "8:00"));
                        XDocument xml = new XDocument(
                           new XDeclaration("1.0", "utf-8", "yes"),
                           new XElement("DniPlanu",
                               from DzienPlanu in dzienplanu
                               select new XElement("DzienPlanu",
                                   new XElement("Pracownik", DzienPlanu.Pracownik),
                                   new XElement("Data", DzienPlanu.Data),
                                   new XElement("Definicja", DzienPlanu.Definicja),
                                   new XElement("OdGodziny", DzienPlanu.OdGodziny),
                                   new XElement("Czas", DzienPlanu.Czas)

                                   )
                               )
                           );

                        var osoby = dzienplanu.Where(x => x.OdGodziny == "X");
                        foreach (DniPlanu emp in osoby)
                        {
                            emp.Definicja = "Wolne";
                        }
                        var osoby1 = dzienplanu.Where(x => x.OdGodziny == "1");
                        foreach (DniPlanu emp in osoby1)
                        {
                            emp.OdGodziny = "8:00";
                        }
                        var osoby2 = dzienplanu.Where(x => x.OdGodziny == "2");
                        foreach (DniPlanu emp in osoby2)
                        {
                            emp.OdGodziny = "14:00";
                        }
                        var osoby3 = dzienplanu.Where(x => x.OdGodziny == "3");
                        foreach (DniPlanu emp in osoby3)
                        {
                            emp.OdGodziny = "22:00";
                        }
                        var query = from a in xml.Root.Elements("DzienPlanu")
                                    where (a.Element("Definicja").Value == "Wolne")
                                    select a;

                        foreach (var item in query)
                        {
                            item.Element("OdGodziny").Remove();
                            item.Element("Czas").Remove();
                        }

                        xml.Save("plik.xml");
                        Console.WriteLine(xml);
                    }
                }

            }

        }

    }
}