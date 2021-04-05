using System;
using System.Collections.Generic;
using Rider_Projects.lexem;

namespace Rider_Projects {
    public class Lexer {
        public List<Lexem> lexems = new List<Lexem>();

        private static List<LexemDefinition> definitions = new List<LexemDefinition>() {
            new LexemDefinition(@"^\s*(program|var|array|of|function|procedure|if|then|else)\s*", LexemType.Keyword),
            new LexemDefinition(@"^\s*(integer|real)\s*", LexemType.Type),
            new LexemDefinition(@"^\s*(begin|end)\s*", LexemType.Compound),
            new LexemDefinition(@"^//(.*?)(\n|$)", LexemType.Comment),
            new LexemDefinition(@"^\s*\(\*(.*?)\*\)\s*", LexemType.Comment),
            new LexemDefinition(@"^\s*{(.*?)}\s*", LexemType.Comment),
            new LexemDefinition(@"^\s*'(\s*([^']|'#\d+')*)'\s*", LexemType.String),
            new LexemDefinition(@"^\s*[_a-zA-Z][_a-zA-Z0-9]*\s*", LexemType.Identifier),
            new LexemDefinition(@"^\s*[-+]?(\d?[.]\d+(e\d?)?|\d+E\d?|\d)+\s*", LexemType.Number),
            new LexemDefinition(@"^\s*(\(\*|\*\)|\(|\)|\[|\]|;|:|.|,|..|\{|\})\s*", LexemType.SupportTypes), 
            // redundant  
            new LexemDefinition(@"^\s*:=\s*", LexemType.AssignOp), 
            new LexemDefinition(@"^\s*[+\-]{1}\s*", LexemType.AddOp),
            new LexemDefinition(@"^\s*[<>]{1}\s*", LexemType.RelOp),
            new LexemDefinition(@"^\s*[*\\]{1}\s*", LexemType.MulOp),
        };

        public List<Lexem> lexInput(string input) {
            makeMatchLine(input);
            return lexems;
        }


        private void makeMatchLine(string line) {
            int pos = 0;
            int posMulti = -1;
            string mlComment = "";
            int unMatch = 0;
            while (line.Length != 0) {
                bool isMatched = false;

                foreach (var definition in definitions) {
                    var match = definition.Regex.Match(line, 0);
                    if (!match.Success) {
                        continue;
                    }
                    
                    var value = match.Value;
                    if (definition.Type == LexemType.SupportTypes && value == "(* " || value == "(*") {
                        posMulti = pos;
                        pos = 0;
                        unMatch++;
                    }
                    
                    // lexems.Add(new Lexem(definition.Type, value.Trim()));
                    if (definition.Type == LexemType.String) {
                        lexems.Add(new (definition.Type, value));
                    }
                    else {
                        lexems.Add(new Lexem(definition.Type, value.Trim()));
                    }
                    if (posMulti != -1) {
                        mlComment += value;
                    }

                    if (definition.Type == LexemType.SupportTypes && value == " *)" || value == "*)") {
                        if (unMatch > 0) {
                            unMatch--;
                        }
                        else {
                            throw new ArgumentException("Given string can not be parsed");
                        }
                    }
                    
                    line = line.Substring(value.Length);
                    isMatched = true;
                    pos += value.Length;
                    break;
                }
                

                if (!isMatched) throw new ArgumentException("Given string can not be parsed");
            }

            if (posMulti != -1) {
                if (unMatch > 0 || lexems[lexems.Count - 1].Value != "*)") {
                    throw new ArgumentException("Given string can not be parsed");
                }
                else {
                    lexems.Clear();
                    lexems.Add(new Lexem(LexemType.Comment, mlComment));
                }
            }
        }
    }
}