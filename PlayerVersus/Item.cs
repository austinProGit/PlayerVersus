using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayerVersus
{
    public class Item : IItem
    {
        private IItem Decorator;
        private float _weight;
        private float _volume;

        public string Name { get; set; }
        public string LongName => Name + (Decorator != null ? $", {Decorator.LongName}" : "");
        public virtual float Volume
        {
            get
            {
                return _volume + (Decorator != null ? Decorator.Volume : 0.0f);
            }
            set
            {
                _volume = value;
            }
        }
        public virtual float Weight
        {
            get
            {
                return _weight + (Decorator != null ? Decorator.Weight : 0.0f);
            }
            set
            {
                _weight = value;
            }
        }
        public string Description => $"{Name} of weight {Weight} and volume {Volume}";
        public virtual string LongDescription => $"{LongName} of weight {Weight} and volume {Volume}";

        public Item() : this("No Name") { }
        public Item(string name) : this(name, 1.0f) { }
        public Item(string name, float weight) : this(name, weight, 1.0f) { }
        public Item(string name, float weight, float volume)
        {
            Name = name;
            _weight = weight;
            _volume = volume;
            Decorator = null;
        }

        public void AddDecorator(IItem decorator)
        {
            if (Decorator != null)
            {
                Decorator.AddDecorator(decorator);
            }
            else
            {
                Decorator = decorator;
            }
        }

        public virtual void AddItem(IItem item) { }

        public virtual IItem RemoveItem(string name)
        {
            return null;
        }

        public virtual void OpenContainer() { }
    }

    class ItemContainer : Item
    {
        private Dictionary<string, IItem> items;
        public bool isOpen;
        override
        public float Weight
        {
            get
            {
                float tempWeight = base.Weight;
                foreach (IItem item in items.Values)
                {
                    tempWeight += item.Weight;
                }
                return tempWeight;
            }
        }
        override
        public float Volume
        {
            get
            {
                float tempVolume = base.Volume;
                foreach(IItem item in items.Values)
                {
                    tempVolume += item.Volume;
                }
                return tempVolume;
            }
        }
        public ItemContainer(string name, float weight) : base(name, weight)
        {
            items = new Dictionary<string, IItem>();
            isOpen = false;
        }
        override
        public void AddItem(IItem item)
        {
            items[item.Name] = item;
        }
        /*override
        public void AddItem(IItem item)
        {
            items.Add(item.Name, item);
        }*/
        public bool ContainsItem(string name)
        {
            bool containsItem = false;
            if (items.ContainsKey(name)){
                containsItem = true;
            }
            return containsItem;
        }
        override
        public IItem RemoveItem(string name)
        {
            IItem item = null;
            if (this.isOpen)
            {
                if (items.ContainsKey(name))
                {
                    items.TryGetValue(name, out item);
                    items.Remove(name);
                    Console.WriteLine(name + " has been removed from the " + this.Name);
                }
                else
                {
                    Console.WriteLine("Could not find that item.");
                }
            }
            else
            {
                Console.WriteLine("Itemcontainer was not open.");
            }
            return item;
        }
        override
        public void OpenContainer()
        {
            isOpen = true;
        }
        override
        public string LongDescription
        {
            get
            {
                string description = base.LongDescription + "\n";
                foreach(IItem item in items.Values)
                {
                    description += "\t- " + item.LongDescription + "\n";
                }
                return description;
            }
        }
    }

    public abstract class Weapon : Item, IWeapon
    {
        protected int _numberBulletsLoaded = 0;
        protected string _ammoType;
        protected int _maximumCapacity;
        protected int _currentAmmoPower;
        public int NumberBulletsLoaded { get { return _numberBulletsLoaded; } set { _numberBulletsLoaded = value; } }
        public int MaximumCapacity { get { return _maximumCapacity; } set { _maximumCapacity = value; } }
        public string AmmoType { get { return _ammoType; } set { _ammoType = value; } }
        public int CurrentAmmoPower { get { return _currentAmmoPower; } set { _currentAmmoPower = value; } }

        public Weapon() : this("Unnamed weapon") { }
        public Weapon(string name) : this(name , 1.0f) { }
        public Weapon(string name, float weight) : this(name, weight, 1.0f) { }
        public Weapon(string name, float weight, float volume) : base(name, weight, volume) { }
        
        public abstract void Load(Ammo ammo);

        public abstract void Fire(Monster monster);
    }

    public class Rifle : Weapon
    {
        public Rifle() : base() { }
        public Rifle(string name) : base(name)
        {
            AmmoType = "rifleAmmo";
            MaximumCapacity = 30;
            NumberBulletsLoaded = 0;
            Weight = 8.0f;
            Volume = 3.0f;
        }        

        override
        public void Load(Ammo ammo)
        {
            if (ammo.AmmoType.Equals("rifleAmmo") )
            {
                RifleAmmo rifleAmmo = (RifleAmmo)ammo;
                if(NumberBulletsLoaded >= MaximumCapacity)
                {
                    Console.WriteLine("Your weapon is already fully loaded.");
                }
                else
                {
                    while(NumberBulletsLoaded < MaximumCapacity)
                    {
                        while (rifleAmmo.HasMoreBullets())
                        {
                            NumberBulletsLoaded += 1;
                            rifleAmmo.PutBulletsInWeapon();
                        }
                        break;
                    }
                    Console.WriteLine(NumberBulletsLoaded + " rounds have been loaded in the weapon.");
                    CurrentAmmoPower = ammo.Power;
                    Console.WriteLine("Current ammo power set to: " + CurrentAmmoPower);
                }
            }
            else
            {
                Console.WriteLine("Thats the wrong item or ammo type.");
            }
        }
        override
        public void Fire(Monster monster)
        {
            while(NumberBulletsLoaded > 0 && monster.Health > 0)
            {
                NumberBulletsLoaded--;
                monster.TakeDamage(CurrentAmmoPower);
            }
        }
    }
    public class Ammo : Item, IAmmo
    {
        protected int _power;
        protected string _ammoType;
        protected int _quantity;

        public int Power { get { return _power; } set { _power = value; } }
        public string AmmoType { get { return _ammoType; } set { _ammoType = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public Ammo() {
            
            AmmoType = _ammoType;
            Power = _power;
            Quantity = _quantity;
        }
        public bool HasMoreBullets()
        {
            bool hasMoreBullets = true;
            if(Quantity <= 0)
            {
                hasMoreBullets = false;
            }
            return hasMoreBullets;
        }
        public void PutBulletsInWeapon()
        {
            if(HasMoreBullets())
            {
                Quantity--;
            }
            else
            {
                Console.WriteLine("Ammo depleted.");

            }
        }
    }
    public class RifleAmmo : Ammo
    {
        public RifleAmmo()
        {
            Name = "rifleAmmo";
            AmmoType = "rifleAmmo";
            Power = 5;
            Quantity = 30;
            Weight = 4.0f;
            Volume = 1.0f;
        }
    }
}