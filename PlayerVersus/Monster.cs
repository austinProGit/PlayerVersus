using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    public abstract class Monster : IMonster
    {
        
        protected string _description;
        protected Room _currentRoom = null;
        protected int _health;
        protected int _attackHP;
        protected string _name;
        protected string _deathSpeech;
        protected int _takeDamage;
        protected int _ticksToAttack;
        protected int _ticksToMove;
        protected int _currentTicks;

        public string Name { get { return _name; }set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        public int Health { get { return _health; } set { _health = value; } }
        public int AttackHP { get { return _attackHP; }set { _attackHP = value; } }
        public string DeathSpeech { get { return _deathSpeech; }set { _deathSpeech = value; } }
        public int TicksToAttack { get { return _ticksToAttack; } set { _ticksToAttack = value; } }
        public int TicksToMove { get { return _ticksToMove; } set { _ticksToMove = value; } }
        public int CurrentTicks { get { return _currentTicks; } set { _currentTicks = value; } }

        public Monster(Room currentRoom)
        {
            CurrentRoom = currentRoom;
        }

        public Monster(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove)
        {
            Health = _health;
            AttackHP = _attackHP;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
            CurrentTicks = 0;
            Name = name;
            CurrentRoom = currentRoom;
            Description = description;
            DeathSpeech = deathSpeech;
            CurrentRoom.AddMonster(this);
            NotificationCenter.Instance.AddObserver("GameClockTick", ExecuteOnClockTick);
        }
        public Monster(string name, Room currentRoom, string description)
        {
            Health = _health;
            AttackHP = _attackHP;
            DeathSpeech = _deathSpeech;
            TicksToAttack = _ticksToAttack;
            TicksToMove = _ticksToMove;
            CurrentTicks = 0;
            Name = name;
            CurrentRoom = currentRoom;
            Description = description;
            CurrentRoom.AddMonster(this);
            NotificationCenter.Instance.AddObserver("GameClockTick", ExecuteOnClockTick);
        }
        public Monster() { }
        public Monster(string name, Room currentRoom)
        {
            Health = _health;
            AttackHP = _attackHP;
            DeathSpeech = _deathSpeech;
            Description = _description;
            TicksToAttack = _ticksToAttack;
            TicksToMove = _ticksToMove;
            CurrentTicks = 0;
            Name = name;
            CurrentRoom = currentRoom;
            CurrentRoom.AddMonster(this);
            NotificationCenter.Instance.AddObserver("GameClockTick", ExecuteOnClockTick);
        }

        public void Move()
        {
            string[] exitArray = CurrentRoom.GetExits().Split(" ");
            Random random = new Random();
            int randomNum = random.Next(100);
            if (randomNum > 0)
            {
                string monsterExit = exitArray[random.Next(exitArray.Length - 1)];
                Door door = CurrentRoom.GetExit(monsterExit);
                
                if (door != null)
                {
                    Room newRoom = door.OtherRoom(CurrentRoom);
                    if(newRoom.Delegate == null)
                    {                    
                        if (door.IsOpen)
                        {
                            CurrentRoom.RemoveMonster(this);
                            CurrentRoom = newRoom;
                            CurrentRoom.AddMonster(this);
                        }
                        else
                        {
                            if (door.IsLocked)
                            {
                                door.Unlock();
                                door.Open();
                                CurrentRoom.RemoveMonster(this);
                                door.Close();
                                door.Lock();
                                CurrentRoom = newRoom;
                                CurrentRoom.AddMonster(this);
                            }
                            else
                            {
                                Open(door);
                                CurrentRoom.RemoveMonster(this);
                                Close(door);
                                CurrentRoom = newRoom;
                                CurrentRoom.AddMonster(this);
                            }
                        }
                        if(this.Name != "zombie")
                        {
                            Notification notification = new Notification("MonsterMoved", this);
                            NotificationCenter.Instance.PostNotification(notification);
                        }
                    }
                }
            }
        }
        public void Die() {
            Console.WriteLine(this._deathSpeech);
            this._currentRoom.MonsterList.Remove(this);
            NotificationCenter.Instance.RemoveObserver("GameClockTick", ExecuteOnClockTick);
        }
        public void ExecuteOnClockTick(Notification notification)
        {
            if(CurrentTicks >= TicksToAttack)
            {
                if (CurrentRoom.HasPlayer())
                {
                    this.Attack(this.CurrentRoom.GetPlayer());
                    Console.WriteLine(this.Name + " is currently attacking you!");
                }
                else if (!CurrentRoom.HasPlayer())
                {
                    if(CurrentTicks >= TicksToMove)
                    {
                        this.Move();
                        CurrentTicks = 0;
                    }
                }
            }
            else if(CurrentTicks >= TicksToMove)
            {
                this.Move();
                CurrentTicks = 0;
            }
            CurrentTicks++;
        }
        public void Attack(Player player)
        {
            if (CurrentRoom.HasPlayer())
            {
                player.TakeDamage(AttackHP);
            }
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            this.CurrentRoom.GetPlayer().OutputMessage(this.Name + " is taking damage. His current health is : " + Health);
            if (Health <= 0)
            {
                this.Die();
            }
        }
        public void Open(Door door)
        {
            if (door != null)
            {
                if (door.IsClosed)
                {
                    door.Open();
                }
            }
        }
        public void Close(Door door)
        {
            if (door != null)
            {
                if (door.IsOpen)
                {
                    door.Close();
                }
            }
        }
        public void Unlock(Door door)
        {
            if (door != null)
            {
                if (door.IsLocked)
                {
                    door.Unlock();
                }
            }
        }
    }
    public class Cyclops : Monster
    {
        public Cyclops(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 25;
            AttackHP = 5;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Cyclops(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me! If only I had a second eye...";
            Health = 25;
            AttackHP = 5;
        }
        public Cyclops(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am the cyclops.";
            DeathSpeech = "You've slain me! If only I had a second eye.";
            Health = 25;
            AttackHP = 5;
        }
    }
    public class Dragon : Monster
    {
        public Dragon(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 50;
            AttackHP = 50;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Dragon(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me! After so many have tried, you've finally done it.";
            Health = 50;
            AttackHP = 50;
        }
        public Dragon(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am a dragon.";
            DeathSpeech = "You've slain me! After so many have tried, you've finally done it.";
            Health = 50;
            AttackHP = 50;
        }
    }
    public class Fairy : Monster
    {
        public Fairy(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 2;
            AttackHP = 2;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Fairy(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've killed us all!";
            Health = 2;
            AttackHP = 2;
        }
        public Fairy(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "We are the fairies.";
            DeathSpeech = "You've killed us all!";
            Health = 2;
            AttackHP = 2;
        }
    }
    public class Ogre : Monster
    {
        public Ogre(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 10;
            AttackHP = 5;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Ogre(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me!";
            Health = 10;
            AttackHP = 5;
        }
        public Ogre(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am the ogre, and this is my swamp.";
            DeathSpeech = "You've slain me!";
            Health = 10;
            AttackHP = 5;
        }
    }
    public class Hyde : Monster
    {
        public Hyde(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 20;
            AttackHP = 5;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove; 
        }

        public Hyde(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me! I should have listened to Dr. Jekyl.";
            Health = 20;
            AttackHP = 5;
            TicksToAttack = 5;
        }
        public Hyde(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am Mr. Hyde.";
            DeathSpeech = "You've slain me! I should have listened to Dr. Jekyl.";
            Health = 20;
            AttackHP = 5;
            TicksToAttack = 5;
        }
    }
    public class Ork : Monster
    {
        public Ork(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 10;
            AttackHP = 5;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Ork(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain the ork.";
            Health = 10;
            AttackHP = 5;
        }
        public Ork(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am an ork.";
            DeathSpeech = "You've slain the ork.";
            Health = 10;
            AttackHP = 5;
        }
    }
    public class Troll : Monster
    {
        public Troll(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 50;
            AttackHP = 10;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Troll(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me! Who is the real troll here?";
            Health = 50;
            AttackHP = 10;
        }
        public Troll(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "People call me the troll.";
            DeathSpeech = "You've slain me! Who is the real troll here?";
            Health = 50;
            AttackHP = 10;
        }
    }
    public class Vampire : Monster
    {
        public Vampire(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 10;
            AttackHP = 12;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Vampire(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've killed me! Maybe you're the real blood sucker here. Constructor 1";
            Health = 10;
            AttackHP = 12;
        }
        public Vampire(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am Count Dracula.";
            DeathSpeech = "You've killed me! Maybe you're the real blood sucker here. Constructor 2";
            Health = 10;
            AttackHP = 12;
        }
    }
    public class Werewolf : Monster
    {
        public Werewolf(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 10;
            AttackHP = 5;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Werewolf(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've killed me! Betcha wouldn't have without all that silver.";
            Health = 10;
            AttackHP = 5;
        }
        public Werewolf(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am a werewolf.";
            DeathSpeech = "You've killed me! Betcha wouldn't have without all that silver.";
            Health = 10;
            AttackHP = 5;
        }
    }
    public class Wizard : Monster
    {
        public Wizard(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 100000;
            AttackHP = 0;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Wizard(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "You've slain me!";
            Health = 100000;
            AttackHP = 0;
        }
        public Wizard(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "I am the wizard.";
            DeathSpeech = "You've slain me!";
            Health = 100000;
            AttackHP = 0;
        }
    }
    public class Zombie : Monster
    {
        public Zombie(string name, Room currentRoom, string description, string deathSpeech, int ticksToAttack, int ticksToMove) : base(name, currentRoom, description, deathSpeech, ticksToAttack, ticksToMove)
        {
            Health = 10;
            AttackHP = 2;
            TicksToAttack = ticksToAttack;
            TicksToMove = ticksToMove;
        }

        public Zombie(string name, Room currentRoom, string description) : base(name, currentRoom, description)
        {
            DeathSpeech = "The zombie grunts and finally dies...hopefully for the last time.";
            Health = 10;
            AttackHP = 2;
        }
        public Zombie(string name, Room currentRoom) : base(name, currentRoom)
        {
            Description = "Another member of the horde of the undead approaches.";
            DeathSpeech = "The zombie grunts and finally dies...hopefully for the last time.";
            Health = 10;
            AttackHP = 2;
        }
        public Zombie(Room currentRoom)
        {
            Name = "zombie";
            CurrentRoom = currentRoom;
            Description = "Another member of the horde of the undead approaches.";
            DeathSpeech = "The zombie grunts and finally dies...hopefully for the last time.";
            Health = 10;
            AttackHP = 2;
            TicksToAttack = 2;
            TicksToMove = 1;
            CurrentTicks = 0;
        }
    }
}
