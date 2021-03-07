using System;
using System.Collections.Generic;
using System.Linq;
using Rider_Projects.ast;
using Rider_Projects.ast.expression;

namespace Parser_task {
    public class SimpleParser {
        public static IExpression Parse(string text) {
            var supportedOp = new Dictionary<Char, int>()
            {
                {'(', -1},
                {')', -1},
                {'+', 0},
                {'-', 0},
                {'*', 1},
                {'/', 1}
            };

            var expressionsStack = new Stack<IExpression>();
            var operations = new Stack<Char>();
            var isChanged = false;

            for (var i = 0; i < text.Length; i++) {
                var ch = text[i];
                if (supportedOp.ContainsKey(ch)) {
                    ParseUntil(ch, supportedOp, expressionsStack, operations);
                    isChanged = true;
                }
                else if (char.IsDigit(ch)) {
                    expressionsStack.Push(new Literal(ch.ToString()));
                    isChanged = true;
                }
                else if (char.IsLetter(ch)) {
                    expressionsStack.Push(new Variable(ch.ToString()));
                    isChanged = true;
                }

                if (i == text.Length - 1 && isChanged && operations.Count > 0) {
                    WindToLastOperation(expressionsStack, operations);
                }
            }

            
            return expressionsStack.Peek();
        }


        static void ParseUntil(char charAt, Dictionary<Char, int> supportedOp, 
                                        Stack<IExpression> expressions, Stack<Char> operations) {
            var type = supportedOp[charAt];

            var currOperation = Char.MinValue;
            
            if (type == -1) {  // evaluate 
                char nextOperation;
                
                if (charAt.Equals('(')) {
                    operations.Push(charAt);
                    return;
                }
                
                while (!currOperation.Equals('(')) {
                    nextOperation = operations.Pop();
                    if (expressions.Count > 1) {
                        var secondOperation = expressions.Pop();
                        expressions.Push(
                            new BinaryExpression(expressions.Pop(), secondOperation,
                                nextOperation.ToString()));
                    }

                    currOperation = nextOperation;
                }

                var parExpr = new ParenExpression(expressions.Pop());
                expressions.Push(parExpr);
            } 
            else {
                while (operations.Count > 0) {
                    currOperation = operations.Pop();
                    var isNotOp = !supportedOp.ContainsKey(currOperation);
                    if (isNotOp || supportedOp[charAt] > supportedOp[currOperation]) {
                        operations.Push(currOperation);
                        operations.Push(charAt);
                        return;
                    }

                    var secondOperation = expressions.Pop();
                    expressions.Push(
                        new BinaryExpression(expressions.Pop(), secondOperation,
                            currOperation.ToString()));
                }
                
                operations.Push(charAt);
            }
        }


        static void WindToLastOperation(Stack<IExpression> expressions, Stack<Char> operations) {
            if (operations.Count > 0) {
                while (operations.Count > 0) {
                    var operationAt = operations.Pop();

                    if (expressions.Count > 1) {
                        var secondOperation = expressions.Pop();
                        expressions.Push(
                            new BinaryExpression(expressions.Pop(), secondOperation,
                                operationAt.ToString()));
                    }
                    
                }
            }
        }
    }
}