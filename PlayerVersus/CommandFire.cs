using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandFire : Command
    {
        public CommandFire() : base()
        {
            this.Name = "fire";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.Fire(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.OutputMessage("Fire what at what Monster?");
            }
            return false;
        }
    }
}
