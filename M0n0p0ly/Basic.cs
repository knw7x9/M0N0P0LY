using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class Basic : Property {
        #region Constructors
        /// <summary>
        /// Basic type property is to distenquish properties with no special behavior from those that do (railroad and utility).
        /// </summary>
        public Basic(string name, int cost, int rent) : base(name, cost, rent) {
        }
        #endregion
    }
}
