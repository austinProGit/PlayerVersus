using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class CommandOpenContainer : Command
    {
        public CommandOpenContainer() : base()
        {
            this.Name = "open-container";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OpenContainer(this.SecondWord);
            }
            else
            {
                player.OutputMessage("Open what container?");
            }
            return false;
        }
    }
}
