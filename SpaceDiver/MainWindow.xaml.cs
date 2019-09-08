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
using System.Media;
using WMPLib;

namespace SpaceDiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //WMPLib.WindowsMediaPlayer sound = new WindowsMediaPlayer();
        
        
        System.Windows.Forms.Timer tickker = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tickker2 = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tickker3 = new System.Windows.Forms.Timer();


        List<Ellipse> shots = new List<Ellipse>();
        List<Image> easyRocksImgs = new List<Image>();
        List<Image> hardRocksImgs = new List<Image>();
        List<double> randX = new List<double>();
        List<Ellipse> Stars = new List<Ellipse>();

        int round = 0, speed = 0;
        public bool exitDown = false, moveDown = false, startDown = false, optionsDown = false, highScoresDown = false, backDown = false, paused = false, resumed = false, dead = false, mainMenued = false; 

        int points = 0, time = 0;

        public MainWindow()
        {
            InitializeComponent();
            tickker2.Interval = 5;
            tickker.Interval = 300;
            tickker3.Interval = 1000;
            tickker2.Start();
            tickker2.Tick += moveStars;
            tickker2.Tick += stars;
            
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            start_text.FontSize = 55;
        }

        private void text1_MouseLeave(object sender, MouseEventArgs e)
        {
            start_text.FontSize = 48;
        }

        private void Label2_MouseEnter(object sender, MouseEventArgs e)
        {
            options_text.FontSize = 45;
        }

        private void text2_MouseLeave(object sender, MouseEventArgs e)
        {
            options_text.FontSize = 36;
        }

        private void Label3_MouseEnter(object sender, MouseEventArgs e)
        {
            scores_text.FontSize = 40;
        }

        private void text3_MouseLeave(object sender, MouseEventArgs e)
        {
            scores_text.FontSize = 36;
        }

        private void Label4_MouseEnter(object sender, MouseEventArgs e)
        {
            exit_text.FontSize = 55;
        }

        private void text4_MouseLeave(object sender, MouseEventArgs e)
        {
            exit_text.FontSize = 48;
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            exitDown = false;
            moveDown = false;
            startDown = false;
            optionsDown = false;
            highScoresDown = false;
            backDown = false;
            paused = false;
            resumed = false;
            mainMenued = false;
        }

        private void exit_mouse_up(object sender, MouseButtonEventArgs e)
        {
            if (exitDown)
            {
                Application.Current.Shutdown();
            }
        }

        private void exit_mous_down(object sender, MouseButtonEventArgs e)
        {
            exitDown = true;
        }

        private void move_down(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void startMousDown(object sender, MouseButtonEventArgs e)
        {
            startDown = true;
        }

        private void startMousUp(object sender, MouseButtonEventArgs e)
        {
            if (startDown)
            {
                startDown = false;
                dead = false;
                paused = false;
                for (int l = 0; l < hardRocksImgs.Count; l++)
                    gameGrid.Children.Remove(hardRocksImgs[l]);
                for (int l = 0; l < shots.Count; l++)
                    gameGrid.Children.Remove(shots[l]);
                hardRocksImgs.Clear();
                for (int l = 0; l < easyRocksImgs.Count; l++)
                    gameGrid.Children.Remove(easyRocksImgs[l]);
                easyRocksImgs.Clear();
                shots.Clear();
                health.Width = 200;
                msg.Visibility = Visibility.Hidden;
                wlcomeGrid.Visibility = Visibility.Hidden;
                gameGrid.Visibility = Visibility.Visible;

                time = 0;
                points = 0;
                Ntime.Content = time;
                Npoints.Content = points;
                
                tickker.Tick += shoot;
                
                tickker3.Start();
                if ((bool)rdEasy.IsChecked)
                {
                    try
                    {
                        tickker3.Tick -= easyRocks;
                        tickker2.Tick -= easyRocksFall;
                    }
                    catch { }
                    try
                    {
                        tickker3.Tick -= hardRocks;
                        tickker2.Tick -= hardRocksFall;
                    }
                    catch { }
                    tickker3.Tick += easyRocks;
                    tickker2.Tick += easyRocksFall;
                }
                else if ((bool)rdMedium.IsChecked)
                {
                    try
                    {
                        tickker3.Tick -= easyRocks;
                        tickker2.Tick -= easyRocksFall;
                    }
                    catch { }
                    try
                    {
                        tickker3.Tick -= hardRocks;
                        tickker2.Tick -= hardRocksFall;
                    }
                    catch { }
                    tickker3.Tick += hardRocks;
                    tickker2.Tick += hardRocksFall;
                }
                else if ((bool)rdHard.IsChecked)
                {
                    try
                    {
                        tickker3.Tick -= easyRocks;
                        tickker2.Tick -= easyRocksFall;
                    }
                    catch { }
                    try
                    {
                        tickker3.Tick -= hardRocks;
                        tickker2.Tick -= hardRocksFall;
                    }
                    catch { }
                    tickker3.Tick += hardRocks;
                    tickker2.Tick += hardRocksFall;
                    tickker3.Tick += easyRocks;
                    tickker2.Tick += easyRocksFall;
                }
            }
        }

        private void optionsMousDown(object sender, MouseButtonEventArgs e)
        {
            optionsDown = true;
        }

        private void optionsMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (optionsDown)
            {
                wlcomeGrid.Visibility = Visibility.Hidden;
                optionsGrid.Visibility = Visibility.Visible;
            }
        }

        private void highScoresMouseDown(object sender, MouseButtonEventArgs e)
        {
            highScoresDown = true;
        }

        private void highScoresMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (highScoresDown)
            {
                wlcomeGrid.Visibility = Visibility.Hidden;
                highScoresGrid.Visibility = Visibility.Visible;
            }
        }

        
        private void backMouseDown(object sender, MouseButtonEventArgs e)
        {
            backDown = true;
            backBox.FontSize = 42;
            backBox_Copy.FontSize = 42;
            backBox_Copy1.FontSize = 42;
        }

        private void backMouseUp(object sender, MouseButtonEventArgs e)
        {
            backBox.FontSize = 36;
            backBox_Copy.FontSize = 36;
            backBox_Copy1.FontSize = 36;
            if (backDown)
            {
                gameGrid.Visibility = Visibility.Hidden;
                optionsGrid.Visibility = Visibility.Hidden;
                highScoresGrid.Visibility = Visibility.Hidden;
                wlcomeGrid.Visibility = Visibility.Visible;
            }
        }

        private void spaceMouseMove(object sender, MouseEventArgs e)
        {
            if ((bool)rdMouse.IsChecked)
            {
                Thickness th = new Thickness();
                th.Left = (Mouse.GetPosition(gameGrid).X) - (.5 * rocket.Width);
                th.Top = 464;
                rocket.Margin = th;
            }
        }

        private void spaceMouseDown(object sender, MouseButtonEventArgs e)
        {

            if ((bool)rdMouse.IsChecked)
            {
                try
                {
                    tickker2.Tick -= movingShot;
                }
                catch { }
                tickker2.Tick += movingShot;
                lazerShots();
            }
            
        }

        private void stars(object sender, EventArgs e)
        {
            Random randS = new Random();
            double _S = randS.Next(-900, 900);
            double size = randS.Next(1, 3);
            SolidColorBrush s = new SolidColorBrush(Color.FromRgb(225, 225, 225));
            Ellipse ball = new Ellipse()
            {
                Fill = s,
                Width = size,
                Height = size,
                Margin = new Thickness(_S, -600, 0, 0)
            };
            ball.SetValue(Panel.ZIndexProperty, -1);
            Stars.Add(ball);
            gameGrid.Children.Add(ball);
        }

        private void moveStars(object sender, EventArgs e)
        {
            for (int i = 0; i < Stars.Count; i++)
            {
                Stars[i].Margin = new Thickness(Stars[i].Margin.Left, Stars[i].Margin.Top + 10, Stars[i].Margin.Right, Stars[i].Margin.Bottom);
            }
            for (int i = 0; i < Stars.Count; i++)
            {
                if (Stars[i].Margin.Top >= 900)
                {
                    gameGrid.Children.Remove(Stars[i]);
                    Stars.Remove(Stars[i]);
                }
            }
        }

        private void lazerShots()
        {
            if (!dead && !paused)
            {
                SolidColorBrush s = new SolidColorBrush(Color.FromRgb(210, 87, 91));
                Ellipse ball = new Ellipse()
                {
                    Fill = s,
                    Width = 12,
                    Height = 20,
                    Margin = new Thickness(2 * (rocket.Margin.Left - 359.5), 310, 0, 0)
                };
                ball.SetValue(Panel.ZIndexProperty, -1);
                shots.Add(ball);
                gameGrid.Children.Add(ball);

                tickker.Start();
            }

        }

        private void shoot(object sender, EventArgs e)
        {

            if (!dead && !paused)
            {
                SolidColorBrush s = new SolidColorBrush(Color.FromRgb(210, 87, 91));
                Ellipse ball = new Ellipse()
                {
                    Fill = s,
                    Width = 12,
                    Height = 20,
                    Margin = new Thickness(2 * (rocket.Margin.Left - 359.5), 310, 0, 0)
                };
                //sound.URL = "asd.mp3";
                //sound.controls.play();
                shots.Add(ball);
                gameGrid.Children.Add(ball);
            }
        }

        private void movingShot(object sender, EventArgs e)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Margin = new Thickness(shots[i].Margin.Left, shots[i].Margin.Top - 10, shots[i].Margin.Right, shots[i].Margin.Bottom);
            }
            for (int i = 0; i < shots.Count; i++)
            {
                if (shots[i].Margin.Top + 600 == 0)
                {
                    gameGrid.Children.Remove(shots[i]);
                    shots.Remove(shots[i]);
                }
            }
        }

        private void easyRocks(object sender, EventArgs e)
        {
            if ((bool)rdEasy.IsChecked) time++;
            Ntime.Content = time;
            Random ran = new Random();
            double _X = ran.Next(-750,750);
            Image img = new Image();
            img.Width = 40;
            img.Height = 150;
            img.Stretch = Stretch.Fill;
            BitmapImage src = new BitmapImage( new Uri(@"G:\Programing languages\C#\Projects\SpaceDiver\SpaceDiver\imgs\glowingRock.png"));
            img.Source = src;
            img.Margin = new Thickness(_X, -900, 0, 0);
            easyRocksImgs.Add(img);
            gameGrid.Children.Add(img);
        }

        private void easyRocksFall(object sender, EventArgs e)
        {
            for (int i = 0; i < easyRocksImgs.Count; i++)
            {
                easyRocksImgs[i].Margin = new Thickness(easyRocksImgs[i].Margin.Left, easyRocksImgs[i].Margin.Top + 3, easyRocksImgs[i].Margin.Right, easyRocksImgs[i].Margin.Bottom);
            }
            for (int i = 0; i < easyRocksImgs.Count; i++)
            {
                if (easyRocksImgs[i].Margin.Top >= 800)
                {
                    gameGrid.Children.Remove(easyRocksImgs[i]);
                    easyRocksImgs.Remove(easyRocksImgs[i]);
                    try
                    {
                        health.Width -= 10;
                    }
                    catch { health.Width = 0; }
                }
                if (easyRocksImgs[i].Margin.Top >= 210 && easyRocksImgs[i].Margin.Top <= 400 && easyRocksImgs[i].Margin.Left >= 2 * (rocket.Margin.Left - 400) && easyRocksImgs[i].Margin.Left <= 2 * (rocket.Margin.Left - 320))
                {
                    gameGrid.Children.Remove(easyRocksImgs[i]);
                    easyRocksImgs.RemoveAt(i);
                    try
                    {
                        health.Width -= 50;
                    }
                    catch { health.Width = 0; }
                    
                }
                if (health.Width == 0)
                {
                    dead = true;
                    try
                    {
                        tickker3.Tick -= easyRocks;
                        tickker2.Tick -= easyRocksFall;
                    }
                    catch { }
                    try
                    {
                        tickker3.Tick -= hardRocks;
                        tickker2.Tick -= hardRocksFall;
                    }
                    catch { }
                    tickker.Tick -= new EventHandler(shoot);
                    tickker.Tick -= new EventHandler(movingShot);
                    tickker.Stop();
                    tickker3.Stop();
                    Ntime_Copy.Content = time;
                    Npoints_Copy.Content = points;
                    time = 0;
                    points = 0;
                    Ntime.Content = "";
                    Npoints.Content = "";
                    msg.Visibility = Visibility.Visible;
                    for (int l = 0; l < easyRocksImgs.Count; l++)
                        gameGrid.Children.Remove(easyRocksImgs[l]);
                    for (int l = 0; l < shots.Count; l++)
                        gameGrid.Children.Remove(shots[l]);
                    easyRocksImgs.Clear();
                    shots.Clear();
                }
                try
                {
                    for (int j = 0; j < shots.Count; j++)
                    {
                        if (shots[j].Margin.Left >= easyRocksImgs[i].Margin.Left - (easyRocksImgs[i].Width) && shots[j].Margin.Top <= easyRocksImgs[i].Margin.Top + 100 && shots[j].Margin.Top >= easyRocksImgs[i].Margin.Top - 60 && shots[j].Margin.Left <= easyRocksImgs[i].Margin.Left + easyRocksImgs[i].Width + (easyRocksImgs[i].Width))
                        {
                            gameGrid.Children.Remove(easyRocksImgs[i]);
                            easyRocksImgs.RemoveAt(i);
                            gameGrid.Children.Remove(shots[j]);
                            shots.RemoveAt(j);
                            points++;
                            Npoints.Content = points;
                        }
                    }
                }catch{}
            }
        }

        private void hardRocks(object sender, EventArgs e)
        {
            time++;
            Ntime.Content = time;
            Random ran = new Random();
            double _X = ran.Next(-750, 750);
            Image img = new Image();
            img.Width = 60;
            img.Height = 60;
            img.Stretch = Stretch.Fill;
            BitmapImage src = new BitmapImage(new Uri(@"G:\Programing languages\C#\Projects\SpaceDiver\SpaceDiver\imgs\hexagon-xxl.png"));
            img.Source = src;
            img.Margin = new Thickness(_X, -600, 0, 0);
            hardRocksImgs.Add(img);
            Random rnd = new Random();
            double _XX = rnd.Next(-10, 10);
            randX.Add(_XX);
            gameGrid.Children.Add(img);
        }

        private void hardRocksFall(object sender, EventArgs e)
        {
            
            for (int i = 0; i < hardRocksImgs.Count; i++)
            {
                RotateTransform rotateTransform = new RotateTransform(round, 30, 30);
                hardRocksImgs[i].RenderTransform = rotateTransform;
                hardRocksImgs[i].Margin = new Thickness(hardRocksImgs[i].Margin.Left + randX[i], hardRocksImgs[i].Margin.Top + 3, hardRocksImgs[i].Margin.Right, hardRocksImgs[i].Margin.Bottom);
                if (hardRocksImgs[i].Margin.Left <= -750 || hardRocksImgs[i].Margin.Left >= 750) randX[i] = -randX[i];
            }
            round += 5;
            for (int i = 0; i < hardRocksImgs.Count; i++)
            {
                if (hardRocksImgs[i].Margin.Top >= 700)
                {
                    gameGrid.Children.Remove(hardRocksImgs[i]);
                    hardRocksImgs.RemoveAt(i);
                    randX.RemoveAt(i);
                    try
                    {
                        health.Width -= 10;
                    }
                    catch { health.Width = 0; }
                }
                try
                {
                    if (hardRocksImgs[i].Margin.Top >= 300 && hardRocksImgs[i].Margin.Top <= 570 && hardRocksImgs[i].Margin.Left >= 2 * (rocket.Margin.Left - 400) && hardRocksImgs[i].Margin.Left <= 2 * (rocket.Margin.Left - 320))
                    {
                        gameGrid.Children.Remove(hardRocksImgs[i]);
                        hardRocksImgs.Remove(hardRocksImgs[i]);
                        randX.RemoveAt(i);
                        try
                        {
                            health.Width -= 50;
                        }
                        catch { health.Width = 0; }

                    }
                }
                catch { }
                if (health.Width == 0)
                {
                    dead = true;
                    try
                    {
                        tickker3.Tick -= new EventHandler(easyRocks);
                        tickker2.Tick -= new EventHandler(easyRocksFall);
                    }
                    catch { }
                    try
                    {
                        tickker3.Tick -= new EventHandler(hardRocks);
                        tickker2.Tick -= new EventHandler(hardRocksFall);
                    }
                    catch { }
                    tickker.Tick -= new EventHandler(shoot);
                    tickker.Tick -= new EventHandler(movingShot);
                    tickker.Stop();
                    tickker3.Stop();
                    Ntime_Copy.Content = time;
                    Npoints_Copy.Content = points;
                    time = 0;
                    points = 0;
                    Ntime.Content = "";
                    Npoints.Content = "";
                    msg.Visibility = Visibility.Visible;
                    for (int l = 0; l < hardRocksImgs.Count; l++)
                        gameGrid.Children.Remove(hardRocksImgs[l]);
                    for (int l = 0; l < shots.Count; l++)
                        gameGrid.Children.Remove(shots[l]);
                    hardRocksImgs.Clear();
                    shots.Clear();
                }
                try
                {
                    for (int j = 0; j < shots.Count; j++)
                    {
                        if (shots[j].Margin.Left >= hardRocksImgs[i].Margin.Left - (hardRocksImgs[i].Width) && shots[j].Margin.Top <= hardRocksImgs[i].Margin.Top + 60 && shots[j].Margin.Top >= hardRocksImgs[i].Margin.Top - 60 && shots[j].Margin.Left <= hardRocksImgs[i].Margin.Left + hardRocksImgs[i].Width + (hardRocksImgs[i].Width))
                        {
                            gameGrid.Children.Remove(hardRocksImgs[i]);
                            hardRocksImgs.RemoveAt(i);
                            randX.RemoveAt(i);
                            gameGrid.Children.Remove(shots[j]);
                            shots.RemoveAt(j);
                            points++;
                            Npoints.Content = points;
                        }
                    }
                }
                catch { }
            }
        }

        private void spaceMouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((bool)rdMouse.IsChecked)
            {
                //try
                //{
                //    tickker2.Tick -= movingShot;
                //}
                //catch { }
                tickker.Stop();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void spaceKeyDown(object sender, KeyEventArgs e)
        {
            if ((bool)rdKeyboard.IsChecked)
            {
                if (e.Key == Key.Right)
                {
                    rocket.Margin = new Thickness(rocket.Margin.Left + 20, rocket.Margin.Top, rocket.Margin.Right, rocket.Margin.Bottom);
                }
                else if (e.Key == Key.Left)
                {
                    rocket.Margin = new Thickness(rocket.Margin.Left - 20, rocket.Margin.Top, rocket.Margin.Right, rocket.Margin.Bottom);
                }
                if (e.Key == Key.Space)
                {
                    try
                    {
                        tickker2.Tick -= movingShot;
                    }
                    catch { }
                    tickker2.Tick += movingShot;
                    lazerShots();
                }
            }
            if ((e.Key == Key.P || e.Key == Key.Escape) && !paused)
            {
                paused = true;
                if (paused && !dead)
                {
                    pauseMenu.Visibility = Visibility.Visible;
                    tickker.Stop();
                    tickker2.Stop();
                    tickker3.Stop();
                }
            }
            else if ((e.Key == Key.P || e.Key == Key.Escape) && paused)
            {
                resumed = true;
                if (resumed)
                {
                    paused = false;
                    pauseMenu.Visibility = Visibility.Hidden;
                    tickker2.Start();
                    tickker3.Start();
                }
            }
        }

        private void spaceKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) tickker.Stop();
        }

        private void pauseMouseDown(object sender, MouseButtonEventArgs e)
        {
            paused = true;
        }

        private void pauseMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (paused && !dead)
            {
                pauseMenu.Visibility = Visibility.Visible;
                tickker.Stop();
                tickker2.Stop();
                tickker3.Stop();
            }
        }

        private void resumeMouseDown(object sender, MouseButtonEventArgs e)
        {
            resumed = true;
        }

        private void resumeMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (resumed)
            {
                paused = false;
                pauseMenu.Visibility = Visibility.Hidden;
                tickker2.Start();
                tickker3.Start();
            }
        }

        private void menuMouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMenued = true;
        }

        private void menuMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mainMenued)
            {
                mainMenued = false;
                paused = false;
                try
                {
                    tickker3.Tick -= new EventHandler(easyRocks);
                    tickker2.Tick -= new EventHandler(easyRocksFall);
                }
                catch { }
                try
                {
                    tickker3.Tick -= new EventHandler(hardRocks);
                    tickker2.Tick -= new EventHandler(hardRocksFall);
                }
                catch { }
                tickker2.Start();
                tickker.Stop();
                tickker3.Stop();
                for (int l = 0; l < hardRocksImgs.Count; l++)
                    gameGrid.Children.Remove(hardRocksImgs[l]);
                for (int l = 0; l < shots.Count; l++)
                    gameGrid.Children.Remove(shots[l]);
                hardRocksImgs.Clear();
                for (int l = 0; l < easyRocksImgs.Count; l++)
                    gameGrid.Children.Remove(easyRocksImgs[l]);
                easyRocksImgs.Clear();
                shots.Clear();
                gameGrid.Visibility = Visibility.Hidden;
                wlcomeGrid.Visibility = Visibility.Visible;
                pauseMenu.Visibility = Visibility.Hidden;
            }
        }
    }
}
