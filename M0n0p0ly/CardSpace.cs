using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class CardSpace : Tile {
        #region Attributes
        private string[,] _CardsForProcessing;
        private string[,] _ProcessedCard = new string[1, 7];
        #endregion

        #region Properties
        // Get and set the card set (either community chest cards or chance cards)
        public string[,] CardsForProcessing {
            get { return _CardsForProcessing; }
             set { _CardsForProcessing = value; }
        }


        /// <summary>
        /// Get the randomly choosen processed card
        /// </summary>
        public string[,] ProcessedCard {
            get { return _ProcessedCard; }
            set { _ProcessedCard = value; }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Creates a community chest or chance card tile
        /// </summary>
        /// <param name="name">either named community chest or chance</param>
        public CardSpace(string name) : base(name) { }
        #endregion


        #region Methods
        public override void LocationAction(Player player) {          
            // When the card allows the current player to collect money
            if (ProcessedCard[0, 1] == "Collect") {
                // if the current player collects money only once
                if (ProcessedCard[0, 2] == "Once") {
                    // the current player recieves the money
                    int.TryParse(ProcessedCard[0, 3], out int collectedMoney);
                    player.Money += collectedMoney;
                } // the current player collects money from every player
                else if (ProcessedCard[0, 2] == "Every") {
                    int.TryParse(ProcessedCard[0, 3], out int collectedMoney);
                    // Take the money away from each player in the list except the current player
                    foreach (Player member in GameLoop.getInstance().Gameboard.Players) {
                        if (member == player) {
                            continue;
                        } else {
                            member.Money -= collectedMoney;
                            player.Money += collectedMoney;
                        }
                    }
                }
            } else if (ProcessedCard[0, 1] == "Pay") {
                // if the current player collects money only once
                if (ProcessedCard[0, 2] == "Once") {
                    // the current player pays the money
                    int.TryParse(ProcessedCard[0, 3], out int payMoney);
                    player.Money -= payMoney;
                } // the current player pays money from every player
                else if (ProcessedCard[0, 2] == "Every") {
                    int.TryParse(ProcessedCard[0, 3], out int payMoney);
                    if (payMoney != 0) {
                        // Pay money to each player in the list except the current player 
                        foreach (Player member in GameLoop.getInstance().Gameboard.Players) {
                            if (member == player) {
                                continue;
                            } else {
                                member.Money += payMoney;
                                player.Money -= payMoney;
                            }
                        }
                    } 
                }            
            } else if (ProcessedCard[0, 1] == "Advance") {
                // Move to a direct location
                if (ProcessedCard[0, 2] == "NotNearest") {
                    string nameOfTile = ProcessedCard[0, 3];
                    // if the card specifies to move forward or backwards a certain number of spaces, move the player to this new location
                    if (nameOfTile == "Location") {
                        int.TryParse(ProcessedCard[0, 5], out int SpacesToMove);
                        int newLocation = player.Location + SpacesToMove;
                        GameLoop.getInstance().Gameboard.Move(player, newLocation);
                    } 
                    else { // move player to specified tile
                        for (int i = 0; i < GameLoop.getInstance().Gameboard.TileOrder.Length; i++) {
                         // if the tile name equals the name from the processing array, move player to that location
                            if (GameLoop.getInstance().Gameboard.TileOrder[i].Name == nameOfTile) {
                                GameLoop.getInstance().Gameboard.Move(player, i);
                                if (nameOfTile == "Jail") {
                                    player.IsInJail = true;
                                }
                                break;
                            }   

                        }
                    } 
                }
                else if (ProcessedCard[0, 2] == "Nearest") {                    
                    if (ProcessedCard[0, 3] == "Railroad") {
                        // for player locations less than the last railroad on the game board
                        if (player.Location < 35) {
                            for (int i = player.Location + 1; i < GameLoop.getInstance().Gameboard.TileOrder.Length; i++) {
                                // move player to nearest railroad and set advance to nearst railroad attribute used to Calculate Rent
                                if (GameLoop.getInstance().Gameboard.TileOrder[i].GetType() == typeof(Railroad)) {
                                    GameLoop.getInstance().Gameboard.Move(player, i);
                                    Railroad nearestRailroad = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[i];
                                    nearestRailroad.NearestRailroad = true;
                                    break;
                                }
                            }
                        } // for player locations greater than or equal to the last railroad on the game board 
                        else {
                            for (int i = 0; i < GameLoop.getInstance().Gameboard.TileOrder.Length; i++) {
                                // move player to nearest railroad, and set advance to nearst railroad attribute used to Calculate Rent
                                if (GameLoop.getInstance().Gameboard.TileOrder[i].GetType() == typeof(Railroad)) {
                                    GameLoop.getInstance().Gameboard.Move(player, i);
                                    Railroad nearestRailroad = (Railroad)GameLoop.getInstance().Gameboard.TileOrder[i];
                                    nearestRailroad.NearestRailroad = true;
                                    break;
                                }
                            }
                        }
                        
                    } else if (ProcessedCard[0, 3] == "Utility") {
                        // for player locations less than the last utility on the game board
                        if (player.Location < 28) {
                            for (int i = player.Location + 1; i < GameLoop.getInstance().Gameboard.TileOrder.Length; i++) {
                                // move player to nearest utility, and set advance to nearst utility attribute used to Calculate Rent
                                if (GameLoop.getInstance().Gameboard.TileOrder[i].GetType() == typeof(Utility)) {
                                    GameLoop.getInstance().Gameboard.Move(player, i);
                                    Utility nearestUtility = (Utility)GameLoop.getInstance().Gameboard.TileOrder[i];
                                    nearestUtility.NearstUtility = true;
                                    break;
                                }
                            }
                        } // for player locations greater than or equal to the last utility on the game board  
                        else {
                            for (int i = 0; i < GameLoop.getInstance().Gameboard.TileOrder.Length; i++) {
                                // move player to nearest utility, and set advance to nearst utility attribute used to Calculate Rent
                                if (GameLoop.getInstance().Gameboard.TileOrder[i].GetType() == typeof(Utility)) {
                                    GameLoop.getInstance().Gameboard.Move(player, i);
                                    Utility nearestUtility = (Utility)GameLoop.getInstance().Gameboard.TileOrder[i];
                                    nearestUtility.NearstUtility = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }             
        }
        #endregion
    }
}
