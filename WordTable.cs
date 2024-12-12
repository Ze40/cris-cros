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

        public bool IsCanAdd(Word incWord, Word newWord, int x, int y) 
        {
            foreach (Word word in words)
            {
                if (incWord.Equals(word)) continue;
                if (word.isVertical == newWord.isVertical)
                {
                    if (word.isVertical)
                    {
                        if (x == word.x && (y >= word.y || y <= word.y + word.size)) return false;
                        if (((word.x - 1 == x) || (word.x + 1 == x)) || ((word.y - 1 == y + newWord.size) || (word.y + 1 == y))) return false;
                    }
                    else
                    {
                        if (y == word.y && (x >= word.x || x <= word.x + word.size)) return false;
                        if (((word.x - 1 == x+newWord.size) || (word.x + 1 == x)) || ((word.y - 1 == y) || (word.y + 1 == y))) return false;
                    }
                }
                
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
                                int newWordX = insWord.x + i;
                                int newWordY = insWord.y - j;

                                Word newWord = new Word(newWordX, newWordY, word, true);

                                if (IsCanAdd(insWord, newWord, newWordX, newWordY)) {
                                    words.Add(newWord);
                                    wordCount++;
                                    return true;
                                } 
                            }
                            else
                            {
                                //проверка возможности вставки
                                int newWordX = insWord.x + j;
                                int newWordY = insWord.y + i;

                                Word newWord = new Word(insWord.x + j, insWord.y + i, word, false);

                                if (IsCanAdd(insWord, newWord, newWordX, newWordY))
                                {
                                    words.Add(newWord);
                                    wordCount++;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
