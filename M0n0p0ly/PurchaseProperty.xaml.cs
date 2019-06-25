using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for PurchaseProperty.xaml
    /// </summary>
    public partial class PurchaseProperty : Window {
        public PurchaseProperty() {
            InitializeComponent();
        }
        
        /// <summary>
        /// The current player purchases the property
        /// </summary>
        private void btnYesBuy_Click(object sender, RoutedEventArgs e) {
            // Disable buttons
            btnYesBuy.IsEnabled = false;
            btnNoDoNotBuy.IsEnabled = false;
            
            // Get the current player, player location, and property that the player landed on
            Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];
            int playerCurrentLocation = currentPlayer.Location;
            Property property = (Property)GameLoop.getInstance().Gameboard.TileOrder[playerCurrentLocation];

            // Buy the property
            currentPlayer.CurrentPropertyAction = Player.PropertyAction.IsBuying;
            property.LocationAction(currentPlayer);

            //Close window
            Close();
        }

        /// <summary>
        /// The current player does not purchase the property
        /// </summary>
        private void btnNoDoNotBuy_Click(object sender, RoutedEventArgs e) {
            // Disables the buttons and closes the form
            btnYesBuy.IsEnabled = false;
            btnNoDoNotBuy.IsEnabled = false;
            
            // Close window
            Close();
        }
    }
}
