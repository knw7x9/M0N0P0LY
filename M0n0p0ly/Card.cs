using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class Card {
        #region Attributes
        private string _Phrase;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a card, either community chest card or chance card
        /// </summary>
        /// <param name="phrase">the phase on the card</param>
        public Card(string phrase) {
            Phrase = phrase;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get and set the phrase of the card
        /// </summary>
        public string Phrase {
            get { return _Phrase; }
            set { _Phrase = value; }
        }
        #endregion    
    }
}
