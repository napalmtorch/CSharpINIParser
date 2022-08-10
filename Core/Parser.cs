using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NapalmINI
{
    public class INIParser
    {
        public List<string>     Input;
        public List<INISection> Sections;
        public List<INIComment> Comments;

        public INIParser(string filename)
        {
            Parse(filename);
        }
        
        public bool Parse(string filename)
        {
            if (!File.Exists(filename)) { throw new Exception("Unable to locate INI file at '" + filename + "'"); return false; }
            Input    = File.ReadAllLines(filename).ToList<string>();
            Sections = new List<INISection>();
            Comments = new List<INIComment>();
            FormatInput();

            for (int i = 0; i < Input.Count; i++)
            {
                if (Input[i].StartsWith("["))
                {
                    INISection section = ParseSection(i);
                    Sections.Add(section);
                }
                else if (char.IsLetterOrDigit(Input[i][0]))
                {
                    INIObject obj = ParseObject(i);
                    if (Sections.Count == 0) { Sections.Add(new INISection("__DEFAULT")); }
                    Sections[Sections.Count - 1].Objects.Add(obj);
                }
            }

            return true;
        }

        private INISection ParseSection(int line)
        {
            INISection section = new INISection("");

            int pos = 1, closed = 0;
            while (pos < Input[line].Length)
            {
                if (Input[line][pos] == ']') { closed = 1; break; }
                if (!char.IsLetterOrDigit(Input[line][pos]) && Input[line][pos] != '_') { throw new Exception("Invalid character for section ID at line " + (line + 1)); }
                section.ID += Input[line][pos++];
            }
            if (closed == 0) { throw new Exception("Expected ']' for INI section at line " + line + 1); }
            return section;
        }

        private INIObject ParseObject(int line)
        {
            string id = string.Empty;
            int eql = -1, done = 0;
            for (int i = 0; i < Input[line].Length; i++)
            {
                if (Input[line][i] == '=') { eql = i + 2; break; }
                if (Input[line][i] == '\t') { continue; }
                if (Input[line][i] == ' ') { done = 1; }
                if (done == 0) { id += Input[line][i]; }
            }
            if (eql < 1) { throw new Exception("Expected value for INI object at line " + (line + 1)); }

            string value = string.Empty;
            try { value = Input[line].Substring(eql, Input[line].Length - eql); }
            catch(Exception ex) { throw ex; }

            INIObject obj = new INIObject(id, value);
            if (!obj.ParseLiteral()) { throw new Exception("Failed to parse literal value for object at line " + (line + 1)); }
            return obj;
        }

        private void FormatInput()
        {
            for (int i = 0; i < Input.Count; i++)
            {
                if (Input[i].Length == 0) { Input.RemoveAt(i); i = 0; }
                if (IsWhitespace(Input[i])) { Input.RemoveAt(i); i = 0; }
                if (Input[i][0] == '#') { Comments.Add(new INIComment(i + 1, Input[i])); Input.RemoveAt(i); i = 0; }
            }
        }

        public bool Contains(string id)
        {
            for (int i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].ID == id) { return true; }
            }
            return false;
        }

        public INISection Fetch(string id)
        {
            for (int i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].ID == id) { return Sections[i]; }
            }
            throw new Exception("Unable to locate section with identifier '" + id + "'");
        }

        public bool TryFetch(string id, out INISection obj)
        {
            try
            {
                INISection o = Fetch(id);
                if (o != null) { obj = o; return true; }
                obj = null;
                return false;
            }
            catch (Exception ex) { obj = null; return false; }
        }

        public bool IsWhitespace(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]) || value[i] == '\t') { return false; }
            }
            return true;
        }

        public static bool IsDecimal(string value)
        {
            for (int i = 0; i < value.Length; i++) { if (!char.IsDigit(value[i])) { return false; } }
            return true;
        }

        public static bool IsFloat(string value)
        {
            int point = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsDigit(value[i]) && value[i] != '.') { return false; }
                if (value[i] == '.') { point++; }
            }
            if (point > 1) { return false; }
            return true;
        }
    }
}
