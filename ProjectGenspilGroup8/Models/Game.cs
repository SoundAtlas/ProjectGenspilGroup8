using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class Game
    {
        private string _name;
        private string _genre;
        private string _numberOfPlayers;
        private List<Stockitem> _stockItems;

        // Constructor
        public Game(string name, string genre, string numberOfPlayers)
        {
            this._name = name;
            this._genre = genre;
            this._numberOfPlayers = numberOfPlayers;
            _stockItems = new List<StockItem>();
        }
    }
}
