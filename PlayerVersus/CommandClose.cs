using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandClose : Command
    {
        public CommandClose() : base()
        {
            this.Name = "close";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Close(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nClose what?");
            }
            return false;
        }
    }
}
