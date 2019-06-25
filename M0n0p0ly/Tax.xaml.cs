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
    /// Interaction logic for Tax.xaml
    /// </summary>
    public partial class Tax : Window {
        public Tax() {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) {
            // Get the current player, player location, and CardSpace that the player landed on
            Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];
            int playerCurrentLocation = currentPlayer.Location;
            FreeParking freeParking = (FreeParking)GameLoop.getInstance().Gameboard.TileOrder[playerCurrentLocation];

            // Carry out the specifications of the free parking
            freeParking.LocationAction(currentPlayer);
            this.Close();
        }
    }
}
