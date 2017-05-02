using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoRet
{
    public class Value : IComparable<Value>
    {
        public string term { get; set; }
        public double weight { get; set; }


        public Value(string str, double weight)
        {
            term = str;
            this.weight = weight;
        }

        int IComparable<Value>.CompareTo(Value other)
        {
            if (other == null)
                return 1;
            else
                return weight.CompareTo(other.weight);
        }

    }
}
