using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class GoToJail : Tile {
        #region Constructors
        /// <summary>
        /// Default constructor for GoToJail
        /// </summary>
        public GoToJail() {
            Name = "Go to Jail";
        }
        #endregion

        #region Methods  
        /// <summary>
        /// Sends a player to jail
        /// </summary>
        /// <param name="player">current player</param>
        public override void LocationAction(Player player) {
            player.Location = 10;
            player.IsInJail = true;
        }
        #endregion
    }
}
