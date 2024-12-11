using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cris_cros
{
    public class Word
    {
        char[] letters;
        public int x, y;
        public int size;
        public bool isVertical;

        public Word(int x, int y, string word, bool isVertical) { 
            this.x = x; 
            this.y = y;
            this.isVertical = isVertical;
            this.size = word.Length;
            letters = new char[size];
            for (int i = 0; i < size; i++) letters[i] = word[i];
        }

        public char this[int index]
        {
            get { return letters[index]; }
        }

        
    }
}
