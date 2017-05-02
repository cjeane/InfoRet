using System;
using System.Collections.Generic;
using System.IO;

namespace InfoRet
{
    static public class DocCollection
    {
        static public int numOfDocs = 0;
        static public List<Doc> listOfDocs;
        static public Dictionary<string, int> termsDocCount;
        static public List<List<string>> weightMatrix { get; set; }
        static public List<string> termList { get; set; }
        static DocCollection()
        {
            listOfDocs = new List<Doc>();
            termsDocCount = new Dictionary<string, int>();
            termList = new List<string>();
        }
        /// <summary>
        /// cleans collections wiping everything stored
        /// </summary>
        static public void cleanCollection()
        {
            numOfDocs = 0;
            listOfDocs.Clear();
            termsDocCount.Clear();
            termList.Clear();
        }
        /// <summary>
        /// the general test set of docs is loaded
        /// </summary>
        static public void setupDocCollection()
        {
            cleanCollection();
            string[] files;
            string docsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
            docsPath += "DefaultDoc\\";
            if (Directory.Exists(docsPath))
            {
                files = Directory.GetFiles(docsPath);
                foreach (string fileName in files)
                {
                    docReader.parseDoc(fileName);
                }
            }
            else
            {
                //go ask user for filePath
            }
        }
        /// <summary>
        /// calculates the 2d matrix so it can be used for display
        /// </summary>
        static public void calcMatrixForDisplay()
        {

            List<string> listTerms = termList;
            List<string> li;
            if (weightMatrix == null)
            {
                weightMatrix = new List<List<string>>();
            }

            foreach (var entry in termsDocCount)
            {
                if (!listTerms.Contains(entry.Key))
                    listTerms.Add(entry.Key);
            }
            weightMatrix.Clear();
            foreach (var doc in listOfDocs)
            {
                li = new List<string>();

                li.Add(doc.getName());
                foreach (var term in listTerms)
                {
                    if (doc.dict().ContainsKey(term))
                        li.Add(Math.Round(calcIDF(term) * doc.dict()[term], 6).ToString());//IDF * TF
                    else
                        li.Add("0");
                }
                weightMatrix.Add(li);
            }
        }
        /// <summary>
        /// calculates the idf value of the term supplied
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        static public double calcIDF(string term)
        {
            return Math.Log((numOfDocs / (double)termsDocCount[term]));
        }

        /// <summary>
        /// keeps take of all the unique new terms being added
        /// </summary>
        /// <param name="doc"></param>
        static private void manageTermDocCount(Doc doc)
        {
            foreach (KeyValuePair<string, int> entry in doc.dict())
            {
                if (termsDocCount.ContainsKey(entry.Key))
                    termsDocCount[entry.Key] += 1;
                else
                    termsDocCount.Add(entry.Key, 1);
            }
        }
        /// <summary>
        /// the function which addes and call the other function responible for adding 
        /// documents
        /// </summary>
        /// <param name="doc"></param>
        static public void addDoc(Doc doc)
        {
            numOfDocs++;
            listOfDocs.Add(doc);
            manageTermDocCount(doc);
        }
        /// <summary>
        /// removes an document and and everything that relates to it.
        /// </summary>
        /// <param name="name"></param>
        static public void removeDoc(string name)
        {
            Doc temp = null; ;
            foreach (var doc in listOfDocs)
            {
                if (name.Equals(doc.getName(), StringComparison.OrdinalIgnoreCase))
                {
                    temp = doc;
                    numOfDocs--;
                    foreach (var term in doc.dict())
                    {
                        if (termsDocCount.ContainsKey(term.Key))
                        {
                            if (termsDocCount[term.Key] > 1)
                                termsDocCount[term.Key]--;
                            else
                            {
                                termsDocCount.Remove(term.Key);
                                if (termList != null && termList.Contains(term.Key))
                                    termList.Remove(term.Key);
                            }
                        }
                    }
                }
            }
            if (temp != null)
                listOfDocs.Remove(temp);
        }
        /// <summary>
        /// returns the full pathname of the file to caller of 
        /// from a file name provided
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static private string findPathName(string name)
        {
            string path = "";

            foreach (var doc in listOfDocs)
            {
                if (doc.getName().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return doc.getFullName();
                }

            }
            return path;
        }
        /// <summary>
        /// allows the program to ask the OS to attempt to over the file requested
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public bool openDoc(string name)
        {
            var temp = findPathName(name);

            if (temp.Length > 0)
            {
                System.Diagnostics.Process.Start(temp);
                return true;
            }

            return false;
        }
        /// <summary>
        /// return the document object with the name provided
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public Doc findDoc(string name)
        {
            foreach (var doc in listOfDocs)
            {
                if (doc.getName().Equals(name, StringComparison.OrdinalIgnoreCase))
                    return doc;
            }
            return null;
        }
        /// <summary>
        /// checks if the collection already has the file to avoid duplication
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        static public bool containsDoc(string fullName)
        {
            foreach (var doc in listOfDocs)
            {
                if (doc.getFullName().Equals(fullName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
