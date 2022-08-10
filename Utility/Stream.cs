using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NapalmINI
{
    public class Stream<T>
    {
        public T[] Data;
        public int Size;
        public int Position;

        public Stream() { this.Data = null; this.Size = 0; this.Position = 0; }

        public Stream(T[] data, int size) { this.Data = data; this.Size = size; this.Position = 0; }

        public Stream(List<T> data) { this.Data = new T[data.Count]; this.Size = data.Count; this.Position = 0; data.CopyTo(this.Data); }

        public void Seek(int pos)
        {
            if (pos < 0 || pos >= Size) { throw new Exception("Tacter reader seek out of bounds - " + pos); }
            this.Position = pos;
        }

        public T Peek()
        {
            return this.Data[this.Position - 1];
        }

        public T Peek(int pos)
        {
            if (pos < 0 || pos >= Size) { throw new Exception("Tacter reader peek out of bounds - " + pos); }
            return this.Data[pos];
        }

        public T Next()
        {
            if (this.Position < 0 || this.Position >= Size) { return default; }
            T c = this.Data[this.Position++];
            return c;
        }

        public T Back()
        {
            if (this.Position - 1 < 0 || this.Position - 1 >= Size) { return default; }
            this.Position--;
            return this.Data[this.Position];
        }

        public bool IsDone()
        {
            if (this.Data == null) { return true; }
            if (this.Position < 0 || this.Position >= this.Size) { return true; }
            if (this.Data[this.Position] == null) { return true; }
            return false;
        }
    }
}
