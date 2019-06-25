using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    abstract public class Tile {
        #region Attributes
        private string _Name;
        #endregion

        #region Constructors
        public Tile() {
            Name = "Default";
        }

        public Tile(string name) {
            Name = name;
        }
        #endregion

        #region Methods
        public virtual void LocationAction(Player player) {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get and set the name of the tile
        /// </summary>
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
        #endregion
    }
}
