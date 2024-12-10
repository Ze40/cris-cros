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
            foreach (string word in words) MessageBox.Show(word);
        }
    }
}
