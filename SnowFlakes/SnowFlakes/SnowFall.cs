using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ak.Wpf.SnowFlakes
{
    /// <summary>
    /// Snowfall
    /// </summary>
    public class Snowfall
    {
        #region Variables

        /// <summary>
        /// Canvas to draw flakes in
        /// </summary>
        private readonly Canvas _canvas = null;

        /// <summary>
        /// Maximum flakes. It's calculating at the beginning and after canvas resize and depends on SnowCoverage property
        /// </summary>
        private int _maxFlakesQuantity = 0;

        /// <summary>
        /// Flakes
        /// </summary>
        private readonly List<Snowflake> _flakes = new List<Snowflake>();

        /// <summary>
        /// Flakes images
        /// </summary>
        private readonly List<string> _flakesImages = new List<string>();

        /// <summary>
        /// The minimum starting speed
        /// </summary>
        private int _minStartingSpeed = 3;

        /// <summary>
        /// The maximum starting speed
        /// </summary>
        private int _maxStartingSpeed = 10;

        /// <summary>
        /// The minimum horizontal speed
        /// </summary>
        private int _minHorizontalSpeed = 1;

        /// <summary>
        /// The maximum horizontal speed
        /// </summary>
        private int _maxHorizontalSpeed = 3;

        /// <summary>
        /// Snow coverage
        /// </summary>
        private ushort _snowCoverage = 25;

        #endregion

        #region Properties

        /// <summary>
        /// Minimum radius of flake
        /// </summary>
        public int MinRadius { get; set; } = 5;

        /// <summary>
        /// Maximum radius of flake
        /// </summary>
        public int MaxRadius { get; set; } = 30;
        
        /// <summary>
        /// Average snow coverage in percent
        /// </summary>
        public ushort SnowCoverage
        {
            get => _snowCoverage;
            set
            {
                if (value > 100 || value < 1)
                    throw new ArgumentOutOfRangeException("value", "Maximum coverage 100 and minumum 1");
                _snowCoverage = value;
            }
        }

        /// <summary>
        /// Vertical speed ratio of snow
        /// </summary>
        public double VerticalSpeedRatio { get; set; } = 0.1;

        /// <summary>
        /// Horizontal speed ratio of snow
        /// </summary>
        public double HorizontalSpeedRatio { get; set; } = 0.08;

        /// <summary>
        /// True if snow is falling, false if not
        /// </summary>
        public bool IsWorking { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">Canvas where snow falls</param>
        public Snowfall(Canvas canvas) : this(canvas,   "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake1.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake2.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake3.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake4.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake5.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake6.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake7.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake8.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake9.png",
                                                        "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake10.png")
        {            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="canvas">Canvas where snow falls</param>
        /// <param name="flakesImages">Flakes images</param>
        public Snowfall(Canvas canvas, params string[] flakesImages)
        {
            if (canvas == null)
                throw new ArgumentNullException("canvas", "Canvas can't be null");
            
            if (flakesImages == null || flakesImages.Length == 0)
                throw new ArgumentException("Flakes images can't be empty", "flakesImages");            

            _canvas = canvas;
            canvas.IsHitTestVisible = false;
            canvas.SizeChanged += Canvas_SizeChanged;
            _flakesImages.AddRange(flakesImages);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates new snowfall
        /// </summary>
        /// <param name="canvas">Canvas where snow falls</param>
        /// <param name="color">Snowfall color</param>
        /// <returns>New snowfall</returns>
        public static Snowfall Create(Canvas canvas, SnowfallColor color)
        {
            switch (color)
            {
                case SnowfallColor.Default:
                    return new Snowfall(canvas);

                case SnowfallColor.Blue:
                    return new Snowfall(canvas, "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake10.png");

                case SnowfallColor.White:
                    return new Snowfall(canvas, "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake10.png");

                case SnowfallColor.Gray:
                    return new Snowfall(canvas, "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake10.png");


                case SnowfallColor.Mix:
                    return new Snowfall(canvas, "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesBlue/snowflake10.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesWhite/snowflake10.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake1.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake2.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake3.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake4.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake5.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake6.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake7.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake8.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake9.png",
                                                "pack://application:,,,/SnowFlakes;component/SnowflakesGray/snowflake10.png");

                default:
                    throw new ArgumentException(nameof(color));
            }
        }

        /// <summary>
        /// Start snowfall
        /// </summary>
        public void Start()
        {
            IsWorking = true;
            RecalculateMaxFlakes();
            SetFlakes(true);
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        /// <summary>
        /// Stop snowfall
        /// </summary>
        public void Stop()
        {
            IsWorking = false;
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            ClearSnow();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Calculation of flakes number when canvas size changes
        /// </summary>
        private void RecalculateMaxFlakes()
        {
            // Approximate maximum flakes in canvas
            double flakesInCanvas = _canvas.ActualHeight * _canvas.ActualWidth / (MaxRadius * MaxRadius);

            _maxFlakesQuantity = (int)(flakesInCanvas * SnowCoverage / 100);
        }

        /// <summary>
        /// Create image from resource of file
        /// </summary>
        /// <param name="path">Path to image resource</param>
        /// <returns>Wpf-ready image</returns>
        private static BitmapImage CreateImage(string path)
        {
            BitmapImage imgTemp = new BitmapImage();

            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path can't be empty", "path");

            imgTemp.BeginInit();
            imgTemp.CacheOption = BitmapCacheOption.OnLoad;
            imgTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            imgTemp.UriSource = new Uri(path);
            imgTemp.EndInit();
            if (imgTemp.CanFreeze)
                imgTemp.Freeze();

            return imgTemp;
        }

        /// <summary>
        /// Adds missing flakes on canvas
        /// </summary>
        /// <param name="top">true if flakes appear on top of canvas, false if random Y position</param>
        private void SetFlakes(bool top = false)
        {
            int halfCanvasWidth = (int)_canvas.ActualWidth / 2;
            Random rand = new Random();

            for (int i = _flakes.Count; i < _maxFlakesQuantity; i++)
            {
                var flake = new Image
                {
                    Source = CreateImage(_flakesImages[rand.Next(0, _flakesImages.Count)]),
                    Stretch = Stretch.Uniform
                };

                Snowflake snowflake = new Snowflake(flake, VerticalSpeedRatio * rand.Next(_minStartingSpeed, _maxStartingSpeed), rand.Next(MinRadius, MaxRadius));


                // Placing image  
                Canvas.SetLeft(flake, halfCanvasWidth + rand.Next(-halfCanvasWidth, halfCanvasWidth));
                if (!top)
                    Canvas.SetTop(flake, rand.Next(0, (int)_canvas.ActualHeight));
                else
                    Canvas.SetTop(flake, -snowflake.Radius * 2);

                _canvas.Children.Add(flake);

                snowflake.VelocityX = rand.Next(_minHorizontalSpeed, _maxHorizontalSpeed);
                _flakes.Add(snowflake);
            }
        }

        /// <summary>
        /// Clears snow when stop
        /// </summary>
        private void ClearSnow()
        {
            for (int i = _flakes.Count - 1; i >= 0; i--)
            {
                _canvas.Children.Remove(_flakes[i].Flake);
                _flakes[i].Flake = null;
                _flakes.RemoveAt(i);
            }
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the SizeChanged event of the Canvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void Canvas_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            RecalculateMaxFlakes();
            SetFlakes(true);
        }

        /// <summary>
        /// Render snow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!IsWorking)
            {
                return;
            }

            // Add missing flakes
            if (_flakes.Count < _maxFlakesQuantity)
            {
                SetFlakes(true);
                return;
            }

            // Setting position of all flakes
            for (int i = _flakes.Count - 1; i >= 0; i--)
            {
                Snowflake snowflake = _flakes[i];
                var left = Canvas.GetLeft(snowflake.Flake);
                var top = Canvas.GetTop(snowflake.Flake);

                _flakes[i].VelocityX += 0.5 * HorizontalSpeedRatio;

                Canvas.SetLeft(_flakes[i].Flake, left + Math.Cos(_flakes[i].VelocityX));
                Canvas.SetTop(snowflake.Flake, top + 1 * snowflake.VelocityY);

                // Remove image from canvas when it leaves canvas
                if (top >= (_canvas.ActualHeight + snowflake.Radius * 2))
                {
                    _flakes.Remove(snowflake);
                    _canvas.Children.Remove(snowflake.Flake);
                }
            }
        }

        #endregion
    }
}
