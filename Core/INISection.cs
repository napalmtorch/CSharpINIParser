using System;
using System.Collections.Generic;
using System.Text;

namespace NapalmINI
{
    public class INISection
    {
        public string ID;
        public List<INIObject> Objects;

        public INISection(string id) { this.ID = id; this.Objects = new List<INIObject>(); }

        public override string ToString() { return "ID:" + ID + " OBJS:" + Objects.Count; }

        public bool Contains(string id)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].ID == id) { return true; }
            }
            return false;
        }

        public INIObject Fetch(string id)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].ID == id) { return Objects[i]; }
            }
            throw new Exception("Unable to locate object with identifier '" + id + "' in section '" + ID + "'");
        }

        public bool TryFetch(string id, out INIObject obj)
        {
            try
            {
                INIObject o = Fetch(id);
                if (o != null) { obj = o; return true; }
                obj = null;
                return false;
            }
            catch (Exception ex) { obj = null; return false; }
        }
    }
}
