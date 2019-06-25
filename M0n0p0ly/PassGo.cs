using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class PassGo : Tile {
        #region Constructors
        /// <summary>
        /// Default Constructor for PassGo
        /// </summary>
        public PassGo() {
            Name = "Go";
        }      
        #endregion


        #region Methods
        /// <summary>
        /// Gives the player $200 for passing go
        /// </summary>
        /// <param name="player">current player</param>
        public override void LocationAction(Player player) {            
            player.Money += 200;        
        }
        #endregion
    }
}
