using System.Windows.Controls;

namespace Ak.Wpf.SnowFlakes
{
    /// <summary>
    /// SnowFlake
    /// </summary>
    internal class Snowflake
    {
        #region Properties

        /// <summary>
        /// Snowflake image
        /// </summary>
        public Image Flake { get; set; }

        /// <summary>
        /// Velocity at X-axis
        /// </summary>
        public double VelocityX { get; set; }

        /// <summary>
        /// Velocity at Y-axis
        /// </summary>
        public double VelocityY { get; set; }

        /// <summary>
        /// Radius of the flake
        /// </summary>
        public int Radius { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Snowflake"/> class.
        /// </summary>
        /// <param name="flake">The flake.</param>
        /// <param name="velocityY">The velocity at y-axis.</param>
        /// <param name="radius">The radius.</param>
        public Snowflake(Image flake, double velocityY, int radius)
        {
            VelocityY = velocityY;
            Flake = flake;
            flake.Width = radius;
            Radius = radius;
        }

        #endregion        
    }
}
