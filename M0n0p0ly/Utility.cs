using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace M0n0p0ly {
    [Serializable]
    public class Utility : Property {
        #region Attributes
        private bool _NearestUtility = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets whether the player advanced to the nearest utility by the chance card 
        /// </summary>
        public bool NearstUtility {
            get { return _NearestUtility; }
            set { _NearestUtility = value; }
        }
        #endregion

        #region Constructors
        public Utility(string name, int cost, int rent) : base(name, cost, rent) {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the rent due when landing on a utility property
        /// </summary>
        /// <param name="utility">one of the utility properties</param>
        public void CalculateRent(Utility utility1, Utility utility2, bool advanceToNearstUtility) {
            // An array of the utilities owned
            bool[] utilityOwned = { false, false };

            // if a chance card, times the role of the dice by 10 (senerio of both utilities being owned)
            if (advanceToNearstUtility) {
                utilityOwned[0] = true;
                utilityOwned[1] = true;
                advanceToNearstUtility = false;
            } // if not, check the utility properties for ownership
            else {
                if (utility1.IsOwned) {
                    utilityOwned[0] = true;
                }
                if (utility2.IsOwned) {
                    utilityOwned[1] = true;
                }
            }
           
            //Updates the array depending on which utilities are owned on the gameboard
            int count = 0;
            for (int i = 0; i < utilityOwned.Length; i++) {
                if (utilityOwned[i] == true) {
                    count++;
                }
            }

            // Need to access the dice roll 
            int diceOne = GameLoop.getInstance().Gameboard.RolledDice[0];
            int diceTwo = GameLoop.getInstance().Gameboard.RolledDice[1];            

            // Returns the amount of rent due depending on the number of utilities owned on the gameboard and the dice roll
            if ( count == 1) {
                Rent = 4 * (diceOne + diceTwo);
            }
            if (count == 2) {
                Rent = 10 * (diceOne + diceTwo);
            }
        }
        #endregion
    }
}

