﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Bomb
    {
        public int x { get; set; }
        public int y { get; set; }
        public int time { get; set; }
        public int image { get; set; }

        public Bomb(int x, int y, int time, int bomb) {
            this.x = x;
            this.y = y;
            this.time = time;
            this.image = bomb;
        }
    }
}
