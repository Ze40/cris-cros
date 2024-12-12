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
                }else
                {
                    if (!word.isVertical)
                    {
                        if ((word.x <= x && x <=  word.x + word.size) && (y <= word.y && word.y <= y + newWord.size))
                        {
                            int incX = x;
                            int incY = word.y;
                            if (word[incX] != newWord[incY]) return false;
                        }
                    }
                    else
                    {
                        if (x <= word.x && word.x <= x+newWord.size && word.y <= y && y <= word.y + word.size)
                        {
                            int incX = word.x;
                            int incY = y;
                            if (word[incY] != newWord[incX]) return false;
                        }
                    }
                }
                
            }
            return true;
        }

        public int CountOfIntersection(Word incWord, int x, int y)
        {
            int cnt = 0;
            foreach (Word word in words)
            {
                if (incWord.isVertical == word.isVertical) continue;
                if (word.isVertical)
                {
                    if ((word.x <= x + incWord.size && word.x >= x) && (y <= word.y + word.size && y >= word.y)) cnt++;
                    continue;
                }
                if ((word.y <= y + incWord.size && word.y >= y) && (x <= word.x + word.size && x >= word.x)) cnt ++;
            }
            return cnt;
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

            double currRatio = area / countOfCrossing;
            double minRatio = currRatio;

            int currWordX = 0;
            int currWordY = 0;

            Word wordToAdd = new Word(currWordX, currWordY, word, true);
            int newHeigth = height;
            int newWidth = width;
            int inter = 1;
            int maxInter = 1;

            bool isAdd = false;

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

                                Word curWord = new Word(newWordX, newWordY, word, true);

                                if (IsCanAdd(insWord, wordToAdd, newWordX, newWordY)) {

                                    if (!isAdd) wordToAdd = curWord;

                                    // найти количество перестановок
                                    isAdd = true;
                                    inter = CountOfIntersection(insWord, newWordX, newWordY);
                                    newHeigth = word.Length > height ? word.Length : height;
                                    minRatio = newHeigth * width / (countOfCrossing + inter);

                                    if (currRatio > minRatio)
                                    {
                                        currWordX = newWordX;
                                        currWordY = newWordY;
                                        currRatio = minRatio;
                                        maxInter = inter;
                                        wordToAdd = new Word(currWordX, currWordY, word, true);
                                    }
                                } 
                            }
                            else
                            {
                                //проверка возможности вставки
                                int newWordX = insWord.x - j;
                                int newWordY = insWord.y + i;

                                Word curWord = new Word(newWordX,  newWordY, word, false);

                                if (IsCanAdd(insWord, wordToAdd, newWordX, newWordY))
                                {
                                    if (!isAdd) wordToAdd = curWord;
                                    
                                    isAdd = true;
                                    inter = CountOfIntersection(insWord, newWordX, newWordY);
                                    newWidth = word.Length > width ? word.Length : width;
                                    minRatio = newWidth * height / (countOfCrossing + inter);

                                    if (currRatio > minRatio)
                                    {
                                        currWordX = newWordX;
                                        currWordY = newWordY;
                                        currRatio = minRatio;
                                        maxInter = inter;
                                        wordToAdd = new Word(currWordX, currWordY, word, false);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isAdd) {
                width = newWidth;
                height = newHeigth;
                countOfCrossing += maxInter;
                words.Add(wordToAdd);
                wordCount++;
                return true;
            }

            return false;
        }
    }
}
