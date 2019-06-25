using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    public class Player {
        #region Attributes
        private string _Name;
        private int _IconIndex;
        private int _Money = 1500;
        private int _Location = 0;
        private int _DoublesRolled = 0;
        private bool _IsInJail = false;
        private int _TurnsSpentInJail = 0;
        private List<Property> _PropertiesOwned = new List<Property>();
        private PropertyAction _CurrentPropertyAction;
        #endregion

        #region Enums
        /// <summary>
        /// Represents the action the player is currently doing upon a property
        /// </summary>
        public enum PropertyAction : int {
            None = 0,
            IsBuying = 1,
            IsPayingRent = 2
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for player
        /// </summary>
        public Player() { }

        /// <summary>
        /// Creates a player with a name and icon
        /// </summary>
        /// <param name="name">player's name</param>
        /// <param name="icon">player's icon</param>
        public Player(string name, int iconIndex) {
            Name = name;
            IconIndex = iconIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the name of the player
        /// </summary>
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Gets and sets the index of the chosen player icon in the list of icons
        /// </summary>
        public int IconIndex {
            get { return _IconIndex; }
            set { _IconIndex = value; }
        }

        /// <summary>
        /// Gets and sets the amount of money the player has
        /// </summary>
        public int Money {
            get { return _Money; }
            set { _Money = value; }
        }

        /// <summary>
        /// Gets and sets the location of the player
        /// </summary>
        public int Location {
            get { return _Location; }
            set { _Location = value; }
        }

        /// <summary>
        /// Gets and sets the number of doubles rolled by the player
        /// </summary>
        public int DoublesRolled {
            get { return _DoublesRolled; }
            set { _DoublesRolled = value; }
        }

        /// <summary>
        /// Gets and sets if the player is in jail
        /// </summary>
        public bool IsInJail {
            get { return _IsInJail; }
            set { _IsInJail = value; }
        }

        /// <summary>
        /// Gets and sets the number of turns spent in jail
        /// </summary>
        public int TurnsSpentInJail {
            get { return _TurnsSpentInJail; }
            set { _TurnsSpentInJail = value; }
        }

        /// <summary>
        /// Gets and sets the list of properties that are owned by the player
        /// </summary>
        public List<Property> PropertiesOwned {
            get { return _PropertiesOwned; }
            set { _PropertiesOwned = value; }
        }

        /// <summary>
        /// Gets and sets the action the player is currently doing upon a property
        /// </summary>
        public PropertyAction CurrentPropertyAction {
            get { return _CurrentPropertyAction; }
            set { _CurrentPropertyAction = value; }
        }
        #endregion
    }
}
