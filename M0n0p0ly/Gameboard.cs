using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace M0n0p0ly {
    [Serializable]
    public class Gameboard {
        #region Attributes
        private Tile[] _TileOrder;
        private List<Player> _Players = new List<Player>();
        private int _TurnCount = 1;
        private string[,] _CommunityChestCardsForProcessing;
        private string[,] _ChanceCardsForProcessing;
        private List<Card> _CommunityChestCards = new List<Card>();
        private List<Card> _ChanceCards = new List<Card>();
        // Seed generation for random number generator: https://stackoverflow.com/questions/1785744/how-do-i-seed-a-random-class-to-avoid-getting-duplicate-random-values 
        private static Random _Rand = new Random(Guid.NewGuid().GetHashCode());
        private int[] _RolledDice = new int[2];
        private int _CurrentPlayerIndex = 0;
        private bool _TurnIsReadyToEnd = false;
        #endregion

        #region Constructors
        public Gameboard() {
            InitializeTileOrder();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the array of tiles
        /// </summary>
        public Tile[] TileOrder {
            get { return _TileOrder; }
        }

        /// <summary>
        /// Gets the list of players
        /// </summary>
        public List<Player> Players {
            get { return _Players; }
        }

        /// <summary>
        /// Gets and sets the turn count
        /// </summary>
        public int TurnCount {
            get { return _TurnCount; }
            set { _TurnCount = value; }
        }

        /// <summary>
        /// Gets and sets the community chest cards (processing) array
        /// </summary>
        public string[,] CommunityChestCardsForProcessing {
            get { return _CommunityChestCardsForProcessing; }
            set { _CommunityChestCardsForProcessing = value; }
        }

        /// <summary>
        /// Gets and sets the chance cards (processing) array
        /// </summary>
        public string[,] ChanceCardsForProcessing {
            get { return _ChanceCardsForProcessing; }
            set { _ChanceCardsForProcessing = value; }
        }

        /// <summary>
        /// Gets and sets the community chest cards list
        /// </summary>
        public List<Card> CommunityChestCards {
            get { return _CommunityChestCards; }
            set { _CommunityChestCards = value; }
        }

        /// <summary>
        /// Gets and sets the chance cards list
        /// </summary>
        public List<Card> ChanceCards {
            get { return _ChanceCards; }
            set { _ChanceCards = value; }
        }

        /// <summary>
        /// Gets the values of the dice that were rolled
        /// </summary>
        public int[] RolledDice {
            get { return _RolledDice; }
            set { _RolledDice = value; }
        }

        /// <summary>
        /// Gets and sets the List-index of Players that represents the current player
        /// </summary>
        public int CurrentPlayerIndex {
            get { return _CurrentPlayerIndex; }
            set { _CurrentPlayerIndex = value; }
        }

        /// <summary>
        /// Gets and sets whether the current turn is ready to end
        /// </summary>
        public bool TurnIsReadyToEnd {
            get { return _TurnIsReadyToEnd; }
            set { _TurnIsReadyToEnd = value; }
        }

        /// <summary>
        /// Gets the random number generator
        /// </summary>
        public Random Rand {
            get { return _Rand; }
        }
        #endregion

        #region Main Methods
        /// <summary>
        /// Adds a player to the list of players
        /// </summary>
        /// <param name="p">player to added to the game (max 4 players)</param>
        public void AddPlayer(Player p) {
            if (Players.Count < 4) {
                _Players.Add(p);
            }
        }

        /// <summary>
        /// Initializes all the tiles in the game and stores them in order.
        /// </summary>
        private void InitializeTileOrder() {
            _TileOrder = new Tile[40];

            _TileOrder[0] = new PassGo();
            _TileOrder[1] = new Basic("Mediterranean Avenue", 60, 2);
            _TileOrder[2] = new CardSpace("Community Chest");
            _TileOrder[3] = new Basic("Baltic Avenue", 80, 4);
            _TileOrder[4] = new FreeParking("Income Tax");  //IncomeTax
            _TileOrder[5] = new Railroad("Reading Railroad", 200, 25);
            _TileOrder[6] = new Basic("Oriental Avenue", 100, 6);
            _TileOrder[7] = new CardSpace("Chance");
            _TileOrder[8] = new Basic("Vermont Avenue", 100, 6);
            _TileOrder[9] = new Basic("Connecticut Avenue", 120, 8);
            _TileOrder[10] = new Jail();
            _TileOrder[11] = new Basic("St. Charles Place", 140, 10);
            _TileOrder[12] = new Utility("Electric Company", 150, 1);
            _TileOrder[13] = new Basic("States Avenue", 140, 10);
            _TileOrder[14] = new Basic("Virginia Avenue", 160, 12);
            _TileOrder[15] = new Railroad("Pennsylvania Railroad", 200, 25);
            _TileOrder[16] = new Basic("St. James Place", 180, 14);
            _TileOrder[17] = new CardSpace("Community Chest");
            _TileOrder[18] = new Basic("Tennessee Avenue", 180, 14);
            _TileOrder[19] = new Basic("New York Avenue", 200, 16);
            _TileOrder[20] = new FreeParking();
            _TileOrder[21] = new Basic("Kentucky Avenue", 220, 18);
            _TileOrder[22] = new CardSpace("Chance");
            _TileOrder[23] = new Basic("Indiana Avenue", 220, 18);
            _TileOrder[24] = new Basic("Illinois Avenue", 240, 20);
            _TileOrder[25] = new Railroad("B & O Railroad", 200, 25);
            _TileOrder[26] = new Basic("Atlantic Avenue", 260, 22);
            _TileOrder[27] = new Basic("Ventnor Avenue", 260, 22);
            _TileOrder[28] = new Utility("Water Works", 150, 1);
            _TileOrder[29] = new Basic("Marvin Gardens", 280, 24);
            _TileOrder[30] = new GoToJail();
            _TileOrder[31] = new Basic("Pacific Avenue", 300, 26);
            _TileOrder[32] = new Basic("North Carolina Avenue", 300, 26);
            _TileOrder[33] = new CardSpace("Community Chest");
            _TileOrder[34] = new Basic("Pennsylvania Avenue", 320, 28);
            _TileOrder[35] = new Railroad("Short Line Railroad", 200, 25);
            _TileOrder[36] = new CardSpace("Chance");
            _TileOrder[37] = new Basic("Park Place", 350, 35);
            _TileOrder[38] = new FreeParking("Luxury Tax");   //LuxuryTax
            _TileOrder[39] = new Basic("Boardwalk", 400, 50);
        }

        /// <summary>
        /// Rolls two 6-sided dice
        /// </summary>
        public void RollDice() {
            _RolledDice[0] = _Rand.Next(1, 7);
            _RolledDice[1] = _Rand.Next(1, 7);
        }

        /// <summary>
        /// Moves the player's location through the tile list and calls PassGo's location action if it was passed mid-move
        /// </summary>
        /// <param name="player">Current player</param>
        public void Move(Player player) {
            int newLocation = player.Location + RolledDice[0] + RolledDice[1];
            if (newLocation >= TileOrder.Length) {
                newLocation = newLocation - TileOrder.Length;
                if (newLocation != 0) {
                    // If player passed Go (And didn't land on it)
                    TileOrder[0].LocationAction(player);
                }                                
            }
            player.Location = newLocation;
            // Bryan's Chance/Community Card tester code
            /*
            if (player.Location == 36)
            {
                player.Location = 33;
            } else {
                player.Location = 36;
            }
            */
        }
        #endregion

        #region Chance/Community Card Methods
        /// <summary>
        /// Add community chest cards and chance cards to their designated list
        /// </summary>
        public void InitializeCards()
        {
            // Add community chest cards to the list
            for (int i = 0; i < _CommunityChestCardsForProcessing.GetLength(0); i++)
            {
                _CommunityChestCards.Add(new Card(_CommunityChestCardsForProcessing[i, 0]));
            }
            // Add chance cards to the list
            for (int i = 0; i < _ChanceCardsForProcessing.GetLength(0); i++)
            {
                _ChanceCards.Add(new Card(_ChanceCardsForProcessing[i, 0]));
            }
        }

        /// <summary>
        /// Populates the _CommunityChestCards and the _ChanceCards arrays from a file
        /// </summary>
        public void ReadInFiles()
        {
            // Creates an array composed of each line of the file
            string[] CommunityChestCards;
            string[] ChanceCards;
            try
            {
                CommunityChestCards = File.ReadAllLines("CommunityChestCards.txt");
                ChanceCards = File.ReadAllLines("ChanceCards.txt");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { }

            // Initialize arrays
            _CommunityChestCardsForProcessing = new string[CommunityChestCards.Length, 7];
            _ChanceCardsForProcessing = new string[ChanceCards.Length, 7];
            string[] splitArray;

            // Populates the _CommunityChestCards array by splitting each line by the "|"
            for (int i = 0; i < CommunityChestCards.Length; i++)
            {
                string phrase = CommunityChestCards[i];
                splitArray = phrase.Split('|');
                for (int j = 0; j < splitArray.Length; j++)
                {
                    _CommunityChestCardsForProcessing[i, j] = splitArray[j];
                    splitArray[j] = "";
                }
            }

            // Populates the _ChanceCards array by splitting each line by the "|"
            for (int i = 0; i < ChanceCards.Length; i++)
            {
                string phrase = ChanceCards[i];
                splitArray = phrase.Split('|');
                for (int j = 0; j < splitArray.Length; j++)
                {
                    _ChanceCardsForProcessing[i, j] = splitArray[j];
                    splitArray[j] = "";
                }
            }
        }

        /// <summary>
        /// Moves the player's location through the tile list and calls PassGo's location action if it was passed mid-move
        /// </summary>
        /// <param name="player">Current player</param>
        public void Move(Player player, int differentLocation = -1) {
            int newLocation;
            // For using the Move method with the chance cards
            // When differentLocation is not specified, add the rolled dice to the player's location
            if (differentLocation == -1) {
                newLocation = player.Location + RolledDice[0] + RolledDice[1];
            } // If the differentLocation is previously determined, set newLocation
            else {
                newLocation = differentLocation;
            }
            // // if the player passes go and is not in jail
            if (newLocation < player.Location && newLocation != 10) {
                if (GameLoop.getInstance().Gameboard.TileOrder[player.Location].GetType() == typeof(CardSpace)) {
                    CardSpace cardTile = (CardSpace)GameLoop.getInstance().Gameboard.TileOrder[player.Location];
                    // if the card tile calls for going back 3 spaces and is not the first community chest card at location 2
                    if (cardTile.ProcessedCard[0, 3] == "Location" && GameLoop.getInstance().Gameboard.TileOrder[player.Location] != GameLoop.getInstance().Gameboard.TileOrder[2]) {
                        // don't collect go
                    } else {
                        // If player passed Go, collect $200
                        TileOrder[0].LocationAction(player);
                    }
                }                
            }
            player.Location = newLocation;
        }
        #endregion
    }
}
