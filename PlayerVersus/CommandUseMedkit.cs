using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandUseMedkit : Command
    {
        public CommandUseMedkit() : base()
        {
            this.Name = "use-medkit";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.UseMedkit(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nUse what?");
            }
            return false;
        }
    }
}
