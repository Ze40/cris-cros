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
        public MainWindow()
        {
            InitializeComponent();
            words = new HashSet<string>();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
;
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
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
            WordTable wordTable = new WordTable();
            if (!wordTable.FillWordTable(words))
            {
                MessageBox.Show("0");
                return;
            }
            PaintTable(wordTable, 20, 20);
        }

        public void PaintTable(WordTable table, int x, int y)
        {
            List<Word> words = table.words;
            Canvas.Children.Clear();
            Rectangle R;
            foreach (Word word in words)
            {
                int X = x + word.x*20;
                int Y = y + word.y*20;
                int isVert = word.isVertical ? 1 : 0;
                for (int i = 0; i < word.size; i++)
                {
                    // Расчитываем координаты ячеки с буквой
                    int letterX = X + i * 20 * (Math.Abs(isVert - 1));
                    int letterY = Y + i * 20 * isVert;

                    //Рисуем ячейку
                    R = new Rectangle();
                    R.Width = 20;
                    R.Height = 20;
                    R.Margin = new Thickness(letterX, letterY, 0, 0);
                    R.Stroke = new SolidColorBrush(Colors.Black);
                    R.StrokeThickness = 2;
                    R.Fill = new SolidColorBrush(Colors.White);

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
