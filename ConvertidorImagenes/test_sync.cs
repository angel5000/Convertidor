using System;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

class Program {
    static void Main() {
        WordDocument doc = new WordDocument();
        IWSection sec = doc.AddSection();
        IWParagraph p = sec.AddParagraph();
        p.AppendHTML("<b>Test</b>");
        Console.WriteLine("AppendHTML exists");
    }
}
