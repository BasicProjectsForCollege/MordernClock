
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Reflection;

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

        void Oscilation(UserControl user, int row)
        {
            int count = 0;
            TranslateTransform translateTransform = new TranslateTransform();
            user.RenderTransform = translateTransform;

            double up = Math.Sin(count * (Math.PI / ( row)));
            double from = up>0 ? 0 : offset * row;
            double to = up<0 ? -offset * row :0;

            DoubleAnimation animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
                
            
            translateTransform.BeginAnimation(TranslateTransform.YProperty, animation);
            count++;

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
                else
                {
                    columns[i] = new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
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

        int count = 2;
        

        void OnMainGrid()
        {
            UserControl grid0 = GenGrid(0,3);
            Grid.SetColumn(grid0, 0);
            Main.Children.Add(grid0);
            Oscilation(grid0, 3);

            UserControl grid1 = GenGrid(1, 6);
            grid1.Margin = new Thickness(0, 0, 0, offset);
            Grid.SetColumn(grid1, 1);
            Main.Children.Add(grid1);

            UserControl grid3 = GenGrid(1, 6);
            grid3.Margin = new Thickness(0, 0, 0, offset);
            Grid.SetColumn(grid3, 3);
            Main.Children.Add(grid3);

            UserControl grid4 = GenGrid(2, 10);
            grid4.Margin = new Thickness(0, 0, 0, offset);
            Grid.SetColumn(grid4, 4);
            Main.Children.Add(grid4);

            UserControl grid6 = GenGrid(1, 6);
            grid6.Margin = new Thickness(0, 0, 0, offset);
            Grid.SetColumn(grid6, 6);
            Main.Children.Add(grid6);

            UserControl grid7 = GenGrid(2, 10);
            grid7.Margin = new Thickness(0, 0, 0, offset);
            Grid.SetColumn(grid7, 7);
            Main.Children.Add(grid7);

        }

    }
}
