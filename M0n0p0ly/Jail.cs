using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class Jail : Tile {
        #region Constructors
        /// <summary>
        /// Default Constructor for Jail
        /// </summary>
        public Jail() {
            Name = "Jail";
        }
        #endregion

        #region Methods
        public override void LocationAction(Player player) {
        }
        #endregion

    }
}
