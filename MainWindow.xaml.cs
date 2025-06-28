
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MordernClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        Color Color = Color.FromArgb(255, 51, 51, 51);
       
        TranslateTransform[] gridTransforms = new TranslateTransform[3];
        double[] currPositions = new double[3];
        int offset = 50;
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = this;
            GENERATEGRIDCOLUMNS();
            OnMainGrid();
          


        }

        TimeSpan time = new TimeSpan(0, 0, 0);
        void OnInput(object sender,KeyEventArgs e)
        {
            int grid_index=2;


            if (grid_index < 0 || grid_index > gridTransforms.Length) return;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            switch (e.Key)
            {
                case Key.W:
                    {
                        
                       animation.From = currPositions[grid_index];
                       currPositions[grid_index] += offset;
                       animation.To = currPositions[grid_index];

                       gridTransforms[grid_index].BeginAnimation(TranslateTransform.YProperty, animation);
                            
                       
                        break;
                    }
                case Key.S:
                    {
                        
                        
                        animation.From = currPositions[grid_index];
                        currPositions[grid_index] -= offset;
                        animation.To = currPositions[grid_index];
                        gridTransforms[grid_index].BeginAnimation(TranslateTransform.YProperty, animation);
                        break;
                    }

            }
            
        }
        IEasingFunction ease = new MordernClock.EasingFunction
        {
            EasingMode = EasingMode.EaseInOut
        };
        
        
       
        void StartCycle(Dictionary<int,UserControl> Col)
        {
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime local;
            timer.Tick += (s, e) =>
            {
                
                local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
                string Time_ = local.ToString("HHmmss");
                Debug.WriteLine(Time_);
                
               foreach(var pair in Col) 
               {
                    UserControl g = pair.Value;
                    int rowCount = pair.Key;
                    int digit = Time_[rowCount-1] - '0';
                    AnimateGrid(g, digit);
                    
               }
                

            };
            timer.Start();
        }


        void AnimateGrid(UserControl grid,int digits)
        {

            if (!(grid.RenderTransform is TranslateTransform tt))
            {
                tt = new TranslateTransform();
                grid.RenderTransform = tt;
            }
           

            DoubleAnimation anima = new DoubleAnimation()
            {
                To= -(digits*offset),
                Duration = TimeSpan.FromSeconds(0.4),
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            tt.BeginAnimation(TranslateTransform.YProperty, anima);
        }

        Grid[] Grids = new Grid[3];
        Grid[] ParentGrids = new Grid[3];
        ColumnDefinition[] columns = new ColumnDefinition[8];

        void GENERATEGRIDCOLUMNS()
        {
            for (int i =0; i<8; i++)
            {
                if ((i+1) % 3 != 0)
                {
                    columns[i] = new ColumnDefinition
                    {
                        Width = new GridLength(0.5, GridUnitType.Star)
                    };
                    Main.ColumnDefinitions.Add(columns[i]);
                }
            }

        }

        UserControl GenGrid(int grid_index,int row)
        {
            Grids[grid_index] = new Grid
            {
                Width = 50,
                Height = offset * row,
                
            };
            ParentGrids[grid_index] = new Grid();

            Border OuterBorder = new Border
            {
                Width = 50,
                
                CornerRadius = new CornerRadius(10),
            };

            Border InnerBorder = new Border
            {
                Width = 50,
                Style = (Style)FindResource("ShadowBox"),
                CornerRadius = new CornerRadius(10),
            };

            Border Emmission = new Border
            {
                Width = 50,
                Style = (Style)FindResource("Emission"),
                CornerRadius = new CornerRadius(10),
            };
            Emmission.Effect = new DropShadowEffect
            {
                Color = Colors.White,
                ShadowDepth = 5,
                BlurRadius = 8,
                Opacity = 0.8,
                Direction = 145,
            };

            Rectangle rect = new Rectangle
            {
                Fill = new SolidColorBrush(Color),
                RadiusX = 10,
                RadiusY = 10,
            };

            Emmission.Child = OuterBorder;
            OuterBorder.Child = InnerBorder;
            InnerBorder.Child = rect;

            
            rect.Width = Grids[grid_index].Width;
            rect.Height = Grids[grid_index].Height;


            for (int i =0; i<row; i++)
            {
                Grids[grid_index].RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });

                TextBlock textBlock = new TextBlock
                {
                    Text = $"{i}",
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = Brushes.White,
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Wheat,
                        ShadowDepth = 1,
                        BlurRadius = 2,
                        Opacity = 0.5,
                        Direction = 0,
                    }

                };
                Grid.SetRow(textBlock, i);
                Grids[grid_index].Children.Add(textBlock);

            }
            ParentGrids[grid_index].Children.Add(Emmission);
            ParentGrids[grid_index].Children.Add(Grids[grid_index]);
           
            TranslateTransform transform = new TranslateTransform();
            ParentGrids[grid_index].RenderTransform = transform;
            gridTransforms[grid_index] = transform;

            UserControl wrapper = new UserControl
            {
                Content = ParentGrids[grid_index],
                Width = ParentGrids[grid_index].Width,
                Height = ParentGrids[grid_index].Height,
            };

            return wrapper;
        }

        

        void SetRegion(string Region)
        {
            Grid WinPopup = new Grid() {
                Background = new SolidColorBrush(Color.FromArgb(255, 31, 31, 31)),
                Height = 500,
                Width = 500,
            };

            Window popup = new Window()
            {
                Background = new SolidColorBrush(Color.FromArgb(255,31,31,31)),
                Height= 500,
                Width=500,
            };
            


        }
        

        void OnMainGrid()
        {
            Dictionary<int, UserControl> GridCol = new Dictionary<int, UserControl>();

            //string HR = "", MIN = "", SEC = "";
            int c = 0;
            UserControl grid0 = GenGrid(0,3);
            grid0.Margin = new Thickness(100, 100, 0, 0);
            Grid.SetColumn(grid0, 0);
            Main.Children.Add(grid0);
            GridCol.Add(1,grid0);

            UserControl grid1 = GenGrid(2, 10);
            grid1.Margin = new Thickness(0, offset*9, 0, 0);
            Grid.SetColumn(grid1, 1);
            Main.Children.Add(grid1);
            GridCol.Add(2, grid1);

            UserControl grid2 = GenGrid(1, 6);
            grid2.Margin = new Thickness(0, offset*5, 0, 0);
            Grid.SetColumn(grid2, 2);
            Main.Children.Add(grid2);
            GridCol.Add(3, grid2);


            UserControl grid3 = GenGrid(2, 10);
            grid3.Margin = new Thickness(0, offset*9, 0, 0);
            Grid.SetColumn(grid3, 3);
            Main.Children.Add(grid3);
            GridCol.Add(4, grid3);

            UserControl grid4 = GenGrid(1, 6);
            grid4.Margin = new Thickness(0, offset*5, 0, 0);
            Grid.SetColumn(grid4, 4);
            Main.Children.Add(grid4);
            GridCol.Add(5, grid4);

            UserControl grid5 = GenGrid(2, 10);
            grid5.Margin = new Thickness(0, offset*9, 100, 0);
            Grid.SetColumn(grid5, 5);
            Main.Children.Add(grid5);
            GridCol.Add(6, grid5);
            
            StartCycle(GridCol);

        }

    }
}
