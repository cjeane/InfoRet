using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    class Results
    {
        public List<Value> results { get; set; }
        Value res;
        public Results(Query queryTerms)
        {
            results = new List<Value>();
            foreach (var doc in DocCollection.listOfDocs)
            {
                res = new Value(doc.getName(), Similarity.cosineWeight(queryTerms, doc));
                if (res.weight > 0)
                    results.Add(res);
            }
            results.Sort();
            results.Reverse();
            if (results.Count > 5)
                results.RemoveRange(5, results.Count - 5);
        }

        public Results(Query queryTerms, int lim)
        {
            results = new List<Value>();
            foreach (var doc in DocCollection.listOfDocs)
            {
                var res = new Value(doc.getName(), Similarity.cosineWeight(queryTerms, doc));
                if (res.weight > 0)
                    results.Add(res);
            }
            results.Sort();
            results.Reverse();
            if (results.Count > lim)
                results.RemoveRange(lim, results.Count - lim);
        }
    }
}
