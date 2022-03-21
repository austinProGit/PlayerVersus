using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandDropItem : Command
    {
        public CommandDropItem() : base()
        {
            this.Name = "drop";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.DropItem(this.SecondWord);
            }
            else
            {
                player.OutputMessage("Drop what?");
            }
            return false;
        }
    }
}
