using System;
using System.Collections.Generic;
using System.IO;

namespace InfoRet
{
    static class docReader
    {
        static public HashSet<string> stopWords;
        public static double[] getArrayNumFromString(string[] strList)
        {
            List<double> tList = new List<double>();
            double num;
            foreach (string term in strList)
            {
                if (double.TryParse(term, out num))
                {
                    tList.Add(num);
                }
            }
            return tList.ToArray();
        }

        public static List<double[]> readWeights(string fileName, List<double[]> li)
        {

            string line;
            List<double> tList;
            try
            {
                using (TextReader input = File.OpenText(fileName))
                {
                    while ((line = input.ReadLine()) != null)
                    {

                        tList = new List<double>();
                        string[] weightsLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        double aNum;

                        foreach (string num in weightsLine)
                        {
                            if (double.TryParse(num, out aNum))
                                tList.Add(aNum);
                        }

                        li.Add(tList.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("File not read error: " + ex.ToString());
            }

            return li;
        }

        static docReader()
        {
            string path;
            string line;
            stopWords = new HashSet<string>();
            path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            path += "StopWordList1.txt";
            try
            {
                using (TextReader input = File.OpenText(path))
                {
                    while ((line = input.ReadLine()) != null)
                    {
                        string[] wordsLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        foreach (string word in wordsLine)
                        {
                            stopWords.Add(word);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("File not read error: " + ex.ToString());
            }
        }

        /// <summary>
        /// reads in the document and passes each word to the stemmer
        /// </summary>
        /// <param name="fileName"></param>
        public static void parseDoc(string fileName)
        {


            if (DocCollection.containsDoc(fileName))
            {
                return;
            }

            Doc currentDoc = new Doc(fileName);

            string line;
            try
            {
                using (TextReader input = File.OpenText(fileName))
                {
                    while ((line = input.ReadLine()) != null)
                    {
                        string[] wordsLine = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        foreach (string str in wordsLine)
                        {
                            if (!stopWords.Contains(str.ToLower()))
                            {
                                string temp = str.ToLower();
                                temp = Stemmer.stem(temp);
                                currentDoc.addTerms(temp);
                            }
                        }
                    }
                    ((MainWindow)System.Windows.Application.Current.MainWindow).ProgressLabel.Content = "Read " + currentDoc.getName();
                    DocCollection.addDoc(currentDoc);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("!!!File not read error: " + ex.ToString());
            }
        }

    }
}
