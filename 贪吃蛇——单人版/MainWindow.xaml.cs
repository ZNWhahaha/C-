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
using System.Windows.Threading;


namespace 贪吃蛇__单人版
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        //创建泛型Snake，存储蛇的身体
        List<Border> Snake = new List<Border>();

        //创建泛型Snake_local,存储蛇的每截身体的位置
        List<Border> Snake_local = new List<Border>();

        //创建计时器Btime
        DispatcherTimer Btime = new DispatcherTimer();

        //创建随机数
        Random rd = new Random();

        //创建泛型M_food，存储创建出食物的位置
        List<Border> M_food = new List<Border>();

        //创建全局变量表示果实
        Border fruit;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //创建位置
            this.Top = 0;
            this.Left = 0;
            this.Background = Brushes.GhostWhite;

            //创建幕布大小,颜色，默认为全屏
            this.Width = SystemParameters.FullPrimaryScreenWidth;
            this.Height = SystemParameters.FullPrimaryScreenHeight;
            back.Background = Brushes.Black;
            back.Width = 800;
            back.Height = 600;

            //创建蛇的身体，方向，位置，颜色
            for (int i = 0; i < 5; i++)
            {
                Border snake_body = new Border();
                snake_body.BorderThickness = new Thickness(2);
                snake_body.BorderBrush = Brushes.Yellow;
                if (i == 0)
                {
                    snake_body.Background = Brushes.Blue;        //对蛇头蛇身颜色进行分类
                }
                else
                {
                    snake_body.Background = Brushes.Yellow;
                }
                snake_body.Width = snake_body.Height = 20;
                snake_body.CornerRadius = new CornerRadius(20);     //使方块变圆
                Canvas.SetLeft(snake_body, 660);
                Canvas.SetTop(snake_body, 260 + i * snake_body.Height);
                back.Children.Add(snake_body);
                Snake.Add(snake_body);

                //利用泛型Snake_locol,存储蛇的位置
                Border local = new Border();
                Canvas.SetLeft(local, 660);
                Canvas.SetTop(local, 260 + i * snake_body.Height);
                Snake_local.Add(local);
            }
            //创建果实
            fruit = new Border();
            fruit.Width = fruit.Height = 20;
            fruit.Background = Brushes.Green;
            Canvas.SetTop(fruit, rd.Next(30) * 20);
            Canvas.SetLeft(fruit, rd.Next(40) * 20);
            back.Children.Add(fruit);




            //添加键盘反馈控制
            this.KeyDown += MainWindow_KeyDown;

            //添加计时器，控制事件的进行
            Btime.Interval = TimeSpan.FromMilliseconds(50);        //事件进行速度
            Btime.Tick += Btime_Tick;
            Btime.Start();
        }

        private void Btime_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //根据键盘监听返回得数值，采取不同的行动
            if (H == "Up")
            {
                Canvas.SetTop(Snake[0], Canvas.GetTop(Snake[0]) - Snake[0].Height);
                Move();
            }
            else if (H == "Left")
            {
                Canvas.SetLeft(Snake[0], Canvas.GetLeft(Snake[0]) - Snake[0].Width);
                Move();
            }
            else if (H == "Down")
            {
                Canvas.SetTop(Snake[0], Canvas.GetTop(Snake[0]) + Snake[0].Height);
                Move();
            }
            else if (H == "Right")
            {
                Canvas.SetLeft(Snake[0], Canvas.GetLeft(Snake[0]) + Snake[0].Width);
                Move();
            }

            //判断蛇头是否碰触到果实，碰触到则蛇增加一个长度，并且果实改变位置
            if (Canvas.GetTop(Snake[0]) == Canvas.GetTop(fruit) && Canvas.GetLeft(Snake[0]) == Canvas.GetLeft(fruit))
            {
                //改变果实位置
                Canvas.SetTop(fruit, rd.Next(30) * 20);
                Canvas.SetLeft(fruit, rd.Next(40) * 20);

                //增加蛇的身体
                    Border bd = new Border();
                bd.Width = bd.Height = 20;
                bd.BorderThickness = new Thickness(2);
                bd.BorderBrush = Brushes.Yellow;
                bd.CornerRadius = new CornerRadius(10);
                bd.Background = Brushes.Red;
                Canvas.SetTop(bd, Canvas.GetTop(Snake_local[Snake.Count - 1]));
                Canvas.SetLeft(bd, Canvas.GetLeft(Snake_local[Snake.Count - 1]));
                back.Children.Add(bd);
                Snake.Add(bd);

                //将坐标存储到snake_local中
                Border bs = new Border();
                Canvas.SetTop(bs, Canvas.GetTop(bd));
                Canvas.SetLeft(bs, Canvas.GetLeft(bd));
                Snake_local.Add(bs);
            }

            //判断蛇是否碰触到边框
                if (Canvas.GetTop(Snake[0]) < 0 || (Canvas.GetTop(Snake[0]) + Snake[0].Height) > 600
                            || Canvas.GetLeft(Snake[0]) < 0 || (Canvas.GetLeft(Snake[0]) + Snake[0].Width) > 800)
            {
                Btime.Stop();
                MessageBox.Show("GAME OVER");
            }
        }

        //构造方法，进行蛇的移动操作
        public void Move()
        {
            for (int i = 1; i < Snake.Count; i++)   // 第i个蛇身为他的i-1的蛇身的地址
            {
                Canvas.SetTop(Snake[i], Canvas.GetTop(Snake_local[i - 1]));
                Canvas.SetLeft(Snake[i], Canvas.GetLeft(Snake_local[i - 1]));
            }

            for (int i = 0; i < Snake.Count; i++)        //进行坐标更新
            {
                Canvas.SetTop(Snake_local[i], Canvas.GetTop(Snake[i]));
                Canvas.SetLeft(Snake_local[i], Canvas.GetLeft(Snake[i]));
            }
        }

        //创建全局变量，存储键盘反馈所给的信息
        string H = "";

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //throw new NotImplementedException();

            //通过键盘监听，获取不同的指令，来判断蛇走的方向
            if (e.Key == Key.Up && H != "Down")
            {
                H = "Up";
            }
            else if (e.Key == Key.Left && H != "Right")
            {
                H = "Left";
            }
            else if (e.Key == Key.Down && H != "Up")
            {
                H = "Down";
            }
            else if (e.Key == Key.Right && H != "Left")
            {
                H = "Right";
            }
        }

    }
}
