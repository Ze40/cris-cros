using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cris_cros
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HashSet<string> words;
        WordTable table;

        double canvasWidth;
        double canvasHeight;

        double boxSize = 20;

        public MainWindow()
        {
            InitializeComponent();
            words = new HashSet<string>();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
;
        }
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
    }
}
