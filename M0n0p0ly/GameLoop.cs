using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace M0n0p0ly {
    public class GameLoop {
        #region Attributes
        private static GameLoop uniqueInstance;
        private Gameboard _Gameboard = new Gameboard();
        private List<Icon> _Icons = new List<Icon>();
        #endregion

        #region Singleton Code
        private GameLoop() { }

        public static GameLoop getInstance() {
            if (uniqueInstance == null) {
                uniqueInstance = new GameLoop();
            }
            return uniqueInstance;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the Gameboard
        /// </summary>
        public Gameboard Gameboard {
            get { return _Gameboard; }
            set { _Gameboard = value; }
        }

        /// <summary>
        /// Gets and sets the list of possible icons in the game
        /// </summary>
        public List<Icon> Icons {
            get { return _Icons; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes player icon images into a list
        /// </summary>
        public void initilizeIconList() {
            Icons.Add(new Icon("Images/car.png"));
            Icons.Add(new Icon("Images/dog.png"));
            Icons.Add(new Icon("Images/hat.png"));
            Icons.Add(new Icon("Images/iron.png"));
            Icons.Add(new Icon("Images/ship.png"));
            Icons.Add(new Icon("Images/shoe.png"));
            Icons.Add(new Icon("Images/thimble.png"));
            Icons.Add(new Icon("Images/wheelbarrow.png"));
        }

        /// <summary>
        /// Saves the current game state to a file
        /// </summary>
        /// <returns>Success or failure</returns>
        public bool SaveGame() {
            if (_Gameboard != null) {

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Save file(*.sav)|*.sav|All files|*.*";

                if (sfd.ShowDialog() == true) {
                    string fileName = sfd.FileName;
                    FileStream fs = File.Create(fileName);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, Gameboard);
                    fs.Close();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loads a game state from a file
        /// </summary>
        /// <returns>Success or failure</returns>
        public bool LoadGame() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Save file(*.sav)|*.sav|All files|*.*";

            if (ofd.ShowDialog() == true) {
                string fileName = ofd.FileName;
                FileStream fs = File.OpenRead(fileName);
                BinaryFormatter bf = new BinaryFormatter();
                Gameboard = (Gameboard)bf.Deserialize(fs);
                fs.Close();
                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}
