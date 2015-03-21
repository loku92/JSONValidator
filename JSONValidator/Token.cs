
namespace JSONValidator
{
   
    public class Token
    {
        public static readonly int JSONFILE = 0;
        public static readonly int END = 100;
        public static readonly int OBJECTSTART = 1;
        public static readonly int OBJECTEND = 2;
        public static readonly int ARRAYSTART = 3;
        public static readonly int ARRAYEND = 4;
        public static readonly int QUOTE = 5;
        public static readonly int COLON = 6;
        public static readonly int COMMA = 7;
        public static readonly int DOT = 8;
        public static readonly int NUMBER = 10;
        public static readonly int CHAR = 20;
        public static readonly int TRUE = 30;
        public static readonly int FALSE = 40;
        public static readonly int NULL = 50;


        public int line;
        public int cnumber;
        public int token;

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
            return token.ToString();
        }
        
    }
}
