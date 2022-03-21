using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class GameWorld
    {
        private static GameWorld instance = null;
        private Room entrance;
        private Room exit;

        public Room Entrance { get { return entrance; } }
        public Room Exit { get { return exit; } }
        private Room magicRoom;
        public Room MagicRoom { get { return magicRoom; } }
        //private Dictionary<Room, WorldEvent> worldEvents;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }
        private GameWorld()
        {
            //worldEvents = new Dictionary<Room, WorldEvent>();
            CreateWorld();
            //NotificationCenter.Instance.AddObserver("PlayerWillExitRoom", PlayerWillExitRoom);
            //NotificationCenter.Instance.AddObserver("PlayerWillEnterRoom", PlayerWillEnterRoom);
            NotificationCenter.Instance.AddObserver("GameClockTick", GameClockTick);
        }
        public void PlayerWillExitRoom(Notification notification)
        {
            Player player = (Player)notification.Object;
            if (player.CurrentRoom.Equals(exit))
            {
                player.OutputMessage("Player left the exit room.");
            }
            //player.OutputMessage("Player will exit " + player.CurrentRoom.Tag);
        }
        /*public void PlayerWillEnterRoom(Notification notification)
        {
            //magic room. opens exit in another room
            Player player = (Player)notification.Object;
            WorldEvent we = null;
            worldEvents.TryGetValue(player.CurrentRoom, out we);
            if (we != null)
            {
                Console.WriteLine("World was changed.");
                we.Execute();
            }
        }*/

        public void GameClockTick(Notification notification)
        {
        }
        private void CreateWorld()
        {
            Room OrientationRoom = new Room("the orientation room.");
            Room VampireRoom = new Room("the room of fangs");
            Room TrollRoom = new Room("the room under the bridge");
            Room ZombiesRoom = new Room("the room of the undead");
            Room FairiesRoom = new Room("the fairy room");
            Room WerewolfRoom = new Room("the room of the full moon");
            Room TrapRoom1 = new Room("the doldrums");
            Room OrkRoom = new Room("the room with drums in the deep");
            Room AmmoRoom = new Room("the room of bullets");
            Room OgreRoom = new Room("the swamp room");
            Room RecoveryRoom = new Room("the recovery room");
            Room HydeRoom = new Room("the Office of Dr. Jekyll");
            Room TrapRoom2 = new Room("the room where time stands still");
            Room CyclopsRoom = new Room("the room with one eye");
            Room Vampire2Room = new Room("the room of lamer fangs");
            Room DragonRoom = new Room("the room of fire");

            Door door = Door.ConnectRooms(OrientationRoom, VampireRoom, "south", "north");
            door.Close();

            door = Door.ConnectRooms(TrollRoom, VampireRoom, "west", "east");
            door.Close();
            door = Door.ConnectRooms(TrollRoom, FairiesRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(VampireRoom, ZombiesRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(FairiesRoom, TrapRoom1, "south", "north");
            ILocking aLock = new RegularLock();
            door.InstallLock(aLock);
            door.Close();
            door.Lock();
            door = Door.ConnectRooms(ZombiesRoom, WerewolfRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(TrapRoom1, WerewolfRoom, "west", "east");
            door.Close();
            door = Door.ConnectRooms(WerewolfRoom, OrkRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(OrkRoom, AmmoRoom, "east", "west");
            door.Close();
            door = Door.ConnectRooms(OrkRoom, OgreRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(AmmoRoom, RecoveryRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(TrollRoom, VampireRoom, "west", "east");
            door.Close();
            door = Door.ConnectRooms(RecoveryRoom, OgreRoom, "west", "east");
            door.Close();
            door = Door.ConnectRooms(OgreRoom, HydeRoom, "south", "north");
            aLock = new RegularLock();
            door.InstallLock(aLock);
            door.Close();
            door.Lock();
            door = Door.ConnectRooms(HydeRoom, TrapRoom2, "east", "west");
            door.Close();
            door = Door.ConnectRooms(TrapRoom2, Vampire2Room, "south", "north");
            door.Close();
            door = Door.ConnectRooms(CyclopsRoom, DragonRoom, "south", "north");
            door.Close();
            door = Door.ConnectRooms(Vampire2Room, CyclopsRoom, "west", "east");
            door.Close();

            TrapRoom newTrapRoom1 = new TrapRoom();
            newTrapRoom1.Password = "potter";
            newTrapRoom1.ContainingRoom = TrapRoom1;
            TrapRoom1.Delegate = newTrapRoom1;

            TrapRoom newTrapRoom2 = new TrapRoom();
            newTrapRoom2.Password = "voldemort";
            newTrapRoom2.ContainingRoom = TrapRoom2;
            TrapRoom2.Delegate = newTrapRoom2;

            ItemContainer chest = new ItemContainer("chest", 50.0f);
            IItem rifle = new Rifle("rifle");
            rifle.AddDecorator(new Item("sling", .2f, .2f));
            rifle.AddDecorator(new Item("optic", .2f, .2f));
            rifle.AddDecorator(new Item("light", .2f, .2f));
            rifle.AddDecorator(new Item("bayonet", .2f, .2f));
            rifle.AddDecorator(new Item("laser", .2f, .2f));
            chest.AddItem(rifle);
            chest.AddItem(new RifleAmmo());
            OrientationRoom.Drop(chest);
            OrientationRoom.Drop(new RifleAmmo());

            ItemContainer medCrate = new ItemContainer("medCrate", 50.0f);
            medCrate.AddItem(new Medkit());
            RecoveryRoom.Drop(medCrate);
            RecoveryRoom.Drop(new Medkit());

            ItemContainer ammoCrate = new ItemContainer("ammoCrate", 50.0f);
            ammoCrate.AddItem(new RifleAmmo());
            AmmoRoom.Drop(ammoCrate);
            AmmoRoom.Drop(new RifleAmmo());
            AmmoRoom.Drop(new RifleAmmo());
            AmmoRoom.Drop(new RifleAmmo());
            AmmoRoom.Drop(new RifleAmmo());

            MasterKey key = new MasterKey(OrientationRoom);
            ZombiesRoom.Drop(key);            

            Monster newVampire = new Vampire("dracula", VampireRoom, "A haunting figure appears. He appears to be a man. " +
                "You see his dark cloak sway gently. His face appears pale, and gaunt, and you quickly realize who and what " +
                "you are dealing with when he announces, \"I am Count Dracula. Thank you for inviting yourself in.", "This is " +
                "not the true end of me. I will return.\"", 10,5000);
            Monster newTroll = new Troll("grendel", TrollRoom, "A nasty little creature appears. You hear a voice whose sound matches" +
                "the look of this hideous monster: \"I a grendel. You should have picked a different bridge to cross.\"","You have " +
                "killed me you monster!", 10,5000);
            Monster newFairy = new Fairy("fairies", FairiesRoom, "You hear an ominous buzzing sound as you enter the room." +
                "At first, you expect bees. As you see the teeth on the swarm of twenty or more fairies in front of you, " +
                "you wish you had kicked a hornet's nest.", "Ahhhhh let us out of here!", 10, 5000);
            Monster newWerewolf = new Werewolf("corvin", WerewolfRoom, "At first you think you see a man, but your eyes seem to " +
                "be deceiving you. He is writhing in pain, making horrible grunts and screams, and seems to be....changing." +
                "As his screams turn to howls, you know that you have stumbled upon a werewolf.", "As the silver does its terrible" +
                " work, the ill-fated man utters his last words: \"I had no choice...you've killed an innocent man.\"", 10,5000);
            Monster newOrk = new Ork("lurtz", OrkRoom, "A horrible stench arises. You see before you a lightly armored beast," +
                "standing nearly seven feet tall and dripping in mud and filth. You hear the ork growl two words: \"Man flesh!\"", "" +
                "Arggggghhhhh", 10, 5000);
            Monster newOgre = new Ogre("shrek", OgreRoom, "As you enter the room, you hear a surprisingly-eloquent ogrish voice" +
                "say, \"Is that you, donkey?\" You see a friendly-looking ogre, standing about six feet tall, smiling at you." +
                " But then his smile fades, and as it does, he menacingly exclaims, \"This is my swamp, and now you're my dinner.\"", 
                "I'll haunt you if you try to date Fionna.", 10, 5000);
            Monster newHyde = new Hyde("hyde", HydeRoom, "A hulking giant of a man.Grotesque veins protrude from his" +
            " monstrous limbs, and his red eyes stare at you maniacally.", "You've slain me! I should have listened" +
            "to the doctor.", 10, 15);
            Monster newVampire2 = new Vampire("edward", Vampire2Room, "A self-absorbed man with the appearance of a teenager appears" +
                "before you. In an incredibly irritating voice, he says, \"I'm Edward. If you made it this far, it means you defeated" +
                " my uncle Dracula. Well, no matter. He was old-fashioned and lame. Now prepare to die.\"", "Noooooo... Now how can " +
                "I make more terrible movies?!", 10,100);
            Monster newCyclops = new Cyclops("polyphemus", CyclopsRoom, "You see a giant monster, at least twenty feet in height." +
                " You see that he only has one eye, but you immediately realize that he has already seen you with it.", "Of course " +
                "you had to go for the eye.....they always go for the eye!", 10, 5000);
            Monster newDragon = new Dragon("smaug", DragonRoom, "Before you even open the door, you can feel heat. As you enter the " +
                "room, you see a terrifying sight: a fully grown dragon at least thirty feet tall. His scales rattle as he turns to " +
                "look menacingly at you. You hear a deep voice rumble, \"I've been waiting for someone to disturb my slumber..." +
                "Finally you have come to entertain me.\"", "After all this time, a man has finally bested me.", 10, 500);
            Monster newWizard = new Wizard("draco", TrapRoom1, "You open the door and see a smug, blonde-haired boy wearing a robe" +
                "smirking at you. Puzzled at his silence, you awkwardly walk passed him and try to open the doors out of the room. You can't." +
                "Suddenly the boy says, \"You'll have to solve my riddle if you want out of here: I'm ugly and scarred, I whine all the " +
                "time. I always make trouble, and step out of line. Pretentious as heck, and smug as can be, I am so unlikeable that " +
                "even my family didn't like me. Who am I? Say my last name.\"", "I'm glad we are on the same page about that loser." +
                " You can pass....for now.", 100000, 100000);
            Monster newWizard2 = new Wizard("harry", TrapRoom2, "You open the door to the room and you again have the feeling that you're" +
                " trapped. Sure enough, there is another young man wearing a robe standing in the center of the room. Unlike before, he boldly" +
                " strolls up to you and introduces himself as none other than Harry Potter... You are taken aback by his almost brash friendliness " +
                "in the face of your present dire circumstances, and you privately think to yourself, \" Man that Draco guy was totally right " +
                "about how pretentious this guy is.\" Suddenly, Harry says, \"Sorry, but I've gotta do this. Just part of the job, ya know. " +
                "Here is the riddle if you wanna continue on your quest: 'I tried my hardest to be evil and cool, but at the end of the day " +
                "my life's work was to take over a high school. Who am I?'\"","Great job, buddy! Have a great time with the rest of your adventure!"
                , 100000, 100000);
            Monster newZombie = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 2, 5);
            Monster newZombie2 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 2, 5);
            Monster newZombie3 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 1, 1);
            Monster newZombie4 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 3, 10);
            Monster newZombie5 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 3, 10);
            Monster newZombie6 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 3, 10);
            Monster newZombie7 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 3, 10);
            Monster newZombie8 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 3, 10);
            Monster newZombie9 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 2, 5);
            Monster newZombie10 = new Zombie("zombie", ZombiesRoom, "Another member of the horde of the undead approaches.", "The zombie grunts and " +
              "finally dies...hopefully for the last time.", 1, 1);

            entrance = OrientationRoom;
            exit = DragonRoom;
            magicRoom = AmmoRoom;
        }
        private interface WorldEvent
        {
            void Execute();
        }
        private class WorldChange : WorldEvent
        {
            public Room RoomA { get; set; }
            public Room RoomB { get; set; }
            public string AtoB { get; set; }
            public string BtoA { get; set; }

            public WorldChange(Room roomA, Room roomB, string aToB, string bToA)
            {
                RoomA = roomA;
                RoomB = roomB;
                AtoB = aToB;
                BtoA = bToA;
            }
            public void Execute()
            {
                Door.ConnectRooms(RoomA, RoomB, AtoB, BtoA);
            }
        }
    }
}
