using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Calculadora_V2
{
    public class Token
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Line {get; set; }

        public Token(string name, string value, int line){
            Name = name;
            Value = value;
            Line = line;
        }

        public Token(){

        }

        public void CreateToken(string name, string value, int line,  List<Token> tokens){
            Token token = new Token(name, value, line);
            tokens.Add(token);
        }

        public List<string> ListTokens(List<Token> tokens){
            List<string> values = new List<string>();
            tokens.ForEach( token => {
               values.Add($"TOKEN: {token.Name} - VALUE: {token.Value} - LINE: {token.Line}");
            });
            return values;
        }

    } 
}

