using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayerVersus
{
    public class Game
    {
        Player player;
        Parser parser;
        GameClock gc;

        public Game()
        {
            gc = new GameClock(5000);
            parser = new Parser(new CommandWords());
            player = new Player(GameWorld.Instance.Entrance);
        }

        public void Play()
        {
            bool finished = false;
            while (!finished)
            {
                Console.Write("\n>");
                Command command = parser.ParseCommand(Console.ReadLine());
                if (command == null)
                {
                    Console.WriteLine("I don't understand...");
                }
                else
                {
                    finished = command.Execute(player);
                }
            }
        }

        public void Start()
        {
            player.OutputMessage(Welcome());
        }

        public void End()
        {
            player.OutputMessage(Goodbye());
        }

        public string Welcome()
        {
            return "You look around you. You see that you're in a dimly lit room with a nothing but a chest in the center of the room. You don't know how or why" +
                    "you got here. All you know is that you have to go forward to escape. \n.\n\nType 'help' if you need help." + player.CurrentRoom.Description();
        }

        public string Goodbye()
        {
            return "\nYou see your exit. The bright light is almost blinding. You've done it. You've made it. You've survived. \n";
        }
    }
}
