using System.Collections;
using System.Collections.Generic;

namespace PlayerVersus
{
    public class CommandHelp : Command
    {
        CommandWords words;

        public CommandHelp() : this(new CommandWords())
        {
        }

        public CommandHelp(CommandWords commands) : base()
        {
            words = commands;
            this.Name = "help";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nI cannot help you with " + this.SecondWord);
            }
            else
            {
                player.OutputMessage("\n\nYour available commands are " + words.Description());
            }
            return false;
        }
    }
}
