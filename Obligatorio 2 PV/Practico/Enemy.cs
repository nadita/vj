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

        public String type { get; set; }
        public int position_X { get; set; }
        public int position_Z {get; set;}
        public int inicial_x { get; set; }
        public int inicial_z { get; set; }
        public int image {get; set;}
        public String enemy_direction {get; set;}
        public int sphere { get; set; }

        public Enemy(String type, int x, int z) {
            this.type = type;
            this.position_X = x;
            this.position_Z = z;
            this.inicial_x = x;
            this.inicial_z = z;
            this.image = 0;
            this.sphere = 0;
            this.enemy_direction = Constants.RIGHT;
        }

        public Enemy() {
            this.type = Enemy.ORION;
            this.position_X = 0;
            this.position_Z = 0;
            this.inicial_x = 0;
            this.inicial_z = 0;
            this.image = 0;
            this.sphere = 0;
            this.enemy_direction = Constants.RIGHT;
        }
    }
}
