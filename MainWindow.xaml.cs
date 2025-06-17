
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
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = this;
            GENERATEGRIDCOLUMNS();
            
            

        }
        
        TimeSpan time = new TimeSpan(0, 0, 0);
        void OnInput(object sender,KeyEventArgs e)
        {
            int grid_index=2;


            if (grid_index < 0 || grid_index > gridTransforms.Length) return;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = currPositions[grid_index];
            animation.To = currPositions[grid_index];
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            switch (e.Key)
            {
                case Key.W:
                    {
                        if (grid_index <= 2)
                        {
                            animation.From = currPositions[grid_index];
                            currPositions[grid_index] += 50;
                            animation.To = currPositions[grid_index];
                            
                            gridTransforms[grid_index].BeginAnimation(TranslateTransform.YProperty, animation);
                            break;
                        }
                        else
                        {
                            animation.From = currPositions[grid_index];
                            currPositions[grid_index] += 15;
                            animation.To = currPositions[grid_index];
                            
                            gridTransforms[grid_index].BeginAnimation(TranslateTransform.YProperty, animation);
                            break;
                        }
                        }
                case Key.S:
                    {
                        
                        
                        animation.From = currPositions[grid_index];
                        currPositions[grid_index] -= 50;
                        animation.To = currPositions[grid_index];
                        gridTransforms[grid_index].BeginAnimation(TranslateTransform.YProperty, animation);
                        break;
                    }

            }
            
        }
        Grid[] Grids = new Grid[3];
        Grid[] ParentGrids = new Grid[8];
        ColumnDefinition[] columns = new ColumnDefinition[8];
        
        void GENERATEGRIDCOLUMNS()
        {

            //Generates 3 grid 1:0-2 2:0-5 3:0-9 (i)
            for (int i = 1; i <= 8; i++)
            {

                ParentGrids[i - 1] = new Grid();

                columns[i-1] = new ColumnDefinition();
                if (i % 3 == 0)
                {
                    columns[i - 1].Width = new GridLength(0.6f, GridUnitType.Star);
                    Main.ColumnDefinitions.Add(columns[i - 1]);
                }
                else
                {

                    columns[i - 1].Width = new GridLength(1f, GridUnitType.Star);
                    Main.ColumnDefinitions.Add(columns[i - 1]);
                }


                if (i <= 3)
                {
                    Grids[i-1] = new Grid();

                    Border OuterBorder = new Border
                    {
                        Height = 150 * i,
                        Width = 50,
                        //Style = (Style)FindResource("ShadowBox"),
                        CornerRadius = new CornerRadius(10),
                    };

                    Border InnerBorder = new Border
                    {
                        Height = 150 * i,
                        Width = 50,
                        Style = (Style)FindResource("ShadowBox"),
                        CornerRadius = new CornerRadius(10),
                    };

                    Border Emmission = new Border
                    {
                        Height = 150 * i,
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
                        Direction=145,
                    };

                    Rectangle rect = new Rectangle
                    {
                        Height = 150 * i,
                        Fill = new SolidColorBrush(Color),
                        RadiusX = 10,
                        RadiusY = 10
                    };
                    
                    Emmission.Child = OuterBorder;
                    OuterBorder.Child = InnerBorder;
                    InnerBorder.Child = rect;
                    

                    rect.Width = Grids[i - 1].Width;
                    rect.Height = Grids[i - 1].Height;
                    for (int j = 1; j<=i*3; j++)
                    {
                        Grids[i-1].Height = 150*i;
                        Grids[i - 1].Width = 50;

                        if (i != 3)
                        {
                            Grids[i - 1].RowDefinitions.Add(new RowDefinition
                            {
                                Height = new GridLength(1f, GridUnitType.Star)
                            });
                            TextBlock text = new TextBlock
                            {
                                Text = $"{j - 1}",
                                Foreground = Brushes.White,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };
                            text.Effect = new DropShadowEffect {
                                Color = Colors.White,
                                ShadowDepth = 1,
                                BlurRadius = 20,
                                Opacity = 1,
                            };



                            Grid.SetRow(text, j-1);
                            Grids[i-1].Children.Add(text);
                           

                        }
                        else
                        {
                            for (int k = 0; k <= 9; k++)
                            {
                                Grids[i - 1].RowDefinitions.Add(new RowDefinition
                                {
                                    Height = new GridLength(1f, GridUnitType.Star)
                                });
                                TextBlock text = new TextBlock
                                {
                                    Text = $"{k}",
                                    Foreground = Brushes.White,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Center
                                };
                                text.Effect = new DropShadowEffect
                                {
                                    Color = Colors.White,
                                    ShadowDepth = 1,
                                    BlurRadius = 20,
                                    Opacity = 1,
                                    
                                };

                                Grid.SetRow(text, k);
                                Grids[i - 1].Children.Add(text);
                            }
                            //ParentGrids[i - 1].Children.Add(Grids[i - 1]);
                            break;
                        }

                    }
                    ParentGrids[i - 1].Children.Add(Emmission);
                    ParentGrids[i - 1].Height = 150 * i;
                    ParentGrids[i - 1].Width = 50;
                   
                    ParentGrids[i - 1].Children.Add(Grids[i - 1]);
                    if (i == 2 || i ==3)
                    {
                        ParentGrids[i - 1].Margin = new Thickness(0, 0, 0,50);
                    }
                    TranslateTransform indivisualTransform = new TranslateTransform();
                    gridTransforms[i - 1] = indivisualTransform;
                    currPositions[i - 1] = 0;
                    ParentGrids[i - 1].RenderTransform = indivisualTransform;

                }

                


            }
            


           
            for (int i =0; i <3; i++)
            {

                Grid.SetColumn(ParentGrids[i], i);
                Main.Children.Add(ParentGrids[i]);
            }


           

            }


        

    }

    }
