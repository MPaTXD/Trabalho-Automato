using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Calculadora_V2
{
    class Program
    {
        enum TypeToken 
        {
           OpMulti, OpDiv, OpSum, OpSubtraction, Caracter, Fim, Enquanto,
           Entao, Se, SeNao, Imprimir, OpIgual, Var, Declaracao, 
           OpMenorIgual, NumInt, NumReal, PalavraReservada, Simbolo, Variavel, Invalido, Texto
        }

        static void Main(string[] args)
        {
            var text = @"function main(){
                var nome = `teste`;
                var numero = 2;
                switch(nome){
                    case `teste` :
                    nome = `passou`;
                    numero = 2 + 1;
                    printf(nome);
                    break;
                }
                if(numero = 2){
                  numero = numero * 2;
                }else{
                  numero = 3umero - 2;
                }
                printf(2umero);
            }";
            StartLexical(text, 1);
        }

        static void StartLexical(string text, int blocs){
            Token token = new Token();
            List<Token> tokens = new List<Token>();
            int range = text.Length;
            var content = "";
            var state = 0;
            var automaton = "";
            int line = 1;
            Regex rxEmpty = new Regex(@"^\s+$");
            Regex rxCaracter = new Regex(@"[A-Za-z]");
            Regex rxDigit = new Regex(@"\b(?<!\.)\d+(?!\.)\b");
            Regex rxSymbol = new Regex(@"[{]|[}]|[[]|[]]|[(]|[)]|[:]");
            Regex rxDeclaration = new Regex(@"[=]");
            Regex rxOperation = new Regex(@"[+]|[/]|[*]|[-]");
            Regex rxConditions = new Regex(@"[>]|[<]");
            Regex rxFinish = new Regex(@"[;]");
            Regex rxVariable = new Regex(@"^[a-zA-Z].*");
            Regex rxText = new Regex(@"[`]");
            for (int i = 0; i < range; i += blocs){
               if (i + blocs > range) blocs = range - i;
                   content = text.Substring(i, blocs);
                   
                   switch (state)
                   {
                       //ESTADO DE ESPAÇO
                       case 0:
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                          if(automaton.Contains("main")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("function")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("var")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("switch")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("break")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("case")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("printf")){
                            var definition = TypeToken.Imprimir.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("if")){
                            var definition = TypeToken.Se.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("while")){
                            var definition = TypeToken.Enquanto.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("if")){
                            var definition = TypeToken.Se.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("else")){
                            var definition = TypeToken.SeNao.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(!rxVariable.IsMatch(automaton.Trim()) && automaton.Trim().Length > 0){
                            var definition = TypeToken.Invalido.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("\n")){
                             line++;
                          }
                       }
                       else
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                       }
                       else
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }
                       else
                       if(rxSymbol.IsMatch(content)){
                          state = 3;
                          var definition = TypeToken.Simbolo.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }
                       else
                       if(rxDeclaration.IsMatch(content)){
                          state = 6;
                          var definition = TypeToken.Declaracao.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxText.IsMatch(content)){
                          state = 5;
                          automaton += content;
                       }else
                       if(rxOperation.IsMatch(content)){
                          state = 7;
                          if(content.Contains("+")){
                            var definition = TypeToken.OpSum.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("-")){
                            var definition = TypeToken.OpSubtraction.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("/")){
                            var definition = TypeToken.OpDiv.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("*")){
                            var definition = TypeToken.OpMulti.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }
                       }
                       break;



                       //ESTADO DE CARACTERER
                       case 1: 
                       
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                          if(automaton.Contains("main")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("function")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("var")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("switch")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("break")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("case")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("printf")){
                            var definition = TypeToken.Imprimir.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("if")){
                            var definition = TypeToken.Se.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("else")){
                            var definition = TypeToken.SeNao.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                          automaton += content;
                          if(automaton.Contains("main")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("function")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("var")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("switch")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("break")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("case")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("printf")){
                            var definition = TypeToken.Imprimir.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("if")){
                            var definition = TypeToken.Se.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("else")){
                            var definition = TypeToken.SeNao.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(rxVariable.IsMatch(automaton.Trim())){
                            var definition = TypeToken.Variavel.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(!rxVariable.IsMatch(automaton.Trim()) && automaton.Trim().Length > 0){
                            var definition = TypeToken.Invalido.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }
                       }else
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }else
                       if(rxSymbol.IsMatch(content)){
                          state = 3;
                          var definition = "";
                          if(rxCaracter.IsMatch(automaton)){
                            if(!rxVariable.IsMatch(automaton)){
                              definition = TypeToken.Invalido.ToString();
                              token.CreateToken(automaton.Trim(), definition, line, tokens);
                            }else{
                              definition = TypeToken.Variavel.ToString();
                              token.CreateToken(automaton.Trim(), definition, line, tokens);
                            }
                          }
                          definition = TypeToken.Simbolo.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxFinish.IsMatch(content)){
                          state = 4;
                          var definition = "";
                          definition = TypeToken.Fim.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxText.IsMatch(content)){
                         state = 5;
                         automaton += content;
                         var definition = TypeToken.Texto.ToString();
                         token.CreateToken(automaton.Trim(), definition, line, tokens);
                         automaton = "";
                       }
                       break;


                       // ESTADO DE DIGITO
                       case 2:
                      
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }else
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                          var definition = TypeToken.NumInt.ToString();
                          token.CreateToken(automaton.Trim(), definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxFinish.IsMatch(content)){
                          state = 4;
                          var definition = "";
                          if(rxCaracter.IsMatch(automaton)){
                            definition = TypeToken.Variavel.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                          }else
                          if(rxDigit.IsMatch(automaton)){
                            definition = TypeToken.NumInt.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                          }
                          definition = TypeToken.Fim.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxOperation.IsMatch(content)){
                          state = 7;
                          if(rxDigit.IsMatch(automaton.Trim())){
                            var definition = TypeToken.NumInt.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                          }
                          if(content.Contains("+")){
                            var definition = TypeToken.OpSum.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("-")){
                            var definition = TypeToken.OpSubtraction.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("/")){
                            var definition = TypeToken.OpDiv.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }else
                          if(content.Contains("*")){
                            var definition = TypeToken.OpMulti.ToString();
                            token.CreateToken(content, definition, line, tokens);
                            automaton = "";
                          }
                       }
                  
                       break;


                       // ESTADO DE SIMBOLO

                       case 3:
                       if(rxSymbol.IsMatch(content)){
                          state = 3;
                          var definition = TypeToken.Simbolo.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                       }else
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                       }else
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }else
                       if(rxFinish.IsMatch(content)){
                         state = 4;
                          var definition = "";
                          definition = TypeToken.Fim.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }
                       break;


                       // ESTADO DE FECHAMENTO

                       case 4:
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                          
                          if(automaton.Contains("main")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("function")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("var")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("switch")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("break")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("case")){
                            var definition = TypeToken.PalavraReservada.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("printf")){
                            var definition = TypeToken.Imprimir.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("if")){
                            var definition = TypeToken.Se.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }else
                          if(automaton.Contains("else")){
                            var definition = TypeToken.SeNao.ToString();
                            token.CreateToken(automaton.Trim(), definition, line, tokens);
                            automaton = "";
                          }
                       }
                       break;


                       // ESTADO DE TEXTO

                       case 5:
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }else
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                       }else
                       if(rxFinish.IsMatch(content)){
                          state = 4;
                          var definition = "";
                          definition = TypeToken.Fim.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }
                       break;


                       // ESTADO DE DECLARAÇÃO
                       case 6:
                       if(rxSymbol.IsMatch(content)){
                          state = 3;
                          var definition = TypeToken.Simbolo.ToString();
                          token.CreateToken(content, definition, line, tokens);
                          automaton = "";
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                       }else
                       if(rxCaracter.IsMatch(content)){
                          state = 1;
                          automaton += content;
                       }else
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }
                       break;


                       // ESTADO DE OPERADOR
                       case 7:
                       if(rxDigit.IsMatch(content)){
                          state = 2;
                          automaton += content;
                       }else
                       if(rxEmpty.IsMatch(content) || content.Contains("\n") || content.Contains("\r")){
                          state = 0;
                       }
                       break;
                   }   
            }

            var values = token.ListTokens(tokens);
            values.ForEach( v => {
                Console.WriteLine(v);
            });
        }
    }
}

