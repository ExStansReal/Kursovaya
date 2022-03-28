using Microsoft.Win32;
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
    /// Логика взаимодействия для Изменение_данных.xaml
    /// </summary>
    public partial class Изменение_данных : Window
    {
        public string MeUser = "";
        public Изменение_данных(string name)
        {
            InitializeComponent();
            MeUser = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Платная_версия a = new Платная_версия(MeUser);
            a.Top = this.Top;
            a.Left = this.Left;
            a.Show();
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool waaa = true;
            string login, parol, ava;
            using (BinaryReader reader = new BinaryReader(File.Open(@"users\" + MeUser + @"\info.dat", FileMode.Open)))
            {
                login = reader.ReadString();
                parol = reader.ReadString();
                ava = reader.ReadString();
            }
            if (avatarka == "")
            {
                avatarka = ava;
            }
            if (log1.Text == null || log1.Text == "")
            {
                log1.Text = login;
                waaa = false;
            }
            if (pas1.Text == null || pas1.Text == "")
            {
                pas1.Text = parol;
            }

            if (waaa == true)
            {
                if (Directory.Exists(@"users\" + log1.Text) == false)
                {
                    if (String.IsNullOrWhiteSpace(log1.Text) == false && String.IsNullOrWhiteSpace(pas1.Text) == false && log1.Text.Length > 3 && pas1.Text.Length > 3)
                    {
                        Directory.CreateDirectory(@"users\" + log1.Text);
                        Directory.Move(@"users\" + MeUser + @"\playlists", @"users\" + log1.Text + @"\playlists");
                        File.Move(@"users\" + MeUser + @"\theme.dat", @"users\" + log1.Text + @"\theme.dat");
                        using (BinaryWriter writer = new BinaryWriter(File.Open(@"users\" + log1.Text + @"\info.dat", FileMode.Create)))
                        {
                            writer.Write(log1.Text);
                            writer.Write(pas1.Text);
                            writer.Write(avatarka);
                        }
                        File.Delete(@"users\" + MeUser + @"\info.dat");
                        Directory.Delete(@"users\" + MeUser);
                        MeUser = log1.Text;
                    }
                }
                else
                {
                    MessageBox.Show("С таким логином пользователь уже существует. Выберете другой");
                }
                
            }
            else
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(@"users\" + MeUser + @"\info.dat", FileMode.Open)))
                {
                    writer.Write(MeUser);
                    writer.Write(pas1.Text);
                    writer.Write(avatarka);
                }
            }
            avatarka = "";
        }

        private string avatarka = "";

        private void Button_Click_2(object sender, RoutedEventArgs e) //аватарка
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JPG (*jpg)|*.jpg|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                avatarka = dlg.FileName;
            }
        }
    }
}
