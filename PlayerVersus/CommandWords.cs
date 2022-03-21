using System.Collections;
using System.Collections.Generic;
using System;

namespace PlayerVersus
{
    public class CommandWords
    {
        Dictionary<string, Command> commands;
        private static Command[] commandArray = { new CommandClose(),new CommandDropItem(), new CommandFire(),
        new CommandGo(), new CommandInspect(),  new CommandLoad(), new CommandOpen(),new CommandOpenContainer(),new CommandQuit(),  new CommandRemove(), new CommandSay(), new CommandShowInventory(),
        new CommandTakeItem(), new CommandUnlock(), new CommandUseMedkit() };

        public CommandWords() : this(commandArray)
        {
        }

        public CommandWords(Command[] commandList)
        {
            commands = new Dictionary<string, Command>();
            foreach (Command command in commandList)
            {
                commands[command.Name] = command;
            }
            Command help = new CommandHelp(this);
            commands[help.Name] = help;
            Command back = new CommandRetrace(this);
            commands[back.Name] = back;
        }

        public Command Get(string word)
        {
            Command command = null;
            commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = commands.Keys;
            foreach (string commandName in keys)
            {
                commandNames += " " + commandName;
            }
            return commandNames;


        }
    }
}
