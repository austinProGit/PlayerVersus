using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    public class CommandRetrace : Command
    {
        CommandWords words;

        public CommandRetrace() : this(new CommandWords())
        {
        }

        public CommandRetrace(CommandWords commands) : base()
        {
            words = commands;
            this.Name = "back";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nI can only backtrack room by room.");
            }
            else
            {
                player.Retrace();
            }
            return false;
        }
    }
}
