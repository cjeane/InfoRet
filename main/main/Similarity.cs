using System;

namespace InfoRet
{
    static class Similarity
    {
        /// <summary>
        /// outdated function do not use
        /// </summary>
        /// <param name="query"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        static public double cosWeighted(double[] query, double[] doc)
        {
            double term1 = 0, docTerm = 0, queryTerm = 0;
            if (query.Length != doc.Length)
                return -1;
            else if (query.Length > 0)
            {
                for (int i = 0; i < query.Length; i++)
                {
                    term1 += query[i] * doc[i];
                    docTerm += doc[i] * doc[i];
                    queryTerm += query[i] * query[i];
                }
                return term1 / (Math.Sqrt(docTerm) + Math.Sqrt(queryTerm));
            }
            else
                return 0;
        }

        /// <summary>
        /// takes in a query and a document and returns it's cosine similiarity
        /// </summary>
        /// <param name="query"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        static public double cosineWeight(Query query, Doc doc)
        {
            double term1 = 0;
            double bTerm2 = 0;
            double bTerm3 = 0;
            if (query == null || query.qTerms == null || query.qTerms.Count == 0)
                return 0;

            foreach (var entry in doc.dict())
            {
                bTerm2 += Math.Pow((DocCollection.calcIDF(entry.Key) * entry.Value), 2);//document's weights squared
            }
            bTerm2 = Math.Sqrt(bTerm2);

            foreach (var queryTerm in query.qTerms)
            {
                if (doc.dict().ContainsKey(queryTerm.term))
                {
                    term1 += queryTerm.weight * //query's weights
                        ((double)doc.dict()[queryTerm.term] * DocCollection.calcIDF(queryTerm.term)); //TFIDF weight for term in collection & doc
                }
                bTerm3 += Math.Pow(queryTerm.weight, 2);//query's weights squared
            }
            bTerm3 = Math.Sqrt(bTerm3);
            return term1 / (bTerm2 * bTerm3);//cosine value 
        }
    }

}
