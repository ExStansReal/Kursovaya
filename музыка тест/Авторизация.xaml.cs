using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace музыка_тест
{
    /// <summary>
    /// Логика взаимодействия для Авторизация.xaml
    /// </summary>
    public partial class Авторизация : Window
    {
        public Авторизация()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateDirs();

            if (Directory.Exists(@"users\" + log.Text))
            {
                string login, parol = "";
                using (BinaryReader reader = new BinaryReader(File.Open(@"users\" + log.Text + @"\info.dat", FileMode.Open)))
                {
                    login = reader.ReadString();
                    parol = reader.ReadString();
                }
                if (log.Text == login && pas.Password == parol)
                {
                    Платная_версия a = new Платная_версия(log.Text);
                    a.Top = this.Top;
                    a.Left = this.Left;
                    a.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Не верный логин или пароль");
                }
            }
            else
            {
                MessageBox.Show("Такого пользователя нет");
            }
        }

        private void CreateDirs()
        {
            Directory.CreateDirectory(@"users");
            Directory.CreateDirectory(@"playlists");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateDirs();
            Регестрация a = new Регестрация();
            a.Top = this.Top;
            a.Left = this.Left;
            a.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            CreateDirs();
            Бесплатная_версия a = new Бесплатная_версия();
            a.Top = this.Top;
            a.Left = this.Left;
            a.Show();
            this.Hide();
        }
    }
}
