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
using System.Windows.Shapes;

namespace M0n0p0ly {
    /// <summary>
    /// Interaction logic for TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : Window {
        public TitleScreen() {
            InitializeComponent();
        }

        /// <summary>
        /// Start New Game
        /// </summary>
        private void BtnNewGame_Click(object sender, RoutedEventArgs e) {
            PlayerSelect ps = new PlayerSelect();
            ps.Show();
            Close();
        }

        /// <summary>
        /// Load Game
        /// </summary>
        private void BtnLoadGame_Click(object sender, RoutedEventArgs e) {
            if (GameLoop.getInstance().LoadGame()) {
                MainWindow main = new MainWindow();
                main.Show();
                Close();
            }
        }

        /// <summary>
        /// Exit Game
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
