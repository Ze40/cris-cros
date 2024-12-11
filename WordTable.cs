using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cris_cros
{
    public class WordTable
    {
        int X;
        int Y;
        public int height;
        public int width;

        public List<Word> words = new List<Word>();

        int wordCount;
        int area;
        int countOfCrossing;

        public WordTable() {
            area = 0;
            X = 0;
            Y = 0;
            height = 0;
            width = 0;
            countOfCrossing = 0;
            area = 0;
            wordCount = 0;
            words = new List<Word>();
        }

        public bool FillWordTable(HashSet<string> words)
        {
            foreach (string word in words)
            {
                if (!AddWord(word)) return false;
            }
            return true;
        }

        public bool AddWord(string word)
        {
            if (wordCount == 0) { 
                Word newWord = new Word(0, 0, word, false);
                words.Add(newWord);
                width = newWord.size;
                height = 1;
                area = width * height;
                countOfCrossing++;
                wordCount++;
                return true;
            }

            foreach (Word insWord in words) {
                for (int i = 0; i < insWord.size; i++) {
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (insWord[i] == word[j])
                        {
                            if (!insWord.isVertical)
                            {
                                //проверка возможности вставки
                                Word newWord = new Word(insWord.x + i, insWord.y - j, word, true);
                                words.Add(newWord);
                                wordCount++;
                                return true;
                            }
                            else
                            {
                                //проверка возможности вставки
                                Word newWord = new Word(insWord.x + j, insWord.y + i, word, false);
                                words.Add(newWord);
                                wordCount++;
                                return true;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
