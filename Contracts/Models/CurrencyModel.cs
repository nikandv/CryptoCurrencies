using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public class CurrencyModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        private string _rank;
        public string Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
            }
        }
        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
            }
        }
        private string _difference;
        public string Difference
        {
            get { return _difference; }
            set
            {
                _difference = value;
            }
        }

        private string _valueInUSD;
        public string ValueInUSD
        {
            get { return _valueInUSD; }
            set
            {
                _valueInUSD = value;
            }
        }

        private string _valueInRUB;
        public string ValueInRUB
        {
            get { return _valueInRUB; }
            set
            {
                _valueInRUB = value;
            }
        }

        public override string ToString()
        {
            return $"{Name}: {ValueInRUB}";
        }
    }
}
