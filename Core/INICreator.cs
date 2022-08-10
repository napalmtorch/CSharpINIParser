using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace NapalmINI
{
    public class INICreator
    {
        public List<INISection> Sections;
        public int IDPadding;

        public INICreator(int padding = 16)
        {
            this.Sections  = new List<INISection>();
            this.IDPadding = 16;
        }

        public void Save(string filename)
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < Sections.Count; i++)
            {
                List<string> sections = GenerateSection(Sections[i]);
                lines.AddRange(sections);
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public List<string> GenerateSection(INISection section)
        {
            List<string> lines = new List<string>();
            lines.Add("[" + section.ID + "]");
            for (int i = 0; i < section.Objects.Count; i++) { lines.Add(GenerateObject(section.Objects[i])); }
            return lines;
        }

        public string GenerateObject(INIObject obj)
        {
            string line = string.Empty;
            line += obj.ID.PadRight(IDPadding, ' ');
            line += "= ";
            if (obj.Type == INIType.String)    { return line + '"' + obj.Value.ToString() + '"'; }
            if (obj.Type == INIType.Character) { return line + "'" + obj.Value.ToString() + "'"; }
            line += obj.Value.ToString();
            return line;
        }

        public void Clear() { Sections.Clear(); }

        public INISection AddSection(string id)
        {
            INISection section = new INISection(id);
            Sections.Add(section);
            return section;
        }

        public void RemoveSection(string id)
        {
            for (int i = 0; i < Sections.Count; i++) { if (Sections[i].ID == id) { Sections.RemoveAt(i); return; } }
            throw new Exception("Unable to locate INI section with id '" + id + "'");
        }

        public void RemoveSectionAt(int index)
        {
            if (index < 0 || index >= Sections.Count) { throw new Exception("Index out of bounds while attempting to remove INI section"); }
            Sections.RemoveAt(index);
        }
    }
}
