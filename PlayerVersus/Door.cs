using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    public class RegularLock : ILocking
    {
        public bool IsLocked { get; private set; }
        public bool IsUnlocked => !IsLocked;
        public bool MayOpen => IsUnlocked;
        public bool MayClose => true;
        public RegularLock()
        {
            IsLocked = false;
        }
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
    }

    public class Door : ILocking
    {
        private Room room1;
        private Room room2;
        bool closed;
        public bool IsClosed { get { return closed; } }
        public bool IsOpen { get { return !closed; } }
        private ILocking _lock;
        public Door(Room room1, Room room2)
        {
            this.room1 = room1;
            this.room2 = room2;
            closed = false;
            _lock = null;
        }

        public Room OtherRoom(Room thisRoom)
        {
            return thisRoom == room1 ? room2 : room1;
        }
        public void Close()
        {

            if (_lock != null)
            {
                closed = _lock.MayClose;
            }
            else
            {
                closed = true;
            }
        }
        public void Open()
        {
            if (_lock != null)
            {
                closed = !_lock.MayOpen;
            }
            else
            {
                closed = false;
            }
        }
        public bool IsLocked { get { return _lock != null ? _lock.IsLocked : false; } }
        public bool IsUnlocked { get { return _lock != null ? _lock.IsUnlocked : true; } }
        public bool MayOpen { get { return _lock != null ? _lock.MayOpen : true; } }
        public bool MayClose { get { return _lock != null ? _lock.MayClose : false; } }
        public void Lock()
        {
            if(_lock != null)
            {
                _lock.Lock();
            }
        }
        public void Unlock()
        {
            if(_lock != null)
            {
                _lock.Unlock();
            }
        }
        public void InstallLock(ILocking newLock)
        {
            _lock = newLock;
        }
        public static Door ConnectRooms(Room RoomA, Room RoomB, string BtoA, string AtoB)
        {
            Door door = new Door(RoomA, RoomB);
            RoomA.SetExit(AtoB, door);
            RoomB.SetExit(BtoA, door);
            return door;
        }
    }
    public class MasterKey : Item
    {
        private Room _currentRoom;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }
        public MasterKey(Room currentRoom)
        {
            CurrentRoom = currentRoom;
            Name = "key";
            Weight = 1.0f;
            Volume = 1.0f;
        }
        public void Unlock(Door door)
        {
            door.Unlock();
        }
    }
}