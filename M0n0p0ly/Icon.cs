using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace M0n0p0ly {
    public class Icon {
        #region Attributes
        private Image _PlayerIcon;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an icon object from a filepath
        /// </summary>
        /// <param name="source"></param>
        public Icon(string source) {
            PlayerIcon = new Image();
            PlayerIcon.Width = 25;
            PlayerIcon.Height = 25;
            PlayerIcon.VerticalAlignment = VerticalAlignment.Top;
            PlayerIcon.HorizontalAlignment = HorizontalAlignment.Left;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(@source, UriKind.Relative);
            image.EndInit();
            PlayerIcon.Source = image;
            Grid.SetRow(PlayerIcon, 0);
            Grid.SetColumn(PlayerIcon, 0);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the player icon image
        /// </summary>
        public Image PlayerIcon {
            get { return _PlayerIcon; }
            set { _PlayerIcon = value; }
        }
        #endregion
    }
}
