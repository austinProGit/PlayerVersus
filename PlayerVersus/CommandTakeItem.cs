using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandTakeItem : Command
    {
        public CommandTakeItem() : base()
        {
            this.Name = "take";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.TakeItem(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nTake what?");
            }
            return false;
        }
    }
}
