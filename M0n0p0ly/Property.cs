using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    [Serializable]
    abstract public class Property : Tile {
        #region Attributes
        private int _Cost;
        private int _Rent;
        private bool _IsOwned;
        private Player _Owner;
        #endregion

        #region Constructors
        public Property(string name, int cost, int rent) : base(name) {
            _Cost = cost;
            _Rent = rent;
            _IsOwned = false;       // Default is unowned
            _Owner = null;          // Default is no owner
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the cost of the property
        /// </summary>
        public int Cost
        {
            get { return _Cost; }
            set { _Cost = value; }
        }

        /// <summary>
        /// Gets and sets the rent of the property
        /// </summary>
        public int Rent
        {
            get { return _Rent; }
            set { _Rent = value; }
        }

        /// <summary>
        /// Gets and sets whether the property is owned or not
        /// </summary>
        public bool IsOwned
        {
            get { return _IsOwned; }
            set { _IsOwned = value; }
        }

        /// <summary>
        /// Gets and sets which player owns the property
        /// </summary>
        public Player Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the player is buying the property or paying rent
        /// </summary>
        /// <param name="player">current player</param>
        public override void LocationAction(Player player) {
            if (player.CurrentPropertyAction == Player.PropertyAction.IsBuying) {
                Buy(player);
            } else if (player.CurrentPropertyAction == Player.PropertyAction.IsPayingRent) {
                PayRent(player);
            } else {

            }
            player.CurrentPropertyAction = Player.PropertyAction.None;
        }

        /// <summary>
        /// The current player buys the property
        /// </summary>
        /// <param name="player">the current player's turn</param>
        private void Buy(Player player) {
            // Update player attributes
            player.PropertiesOwned.Add(this);
            player.Money -= Cost;
            
            // Update Property attributes
            IsOwned = true;
            Owner = player;
        }

        /// <summary>
        /// Pays rent to the owner and takes the rent away from the current player
        /// </summary>
        /// <param name="player">the current player's turn</param>
        private void PayRent(Player player) {
            // Check if a utility, and calculates the rent
            if (GetType() == typeof(Utility)) {
                Utility currentUtility = (Utility) this;
                currentUtility.CalculateRent((Utility)GameLoop.getInstance().Gameboard.TileOrder[12], (Utility)GameLoop.getInstance().Gameboard.TileOrder[28], currentUtility.NearstUtility);
            }
            // Checks if a railroad, and and calculates the rent
            if (GetType() == typeof(Railroad)) {
                Railroad currentRailroad = (Railroad)this;
                currentRailroad.CalculateRent((Railroad)GameLoop.getInstance().Gameboard.TileOrder[5], (Railroad)GameLoop.getInstance().Gameboard.TileOrder[15],
                    (Railroad)GameLoop.getInstance().Gameboard.TileOrder[25], (Railroad)GameLoop.getInstance().Gameboard.TileOrder[35], currentRailroad.NearestRailroad);
            }
            // Takes the money away from the current player
            player.Money -= Rent;

            // Pays rent to the owner
            foreach (Player plyr in GameLoop.getInstance().Gameboard.Players) {
                if (plyr == Owner) {
                    plyr.Money += Rent;
                    break;
                }
            }
        }
        #endregion
    }
}
