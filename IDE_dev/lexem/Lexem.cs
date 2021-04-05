using System;
using System.Text.RegularExpressions;

namespace Rider_Projects.lexem {
    public enum LexemType {
        Keyword,
        Type,
        Comment,
        Compound,
        Identifier,
        Number,
        String,
        SupportTypes,
        AssignOp,
        RelOp,
        AddOp,
        MulOp,
    }

    public class Lexem {
        public LexemType LexemType { get; }
        public string Value { get; }

        public Lexem(LexemType lexemType, string value) {
            LexemType = lexemType;
            Value = value;
        }
    }


    public class LexemDefinition {
        public Regex Regex { get; }
        public LexemType Type { get; }

        public LexemDefinition(String regexStr, LexemType type) {
            Regex = new Regex(regexStr);
            Type = type;
        }
    }
}