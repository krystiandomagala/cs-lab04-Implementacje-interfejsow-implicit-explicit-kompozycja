using System;

namespace Zadanie3
{
    public interface IDevice
    {
        enum State { on, off };

        void PowerOn(); // uruchamia urządzenie, zmienia stan na `on`
        void PowerOff(); // wyłącza urządzenie, zmienia stan na `off
        State GetState(); // zwraca aktualny stan urządzenia

        int Counter { get; }  // zwraca liczbę charakteryzującą eksploatację urządzenia,
                              // np. liczbę uruchomień, liczbę wydrukow, liczbę skanów, ...
    }

    public abstract class BaseDevice : IDevice
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        public void PowerOff()
        {
            state = IDevice.State.off;
            Console.WriteLine("... Device is off !");
        }

        public void PowerOn()
        {
            state = IDevice.State.on;
            Console.WriteLine("Device is on ...");
        }

        public int Counter { get; private set; } = 0;
    }

    public interface IPrinter : IDevice
    {
        /// <summary>
        /// Dokument jest drukowany, jeśli urządzenie włączone. W przeciwnym przypadku nic się nie wykonuje
        /// </summary>
        /// <param name="document">obiekt typu IDocument, różny od `null`</param>
        void Print(in IDocument document);
    }

    public interface IScanner : IDevice
    {
        // dokument jest skanowany, jeśli urządzenie włączone
        // w przeciwnym przypadku nic się dzieje
        void Scan(out IDocument document, IDocument.FormatType formatType);
    }
    public interface IFax : IDevice
    {
        void Send(in IDocument document);
    }
    public class Printer : IPrinter
    {
        public int Counter { get; set; } = 0;
        public int PrintCounter { get; set; } = 0;

        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState()
        {
            return state;
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Device is off !");
            }
            
        }

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Counter++;
                Console.WriteLine("Device is on ...");

            }
        }

        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{DateTime.Now} Print: {document.GetFileName()}");
                PrintCounter++;
            }
        }
    }
    public class Scanner : IScanner
    {
        public int Counter { get; set; } = 0;
        public int ScanCounter { get; set; } = 0;

        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState()
        {
            return state;
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Device is off !");
            }
        }

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Counter++;
                Console.WriteLine("Device is on ...");
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.PDF)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                if (formatType == IDocument.FormatType.PDF)
                    document = new PDFDocument("aaa.pdf");
                else if (formatType == IDocument.FormatType.JPG)
                    document = new ImageDocument("aaa.jpg");
                else
                    document = new TextDocument("aaa.txt");

                ScanCounter++;
                Console.WriteLine($"{DateTime.Now} Scan: {document.GetFileName()}");
            }
        }
    }
    public class Copier
    {
        Printer printer;
        Scanner scanner;
        
        public Copier(Printer _printer, Scanner _scanner)
        {
            printer = _printer;
            scanner = _scanner;
        }

        public int PrintCounter
        {
            get { return scanner.ScanCounter; }
            set { scanner.ScanCounter = value;  }
        }
        public int ScanCounter
        {
            get { return printer.PrintCounter; }
            set { printer.PrintCounter = value; }
        }

        public int Counter { get; set; } = 0;
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState()
        {
            return state;
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                printer.PowerOff();
                scanner.PowerOff();
                state = IDevice.State.off;
                Console.WriteLine("... Device is off !");
            }
        }
        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                printer.PowerOn();
                scanner.PowerOn();
                state = IDevice.State.on;
                Counter++;
                Console.WriteLine("Device is on ...");
            }
        }
        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{DateTime.Now} Print: {document.GetFileName()}");
                PrintCounter++;
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.PDF)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                if (formatType == IDocument.FormatType.PDF)
                    document = new PDFDocument("aaa.pdf");
                else if (formatType == IDocument.FormatType.JPG)
                    document = new ImageDocument("aaa.jpg");
                else
                    document = new TextDocument("aaa.txt");

                ScanCounter++;
                Console.WriteLine($"{DateTime.Now} Scan: {document.GetFileName()}");
            }
        }

        public void ScanAndPrint()
        {
            if (scanner.GetState() == IDevice.State.on && printer.GetState() == IDevice.State.on)
            {
                IDocument document;
                Scan(out document, formatType: IDocument.FormatType.JPG);
                Print(document);
            }
        }
    }
    public class Fax : IFax
    {
        public int Counter { get; set; } = 0;
        public int SendCounter { get; set; } = 0;

        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState()
        {
            return state;
        }
        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Device is off !");
            }
        }

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Counter++;
                Console.WriteLine("Device is on ...");
            }
        }

        public void Send(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{DateTime.Now} Send: {document.GetFileName()}");
                SendCounter++;
            }
        }

    

    }
    public class MultidimensionalDevice
    {
        Printer printer;
        Scanner scanner;
        Fax fax;

        public MultidimensionalDevice(Printer _printer, Scanner _scanner, Fax _fax)
        {
            printer = _printer;
            scanner = _scanner;
            fax = _fax;
        }
        public int ScanCounter
        {
            get { return scanner.ScanCounter; }
            set { scanner.ScanCounter = value; }
        }
        public int PrintCounter
        {
            get { return printer.PrintCounter; }
            set { printer.PrintCounter = value; }
        }

        public int SendCounter
        {
            get { return fax.SendCounter; }
            set { fax.SendCounter = value; }
        }

        public int Counter { get; set; } = 0;

        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState()
        {
            return state;
        }
        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                printer.PowerOff();
                scanner.PowerOff();
                state = IDevice.State.off;
                Console.WriteLine("... Device is off !");
            }
        }
        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                printer.PowerOn();
                scanner.PowerOn();
                state = IDevice.State.on;
                Counter++;
                Console.WriteLine("Device is on ...");
            }
        }
        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{DateTime.Now} Print: {document.GetFileName()}");
                PrintCounter++;
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.PDF)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                if (formatType == IDocument.FormatType.PDF)
                    document = new PDFDocument("aaa.pdf");
                else if (formatType == IDocument.FormatType.JPG)
                    document = new ImageDocument("aaa.jpg");
                else
                    document = new TextDocument("aaa.txt");

                ScanCounter++;
                Console.WriteLine($"{DateTime.Now} Scan: {document.GetFileName()}");
            }
        }

        public void ScanAndPrint()
        {
            if (scanner.GetState() == IDevice.State.on && printer.GetState() == IDevice.State.on)
            {
                IDocument document;
                Scan(out document, formatType: IDocument.FormatType.JPG);
                Print(document);
            }
        }

        public void Send(in IDocument document)
        {
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{DateTime.Now} Send: {document.GetFileName()}");
                SendCounter++;
            }
        }
    }
}