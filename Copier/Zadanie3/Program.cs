using System;

namespace Zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            var print = new Printer();
            var scan = new Scanner();
            var fax = new Fax();
            var multi = new MultidimensionalDevice(print, scan, fax);
            var xerox = new Copier(print,scan);
            xerox.PowerOn();
            fax.PowerOn();
            multi.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            xerox.Print(in doc1);
            multi.Scan(out doc1);
            fax.Send(doc1);

            IDocument doc2;
            xerox.Scan(out doc2);


            xerox.ScanAndPrint();
            System.Console.WriteLine(xerox.Counter);
            System.Console.WriteLine(xerox.PrintCounter);
            System.Console.WriteLine(xerox.ScanCounter);
        }
    }
}
