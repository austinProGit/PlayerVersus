using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerVersus
{
    class Medkit : Item
    {
        private int _hp;
        public int HP { get { return _hp; } set { _hp = value; } }
        public Medkit()
        {
            Weight = 5.0f;
            Volume = 5.0f;
            Name = "medkit";
            HP = 50;
        }
    }
}
