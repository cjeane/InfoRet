using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    public class Doc
    {
        Dictionary<string, int> termsTF;
        string name;

        public Doc(string str)
        {
            name = str;
            termsTF = new Dictionary<string, int>();
        }
        /// <summary>
        /// returns the full pathname of the document
        /// </summary>
        /// <returns></returns>
        public string getFullName()
        {
            string temp = name;
            return temp;
        }
        /// <summary>
        /// adds a term to the dictionary or increment it by one if it 
        /// exists
        /// </summary>
        /// <param name="term"></param>
        public void addTerms(string term)
        {
            if (termsTF.ContainsKey(term))
                termsTF[term] += 1;
            else
                termsTF.Add(term, 1);
        }
        /// <summary>
        /// helper function to random generation
        /// since random knows all the values as it generated
        /// so it doesn't need to reparse
        /// </summary>
        /// <param name="term"></param>
        /// <param name="i"></param>
        public void setTerms(string term, int i)
        {
            if (termsTF.ContainsKey(term))
                termsTF[term] = i;
            else
                termsTF.Add(term, i);
        }
        /// <summary>
        /// returns the standard file name without pathing
        /// </summary>
        /// <returns></returns>
        public string getName()
        {
            string temp = name;
            return Path.GetFileName(temp).Length > 0 && Path.GetFileName(temp).Length < name.Length ? Path.GetFileName(temp) : temp;
        }
        /// <summary>
        /// allows outside access to the dictionary of terms
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> dict()
        {
            return termsTF;
        }

    }
}
