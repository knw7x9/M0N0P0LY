using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class Railroad : Property {

        #region Attributes
        private bool _NearestRailroad = false;
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets whether the player advanced to the nearest railroad by the chance card 
        /// </summary>
        public bool NearestRailroad {
            get { return _NearestRailroad; }
            set { _NearestRailroad = value; }
        }
        #endregion

        #region Constructors
        public Railroad(string name, int cost, int rent) : base(name, cost, rent) {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the rent due when landing on a railroad property
        /// </summary>
        /// <param name="railroad1">one of the railroads</param>
        /// <param name="railroad2">one of the railroads</param>
        /// <param name="railroad3">one of the railroads</param>
        /// <param name="railroad4">one of the railroads</param>
        /// <param name="advanceToNearestRailroad">whether or not the player is advancing to the nearest railroad</param>
        public void CalculateRent(Railroad railroad1, Railroad railroad2, Railroad railroad3, Railroad railroad4, bool advanceToNearestRailroad) {
            // an array of the railroads owned
            bool[] railroadsOwned = { false, false, false, false };

            //Updates the array depending on which railroads are owned on the gameboard
            railroadsOwned[0] = railroad1.IsOwned;
            railroadsOwned[1] = railroad2.IsOwned;
            railroadsOwned[2] = railroad3.IsOwned;
            railroadsOwned[3] = railroad4.IsOwned;

            // Counts the number of railroads owned
            int count = 0;
            for (int i = 0; i < railroadsOwned.Length; i++) {
                if (railroadsOwned[i] == true) {
                    count++;
                }
            }

            if (advanceToNearestRailroad) {
                Rent = AmountOfRent(count, 25) * 2;
                advanceToNearestRailroad = false;
            } //Gets the rent based on the number of railroads owned on the board
            else {
                Rent = AmountOfRent(count, 25);
            }

        }

        /// <summary>
        /// Gets the rent cost depending on the number of railroads owned on the board
        /// </summary>
        /// <param name="count">the number of railroads owned on the board</param>
        /// <param name="amount">the amount that one railroad owned costs</param>
        /// <returns></returns>
        public int AmountOfRent (int count, int amount) {
            if (count > 1) {
                return AmountOfRent(--count, amount) * 2;
                
            } else {
                return amount;
            }
        }
        #endregion
    }
}
