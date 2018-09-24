using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToInvoice
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.InitTextWriteListener();

            Converter csv2DbfConverter = new Converter();

            csv2DbfConverter.Run();

            Console.ReadKey();
        }
    }
}
