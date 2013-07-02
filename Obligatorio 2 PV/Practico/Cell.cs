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
        public const String ORION = "ORION"; //REVISAR
        public const String SIRIUS = "SIRIUS";
        public const String ENEMY = "ENEMY";
        
        //Cell Complement
        public const String MAX_SPEED = "MAX_SPEED";
        public const String EXTRA_POWER = "EXTRA_POWER";


        public String type;
        public String complement;

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
