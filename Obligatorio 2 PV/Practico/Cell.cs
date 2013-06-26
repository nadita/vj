using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Cell
    {
        //Cell Types
        public const String BLOCK = "BLOCK";
        public const String POWERUP = "POWERUP";
        public const String NONE = "NONE";
        
        //Cell Complement
        public const String MAX_SPEED = "MAX_SPEED";
        public const String EXTRA_POWER = "EXTRA_POWER";


        public String type; //None, Brick, Concrete
        public String complement; //None, Bomb, PowerUp_Speed, PowerUp_Super

        public Cell(String type, String complement) {
            this.type = type;
            this.complement = complement;
        }

        public Cell() {
            this.type = Cell.NONE; //Default Value
            this.complement = Cell.NONE; //Default Value
        }
    }
}
