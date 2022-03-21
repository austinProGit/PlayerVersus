using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandRemove : Command
    {
        public CommandRemove() : base()
        {
            this.Name = "remove";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasThirdWord())
            {
                player.RemoveItem(this.SecondWord, this.ThirdWord);
            }
            else
            {
                player.OutputMessage("Remove what from where?");
            }
            return false;
        }
    }
}
