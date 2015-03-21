using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSONValidator
{
    /* Analizator leksykalny */
    public class Lexer
    {
        private static Regex numberRegex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$"); // double, int i postaci wykladnicze
        private static char[] numberChars = { 'e', 'E', '-' , '.', '+' };//tablica z akceptowanymi znaczkami dla liczby nie liczac cyfr


        /*Analiza leksykalna*/
        public bool lexer(string json, ref List<Token> TokensList)
        {
            int line = 1;
            int i = 0;
            char lastChar = ' '; //przechowuje przedostatni znak
            char c; //zmienna do iteracji
            string tmp = ""; //tymczasowa pomocnicza zmienna
            bool isIn = false;// zmienna do sprawdzania czy jest w srodku czy poza napisem
            json = json.Trim(); // usuwamy biale znaki z przodu i tylu             
          
            // Jeśli nic nie dostalismy => koniec
            if (json.Length == 0) 
            {
                throw new ParseJSONException("JSON file is empty.\n");
            }

            TokensList.Add(new Token(Token.JSONFILE, i, line));

            for (; i < json.Length; i++)
            {
                c = json[i];

                if (c == '{')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.OBJECTSTART, i, line));
                }
                else if (c == '}')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.OBJECTEND, i, line));
                }
                else if (c == '[')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.ARRAYSTART, i, line));
                }
                else if (c == ']')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.ARRAYEND, i, line));
                }

                /*
                 * Sprawdzamy:
                 * 1. Czy jestesmy w napisie i czy NIE ma \ przed znakiem => QUOTE
                 * 2. Czy jestesmy w napisie i czy jest \ przed znakiem => CHAR
                 * 3. Jesli nie jestesmy w napisie => QUOTE                 
                 */

                else if (c == '"')
                {
                    if (isIn && lastChar != '\\')
                    {
                        isIn = false;
                        TokensList.Add(new Token(Token.QUOTE, i, line));
                    }
                    else if (isIn && lastChar == '\\')
                    {
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    }
                    else
                    {
                        TokensList.Add(new Token(Token.QUOTE, i, line));
                        isIn = true;
                    }
                }
                else if (c == ':')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.COLON, i, line));
                }
                else if (c == ',')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.COMMA, i, line));
                }
                else if (c == '.')
                {
                    if (isIn)
                        TokensList.Add(new Token(Token.CHAR, i, line));
                    else
                        TokensList.Add(new Token(Token.DOT, i, line));
                }
                else if (c == '\n')
                {
                    line++;
                }                
                else if (Char.IsWhiteSpace(c))
                {
                    continue;
                }
                /* Jesli nie jestesmy w napisie a pojawia sie litery lub cyfry
                * 1. Jesli liczba  => NUMBER
                * 2. Sprawdzamy czy moze jakas wartosc logiczna => TRUE v FALSE
                * 3. Sprawdzamy czy null => NULL
                * 
                * Jeśli żaden z powyższych zgłoś błąd 
                */
                else if (!isIn)
                {
                    try
                    {
                        //NUMBER
                        if (c == '-' || c == '+' || c == 'E' || c == 'e' || c == '.' || Char.IsDigit(c))
                        {
                            //iterujemy i laczymy cyferki/znaczki a potem patrzymy czy dobry regex
                            while (numberChars.Contains(json[i]) || Char.IsDigit(json[i]))
                            {
                                tmp += json[i].ToString();
                                i++;
                            }
                            i--;
                            if (numberRegex.IsMatch(tmp))
                                TokensList.Add(new Token(Token.NUMBER, i, line));
                            else
                                throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! Number was expected!");
                            tmp = "";
                        }
                        //TRUE
                        else if (c == 't' || c == 'T')
                        {
                            //Console.WriteLine(json[i].ToString()+ " " + json[i + 1].ToString() + " " + json[i + 2].ToString() +" "+ json[i + 3].ToString());
                            //tmp = json.Substring(i, i + 3).ToLower();   
                            //Powinno byc to co powyzej ale jest bug w bibliotece i nie zawsze dziala poprawnie funkcja substring
                            tmp = (json[i].ToString() + json[i + 1].ToString() + json[i + 2].ToString() + json[i + 3].ToString()).ToLower(); 
                            if (String.Equals(tmp,"true"))
                            {
                                TokensList.Add(new Token(Token.TRUE, i, line));
                                i += 3;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! String not in quotes!");
                            }
                        }
                        //FALSE
                        else if (c == 'f' || c == 'F')
                        {
                            //tmp = json.Substring(i, i + 5).ToLower();
                            tmp = (json[i].ToString() + json[i+1].ToString() + json[i+2].ToString() + json[i+3].ToString() + json[i+4]).ToLower();
                            if (String.Equals(tmp, "false"))
                            {
                                TokensList.Add(new Token(Token.FALSE, i, line));
                                i += 4;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! String not in quotes!");
                            }
                        }
                        //NULL
                        else if (c == 'n' || c == 'N')
                        {
                           // tmp = json.Substring(i , i + 4).ToLower();
                            tmp = (json[i].ToString() + json[i + 1].ToString() + json[i + 2].ToString() + json[i + 3].ToString()).ToLower();
                            if (String.Equals(tmp, "null"))
                            {
                                TokensList.Add(new Token(Token.NULL, i, line));
                                i += 3;
                                tmp = "";
                                c = json[i];
                            }
                            else
                            {
                                throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! String not in quotes!");
                            }
                        }
                            //else jest zle
                        else
                        {
                            throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! String not in quotes!");
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! Unexpected EOF!");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ParseJSONException("Failed in line:" + line + " char:" + i + "! Unexpected EOF!");
                    }
                }
                else if (isIn)
                {
                    TokensList.Add(new Token(Token.CHAR, i, line));
                }

                lastChar = c;
            }

            //Kończymy
            TokensList.Add(new Token(Token.END, i, line));
            return false;
        }
    }
}
