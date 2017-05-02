using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    static class Generator
    {
        static public string path { get; set; }

        static Generator()
        {
            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            path += "Docs\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //Console.Write(path);
            
        }

        static public void generateRanDocs(int start, int n, int termR1, int termR2, string docsPath)
        {
            var random = new Random(DateTime.Now.Millisecond);
            Doc doc;
            string fileName;
            string term;

            int rnd;


            for (int i = start; i < start + n; i++)
            {
                fileName = "TestDoc" + i.ToString() + ".txt";
                doc = new Doc(docsPath + fileName);
                ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressLabel.Content = "Generating " + fileName;
                for (int j = termR1; j < termR2; j++)
                {
                    term = "term";
                    term += j.ToString();
                    rnd = random.Next(0, 10) > 3 ? random.Next(0, 10) : 0; // to reduce the chances of IDF of being zero if term appears in all documents
                    doc.setTerms(term, rnd);
                }
                docWriter.writeDoc(docsPath, doc);
            }
            ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressLabel.Content = "Done generating files.";
        }
    }
}
