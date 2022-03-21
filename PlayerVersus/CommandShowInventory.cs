using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandShowInventory : Command
    {
        public CommandShowInventory() : base()
        {
            this.Name = "inventory";
        }

        override
        public bool Execute(Player player)
        {
            bool answer = false;
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nShow inventory is a one-word command!");
            }
            player.ShowInventory();
            return answer;
        }
    }
}
