using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Cell
    {
        public String type { get; set; } //None, Brick, Concrete
        public String complement { get; set; } //None, Bomb, PowerUp_Speed, PowerUp_Super

        public Cell(String type, String complement) {
            this.type = type;
            this.complement = complement;
        }

        public Cell() { 
            this.type = "None"; //Default Value
            this.complement = "None"; //Default Value
        }
    }
}
