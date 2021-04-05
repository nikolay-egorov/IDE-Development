using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rider_Projects.lexem;

namespace Rider_Projects {
    public class LexerTests {
        static object PairCreator(Lexem i) => new {i.Value, TokenType = i.LexemType};

        [Test]
        public void CommentTest() {
            string expression = "// Comment,a text textaasdasdasd.";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }

        [Test]
        public void CommentTest2() {
            string expression = "// Text { asdasdas }";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }

        [Test]
        public void CommentTest3() {
            string expression = "{ asdasdas }";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }
        
        [Test]
        public void CommentTest4() {
            string expression = "(* asdasdas *)";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }
        
        [Test]
        public void CommentTest5() {
            string expression = "(* asdasd { adasdasd } *)";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }
        
        [Test]
        public void CommentTest6() {
            string expression = "// Another (* asdasd *)";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {new(LexemType.Comment, expression)};
            Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
        }
        
        [Test]
        public void MultiLineCommentTest() {
            string expression = "(* one \n" +
                                "two\n" +
                                "three *)";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.SupportTypes, expression), 
            };
            
            Assert.AreEqual(expected.First().Value.Trim(), actual.First().Value.Trim());
        }


        [Test]
        public void LineBasicTest() {
            string expression = "var x, y, z : integer;";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.Keyword, "var"),
                new (LexemType.Identifier, "x"),
                new (LexemType.SupportTypes, ","),
                new (LexemType.Identifier, "y"),
                new (LexemType.SupportTypes, ","),
                new (LexemType.Identifier, "z"),
                new (LexemType.SupportTypes, ":"), 
                new (LexemType.Type, "integer"),
                new (LexemType.SupportTypes, ";")
            };
            Assert.AreEqual(actual.Count, expected.Count);
            for (var i = 0; i < actual.Count; i++) {
                Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
            }
        }
        
        
        [Test]
        public void StringTest() {
            string expression = "'String'\n" +
                                "'a'\n" +
                                "'A tabulator character: '#34' is easy to embed'\n";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.String, "'String'\n"),
                new (LexemType.String, "'a'\n"),
                new (LexemType.String, "'A tabulator character: '#34' is easy to embed'\n")
            };
            Assert.AreEqual(expected.Count, actual.Count);
            for (var i = 0; i < actual.Count; i++) {
                Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
            }
        }

        [Test]
        public void NumberTest() {
            string expression = "1\n" +
                                "47\n" +
                                "3347\n" +
                                "1703";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.Number, "1"),
                new (LexemType.Number, "47"),
                new (LexemType.Number, "3347"),
                new (LexemType.Number, "1703"),
            };
            Assert.AreEqual(actual.Count, expected.Count);
            for (var i = 0; i < actual.Count; i++) {
                Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
            }
        }
        
        
        [Test]
        public void SignNumberTest() {
            string expression = "-1\n" +
                                "-22\n" +
                                "333.12\n" +
                                "1703";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.Number, "-1"),
                new (LexemType.Number, "-22"),
                new (LexemType.Number, "333.12"),
                new (LexemType.Number, "1703"),
            };
            Assert.AreEqual(actual.Count, expected.Count);
            for (var i = 0; i < actual.Count; i++) {
                Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
            }
        }

        [Test]
        public void ExpNumberTest() {
            string expression = "1E0\n" +
                                "1E345\n" +
                                "1.2e3\n" + 
                                "0.23e12";
            var lexer = new Lexer();
            var actual = lexer.lexInput(expression);
            var expected = new List<Lexem> {
                new(LexemType.Number, "1E0"),
                new (LexemType.Number, "1E345"),
                new (LexemType.Number, "1.2e3"),
                new (LexemType.Number, "0.23e12"),
            };
            Assert.AreEqual(actual.Count, expected.Count);
            for (var i = 0; i < actual.Count; i++) {
                Assert.That(actual.Select(PairCreator), Is.EquivalentTo(expected.Select(PairCreator)));
            }
        }

        
    }
}