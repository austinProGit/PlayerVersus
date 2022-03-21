using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayerVersus
{
    public class TrapRoom : IRoomDelegate
    {
        private Door trickDoor;
        private Room containingRoom;
        private string password;
        public string Password { get { return password; } set { password = value; } }
        public Dictionary<string, Door> Exits { get; set; }
        public TrapRoom()
        {
            Password = "";
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public Door GetExit(string exitName)
        {
            //door connects to same room
            return trickDoor;
        }
        public Room ContainingRoom 
        {
            get
            {
                return containingRoom;
            }
            set 
            { 
                containingRoom = value;
                trickDoor = new Door(containingRoom, containingRoom);
            } 
        }
        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player.CurrentRoom == ContainingRoom)
            {
                Dictionary<string, Object> userInfo = notification.UserInfo;
                string word = (string)userInfo["word"];
                if(word.Equals(password))
                {
                    player.OutputMessage("You said the magic word!");
                    ContainingRoom.Delegate = null;
                }
            }
        }
        public string Description()
        {
            return "You are now trapped!";
        }
    }
    public class EchoRoom : IRoomDelegate
    {
        public Dictionary<string, Door> Exits { get; set; }
        private Room containingRoom;
        public EchoRoom()
        {
            NotificationCenter.Instance.AddObserver("PlayerDidSayWord", PlayerDidSayWord);
        }
        public Room ContainingRoom
        {
            get
            {
                return containingRoom;
            }
            set
            {
                containingRoom = value;
            }
        }
        public Door GetExit(string exitName)
        {
            Door door = null;
            Exits.TryGetValue(exitName, out door);
            return door;
        }
        public string Description()
        {
            return "You are now in the echo room";
        }
        public void PlayerDidSayWord(Notification notification)
        {
            Player player = (Player)notification.Object;
            if(player.CurrentRoom == ContainingRoom)
            {
                Dictionary<string, Object> userInfo = notification.UserInfo;
                string word = (string)userInfo["word"];
                player.OutputMessage(word + "... " + word + "... " + word + "...");
            }
        }
    }
    public class Room
    {
        private Dictionary<string, Door> _exits;
        private string _tag;
        public string Tag { get { return _tag; } set { _tag = value; } }
        private IRoomDelegate _delegate;
        private List<IItem> _itemList;
        private IItem Item;
        private List<Monster> _monsterList;
        private List<Player> _playerList;
        
        public List<IItem> ItemList
        {
            get
            {
                return _itemList;
            }
            set
            {
                _itemList = value;
            }
        }
        public List<Monster> MonsterList
        {
            get { return _monsterList; }
            set { _monsterList = value; }
        }
        public List<Player> PlayerList
        {
            get { return _playerList; }
            set { _playerList = value; }
        }
        
        
        public IRoomDelegate Delegate 
        { 
            set 
            { 
                _delegate = value;
                if (_delegate != null)
                {
                    _delegate.Exits = _exits;
                }
            }
            get
            {
                return _delegate;
            }
        }

        public Room() : this("No Tag")
        {

        }

        public Room(string tag)
        {
            _exits = new Dictionary<string, Door>();
            this.Tag = tag;
            _delegate = null;
            _itemList = new List<IItem>();
            _monsterList = new List<Monster>();
            _playerList = new List<Player>();
        }

        public void SetExit(string exitName, Door door)
        {
            _exits[exitName] = door;
        }

        public Door GetExit(string exitName)
        {
            if (_delegate == null)
            {
                Door door = null;
                _exits.TryGetValue(exitName, out door);
                return door;
            }
            else
            {
                return _delegate.GetExit(exitName);
            }
        }

        public string GetExits()
        {
            string exitNames = "";
            Dictionary<string, Door>.KeyCollection keys = _exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += (exitName + " ");
            }

            return exitNames;
        }
        public string GetItems()
        {
            string items = "";
            if (_itemList.Count > 0)
            {
                items += "\nItems: ";
                foreach (IItem item in _itemList)
                {
                    items += item.Description + "\n";
                }
            }
            return items;
        }
        public void Drop(IItem item)
        {
            _itemList.Add(item);
        }
        
        public IItem Remove()
        {
            IItem result = Item;
            Item = null;
            return result;
        }
        public IItem RemoveItem(string itemName)
        {
            IItem item = _itemList.Find(_item => _item.Name == itemName);
            _itemList.Remove(item);
            return item;
        }

        public string Description()
        {
            return (_delegate == null ? "" : _delegate.Description() + "\n") + ("You are in the " + this.Tag + "\n *** " + this.GetExits()) + (this.GetItems()) + this.GetMonsters();
        }
        public void AddMonster(Monster monster)
        {
            _monsterList.Add(monster);
        }
        public void RemoveMonster(Monster monster)
        {
            _monsterList.Remove(monster);
        }
        public string GetMonsters()
        {
            string monsterList = "";
            if(_monsterList.Count > 0)
            {
                monsterList += "\nMonsters: ";
                foreach (Monster monster in _monsterList)
                {
                    monsterList += monster.Name + ": " + monster.Description +"\n";
                }
            }
            return monsterList;
        }
        public Player GetPlayer()
        {
            Player player = null;
            if(PlayerList.Count > 0)
            {
                player = PlayerList[0];
            }
            return player;
        }
        public void AddPlayer(Player player)
        {
            _playerList.Add(player);
        }
        public void RemovePlayer(Player player)
        {
            _playerList.Remove(player);
        }
        public bool HasPlayer()
        {
            return PlayerList.Count > 0;
        }
        public bool HasMonster()
        {
            return MonsterList.Count > 0;
        }
    }
}
