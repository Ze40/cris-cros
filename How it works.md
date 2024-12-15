# Основной алгоритм
Задача состоит в том, чтобы создать алгоритм, которой будет генерировать таблицу пересекающихся по вертикали и горизонтали слов, с как можно большей связностью. За коэффициент связности будем принимать отношение площади таблицы к количеству пересечений.

В начале необходимо определится каким образом будет задаваться таблица. Я выбрал способ, в котором каждое слово представляет собой отдельный класс, у которого есть свойства задающие координаты слова в таблице и его размер.
```csharp
	public class Word
	{
        char[] letters;
        public int x, y;
        public int size;
        public bool isVertical;

		//Конструктор принимающий координаты с слово
        public Word(int x, int y, string word, bool isVertical) { 
            this.x = x; 
            this.y = y;
            this.isVertical = isVertical;
            this.size = word.Length;
            letters = new char[size];
            for (int i = 0; i < size; i++) letters[i] = word[i];
        }
		//Индексатор, возвращающий символ слова
        public char this[int index]
        {
            get { return letters[index]; }
        }
    }
```

Также необходимо свойство, указывающее на ориентацию слова и конечно же массив символов, которые и задают это слово.  Для удобства был создан конструктор и индексатор.

Таблица тоже будет представлять собой отдельный класс, внутри которого будут свойства, описывающие его ширину, высоту, площадь, количество слов и пересечений и список всех слов.
```csharp
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
    }
```

Теперь необходимо определится с общим алгоритмом выполнения программы: изначально мы получаем список слов от пользователя, который должны разместить в таблице, это значит что мы по очереди каждое слово будем добавлять в таблицу. При добавлении сначала необходимо проверить можно ли добавить слово на данную позицию, затем рассмотреть более и выгодные варианты и только тогда разместить слово на самую выгодную позицию. Рассмотрим каждый шаг в отдельности.

Так как существуют ситуации когда таблицу из слов составить невозможно т.е. когда слова не имеют пересекающихся символов, то сделаем так чтобы метод заполнения имел булево возвращаемое значение.
```c#
        public bool FillWordTable(HashSet<string> words)
        {
            foreach (string word in words)
            {
                if (!AddWord(word)) return false;
            }
            return true;
        }
```

По сути этот метод лишь поочередно вызывает добавление элемента в таблицу и если, какой-то элемент нельзя добавить возвращает false, если же получилось добавить все элементы, то возвращает true.

Внутри функции добавления будут вызываться 2 вспомогательные функции:
1. Проверка на возможность добавления слова в рассматриваемую позицию
2. Нахождение количества пересечений слова с другими для дальнейшего нахождения коэффициента связности.
Для начала рассмотрим эти функции.

Как только мы захотим добавить слово на рассматриваемую позицию, нужно проверить не будет ли иметь оно вокруг себя другие слова (кроме как по диагонали), и если оно пересекается с другими словами, то должно пересекаться в одинаковых символах. Для этого будет проходится по всем словам в таблице и проверять эти условия.
```c#
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
```

Функция принимает на вход "incWord" - слово к которому мы хотим присоединить новое слово - "newWord", и координаты на которых будет новое слово.
Теперь в цикле просматриваем каждое слово, если оно равно "incWord", то пропускаем дальнейшие шаги и начинаем новую итерацию.

Теперь у нас возможны 2 случая:
1. Слова находятся в одном положении (вертикаль или горизонталь)
2. Слова находятся в разных
В обоих случаях нужно проверить на пересечение слов и их близость. Но в отличии от второго в первом случае нас не интересует, пересекаются ли они в одинаковых символах, ведь если они пересекаются это значит что они "наложены" друг на друга.
Так в обоих случаях в зависимости от их расположения (вертикаль или горизонталь), проверяются области их нахождения относительно друг друга, но во втором, если они находятся в одной области т.е. пересекаются необходимо сверить равны ли символы на которых они пересекаются. Для этого необходимо найти их положение в обоих словах.

Теперь рассмотрим функцию нахождения количества пересечений.
```c#
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
```
Эта функция принимает вставляемое слово и его координаты. По сути оно также просто проверяет области в зависимости от ориентации и если слова находятся в одной области то увеличивает счетчик.

Теперь основная функция добавления:
```c#
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
```

Она имеет возвращаемый булевский тип для того чтобы понимать можно ли вставить слово в таблицу, а соответственно и заполнить её всю. На вход она получает слово в строковом формате, которое необходимо вставить.

Если слово будет первым в таблице то метод, добавляет это слово на координаты (0,0) и возвращает true.
Иначе алгоритм проходится по каждому слово в таблице и по каждому его символу сравнивая символы слов из таблицы и новым словом. В случае совпадения проверяет ориентацию слова, к которому будет присоединено новое и на основе этого рассчитывает координаты для нового элемента таблицы.

После создается шаблон слова и проверяется можно ли его вставить на указанные координаты, если нет то начинается следующая итерация, а если да то флаг того что вставить слово в таблицу возможно помечается true, и рассчитывается коэффициент связности для этого варианта вставки. Затем происходит проверка является ли данный вариант "выгоднее", если да, то новые координаты и элемент таблицы запоминаются, иначе алгоритм начинает новую итерацию.
```c#
//Проверка возможности вставки
if (IsCanAdd(insWord, curWord, newWordX, newWordY))
                                {
                                    if (!isAdd) wordToAdd = curWord;
                                    
                                    isAdd = true;
                                    //Нахожение количества пересичений
                                    inter = CountOfIntersection(curWord, newWordX, newWordY);
                                    newWidth = word.Length > width ? word.Length : width;
                                    //Расчет коэффициента связности
                                    minRatio = newWidth * height / (countOfCrossing + inter);
									//Сравнение выгоды
                                    if (currRatio > minRatio)
                                    {
	                                    //Запоминание варианта
                                        currWordX = newWordX;
                                        currWordY = newWordY;
                                        currRatio = minRatio;
                                        maxInter = inter;
                                        wordToAdd = new Word(currWordX, currWordY, word, false);
                                    }
                                }
```

После всех итераций, если слово можно вставить т.е. проверяется флаг добавления "isAdd", то слово добавляется в список слов таблицы на самое выгодное место и возвращается true, иначе возвращается false.

# Визуализация
Для визуализации будет использоваться WPF .NET Framework. С помощью компонентов создадим интерфейс, который должен предусматривать возможность добавления слов пользователем и заполнение таблицы.

Для начала в основном классе создадим поля в которых будут хранится список слов (точнее хэш-сет для того чтобы избежать повторов), таблица и размеры самого холста с размером отдельной ячейки:
```c#
    public partial class MainWindow : Window
    {
	    //Сет слов
        HashSet<string> words;
        //Таблица
        WordTable table;
		//Размеры холста
        double canvasWidth;
        double canvasHeight;
		//Размеры ячейки
        double boxSize = 20;

        public MainWindow()
        {
            InitializeComponent();
            words = new HashSet<string>();
        }
```

Теперь реализуем добавления слов. Для этого создадим обработчик события, который будет проверять сколько слов введено, и если введено одно слово добавлять его в сет, затем очищать поле ввода и фокусироваться на нем.
```c#
        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            string currWord = textBox.Text;
            if (currWord.Contains(" ")) {
                MessageBox.Show("Введите только одно слово!");
                return;
            }
            if (currWord == null) return;
            words.Add(currWord);
            textBox.Focus();
            textBox.Clear();
        }
```
Также для удобства сделаем такой же алгоритм, для обработчика события нажатия клавиши "Enter":
```c#
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string currWord = textBox.Text;
                if (currWord.Contains(" "))
                {
                    MessageBox.Show("Введите только одно слово!");
                    return;
                }
                if (currWord == null) return;
                words.Add(currWord);
                textBox.Focus();
                textBox.Clear();
            }
        }
```

Теперь метод для заполнений таблицы, который также вызовет метод её отрисовки:
```c#
        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            WordTable wordTable = new WordTable();
            if (!wordTable.FillWordTable(words))
            {
                MessageBox.Show("Невозможно составить таблицу");
                return;
            }

            //Координаты цента
            canvasWidth = Canvas.ActualWidth;
            canvasHeight = Canvas.ActualHeight;
            double x = (canvasWidth - wordTable.width * boxSize) / 2;
            double y = (canvasHeight - wordTable.height * boxSize) / 2;
            this.table = wordTable;

            //Вызов отрисовки в координатах
            PaintTable(wordTable, x, y);
        }
```
Этот метод при нажатии кнопки "начать", вызывает метод заполнения таблицы, выводит сообщение в случае если это невозможно, находит координаты центра холста и вызывает метод отрисовки таблицы.

```c#
        public void PaintTable(WordTable table, double x, double y)
        {
            List<Word> words = table.words;
            Canvas.Children.Clear();
            Rectangle R;
            foreach (Word word in words)
            {
                double X = x + word.x*boxSize;
                double Y = y + word.y*boxSize;
                int isVert = word.isVertical ? 1 : 0;
                for (int i = 0; i < word.size; i++)
                {
                    // Расчитываем координаты ячеки с буквой
                    double letterX = X + i * boxSize * (Math.Abs(isVert - 1));
                    double letterY = Y + i * boxSize * isVert;

                    //Рисуем ячейку
                    R = new Rectangle();
                    R.Width = 20;
                    R.Height = 20;
                    R.Stroke = new SolidColorBrush(Colors.Black);
                    R.StrokeThickness = 2;
                    R.Fill = new SolidColorBrush(Colors.White);

                    //Позиционирование
                    Canvas.SetLeft(R, letterX);
                    Canvas.SetTop(R, letterY);
                    Canvas.Children.Add(R);

                    //Добавляем текст
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = word[i].ToString();
                    textBlock.FontSize = 12; // Установите размер шрифта
                    textBlock.Foreground = new SolidColorBrush(Colors.Black);

                    // Устанавливаем позицию текста по центру прямоугольника
                    Canvas.SetLeft(textBlock, letterX + 5); // Смещение для центрирования
                    Canvas.SetTop(textBlock, letterY + 2); // Смещение для центрирования

                    // Добавляем текст в Canvas
                    Canvas.Children.Add(textBlock);
                }
            }
        }
```
Метод отрисовки проходится по всем словам в таблице и рисует ячейки и символы в них, в соответствии с координатами слова и его ориентации.

В конце для адаптивности создадим обработчик изменения размера окна, который будет перерисовывать таблицу на новых координатах.
```c#
        private void Canvas_Size_Changed(object sender, SizeChangedEventArgs e)
        {
            if (table != null)
            {
                canvasHeight = Canvas.ActualHeight;
                canvasWidth = Canvas.ActualWidth;
                double x = (canvasWidth - table.width * boxSize) / 2;
                double y = (canvasHeight - table.height * boxSize) / 2;

                //Вызов отрисовки в координатах
                PaintTable(table, x, y);
            }

        }
```
