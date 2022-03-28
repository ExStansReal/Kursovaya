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
using System.Windows.Threading;

namespace музыка_тест
{
    /// <summary>
    /// Логика взаимодействия для Бесплатная_версия.xaml
    /// </summary>
    public partial class Бесплатная_версия : Window
    {
        public Бесплатная_версия()
        {
           
            InitializeComponent();
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(0.1);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ShowPosition();
        }

        private void ShowPosition()
        {
            Progress.Value = MediaPlayer.Position.TotalSeconds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MP3 (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                MediaPlayer.Source = new Uri(dlg.FileName);
                uot();
                Thread.Sleep(1000);
                MediaPlayer.Source = new Uri(dlg.FileName);
                uot();
            }
        }

        private void uot()
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan == true)
                Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds / 100;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Play();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            AddPlayList a = new AddPlayList();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        string path = @"playlists\";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            reklama.Source = new BitmapImage(new Uri($@"{CurretDir}/reklama/1.jpg"));

            int allFiles = new DirectoryInfo(path).GetFiles().Length;
            if (allFiles <= 0)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(path + "Моя музыка.dat", FileMode.OpenOrCreate)))
                {
                    writer.Write("U+ME - KillBoy");
                    writer.Write($@"{CurretDir}\audio.mp3");  //ВОТ ТУТ ПОМЕНЯЙ ПУТЬ НА audio.mp3 - ЭТО СТАНДАРТНАЯ ПЕСНЯ. ОНА ТАК-ЖЕ В БИН-ДЕБАГ
                
                }
            }
            UpdateAllList();

            //проверка на конец песни
            DispatcherTimer a = new DispatcherTimer();
            a.Interval = TimeSpan.FromSeconds(1);
            a.Tick += A_Tick;
            a.Start();
           
            //реклама
            DispatcherTimer reklaam = new DispatcherTimer();
            reklaam.Interval = TimeSpan.FromSeconds(5);
            reklaam.Tick += Reklaam_Tick;
            reklaam.Start();

        }
        public string CurretDir = Directory.GetCurrentDirectory();
        int nuberReklama = 1;
        private void Reklaam_Tick(object sender, EventArgs e)
        {
            reklama.Source = new BitmapImage(new Uri($@"{CurretDir}/reklama/{nuberReklama}.jpg"));
            nuberReklama++;
            if (nuberReklama >= 5)
            {
                nuberReklama = 1;
            }
        }

        private void A_Tick(object sender, EventArgs e)
        {
            try
            {
                double sec = MediaPlayer.Position.Seconds;
                double min = MediaPlayer.Position.Minutes;
                while (min >= 1)
                {
                    min -= 1;
                    sec += 60;
                }
                if (kiklMus.IsChecked == true)
                {
                    if (sec >= MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds - 1)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play();
                    }
                }
                if (kikulList.IsChecked == true)
                {
                    if (sec >= MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds - 1)
                    {
                        if (musics.SelectedIndex + 1 <= musics.Items.Count - 1)
                        {
                            MediaPlayer.Stop();
                            musics.SelectedIndex += 1;
                            MediaPlayer.Play();
                        }
                        else
                        {
                            MediaPlayer.Stop();
                            musics.SelectedIndex = 0;
                            Progress.Value = 0;
                            MediaPlayer.Play();
                        }
                    }
                }
                if (sec >= MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds - 1 && kikulList.IsChecked == false && kiklMus.IsChecked == false)
                {
                    if (randomMusic.IsChecked == false)
                    {
                        if (musics.SelectedIndex + 1 <= musics.Items.Count - 1)
                        {
                            MediaPlayer.Stop();
                            Progress.Value = 0;
                            musics.SelectedIndex += 1;
                            MediaPlayer.Play();
                        }
                        else
                        {
                            MediaPlayer.Stop();
                        }
                    }
                    else
                    {
                        MediaPlayer.Stop();
                        Random rnd = new Random();
                        int musicRAN = rnd.Next(0, musics.Items.Count - 1);
                        musics.SelectedIndex = musicRAN;
                        MediaPlayer.Play();
                    }
                }
            }
            catch
            {

            }
        }

        private void UpdateAllList()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"playlists\");
            foreach (var file in directoryInfo.GetFiles()) //проходим по файлам
            {
                string name = file.Name;
                name = name.Replace(".dat", "");
                playlists.Items.Add(name);
            }
        }
        private void playlists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            readPlayList();
        }
        private void readPlayList()
        {
            try
            {
                allMusic.Clear();
                musics.Items.Clear();
                using (BinaryReader reader = new BinaryReader(File.Open(path + Convert.ToString(playlists.SelectedItem) + ".dat", FileMode.Open)))
                {
                    while (reader.PeekChar() > -1)
                    {
                        string nameA = reader.ReadString();
                        string pathMu = reader.ReadString();
                        allMusic.Add(new Music() { name = nameA, pathName = pathMu });
                    }
                }
                updateAllMusic();
            }
            catch
            {

            }
        }
        private void updateAllMusic()
        {
            foreach (Music a in allMusic)
            {
                musics.Items.Add(a.name);
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (playlists.SelectedItem != null)
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "MP3 (*.mp3)|*.mp3|All files (*.*)|*.*";
                if (dlg.ShowDialog() == true)
                {
                    //MessageBox.Show(System.IO.Path.GetFileNameWithoutExtension(dlg.FileName));
                    using (BinaryWriter writer = new BinaryWriter(File.Open(path + Convert.ToString(playlists.SelectedItem) + ".dat", FileMode.Append)))
                    {
                        writer.Write(System.IO.Path.GetFileNameWithoutExtension(dlg.FileName));
                        writer.Write(dlg.FileName);
                    }
                }
                readPlayList();
                MediaPlayer.Stop();
            }
            else
            {
                MessageBox.Show("Выберете сначала плейлист, в который добавете музыку");
            }
        }

        List<Music> allMusic = new List<Music>();

        private void musics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int indexM = musics.SelectedIndex;
                MediaPlayer.Source = new Uri(allMusic[indexM].pathName);
                uot();
                Thread.Sleep(1000);
                indexM = musics.SelectedIndex;
                MediaPlayer.Source = new Uri(allMusic[indexM].pathName);
                uot();
                MediaPlayer.Play();

                gromkost.Value = MediaPlayer.Volume;
            }
            catch
            {
                //MessageBox.Show("Неизвестная ошибка!\n Возможно вы переместили ваш файл с песней в другое место и приложение не смогло его найти.");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                bool minutes = false;
                int timeSec = MediaPlayer.Position.Seconds;
                timeSec -= 5;
                if (timeSec < 0)
                {
                    timeSec = 60 + timeSec;
                    minutes = true;
                }
                string StimeSec = Convert.ToString(timeSec);
                int timeMin = MediaPlayer.Position.Minutes;
                if (minutes == true)
                {
                    timeMin -= 1;
                }
                string StimeMin = Convert.ToString(timeMin);
                string timeHo = Convert.ToString(MediaPlayer.Position.Hours);

                string wow = timeHo + ":" + StimeMin + ":" + StimeSec;
                MediaPlayer.Position = TimeSpan.Parse(wow);
            }
            catch
            {

            }

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                bool minutes = false;
                int timeSec = MediaPlayer.Position.Seconds;
                timeSec += 5;
                if (timeSec > 60)
                {
                    timeSec -= 60;
                    minutes = true;
                }
                string StimeSec = Convert.ToString(timeSec);
                int timeMin = MediaPlayer.Position.Minutes;
                if (minutes == true)
                {
                    timeMin += 1;
                }
                string StimeMin = Convert.ToString(timeMin);
                string timeHo = Convert.ToString(MediaPlayer.Position.Hours);

                string wow = timeHo + ":" + StimeMin + ":" + StimeSec;
                MediaPlayer.Position = TimeSpan.Parse(wow);




            }
            catch
            {

            }
        }

        private void gromkost_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayer.Volume = gromkost.Value;

        }





        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                bool minutes = false;
                double timeSec = MediaPlayer.Position.Seconds;
                timeSec = Math.Round(timeSec);
                timeSec += 10;
                if (timeSec > 60)
                {
                    timeSec -= 60;
                    minutes = true;
                }
                string StimeSec = Convert.ToString(timeSec);
                int timeMin = MediaPlayer.Position.Minutes;
                if (minutes == true)
                {
                    timeMin += 1;
                }
                string StimeMin = Convert.ToString(timeMin);
                string timeHo = Convert.ToString(MediaPlayer.Position.Hours);

                string wow = timeHo + ":" + StimeMin + ":" + StimeSec;
                MediaPlayer.Position = TimeSpan.Parse(wow);
            }
            catch
            {

            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                bool minutes = false;
                int timeSec = MediaPlayer.Position.Seconds;
                timeSec -= 10;
                if (timeSec < 0)
                {
                    timeSec = 60 + timeSec;
                    minutes = true;
                }
                string StimeSec = Convert.ToString(timeSec);
                int timeMin = MediaPlayer.Position.Minutes;
                if (minutes == true)
                {
                    timeMin -= 1;
                }
                string StimeMin = Convert.ToString(timeMin);
                string timeHo = Convert.ToString(MediaPlayer.Position.Hours);


                string wow = timeHo + ":" + StimeMin + ":" + StimeSec;
                MediaPlayer.Position = TimeSpan.Parse(wow);
            }
            catch
            {

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                if (playlists.SelectedItem != null)
                {
                    if (Convert.ToString(playlists.SelectedItem + ".dat") != "Моя музыка.dat")
                    {
                        File.Delete(path + playlists.SelectedItem + ".dat");
                        musics.Items.Clear();
                        allMusic.Clear();
                        playlists.Items.Clear();
                        UpdateAllList();
                    }
                    else
                    {
                        MessageBox.Show("Нельзя удалять основной плейлист");
                    }
                }
                else
                {
                    MessageBox.Show("Выберете плейлист!");
                }
            }
            catch
            {
                MessageBox.Show("Произошла какая-то ошибка. Проверьте что вы делали.");
            }
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            Progress.Maximum = 0;
            Progress.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        public DispatcherTimer Timer;

        private void Progress_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Timer.Stop();
            MediaPlayer.Stop();
        }

        private void Progress_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MediaPlayer.Position = TimeSpan.FromSeconds(Progress.Value);
            Timer.Start();
            MediaPlayer.Play();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            try
            {
                MediaPlayer.Stop();
                allMusic.RemoveAt(musics.SelectedIndex);


                //MessageBox.Show(System.IO.Path.GetFileNameWithoutExtension(dlg.FileName));
                File.Delete(path + Convert.ToString(playlists.SelectedItem) + ".dat");
                using (BinaryWriter writer = new BinaryWriter(File.Open(path + Convert.ToString(playlists.SelectedItem) + ".dat", FileMode.Create)))
                {
                    foreach (Music a in allMusic)
                    {
                        writer.Write(a.name);
                        writer.Write(a.pathName);
                    }

                }
                readPlayList();

            }
            catch
            {

            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            Авторизация a = new Авторизация();
            a.Top = this.Top;
            a.Left = this.Left;
            a.Show();
            this.Hide();
        }
        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musics.SelectedIndex == musics.Items.Count - 1)
                {
                    musics.SelectedIndex = 0;
                }
                else
                {
                    musics.SelectedIndex++;
                }

            }
            catch
            {

            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {
                if (musics.SelectedIndex == 0)
                {
                    musics.SelectedIndex = musics.Items.Count - 1;
                }
                else
                {
                    musics.SelectedIndex--;
                }

            }
            catch
            {

            }
        }

        private void kikulList_Checked(object sender, RoutedEventArgs e)
        {
            kiklMus.IsChecked = false;
        }

        private void kiklMus_Checked(object sender, RoutedEventArgs e)
        {
            kikulList.IsChecked = false;
        }
    }
}
