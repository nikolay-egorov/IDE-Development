using System.Text;
using NUnit.Framework;
using Parser_task;
using Rider_Projects.ast;
using Rider_Projects.ast.expression;
using Rider_Projects.ast.visitor;

namespace MyTests
{

    public class DumpVisitor : IExpressionVisitor
    {
        private readonly StringBuilder myBuilder;

        public DumpVisitor()
        {
            myBuilder = new StringBuilder();
        }

        public void Visit(Literal expression)
        {
            myBuilder.Append("Literal(" + expression.Value + ")");
        }

        public void Visit(Variable expression)
        {
            myBuilder.Append("Variable(" + expression.Name + ")");
        }

        public void Visit(BinaryExpression expression)
        {
            myBuilder.Append("Binary(");
            expression.FirstOperand.Accept(this);
            myBuilder.Append(expression.Operator);
            expression.SecondOperand.Accept(this);
            myBuilder.Append(")");
        }

        public void Visit(ParenExpression expression)
        {
            myBuilder.Append("Paren(");
            expression.Operand.Accept(this);
            myBuilder.Append(")");
        }

        public override string ToString()
        {
            return myBuilder.ToString();
        }
    }
    
    
    
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var dumpVisitor = new DumpVisitor();
            new BinaryExpression(new Literal("1"), new Literal("2"), "+").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)+Literal(2))", dumpVisitor.ToString());
            
            Assert.Pass();
        }

        [Test]
        public void SimpleTest() {
            var toParse = "a+c*d-3";
            var ans = new BinaryExpression(
                new BinaryExpression(
                    new Variable("a"),
                    new BinaryExpression(
                        new Variable("c"),
                        new Variable("d"),
                        "*"),
                    "+"),
                    new Literal("3"), 
                "-");
        
            var dumpVisitor = new DumpVisitor();
            var ansDumpVisitor = new DumpVisitor();
            SimpleParser.Parse(toParse).Accept(dumpVisitor);
            ans.Accept(ansDumpVisitor);
            Assert.AreEqual(ansDumpVisitor.ToString(), dumpVisitor.ToString());
            // Console.WriteLine(ans.ToString());
            
        }

        [Test]
        public void ManyParTest() {
            var toParse = "((((a+c)*d)-3))";
            var ans = new ParenExpression(
                new ParenExpression(
                    new BinaryExpression(
                        new ParenExpression(
                            new BinaryExpression(
                                new ParenExpression(
                                    new BinaryExpression(
                                        new Variable("a"),
                                        new Variable("c"),
                                        "+")
                                ),
                                new Variable("d"),
                                "*")
                        ),
                        new Literal("3"),
                        "-")
                )
            );
        
            var dumpVisitor = new DumpVisitor();
            var ansDumpVisitor = new DumpVisitor();
            SimpleParser.Parse(toParse).Accept(dumpVisitor);
            ans.Accept(ansDumpVisitor);
            Assert.AreEqual(ansDumpVisitor.ToString(), dumpVisitor.ToString());
            
            
        }
        
        [Test]
        public void AllInTest() {
            var toParse = "((a+c)*d)/3 + f*7";
            var ans =
                new BinaryExpression(
                    new BinaryExpression(
                        new ParenExpression(
                            new BinaryExpression(
                                new ParenExpression(
                                    new BinaryExpression(
                                        new Variable("a"),
                                        new Variable("c"),
                                        "+")
                                ),
                                new Variable("d"),
                                "*")
                        ),
                        new Literal("3"),
                        "/"),
                    new BinaryExpression(
                        new Variable("f"),
                        new Literal("7"),
                        "*"
                    ),
                    "+");
            
            
            var dumpVisitor = new DumpVisitor();
            var ansDumpVisitor = new DumpVisitor();
            SimpleParser.Parse(toParse).Accept(dumpVisitor);
            ans.Accept(ansDumpVisitor);
            Assert.AreEqual(ansDumpVisitor.ToString(), dumpVisitor.ToString());
        }
        
        
        [Test]
        public void SimpleParTest() {
            var toParse = "(a+c*d)-3";
            var ans = new BinaryExpression(
                new ParenExpression(
                new BinaryExpression(
                    new Variable("a"),
                    new BinaryExpression(
                        new Variable("c"),
                        new Variable("d"),
                        "*"),
                    "+")
                ),
                new Literal("3"), 
                "-");
        
            var dumpVisitor = new DumpVisitor();
            var ansDumpVisitor = new DumpVisitor();
            SimpleParser.Parse(toParse).Accept(dumpVisitor);
            ans.Accept(ansDumpVisitor);
            Assert.AreEqual(ansDumpVisitor.ToString(), dumpVisitor.ToString());
        }

    }
}