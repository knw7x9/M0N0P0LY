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
    /// Interaction logic for EndGame.xaml
    /// </summary>
    public partial class EndGame : Window {
        public EndGame() {
            InitializeComponent();

            int index = 1;
            bool firstLoop = true;
            while (GameLoop.getInstance().Gameboard.Players.Count() > 0) {        // Loops untill all players have been ordered
                int maxMoney = -1000000;
                Player winner = null;
                foreach (Player p in GameLoop.getInstance().Gameboard.Players) {  // Loops through every player in the list
                    if (p.Money > maxMoney) {       // Gets player with the most money
                        maxMoney = p.Money;
                        winner = p;
                    }
                }
                GameLoop.getInstance().Gameboard.Players.Remove(winner);      // Removes player from the list
                if (firstLoop) {        // If this was the 1st loop, the winner was the game winner
                    tbWinner.Text = winner.Name + " was the winner with " + winner.Money.ToString("c0") + " after 20 turns." + Environment.NewLine + "Congratulations!!!";
                } else {            // If this was NOT the 1st loop, insert them into the general output string
                    tbOtherPlayers.Text += index + ". " + winner.Name.PadRight(9) + string.Format("{0:$#,##0}", winner.Money) + Environment.NewLine;
                }
                firstLoop = false;
                index++;
            }
        }

        /// <summary>
        /// Exits the window
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
