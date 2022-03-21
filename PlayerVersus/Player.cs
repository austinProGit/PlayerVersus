using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayerVersus
{
    public class Player
    {
        private Room _currentRoom = null;
        private List<IItem> _inventory;
        private float _maxWeight;
        private float _currentWeight;
        private float _maxVolume;
        private float _currentVolume;
        private Stack<Room> _pathTraveled;
        private int _health;
        private int _maxHealth;
        
        public Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
            set
            {
                _currentRoom.RemovePlayer(this);
                _currentRoom = value;
                _currentRoom.AddPlayer(this);
            }
        }

        public Player(Room room)
        {
            _pathTraveled = new Stack<Room>();
            _currentRoom = room;
            _inventory = new List<IItem>();
            _currentWeight = 0.0f;
            _maxWeight = 80.0f;
            _currentVolume = 0.0f;
            _maxVolume = 80.0f;
            _health = 99;
            _maxHealth = 100;
            NotificationCenter.Instance.AddObserver("MonsterMoved", MonsterMoved);
        }
        public int Health { get { return _health; } set { _health = value; } }
        public void WalkTo(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            
            if (door != null)
            {
                if(door.IsOpen)
                {
                    Room nextRoom = door.OtherRoom(CurrentRoom);
                    Notification notification = new Notification("PlayerWillExitRoom", this);
                    NotificationCenter.Instance.PostNotification(notification);
                    _pathTraveled.Push(_currentRoom);
                    CurrentRoom.RemovePlayer(this);
                    this._currentRoom = nextRoom;
                    CurrentRoom.AddPlayer(this);
                    this.OutputMessage("\n" + this._currentRoom.Description());
                }
                else
                {
                    this.OutputMessage("\nThe " + direction + " door is closed.");
                }
            }
            else
            {
                this.OutputMessage("\nThere is no door on " + direction);
            }
        }

        public void OpenContainer(string containerName)
        {
            IItem itemContainer = (ItemContainer)(this._currentRoom.ItemList.Find(_itemContainer => _itemContainer.Name == containerName));
            itemContainer = (ItemContainer)itemContainer;
            if (this._currentRoom.ItemList.Count == 0)
            {
                this.OutputMessage("There are no items in this room.");
            }
            else
            {
                if (itemContainer != null)
                {
                    this.OutputMessage(itemContainer.LongDescription);
                    itemContainer.OpenContainer();
                }
                else
                {
                    this.OutputMessage("That container is not in this room.");
                }
            }
        }
        public void RemoveItem(string itemName, string containerName)
        {
            IItem item = this._currentRoom.ItemList.Find(_itemContainer => _itemContainer.Name == containerName);

            if(item != null)
            {
                ItemContainer itemContainer = (ItemContainer)item;
                if (itemContainer.ContainsItem(itemName))
                {
                    this._currentRoom.Drop(itemContainer.RemoveItem(itemName));
                }
                else
                {
                    Console.WriteLine("Unable to locate that item.");
                }
            }
            else
            {
                this.OutputMessage("Unable to locate that container.");
            }
        }
        public void Open(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            if(door != null)
            {
                if(door.IsClosed)
                {
                    door.Open();
                    if(door.IsOpen)
                    {
                        OutputMessage($"\nThe door on {direction} is now open.");
                    }
                    else
                    {
                        this.OutputMessage("\nThe " + direction + " door is still closed");
                    }    
                }
                else
                {
                    this.OutputMessage("\nThe " + direction + " door is already open.");
                }
            }
            else
            {
                this.OutputMessage("\nThere is no door on " + direction);
            }
        }
        public void Close(string direction)
        {
            Door door = this._currentRoom.GetExit(direction);
            if(door != null)
            {
                if(door.IsOpen)
                {
                    door.Close();
                    Dictionary<string, Object> userInfo = new Dictionary<string, object>();
                    userInfo["door"] = door;
                    Notification notification = new Notification("PlayerDidCloseDoor", this, userInfo);
                    NotificationCenter.Instance.PostNotification(notification);

                    OutputMessage($"\nThe door {direction} is now closed.");
                }
                else
                {
                    OutputMessage($"\nThe door {direction} is already closed.");
                }
            }
            else
            {
                OutputMessage($"\nThere is no door to the {direction}");
            }
        }
        public void Unlock(string direction, string keyName)
        {
            Door door = this._currentRoom.GetExit(direction);
            if (door != null)
            {
                Console.WriteLine("Found door.");
                if (door.IsLocked)
                {
                    Console.WriteLine("Door was found to be locked.");
                    IItem item = this._inventory.Find(_key => _key.Name == keyName);
                    if(item != null)
                    {
                        Console.WriteLine("Found key");
                        MasterKey masterKey = (MasterKey)item;
                        Dictionary<string, Object> userInfo = new Dictionary<string, object>();
                        userInfo["door"] = door;
                        OutputMessage($"\nThe door {direction} is now unlocked.");
                        masterKey.Unlock(door);
                    }
                }
                else
                {
                    OutputMessage($"\nThe door {direction} is already unlocked.");
                }
            }
            else
            {
                OutputMessage($"\nThere is no door to the {direction}");
            }
        }
    
        public void Inspect(string itemName)
        {
            IItem item = this._currentRoom.RemoveItem(itemName);
            if (item != null)
            {
                this.OutputMessage("\n" + item.LongDescription);
                CurrentRoom.Drop(item);
            }
            else
            {
                this.OutputMessage("\nThis item is not in the room.");
            }
        }

        public void Say(string word)
        {
            OutputMessage("\n" + word + "\n");
            Dictionary<string, Object> userInfo = new Dictionary<string, object>();
            userInfo["word"] = word;
            Notification notification = new Notification("PlayerDidSayWord", this, userInfo);
            NotificationCenter.Instance.PostNotification(notification);
        }
        public void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void TakeItem(string itemName)
        {
            IItem item = this._currentRoom.ItemList.Find(_item => _item.Name == itemName);
            if(this._currentRoom.ItemList.Count == 0)
            {
                this.OutputMessage("There are no items in this room.");
            }
            else
            {
                if (item != null)
                {
                    if(item.Weight + this._currentWeight > this._maxWeight)
                    {
                        this.OutputMessage("This item exceeds the maximum weight you can carry!");
                    }
                    if(item.Volume + this._currentVolume > this._maxVolume)
                    {
                        this.OutputMessage("This item exceeds the maximum volume you can carry!");
                    }
                    if ((item.Weight + this._currentWeight <= this._maxWeight)&&(item.Volume + this._currentVolume <= this._maxVolume))
                    {
                        this._inventory.Add(item);
                        this._currentWeight += item.Weight;
                        this._currentVolume += item.Volume;
                        this._currentRoom.ItemList.Remove(item);
                        this.OutputMessage("You now have " + item.Name);
                    }
                    else
                    {
                        this.OutputMessage("You were unable to pick up this item.");
                    }
                }
                else
                {
                    this.OutputMessage("That item is not in this room, but there is at least one item in this room. Did you enter the command correctly?");
                }
            }
        }
        public void DropItem(string itemName)
        {
            if(_inventory.Count > 0)
            {
                IItem item = this._inventory.Find(_item => _item.Name == itemName);
                if(item != null)
                {
                    this._currentRoom.ItemList.Add(item);
                    this._inventory.Remove(item);
                    this._currentWeight -= item.Weight;
                    this._currentVolume -= item.Volume;
                    this.OutputMessage("You have removed the " + item.Name + " from your inventory and dropped it in the " + this.CurrentRoom.Tag);
                }
                else
                {
                    this.OutputMessage("The " + itemName + " is not in your inventory");
                }
            }
            else
            {
                this.OutputMessage("Your inventory is empty.");
            }
        }
        public void ShowInventory()
        {
            this.OutputMessage("Current Room: " + this.CurrentRoom.Tag);
            this.OutputMessage("\nCurrent Inventory: ");
            foreach (IItem item in _inventory)
            {
                this.OutputMessage(item.Description);
                if (item.Name == "rifle" || item.Name == "pistol")
                {
                    Weapon weapon = (Weapon)item;
                    string currentLoadout = "";
                    this.OutputMessage(currentLoadout += "Current bullets loaded in weapon: " + weapon.NumberBulletsLoaded);
                }
                if(item.Name == "rifleAmmo" || item.Name == "pistolAmmo")
                {
                    Ammo ammo = (Ammo)item;
                    string currentAmmoCount = "";
                    this.OutputMessage(currentAmmoCount += "Current bullets in magazine: " + ammo.Quantity);
                }
            }
            this.OutputMessage("\nHealth: " + this._health);
            this.OutputMessage("\nCurrent Weight: " + this._currentWeight+ "\nMax Weight: " + this._maxWeight + "\nAvailable Weight: " +
                (this._maxWeight - this._currentWeight));
            this.OutputMessage("\nCurrent Volume: " + this._currentVolume + "\nMax Volume: " + this._maxVolume + "\nAvailable Volume: " +
                (this._maxVolume - this._currentVolume));
        }
        public void Retrace()
        {
            if(_pathTraveled.Count > 0)
            {
                Room previousRoom = _pathTraveled.Pop();
                this._currentRoom = previousRoom;
                this.OutputMessage("\nYou are now in " + this._currentRoom.Tag);
            }
            else
            {
                this.OutputMessage("\nYou are at your starting point.");
            }
        }
        public void UseMedkit(string itemName)
        {
            IItem item = _inventory.Find(_item => _item.Name == itemName);
            if(item != null)
            {
                Medkit medkit = (Medkit)item;
                if (Health == 100)
                {
                    OutputMessage("Your health is already full.");
                }
                else
                {
                    if((Health + medkit.HP) >= _maxHealth)
                    {
                        Health = _maxHealth;
                    }
                    else
                    {
                        Health += medkit.HP;
                    }
                    this._inventory.Remove(item);
                    this.CurrentRoom.ItemList.Remove(item);
                    this.OutputMessage("Your health is now " + Health + ".");
                }
            }
            else
            {
                this.OutputMessage("No medkits available for use.");
            }
        }
        public void Load(string weaponName, string ammoName)
        {
            Weapon weapon = (Weapon)_inventory.Find(_weapon => _weapon.Name == weaponName);
            if(weapon != null)
            {
                this.OutputMessage("weapon name: " + weapon.Name);
                if(weapon.Name == "rifle")
                {
                    Rifle rifle = (Rifle)weapon;
                }
                Ammo ammo = (Ammo)_inventory.Find(_ammo => _ammo.Name == ammoName);
                if (ammo != null)
                {
                    if (ammo.Name == "rifleAmmo")
                    {
                        RifleAmmo rifleAmmo = (RifleAmmo)ammo;
                    }
                }
                else
                {
                    this.OutputMessage("Unable to locate ammo to load in weapon.");
                }
                this.OutputMessage("Loading the weapon.");
                weapon.Load(ammo);
                }
            else
            {
                this.OutputMessage("Unable to locate weapon to load.");
            }
        }
        public void Fire(string weaponName, string monsterName)
        {
            IItem item = _inventory.Find(_weapon => _weapon.Name == weaponName);
            
            Monster monster = this._currentRoom.MonsterList.Find(_monster => _monster.Name == monsterName);
            if(monster == null)
            {
                this.OutputMessage("Having trouble finding that monster.");
            }
            if(item != null)
            {
                this.OutputMessage("You raise your weapon and prepare to fire.");
            }
            else
            {
                this.OutputMessage("Could not locate that weapon.");
            }
            if(monster!= null)
            {
                this.OutputMessage("You see the monster.");
            }
            else
            {
                this.OutputMessage("Unable to target that monster.");
            }
            if(item != null && monster != null)
            {
                
                if (weaponName == "rifle")
                {
                    Rifle rifle = (Rifle) item;
                    rifle.Fire(monster);
                }
                if(monsterName == "smaug" && monster.Health <= 0)
                {
                    GameOver(true);
                }
            }
        }
        public void MonsterMoved(Notification notification){
            Monster monster = (Monster)notification.Object;
            this.OutputMessage(monster.Name + " has moved to " + monster.CurrentRoom.Tag + "!");
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            this.OutputMessage("You've taken damage! Current health: " + Health);
            if(Health <= 0)
            {
                GameOver(false);
            }
        }
        public void GameOver(bool win)
        {
            if(win == true)
            {
                this.OutputMessage("The worst of the monsters is slain. " +
                    "You step over Smaug's smouldering corpse and move towards" +
                    "your exit. Finally.");
                Environment.Exit(0);
            }
            else
            {
                this.OutputMessage("You have died. Just food for the monsters...");
                Environment.Exit(0);
            }
        }
    }
}