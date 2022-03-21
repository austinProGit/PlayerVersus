using System.Collections;
using System.Collections.Generic;

namespace PlayerVersus
{
    public class CommandGo : Command
    {

        public CommandGo() : base()
        {
            this.Name = "go";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.WalkTo(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nGo where?");
            }
            return false;
        }
    }
}
