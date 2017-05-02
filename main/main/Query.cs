using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    public class Query
    {


        public List<Value> qTerms;
        public void setQuery(string qLine)
        {
            qTerms = new List<Value>();
            string[] temp;
            temp = qLine.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            temp = temp.Distinct().ToArray();
            foreach (string str in temp)
            {
                if (!docReader.stopWords.Contains(str.ToLower()))
                    qTerms.Add(new Value(Stemmer.stem(str), 10));
            }
        }

        public bool setQuery(string qLine, string qWLine)
        {
            qTerms = new List<Value>();
            string[] temp;
            temp = qLine.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            temp = temp.ToArray();

            string[] temp2;
            temp2 = qWLine.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            temp2 = temp2.ToArray();

            double num;

            if (temp.Count() != temp2.Count())
                return false;

            for (int i = 0; i < temp.Count(); i++)
            {
                if (double.TryParse(temp2[i], out num))
                    qTerms.Add(new Value(Stemmer.stem(temp[i]), double.Parse(temp2[i])));
                else
                {
                    qTerms.Clear();
                    return false;
                }
            }
            return true;
        }

    }
}
