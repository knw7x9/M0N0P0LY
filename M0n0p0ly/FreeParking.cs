using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class FreeParking : Tile {
        #region Properties
        /// <summary>
        /// Get the amount of money the player has to pay if landing on Income Tax or Luxury Tax
        /// </summary>
        public int AmountToPay {
            get {
                Player currentPlayer = GameLoop.getInstance().Gameboard.Players[GameLoop.getInstance().Gameboard.CurrentPlayerIndex];
                if (this.Name == "Income Tax") {  
                    if ((currentPlayer.Money * 0.10) > 200) {
                        return (int)(currentPlayer.Money * 0.10);
                    } // if $200 is greater than 10% of the player's money, decrease the player's money by $200
                    else {
                        return 200;
                    }
                }// if Luxury Tax, decrease the player's money by $100 
            else if (this.Name == "Luxury Tax") {
                    return 100;
                } else {
                    return 0;
                }
            }
        }         
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor for Free Parking
        /// </summary>
        public FreeParking() {
            Name = "Free Parking";
        }

        public FreeParking(string name) {
            Name = name;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Decreases the player's money of land on Income Tax or Luxury Tax
        /// </summary>
        /// <param name="player">current player</param>
        public override void LocationAction(Player player) {
            // if 10% of the player's money is more than $200, decrease the player's money by 10%
            if (this.Name == "Income Tax") {
                if ((player.Money * 0.10) > 200) {
                    player.Money = (int)(player.Money * 0.90);
                } // if $200 is greater than 10% of the player's money, decrease the player's money by $200
                else {
                    player.Money -= 200;
                }
            }// if Luxury Tax, decrease the player's money by $100 
            else if (this.Name == "Luxury Tax") {
                player.Money -= 100;
            }
        }
        #endregion
    }
}
