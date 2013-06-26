using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Enemy
    {
        //ENEMY
        public const String ORION = "ORION";
        public const String SIRIUS = "SIRIUS";
        public const String LEPUS = "LEPUS";

        public String type;
        public int position_X;
        public int position_Z;

        public Enemy(String type, int x, int z) {
            this.type = type;
            this.position_X = x;
            this.position_Z = z;
        }

        public Enemy() {
            this.type = Enemy.ORION;
            this.position_X = 0;
            this.position_Z = 0;
        }
    }
}
