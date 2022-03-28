using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Регестрация.xaml
    /// </summary>
    public partial class Регестрация : Window
    {
        public Регестрация()
        {
            InitializeComponent();
            pathAvatar = $@"{CurretDir}\stanAvatar.jpg";
        }
        private string pathAvatar = $@"\stanAvatar.jpg";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Авторизация a = new Авторизация();
            a.Top = this.Top;
            a.Left = this.Left;
            a.Show();
            this.Hide();
        }
        string CurretDir = Directory.GetCurrentDirectory();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(log1.Text) == false && String.IsNullOrWhiteSpace(pas1.Password) == false && String.IsNullOrWhiteSpace(pas2.Password) == false && pas1.Password == pas2.Password && log1.Text.Length > 3 && pas1.Password.Length > 3)
            {
                if (Directory.Exists(@"users\" + log1.Text) == false)
                {
                    Directory.CreateDirectory(@"users\" + log1.Text);
                    using (BinaryWriter writer = new BinaryWriter(File.Open(@"users\" + log1.Text + @"\info.dat", FileMode.Create)))
                    {
                        writer.Write(log1.Text);
                        writer.Write(pas1.Password);
                        writer.Write(pathAvatar);
                    }
                    MessageBox.Show("Вы зарегестрированы");
                    Directory.CreateDirectory(@"users\" + log1.Text + @"\playlists");
                    using (BinaryWriter writer = new BinaryWriter(File.Open(@"users\" + log1.Text + @"\theme.dat", FileMode.Create)))
                    {
                        writer.Write("WHITE");
                    }
                }
                else
                {

                    MessageBox.Show("Такой пользователь уже есть");
                }
            }
            else
            {
                MessageBox.Show("Проверьте введённые данные. Вы что-то ввели не правильно");
            }
        }
       
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG (*jpg)|*.jpg|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                pathAvatar = dlg.FileName;
            }
        }
    }
}
