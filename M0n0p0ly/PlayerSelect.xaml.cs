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
    /// Interaction logic for PlayerSelect.xaml
    /// </summary>
    public partial class PlayerSelect : Window {
        public PlayerSelect() {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            int player1IconNumber = 1;
            int player2IconNumber = 2;
            int player3IconNumber = 3;
            int player4IconNumber = 4;

            if ((bool)rbCar1.IsChecked) {
                player1IconNumber = 0;
            } else if((bool)rbDog1.IsChecked) {
                player1IconNumber = 1;
            } else if ((bool)rbHat1.IsChecked) {
                player1IconNumber = 2;
            } else if ((bool)rbIron1.IsChecked) {
                player1IconNumber = 3;
            } else if ((bool)rbShip1.IsChecked) {
                player1IconNumber = 4;
            } else if ((bool)rbShoe1.IsChecked) {
                player1IconNumber = 5;
            } else if ((bool)rbThimble1.IsChecked) {
                player1IconNumber = 6;
            } else if ((bool)rbWheelbarrow1.IsChecked) {
                player1IconNumber = 7;
            }

            if ((bool)rbCar2.IsChecked) {
                player2IconNumber = 0;
            } else if ((bool)rbDog2.IsChecked) {
                player2IconNumber = 1;
            } else if ((bool)rbHat2.IsChecked) {
                player2IconNumber = 2;
            } else if ((bool)rbIron2.IsChecked) {
                player2IconNumber = 3;
            } else if ((bool)rbShip2.IsChecked) {
                player2IconNumber = 4;
            } else if ((bool)rbShoe2.IsChecked) {
                player2IconNumber = 5;
            } else if ((bool)rbThimble2.IsChecked) {
                player2IconNumber = 6;
            } else if ((bool)rbWheelbarrow2.IsChecked) {
                player2IconNumber = 7;
            }

            if ((bool)rbCar3.IsChecked) {
                player3IconNumber = 0;
            } else if ((bool)rbDog3.IsChecked) {
                player3IconNumber = 1;
            } else if ((bool)rbHat3.IsChecked) {
                player3IconNumber = 2;
            } else if ((bool)rbIron3.IsChecked) {
                player3IconNumber = 3;
            } else if ((bool)rbShip3.IsChecked) {
                player3IconNumber = 4;
            } else if ((bool)rbShoe3.IsChecked) {
                player3IconNumber = 5;
            } else if ((bool)rbThimble3.IsChecked) {
                player3IconNumber = 6;
            } else if ((bool)rbWheelbarrow3.IsChecked) {
                player3IconNumber = 7;
            }

            if ((bool)rbCar4.IsChecked) {
                player4IconNumber = 0;
            } else if ((bool)rbDog4.IsChecked) {
                player4IconNumber = 1;
            } else if ((bool)rbHat4.IsChecked) {
                player4IconNumber = 2;
            } else if ((bool)rbIron4.IsChecked) {
                player4IconNumber = 3;
            } else if ((bool)rbShip4.IsChecked) {
                player4IconNumber = 4;
            } else if ((bool)rbShoe4.IsChecked) {
                player4IconNumber = 5;
            } else if ((bool)rbThimble4.IsChecked) {
                player4IconNumber = 6;
            } else if ((bool)rbWheelbarrow4.IsChecked) {
                player4IconNumber = 7;
            }

            if (txtBoxName1.Text == "" || txtBoxName2.Text == "") {
                MessageBox.Show("Please enter names for player 1 and player 2.");
            } else {
                if (player1IconNumber == player2IconNumber || player1IconNumber == player3IconNumber ||
                    player1IconNumber == player4IconNumber || player2IconNumber == player3IconNumber ||
                    player2IconNumber == player4IconNumber || player3IconNumber == player4IconNumber) {
                    MessageBox.Show("Please select icons that are different from each other.");
                } else {
                    GameLoop.getInstance().Gameboard.AddPlayer(new Player(txtBoxName1.Text, player1IconNumber));
                    GameLoop.getInstance().Gameboard.AddPlayer(new Player(txtBoxName2.Text, player2IconNumber));
                    if (txtBoxName3.Text != "") {
                        GameLoop.getInstance().Gameboard.AddPlayer(new Player(txtBoxName3.Text, player3IconNumber));
                    }
                    if (txtBoxName4.Text != "") {
                        GameLoop.getInstance().Gameboard.AddPlayer(new Player(txtBoxName4.Text, player4IconNumber));
                    }
                    MainWindow main = new MainWindow();
                    main.Show();
                    Close();
                }
            }
        }
    }
}
