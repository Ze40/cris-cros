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
            int _x = 10;
            int _y = 10;
            WordTable wordTable = new WordTable(_x, _y);
            wordTable.Add("ahah", 0, 0, true);
            wordTable.Add("hehehe", 1, 0, true);


            char[,] table = wordTable.GetTable();
            PaintTable(table, 20, 20);
        }

        public void PaintTable(char[,] table, int x, int y)
        {

        }
    }
}
