using System.Windows;
using Ak.Wpf.SnowFlakes;

namespace SnowFlakes.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //1. Default instance
            //Snowfall snow = new Snowfall(DrawingCanvas);
            //snow.Start();

            //2. SnowfallColor.Default
            //Snowfall snow = Snowfall.Create(DrawingCanvas, SnowfallColor.Default);
            //snow.Start();

            //3. SnowfallColor.White
            //Snowfall snow = Snowfall.Create(DrawingCanvas, SnowfallColor.White);
            //snow.Start();

            //4. SnowfallColor.Blue
            //Snowfall snow = Snowfall.Create(DrawingCanvas, SnowfallColor.Blue);
            //snow.Start();

            //4. SnowfallColor.Gray
            //Snowfall snow = Snowfall.Create(DrawingCanvas, SnowfallColor.Gray);
            //snow.Start();

            //5. SnowfallColor.Mix
            Snowfall snow = Snowfall.Create(DrawingCanvas, SnowfallColor.Mix);
            snow.Start();
        }
    }
}