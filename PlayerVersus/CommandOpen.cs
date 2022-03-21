using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandOpen : Command
    {
        public CommandOpen() : base()
        {
            this.Name = "open";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Open(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nOpen what?");
            }
            return false;
        }
    }
}
