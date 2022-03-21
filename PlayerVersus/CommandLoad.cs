using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandLoad : Command
    {
        public CommandLoad() : base()
        {
            this.Name = "load";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.Load(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.OutputMessage("Load what with what?");
            }
            return false;
        }
    }
}
