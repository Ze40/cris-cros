using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cris_cros
{
    public class WordTable
    {
        public int height;
        public int width;

        public List<Word> words = new List<Word>();

        int wordCount;
        int area;
        int countOfCrossing;

        public WordTable() {
            area = 0;
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
                        //Проверка в одной ли области y
                        if ((word.y <= y && y <= word.y + word.size) || (y <= word.y && word.y <= y + newWord.size))
                        {
                            //Проверка справа или слева
                            if (x + 1 == word.x || x - 1 == word.x) return false;
                        }
                        //Проверка на одной ли линии
                        if (x == word.x)
                        {
                            //Снизу от нового слова
                            if (y + newWord.size + 1 == word.y) return false;
                            //Сверху от нового слова
                            if (y - 1 == word.y + word.size) return false;
                            //Пересикаются ли
                            if ((y <= word.y && word.y <= y + newWord.size) || (word.y <= y && y <= word.y + word.size)) return false;
                        }
                    }
                    else
                    { 
                        //Проверка в одной ли области x
                        if ((word.x <= x && x <= word.x + word.size) || (x <= word.x && word.x <= x + newWord.size))
                        {
                            //Проверка в сверху или снизу
                            if (y+1 == word.y || y-1 == word.y) return false;
                        }
                        //Проверка на одной ли линии
                        if (y == word.y)
                        {
                            //Справа от нового слова
                            if (x+newWord.size + 1 == word.x) return false;
                            //Слева от нового слова
                            if (x - 1 == word.x + word.size) return false;
                            //Пересикаются ли
                            if ((x <= word.x && word.x <= x + newWord.size) || (word.x <= x && x <= word.x + word.size)) return false; 
                        }
                    }
                }
                //Если слова имеют разную ориентацию
                else
                {
                    //Новое слово горизонтально, а старое вертикально
                    if (word.isVertical)
                    {
                        //Находятся в спорикосновении

                        //Новое слово выше или ниже
                        if ((x <= word.x && word.x <= x + newWord.size - 1) && (y == word.y - 1 || y == word.y + word.size)) return false;
                        //Новое слово справа или слева
                        if ((word.y <= y && y <= word.y + word.size - 1) && (x == word.x + 1 || x + newWord.size == word.x)) return false;

                        //Пересекаются
                        if ((x <= word.x && word.x <= x + newWord.size-1) && (word.y <= y && y <= word.y + word.size-1))
                        {
                            int indX = word.x - x;
                            int indY = y - word.y;
                            //Равны ли символы на пересечении
                            if (newWord[indX] != word[indY]) return false;
                        }
                    }
                    else
                    {
                        //Находятся в соприкосновении

                        //Новое слово выше или ниже
                        if ((word.x <= x && x <= word.x + word.size - 1) && (word.y == y + newWord.size || y == word.y + 1)) return false;
                        //Новое слово справа или слева
                        if ((y <= word.y && word.y <= y + newWord.size - 1) && (x == word.x + word.size || x == word.x - 1)) return false;

                        //Пересекаются
                        if ((word.x <= x && x <= word.x + word.size-1) && (y <= word.y && word.y <= y+newWord.size-1))
                        {
                            int indX = x - word.x;
                            int indY = word.y - y;
                            //Равны ли символы на пересечении
                            if (newWord[indY] != word[indX]) return false;
                        }
                    }
                }
                
            }
            return true;
        }

        public int CountOfIntersection(Word newWord, int x, int y)
        {
            int cnt = 0;
            foreach (Word word in words)
            {
                if (newWord.isVertical == word.isVertical) continue;
                if (word.isVertical)
                {
                    if ((x <= word.x && word.x <= x + newWord.size - 1) && (word.y <= y && y <= word.y + word.size - 1)) cnt++;
                    continue;
                }
                if ((word.x <= x && x <= word.x + word.size - 1) && (y <= word.y && word.y <= y + newWord.size - 1)) cnt ++;
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

                                if (IsCanAdd(insWord, curWord, newWordX, newWordY)) {

                                    if (!isAdd) wordToAdd = curWord;

                                    // найти количество перестановок
                                    isAdd = true;
                                    inter = CountOfIntersection(curWord, newWordX, newWordY);
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

                                if (IsCanAdd(insWord, curWord, newWordX, newWordY))
                                {
                                    if (!isAdd) wordToAdd = curWord;
                                    
                                    isAdd = true;
                                    inter = CountOfIntersection(curWord, newWordX, newWordY);
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
