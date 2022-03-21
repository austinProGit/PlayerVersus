using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    public interface ILocking
    {
        bool IsLocked { get; }
        bool IsUnlocked { get; }
        bool MayOpen { get; }
        bool MayClose { get; }
        void Lock();
        void Unlock();
    }

    public interface IItem
    {
        string Name { get; set; }
        string LongName { get; }
        float Weight { get; set; }
        float Volume { get; set; }
        string Description { get; }
        string LongDescription { get; }
        void AddDecorator(IItem decorator);
        void AddItem(IItem item);
        IItem RemoveItem(string name);
        void OpenContainer();
    }

    public interface IRoomDelegate
    {
        Door GetExit(string exitName);
        Room ContainingRoom { get; set; }
        Dictionary<string, Door> Exits { get; set; }
        string Description();
    }

    public interface IAmmo
    {
        int Power { get; set; }
        string AmmoType { get; set; }
        int Quantity { get; set; }
    }

    public interface IEnemy
    {
        void TakeDamage(float damage);
    }

    public interface IMonster
    {
        void Move();
    }
    public interface IWeapon
    {
        void Load(Ammo ammo);
        void Fire(Monster monster);
    }
}