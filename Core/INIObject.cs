using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmINI
{
    public enum INIType : byte
    {
        String,
        Integer,
        Float,
        Character,
    }

    public class INIObject
    {
        public INIType Type;
        public string  ID;
        public string  LiteralValue;
        public object  Value;

        public INIObject(string id, string value)
        {
            this.ID           = id;
            this.LiteralValue = value;
            this.Value        = null;
        }

        public INIObject(INIType type, string id, object value)
        {
            this.Type         = type;
            this.ID           = id;
            this.Value        = value;
        }

        public override string ToString() { return "TYPE:" + Type.ToString() + " ID:" + ID + " VALUE:" + Value.ToString(); }

        public bool ParseLiteral() { return ParseLiteral(LiteralValue); }

        public bool ParseLiteral(string literal)
        {
            this.LiteralValue = literal;
            if (literal.StartsWith("\"")) { return ParseString(); }
            if (literal.ToUpper().StartsWith("0X")) { return ParseIntHex(); }
            if (INIParser.IsDecimal(literal)) { return ParseInt(); }
            if (INIParser.IsFloat(literal)) { return ParseFloat(); }
            if (literal.StartsWith("\'")) { return ParseChar(); }

            return false;
        }

        private bool ParseString()
        {            
            if (!LiteralValue.EndsWith("\"")) { return false; }
            Type = INIType.String;
            Value = LiteralValue.Substring(1, LiteralValue.Length - 2);
            return true;
        }

        private bool ParseInt()
        {
            int value = 0;
            if (!int.TryParse(LiteralValue, out value)) { return false; }
            Type = INIType.Integer;
            Value = value;
            return true;
        }

        private bool ParseIntHex()
        {
            uint value = 0;
            if (!uint.TryParse(LiteralValue.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out value)) { return false; }
            Type = INIType.Integer;
            Value = (int)value;
            return true;
        }

        private bool ParseFloat()
        {
            float value = 0.0f;
            if (!float.TryParse(LiteralValue, out value)) { return false; }
            Type = INIType.Float;
            Value = value;
            return true;
        }

        private bool ParseChar()
        {
            if (!LiteralValue.EndsWith("\'")) { return false; }
            char value = (char)0;
            string str = LiteralValue.Substring(1, LiteralValue.Length - 2);
            if (!char.TryParse(str, out value)) { return false; }
            Type = INIType.Character;
            Value = value;
            return true;
        }

        public int ReadInt()
        {
            if (Type != INIType.Integer) { throw new Exception("Type mismatch while reading integer from INI object '" + ID + "'"); }
            return (int)Value;
        }

        public string ReadString()
        {
            if (Type == INIType.String) { return (string)Value; }
            else { return LiteralValue; }
        }

        public float ReadFloat()
        {
            if (Type == INIType.Float) { return (float)Value; }
            throw new Exception("Type mismatch while reading float from INI object '" + ID + "'"); 
        }

        public char ReadChar()
        {
            if (Type == INIType.Character) { return (char)Value; }
            throw new Exception("Type mismatch while reading character from INI object '" + ID + "'");
        }
    }
}
