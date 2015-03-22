using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONValidator
{
    public class Parser
    {
        public List<Token> tokens;
        private int currenToken;        //numer obecnie przetwarzanego tokenu
        private int index;              //index obecnie przetwarzanego tokeny

        /*
         * Całość wg gramatyki w docx 
        */
        public Parser()
        {
            tokens = new List<Token>();
            currenToken = 0;
            index = 0;
        }
         
        public void parse(List<Token> TokensList)
        {
            tokens = TokensList;
            jsonfile();            
        }

        private void jsonfile()
        {            
            currenToken = tokens[++index].getToken();
            switch (currenToken)
            {
                case Token.OBJECTSTART:
                    objectFun();
                    break;
                case Token.ARRAYSTART:
                    array();
                    break;
                default:
                    throw new ParseJSONException("Bad char on start of JSON file! Line:1 Char:1");                
            }
        }

        private void array()
        {
            checkLex(Token.ARRAYSTART);
            elements();
            checkLex(Token.ARRAYEND);
        }


        private void objectFun()
        {
            checkLex(Token.OBJECTSTART);
            properties();
            checkLex(Token.OBJECTEND);
        }

        private void properties()
        {
            switch (currenToken)
            {
                case Token.QUOTE:
                    pair();
                    rest();
                    break;
                default:
                    break;
            }
        }

        private void rest()
        {
            switch (currenToken)
            {
                case Token.COMMA:
                    checkLex(Token.COMMA);
                    pair();
                    rest();
                    break;
                default:
                    break;
            }
        }

        private void pair()
        {
            sstring();
            checkLex(Token.COLON);
            value();
        }

        private void elements()
        {
            switch (currenToken)
            {
                case Token.QUOTE:
                case Token.TRUE:
                case Token.FALSE:
                case Token.NUMBER:
                case Token.NULL:
                case Token.ARRAYSTART:
                case Token.OBJECTSTART:                
                    value();
                    restvalue();
                    break;
                default:
                    break;
            }
        }

        private void value()
        {
            switch (currenToken)
            {
                case Token.QUOTE:
                    sstring();
                    break;
                case Token.TRUE:
                    logicVal();
                    break;
                case Token.FALSE:
                    logicVal();
                    break;
                case Token.NUMBER:
                    checkLex(Token.NUMBER);
                    break;
                case Token.NULL:
                    checkLex(Token.NULL);
                    break;
                case Token.ARRAYSTART:
                    array();
                    break;
                case Token.OBJECTSTART: 
                    objectFun();                    
                    break;
                default:
                    throw new ParseJSONException("Bad char on  Line:" + tokens[index].getLine() + " on char:"
                    + tokens[index].getCnumber() + " Got:'" + tokens[index].ToString()
                    + "' Expected any of: \",TRUE,FALSE,Number,Null,array,object ");
            }
        }

        private void sstring()
        {
            switch (currenToken)
            {
                case Token.QUOTE:
                    checkLex(Token.QUOTE);
                    stringrow();
                    checkLex(Token.QUOTE);
                    break;
                default:
                    throw new ParseJSONException("Bad char on  Line:" + tokens[index].getLine() + " on char:"
                    + tokens[index].getCnumber() + " Got:'" + tokens[index].ToString()
                    + "' Expected: QUOTE");
            }
        }

        private void stringrow()
        {
            switch (currenToken)
            {
                case Token.CHAR:
                    checkLex(Token.CHAR);
                    stringrow();
                    break;
                default:
                    break;
            }
        }

        private void logicVal()
        {
            switch (currenToken)
            {
                case Token.TRUE:
                    checkLex(Token.TRUE);
                    break;
                case Token.FALSE:
                    checkLex(Token.TRUE);
                    break;
                default:
                    throw new ParseJSONException("Bad char on  Line:" + tokens[index].getLine() + " on char:"
                    + tokens[index].getCnumber() + " Got:'" + tokens[index].ToString()
                    + "' Expected: true or false");
            }
        }

        private void restvalue()
        {
            switch (currenToken)
            {
                case Token.COMMA:
                    checkLex(Token.COMMA);
                    elements();
                    break;
                default:
                    break;
            }
        }

        //Sprawdzamy czy lex jest taki jak zakladany
        private void checkLex(int p)
        {
            if (currenToken == p)
            {
                currenToken = tokens[++index].getToken();
            }
            else
            {
                throw new ParseJSONException("Bad char on  Line:" + tokens[index].getLine() + " on char:" 
                    + tokens[index].getCnumber() + " Got:'" + tokens[index].ToString() 
                    + "' Expected: " + new Token(p,0,0).ToString() );
            }
        }
    }
}
