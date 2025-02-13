using System;

namespace Zadanie2
{
    public interface IDevice
    {
        enum State {on, off};

        void PowerOn(); // uruchamia urządzenie, zmienia stan na `on`
        void PowerOff(); // wyłącza urządzenie, zmienia stan na `off
        State GetState(); // zwraca aktualny stan urządzenia

        int Counter {get;}  // zwraca liczbę charakteryzującą eksploatację urządzenia,
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

    public class MultiFunctionalDevice : IPrinter, IScanner, IFax
    {
        public int Counter { get; set; } = 0;
        public int PrintCounter { get; set; } = 0;
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
            if (state == IDevice.State.on)
            {
                IDocument document;
                Scan(out document, formatType: IDocument.FormatType.JPG);
                Print(document);
            }
        }
        public void Send(in IDocument document)
        {
            if (state == IDevice.State.on)
                Console.WriteLine($"{DateTime.Now} Send: {document.GetFileName()}");
        }
    }

    public class Copier : IPrinter, IScanner
    {
        public int Counter { get; set; } = 0;
        public int PrintCounter { get; set; } = 0;
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
            else
                return;
        }

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Counter++;
            }
            else
                return;
        }

        public void Print(in IDocument document)
        {
            if(state == IDevice.State.on)
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
            if (state == IDevice.State.on)
            {
                IDocument document;
                Scan(out document, formatType: IDocument.FormatType.JPG);
                Print(document);
            }
        }
    }

}
