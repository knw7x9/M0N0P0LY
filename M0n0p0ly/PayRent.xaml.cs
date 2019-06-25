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
    /// Interaction logic for PayRent.xaml
    /// </summary>
    public partial class PayRent : Window {
        public PayRent() {
            InitializeComponent();
        }

        /// <summary>
        /// The current player pays rent to the owner
        /// </summary>
        private void btnPayRent_Click(object sender, RoutedEventArgs e) {
            btnPayRent.IsEnabled = false;
            
            // Get the current player, player location, and property that the player landed on
            Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];
            int playerCurrentLocation = currentPlayer.Location;
            Property property = (Property)GameLoop.getInstance().Gameboard.TileOrder[playerCurrentLocation];

            // Pay Rent to the owner
            currentPlayer.CurrentPropertyAction = Player.PropertyAction.IsPayingRent;
            property.LocationAction(currentPlayer);

            // Close PayRent form
            Close();
        }
    }
}
