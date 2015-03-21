using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSONValidator
{
    public class ParseJSONException : Exception
    {

        public ParseJSONException(){

        }

        public ParseJSONException(string message)
            : base(message)
        {
            
        }
    }
}
