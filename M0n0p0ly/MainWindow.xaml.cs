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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace M0n0p0ly {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            // Initializes avaliable player icons and Community/Chance Cards
            GameLoop.getInstance().initilizeIconList();
            GameLoop.getInstance().Gameboard.ReadInFiles();
            GameLoop.getInstance().Gameboard.InitializeCards();

            // Testing: Changes starting turn count
            //GameLoop.getInstance().Gameboard.TurnCount = 20;

            if (GameLoop.getInstance().Gameboard.Players.Count > 0) {
                // Displays first player's initial position and all players' icon's initial positions
                UpdatePlayerIconPosition(GameLoop.getInstance().Gameboard.Players[0]);

                // Displays first player's name and the initial turn count
                DisplayPlayerName(GameLoop.getInstance().Gameboard.Players[0]);
                DisplayPlayerIcon(GameLoop.getInstance().Gameboard.Players[0]);
                UpdateBoard(GameLoop.getInstance().Gameboard.Players[0]);
                DisplayTurnCounter();

                // Adds each player's icon to the Grid
                foreach (Player p in GameLoop.getInstance().Gameboard.Players) {
                    gridBoard.Children.Remove(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
                    gridBoard.Children.Add(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
                }
                loadGameUpdateDisplay();
            }
        }

        #region Button events
        /// <summary>
        /// Rolls dice - effectively, takes a turn for the player.
        /// </summary>
        private void BtnRoll_Click(object sender, RoutedEventArgs e) {
            Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];      // Gets current player
            // Stops the button from being repeatedly clicked while a turn is already taking place.
            btnRoll.IsEnabled = false;
            TakeTurn(currentPlayer);    // Current Player takes their turn
        }

        /// <summary>
        /// Ends the player's turn.
        /// </summary>
        private void BtnEndTurn_Click(object sender, RoutedEventArgs e) {
            // Advances player
            GameLoop.getInstance().Gameboard.CurrentPlayerIndex++;
            if (GameLoop.getInstance().Gameboard.CurrentPlayerIndex >= GameLoop.getInstance().Gameboard.Players.Count()) {
                GameLoop.getInstance().Gameboard.CurrentPlayerIndex = 0;      // Loops to the beginning of the player list
                GameLoop.getInstance().Gameboard.TurnCount++;
            }
            UpdateBoard(GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex]);
            // If turn limit is reached...   (must be ">" so every player gets their final turn)
            if (GameLoop.getInstance().Gameboard.TurnCount > 20) {
                EndGame end = new EndGame();
                end.Owner = this;
                end.ShowDialog();
                Application.Current.Shutdown();     // Closes Game
            } else {
                Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];

                GameLoop.getInstance().Gameboard.TurnIsReadyToEnd = false;

                // Updates the player display data to show the next player's data, as of the start of their turn.
                btnEndTurn.IsEnabled = GameLoop.getInstance().Gameboard.TurnIsReadyToEnd;
                DisplayPlayerName(currentPlayer);
                DisplayPlayerIcon(currentPlayer);
                DisplayTurnCounter();
                lblRollResult.Content = "--   --";
                DisplayCurrentPlayerLocation(currentPlayer);
                DisplayCurrentPlayerMoney(currentPlayer);
                DisplayCurrentPlayerProperties(currentPlayer);
                btnRoll.IsEnabled = !GameLoop.getInstance().Gameboard.TurnIsReadyToEnd;
            }
        }

        /// <summary>
        /// Brings up the save file dialog
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            if (GameLoop.getInstance().SaveGame()) {
                MessageBox.Show("Game Saved!");
            } else {
                MessageBox.Show("Error: Game could not be saved to file.");
            }
        }

        /// <summary>
        /// Brings up the load file dialog
        /// </summary>
        private void BtnLoad_Click(object sender, RoutedEventArgs e) {
            foreach (Player p in GameLoop.getInstance().Gameboard.Players) {
                gridBoard.Children.Remove(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
            }
            if (GameLoop.getInstance().LoadGame()) {
                loadGameUpdateDisplay();

                MessageBox.Show("Game Loaded!");
            } else {
                foreach (Player p in GameLoop.getInstance().Gameboard.Players) {
                    gridBoard.Children.Add(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
                }
                MessageBox.Show("Error: Save file could not be loaded.");
            }
        }

        /// <summary>
        /// Brings up the load file dialog
        /// </summary>
        private void loadGameUpdateDisplay() {
            Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];
            // Updates current player data display
            DisplayPlayerName(currentPlayer);
            DisplayTurnCounter();
            lblRollResult.Content = "--   --";
            DisplayCurrentPlayerMoney(currentPlayer);
            DisplayCurrentPlayerProperties(currentPlayer);

            // Updates all players' icon display
            foreach (Player p in GameLoop.getInstance().Gameboard.Players) {
                gridBoard.Children.Remove(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
                gridBoard.Children.Add(GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon);
            }
            UpdateBoard(currentPlayer);
            UpdatePlayerIconPosition(currentPlayer);    // Current player location display also gets updated here
        }

        /// <summary>
        /// Exits the game
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        #endregion

        #region Display Player Info Methods
        /// <summary>
        /// Displays current player name.
        /// </summary>
        private void DisplayPlayerName(Player player) {
            lblPlayerName.Content = player.Name;
        }

        /// <summary>
        /// Displays the player icon.
        /// </summary>
        private void DisplayPlayerIcon(Player player) {
            imgPlayer.Source = GameLoop.getInstance().Icons[player.IconIndex].PlayerIcon.Source;
        }

        /// <summary>
        /// Displays what turn the game is on.
        /// </summary>
        private void DisplayTurnCounter() {
            lblTurnCount.Content = "Turn: " + GameLoop.getInstance().Gameboard.TurnCount + 
                " (" + (GameLoop.getInstance().Gameboard.CurrentPlayerIndex + 1) + "/" + 
                GameLoop.getInstance().Gameboard.Players.Count + ")";
        }

        /// <summary>
        /// Displays what dice were rolled.
        /// </summary>
        private void DisplayRolledDice() {
            lblRollResult.Content = GameLoop.getInstance().Gameboard.RolledDice[0] + "  " + 
                GameLoop.getInstance().Gameboard.RolledDice[1];
        }

        /// <summary>
        /// Displays current player's location.
        /// </summary>
        /// <param name="player">Current player</param>
        private void DisplayCurrentPlayerLocation(Player player) {
            lblLocationResult.Content = "(" + player.Location + ") " + GameLoop.getInstance().Gameboard.TileOrder[player.Location].Name;
        }

        /// <summary>
        /// Displays current player's money.
        /// </summary>
        /// <param name="player">Current player</param>
        private void DisplayCurrentPlayerMoney(Player player) {
            //Negative Number Currency Formatting: https://stackoverflow.com/questions/1001114/string-format0c2-1234-currency-format-treats-negative-numbers-as-posi
            lblMoneyResult.Content = string.Format("{0:$#,##0}", player.Money);
        }

        /// <summary>
        /// Displays current player's properties.
        /// </summary>
        /// <param name="player">Current player</param>
        private void DisplayCurrentPlayerProperties(Player player) {
            lbPropertiesOwned.Items.Clear();
            foreach (Property proprty in player.PropertiesOwned) {
                lbPropertiesOwned.Items.Add(proprty.Name);
            }
            
        }
        #endregion

        #region Display Board Info Methods
        private void UpdateBoard(Player player)
        {
            Property p1 = (Property)GameLoop.getInstance().Gameboard.TileOrder[1];
            Property p3 = (Property)GameLoop.getInstance().Gameboard.TileOrder[3];
            FreeParking p4 = (FreeParking)GameLoop.getInstance().Gameboard.TileOrder[4];
            Property p5 = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[5];
            Property p6 = (Property)GameLoop.getInstance().Gameboard.TileOrder[6];
            Property p8 = (Property)GameLoop.getInstance().Gameboard.TileOrder[8];
            Property p9 = (Property)GameLoop.getInstance().Gameboard.TileOrder[9];
            Property p11 = (Property)GameLoop.getInstance().Gameboard.TileOrder[11];
            Utility p12 = (Utility)GameLoop.getInstance().Gameboard.TileOrder[12];
            Property p13 = (Property)GameLoop.getInstance().Gameboard.TileOrder[13];
            Property p14 = (Property)GameLoop.getInstance().Gameboard.TileOrder[14];
            Property p15 = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[15];
            Property p16 = (Property)GameLoop.getInstance().Gameboard.TileOrder[16];
            Property p18 = (Property)GameLoop.getInstance().Gameboard.TileOrder[18];
            Property p19 = (Property)GameLoop.getInstance().Gameboard.TileOrder[19];
            Property p21 = (Property)GameLoop.getInstance().Gameboard.TileOrder[21];
            Property p23 = (Property)GameLoop.getInstance().Gameboard.TileOrder[23];
            Property p24 = (Property)GameLoop.getInstance().Gameboard.TileOrder[24];
            Property p25 = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[25];
            Property p26 = (Property)GameLoop.getInstance().Gameboard.TileOrder[26];
            Property p27 = (Property)GameLoop.getInstance().Gameboard.TileOrder[27];
            Utility p28 = (Utility)GameLoop.getInstance().Gameboard.TileOrder[28];
            Property p29 = (Property)GameLoop.getInstance().Gameboard.TileOrder[29];
            Property p31 = (Property)GameLoop.getInstance().Gameboard.TileOrder[31];
            Property p32 = (Property)GameLoop.getInstance().Gameboard.TileOrder[32];
            Property p34 = (Property)GameLoop.getInstance().Gameboard.TileOrder[34];
            Property p35 = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[35];
            Property p37 = (Property)GameLoop.getInstance().Gameboard.TileOrder[37];
            FreeParking p38 = (FreeParking)GameLoop.getInstance().Gameboard.TileOrder[38];
            Property p39 = (Property)GameLoop.getInstance().Gameboard.TileOrder[39];
            updateProperty(p1, player, tbValue1);
            updateProperty(p3, player, tbValue3);
            updateProperty(p4, player, tbValue4);
            updateProperty(p5, player, tbValue5);
            updateProperty(p6, player, tbValue6);
            updateProperty(p8, player, tbValue8);
            updateProperty(p9, player, tbValue9);
            updateProperty(p11, player, tbValue11);
            updateProperty(p12, player, tbValue12);
            updateProperty(p13, player, tbValue13);
            updateProperty(p14, player, tbValue14);
            updateProperty(p15, player, tbValue15);
            updateProperty(p16, player, tbValue16);
            updateProperty(p18, player, tbValue18);
            updateProperty(p19, player, tbValue19);
            updateProperty(p21, player, tbValue21);
            updateProperty(p23, player, tbValue23);
            updateProperty(p24, player, tbValue24);
            updateProperty(p25, player, tbValue25);
            updateProperty(p26, player, tbValue26);
            updateProperty(p27, player, tbValue27);
            updateProperty(p28, player, tbValue28);
            updateProperty(p29, player, tbValue29);
            updateProperty(p31, player, tbValue31);
            updateProperty(p32, player, tbValue32);
            updateProperty(p34, player, tbValue34);
            updateProperty(p35, player, tbValue35);
            updateProperty(p37, player, tbValue37);
            updateProperty(p38, player, tbValue38);
            updateProperty(p39, player, tbValue39);
        }

        /// <summary>
        /// Updates the property rent/cost/owned info for the cuttent player
        /// </summary>
        /// <param name="p">property tile</param>
        /// <param name="player">current player</param>
        /// <param name="tb">property's info text block</param>
        private void updateProperty(Property p, Player player, TextBlock tb)
        {
            if (!p.IsOwned)
            {
                tb.Foreground = Brushes.Green;
                tb.Text = p.Cost.ToString("c0");
            }
            else if (p.Owner == player)
            {
                tb.Foreground = Brushes.Black;
                tb.Text = "Owned";
            }
            else
            {
                tb.Foreground = Brushes.Red;
                tb.Text = p.Rent.ToString("c0");
            }
        }

        private void updateProperty(FreeParking p, Player player, TextBlock tb)
        {
            if (p.Name == "Income Tax")
            {
                tb.Foreground = Brushes.Red;
                if (player.Money / 10 > 200)
                {
                    tb.Text = (player.Money / 10).ToString("c0");
                }
                else
                {
                    tb.Text = "$200";
                }
            }
            else {
                tb.Foreground = Brushes.Red;
                tb.Text = "$100";
            }
        }

        private void updateProperty(Utility p, Player player, TextBlock tb)
        {
            if (!p.IsOwned)
            {
                tb.Foreground = Brushes.Green;
                tb.Text = p.Cost.ToString("c0");
            }
            else if (p.Owner == player)
            {
                tb.Foreground = Brushes.Black;
                tb.Text = "Owned";
            }
            else
            {
                tb.Foreground = Brushes.Red;
               
                tb.Text = (p.Rent* GameLoop.getInstance().Gameboard.RolledDice[0]* GameLoop.getInstance().Gameboard.RolledDice[1]).ToString("c0");
            }
        }
        #endregion

        /// <summary>
        /// Updates the visual location of all players' icons. 
        /// </summary>
        /// <param name="player">Current player</param>
        private void UpdatePlayerIconPosition(Player player) {
            DisplayCurrentPlayerLocation(player);
            int playerIDX = 0;

            // Updates the player's displayed location
            foreach (Player p in GameLoop.getInstance().Gameboard.Players) {
                if(playerIDX == 0) {
                    Image playerIcon = GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon;
                    if (p.Location >= 0 && p.Location <= 10)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, 0);
                        Grid.SetColumn(playerIcon, p.Location);
                    }
                    else if (p.Location > 10 && p.Location <= 20)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, p.Location - 10);
                        Grid.SetColumn(playerIcon, 10);
                    }
                    else if (p.Location > 20 && p.Location <= 30)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, 10);
                        Grid.SetColumn(playerIcon, Math.Abs((p.Location - 20) - 10));
                    }
                    else
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, Math.Abs((p.Location - 30) - 10));
                        Grid.SetColumn(playerIcon, 0);
                    }
                }
                else if (playerIDX == 1)
                {
                    Image playerIcon = GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon;
                    if (p.Location >= 0 && p.Location <= 10)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, 0);
                        Grid.SetColumn(playerIcon, p.Location);
                    }
                    else if (p.Location > 10 && p.Location <= 20)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, p.Location - 10);
                        Grid.SetColumn(playerIcon, 10);
                    }
                    else if (p.Location > 20 && p.Location <= 30)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, 10);
                        Grid.SetColumn(playerIcon, Math.Abs((p.Location - 20) - 10));
                    }
                    else
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, Math.Abs((p.Location - 30) - 10));
                        Grid.SetColumn(playerIcon, 0);
                    }
                }
                else if (playerIDX == 2)
                {
                    Image playerIcon = GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon;
                    if (p.Location >= 0 && p.Location <= 10)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Center;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, 0);
                        Grid.SetColumn(playerIcon, p.Location);
                    }
                    else if (p.Location > 10 && p.Location <= 20)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Center;
                        Grid.SetRow(playerIcon, p.Location - 10);
                        Grid.SetColumn(playerIcon, 10);
                    }
                    else if (p.Location > 20 && p.Location <= 30)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Center;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, 10);
                        Grid.SetColumn(playerIcon, Math.Abs((p.Location - 20) - 10));
                    }
                    else
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Center;
                        Grid.SetRow(playerIcon, Math.Abs((p.Location - 30) - 10));
                        Grid.SetColumn(playerIcon, 0);
                    }
                }
                else if (playerIDX == 3)
                {
                    Image playerIcon = GameLoop.getInstance().Icons[p.IconIndex].PlayerIcon;
                    if (p.Location >= 0 && p.Location <= 10)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Center;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Left;
                        Grid.SetRow(playerIcon, 0);
                        Grid.SetColumn(playerIcon, p.Location);
                    }
                    else if (p.Location > 10 && p.Location <= 20)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Top;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Center;
                        Grid.SetRow(playerIcon, p.Location - 10);
                        Grid.SetColumn(playerIcon, 10);
                    }
                    else if (p.Location > 20 && p.Location <= 30)
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Center;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Right;
                        Grid.SetRow(playerIcon, 10);
                        Grid.SetColumn(playerIcon, Math.Abs((p.Location - 20) - 10));
                    }
                    else
                    {
                        playerIcon.VerticalAlignment = VerticalAlignment.Bottom;
                        playerIcon.HorizontalAlignment = HorizontalAlignment.Center;
                        Grid.SetRow(playerIcon, Math.Abs((p.Location - 30) - 10));
                        Grid.SetColumn(playerIcon, 0);
                    }
                }

                playerIDX++;
            }
        }

        /// <summary>
        /// One iteration of a turn loop (Roll, Move, LocationAction) with the proper logic to support doubles, Jail, and normal turns.
        /// Updates the GUI at the proper places.
        /// The button logic is also advanced here, based upon rolling no doubles.
        /// </summary>
        /// <param name="player">Current player</param>
        private void TakeTurn(Player player) {
            bool doubleWasRolled = false;
            GameLoop.getInstance().Gameboard.RollDice();      // Roll dice
            DisplayRolledDice();    // GUI
            UpdateBoard(player);

            if (player.IsInJail) {
                player.TurnsSpentInJail++;
                UpdatePlayerIconPosition(player);    // GUI
                if (GameLoop.getInstance().Gameboard.RolledDice[0] == GameLoop.getInstance().Gameboard.RolledDice[1] || player.TurnsSpentInJail == 3) {   // If double was rolled, or this was the 3rd turn spent in Jail, release the player.
                    player.TurnsSpentInJail = 0;
                    player.IsInJail = false;

                    if (player.TurnsSpentInJail != 3) {     // If player was released by rolling a double:
                        GameLoop.getInstance().Gameboard.Move(player);       // Move player
                        UpdatePlayerIconPosition(player);    // GUI
                        LocationResponse(player);       // Action of player's new location
                        UpdatePlayerIconPosition(player);   // GUI
                    }
                }
            } else {
                if (GameLoop.getInstance().Gameboard.RolledDice[0] == GameLoop.getInstance().Gameboard.RolledDice[1]) {       // If doubles were rolled, increase count
                    doubleWasRolled = true;
                    player.DoublesRolled++;
                }
                if (player.DoublesRolled == 3) {        // If 3rd double was rolled, send player to Jail
                    player.Location = 10;   // Jail space
                    player.IsInJail = true;
                    UpdatePlayerIconPosition(player);   // GUI
                } else {                    
                    GameLoop.getInstance().Gameboard.Move(player);   // Move player
                    UpdatePlayerIconPosition(player);   // GUI
                    DisplayCurrentPlayerMoney(player);
                    LocationResponse(player);   // Action of player's new location
                    UpdatePlayerIconPosition(player);   // GUI
                }
            }
            // Checks to see if the turn should be advanced (dice doubles logic)
            if (!doubleWasRolled || player.IsInJail) {
                player.DoublesRolled = 0;
                GameLoop.getInstance().Gameboard.TurnIsReadyToEnd = true;
            } else {
                GameLoop.getInstance().Gameboard.TurnIsReadyToEnd = false;
            }
            btnEndTurn.IsEnabled = GameLoop.getInstance().Gameboard.TurnIsReadyToEnd;
            btnRoll.IsEnabled = !GameLoop.getInstance().Gameboard.TurnIsReadyToEnd;
        }

        /// <summary>
        /// Performs the action for the player's new location based on what type of tile it is.
        /// </summary>
        /// <param name="player">Current Player</param>
        private void LocationResponse(Player player) {
            // if the player lands on a propertym display pay rent or purchase property form
            if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Basic) ||
                GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Utility) ||
                GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Railroad)) {

                Property property = (Property)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                DisplayPayRentOrPurchasePropertyForm(player, property);

                DisplayCurrentPlayerProperties(player);      // GUI
            } // when a player lands on a community chest spot or chance spot
            else if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(CardSpace)) {
                // Get the cardSpace and display the community or chance form
                CardSpace cardTile = (CardSpace)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                DisplayCommunityOrChanceForm(player, cardTile);
                // if the community or chance card calls for the player to advance to a property, display pay rent or purchase property form
                if (cardTile.ProcessedCard[0, 1] == "Advance" && (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Basic) ||
                    GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Utility) ||
                    GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Railroad))) {
                    DisplayCurrentPlayerMoney(player);
                    DisplayPayRentOrPurchasePropertyForm(player, (Property)GameLoop.getInstance().Gameboard.TileOrder[player.Location]);
                } // if the community or chance card calls for the player to advance to a card space, display community or chance card form
                else if (cardTile.ProcessedCard[0, 1] == "Advance" && (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(CardSpace))){
                    DisplayCurrentPlayerMoney(player);
                    DisplayCommunityOrChanceForm(player, (CardSpace)GameLoop.getInstance().Gameboard.TileOrder[player.Location]);
                } // if the community or chance card calls for the player to advance to the go to jail space, send the player to jail
                else if (cardTile.ProcessedCard[0, 1] == "Advance" && (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(GoToJail))){
                    GoToJail goToJailTile = (GoToJail)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                    goToJailTile.LocationAction(player);
                } // if the community or chance card calls for the player to advance to the go to Income Tax, decrease the player's money by 10% (if more) or pay $200
                else if (cardTile.ProcessedCard[0, 1] == "Advance" && (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(FreeParking))) {
                    FreeParking freeParking = (FreeParking)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                    PayTaxForm(player, freeParking);
                }
            } // if Luxury Tax or Income Tax, display tax form
            else if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(FreeParking)) {
                FreeParking freeParking = (FreeParking)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                if (freeParking.Name == "Income Tax" || freeParking.Name == "Luxury Tax") {
                    PayTaxForm(player, freeParking);
                }  
            }
            // if not a property or community chest or chance card, call location action on that tile
            else {
                GameLoop.getInstance().Gameboard.TileOrder[player.Location].LocationAction(player);
            }
            DisplayCurrentPlayerMoney(player);      // GUI
            DisplayCurrentPlayerLocation(player);
            DisplayCurrentPlayerProperties(player);
            UpdateBoard(player);
        }

        #region Calls to Other Forms
        /// <summary>
        /// Display the pay rent form or the purchase property form 
        /// </summary>
        /// <param name="player">Current player</param>
        /// <param name="property">Property tile that the current player landed on</param>
        private void DisplayPayRentOrPurchasePropertyForm( Player player, Property property) {
            Player propertyOwner = property.Owner;

            // if a property and is not owned, call the purchase property form
            if (propertyOwner == null) {
                // Figured out how to center windows with respect to their predecessors:
                // https://stackoverflow.com/questions/4306593/wpf-how-to-set-a-dialog-position-to-show-at-the-center-of-the-application
                PurchaseProperty purchaseProperty = new PurchaseProperty();
                purchaseProperty.Owner = this;

                // Output information to the purchase property form
                purchaseProperty.tbOutputName.Text = player.Name;
                purchaseProperty.tbOutputMoney.Text = string.Format("{0:$#,##0}", player.Money);
                foreach (Property proprty in player.PropertiesOwned) {
                    purchaseProperty.lbOutputPropertiesOwned.Items.Add(proprty.Name);
                }
                purchaseProperty.tbOutputCostOfProperty.Text = property.Cost.ToString("c0");
                if (player.Money >= property.Cost)
                {
                    purchaseProperty.tbUserInformation.Text = "Would you like to buy " + property.Name + "?";
                } else {
                    purchaseProperty.tbUserInformation.Text = "Sorry, you do not have enough money to purchase " + property.Name + ".";
                    purchaseProperty.btnYesBuy.IsEnabled = false;
                }
                purchaseProperty.ShowDialog();
            } // if the current player owns the property, do nothing
            else if (propertyOwner.Name == player.Name) {

            } // if another player owns the property, call the PayRent form
            else {
                PayRent payRent = new PayRent();
                payRent.Owner = this;

                // Output information to pay rent form
                payRent.tbOutputName.Text = player.Name;
                payRent.tbOutputMoney.Text = string.Format("{0:$#,##0}", player.Money);
                payRent.tbOutputPayRentTo.Text = propertyOwner.Name;

                // Calculates the rent for utility or railroad property before output to the pay rent form
                if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Utility)) {
                    Utility utilityProperty = (Utility)property;
                    utilityProperty.CalculateRent((Utility)GameLoop.getInstance().Gameboard.TileOrder[12], (Utility)GameLoop.getInstance().Gameboard.TileOrder[28], utilityProperty.NearstUtility);
                } else if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(Railroad)) {
                    Railroad railroadProperty = (Railroad)property;
                    railroadProperty.CalculateRent((Railroad)GameLoop.getInstance().Gameboard.TileOrder[5], (Railroad)GameLoop.getInstance().Gameboard.TileOrder[15],
                    (Railroad)GameLoop.getInstance().Gameboard.TileOrder[25], (Railroad)GameLoop.getInstance().Gameboard.TileOrder[35], railroadProperty.NearestRailroad);
                }                
                payRent.tbOutputRentIsHowMuch.Text = property.Rent.ToString("c0");
                payRent.ShowDialog();
            }
        }

        /// <summary>
        /// Displays the community chest form or the chance form
        /// </summary>
        /// <param name="player">the current player</param>
        /// <param name="cardTile">the CardSpace tile that the current player landed on</param>
        private void DisplayCommunityOrChanceForm(Player player, CardSpace cardTile) {
            List<Card> cardsList = new List<Card>();            

            // if land on a community chest card space, set the list of community chest cards  and the processing array to a local variable
            if (player.Location == 2 || player.Location == 17 || player.Location == 33) {
                cardsList = GameLoop.getInstance().Gameboard.CommunityChestCards;
                cardTile.CardsForProcessing = new string[GameLoop.getInstance().Gameboard.CommunityChestCardsForProcessing.GetLength(0), 
                    GameLoop.getInstance().Gameboard.CommunityChestCardsForProcessing.GetLength(1)];
                cardTile.CardsForProcessing = GameLoop.getInstance().Gameboard.CommunityChestCardsForProcessing;
            } // if land on a chance card space, set the list of chance cards and the processing array to local variables
            else if (player.Location == 7 || player.Location == 22 || player.Location == 36) {
                cardsList = GameLoop.getInstance().Gameboard.ChanceCards;
                cardTile.CardsForProcessing = new string[GameLoop.getInstance().Gameboard.ChanceCardsForProcessing.GetLength(0),
                    GameLoop.getInstance().Gameboard.ChanceCardsForProcessing.GetLength(1)];
                cardTile.CardsForProcessing = GameLoop.getInstance().Gameboard.ChanceCardsForProcessing;
            }

            // pick a random proccessed card
            int randomNum = GameLoop.getInstance().Gameboard.Rand.Next(cardTile.CardsForProcessing.GetLength(0));
            for (int j = 0; j < cardTile.ProcessedCard.GetLength(1); j++) {
                cardTile.ProcessedCard[0, j] = cardTile.CardsForProcessing[randomNum, j];
            }

            // if a Community Chest, card display the form
            if (cardTile.Name.Contains("Community Chest")) {
                CommunityChestCard communityChestCard = new CommunityChestCard();
                communityChestCard.Owner = this;
                communityChestCard.tbOutputCommunityChest.Text = cardTile.ProcessedCard[0, 0];
                communityChestCard.ShowDialog();
            } // if a Chance card, display the form 
            else if (cardTile.Name.Contains("Chance")) {
                ChanceCard chanceCard = new ChanceCard();
                chanceCard.Owner = this;
                // Display the card's description
                chanceCard.tbOutputChance.Text = cardTile.ProcessedCard[0, 0];
                chanceCard.ShowDialog();
            }
        }
        /// <summary>
        /// Displays the tax form for Income Tax or Luxury Tax
        /// </summary>
        /// <param name="player"></param>
        /// <param name="freeParking"></param>
        private void PayTaxForm(Player player, FreeParking freeParking) {
            Tax taxfrm = new Tax();
            taxfrm.tbTileType.Text = freeParking.Name;
            taxfrm.tbOutput.Text = "Pay $" + freeParking.AmountToPay;
            taxfrm.ShowDialog();
        }
        #endregion
    }
}
