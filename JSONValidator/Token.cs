
namespace JSONValidator
{
   
    /* Definiton of tokens */
    public class Token
    {
        public const int JSONFILE = 0;
        public const int END = 100;
        public const int OBJECTSTART = 1;
        public const int OBJECTEND = 2;
        public const int ARRAYSTART = 3;
        public const int ARRAYEND = 4;
        public const int QUOTE = 5;
        public const int COLON = 6;
        public const int COMMA = 7;
        public const int DOT = 8;
        public const int NUMBER = 10;
        public const int CHAR = 20;
        public const int TRUE = 30;
        public const int FALSE = 40;
        public const int NULL = 50;

        public int line;        //line 
        public int cnumber;     //char number from beggining
        public int token;       //token

        public Token(int token, int column , int line)
        {
            this.token = token;
            this.cnumber = column;
            this.line = line;
        }

        public int getToken(){
            return this.token;
        }

        public int getLine()
        {
            return this.line;
        }

        public int getCnumber()
        {
            return this.cnumber;
        }

        public override string ToString()
        {
            switch (this.token)
            {
                case 1:
                    return "{";
                case 2:
                    return "}";
                case 3:
                    return "[";
                case 4:
                    return "]";
                case 5:
                    return "\"";
                case 6:
                    return ":";
                case 7:
                    return ",";
                case 8:
                    return ".";
                case 10:
                    return "5";
                case 20:
                    return "b";
                case 30:
                    return "true";
                case 40:
                    return "false";
                case 50:
                    return "null";
                case 100:
                    return "EOF";
                default:
                    break;
            }

            return token.ToString();
        }
        
    }
}
