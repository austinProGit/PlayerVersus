using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandUnlock : Command
    {
        public CommandUnlock() : base()
        {
            this.Name = "unlock";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.Unlock(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.OutputMessage("Unlock what with what?");
            }
            return false;
        }
    }
}
