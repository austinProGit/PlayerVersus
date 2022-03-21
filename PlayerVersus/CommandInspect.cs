using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandInspect : Command
    {
        public CommandInspect() : base()
        {
            this.Name = "inspect";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Inspect(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nInspect what?");
            }
            return false;
        }
    }
}
