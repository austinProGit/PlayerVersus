using System.Collections;
using System.Collections.Generic;

namespace PlayerVersus
{
    public class CommandQuit : Command
    {

        public CommandQuit() : base()
        {
            this.Name = "quit";
        }

        override
        public bool Execute(Player player)
        {
            bool answer = true;
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nI cannot quit " + this.SecondWord);
                answer = false;
            }
            return answer;
        }
    }
}
