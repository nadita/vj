using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Level
    {
        public int bombermanPosX = 0;
        public int bombermanPosZ = 0;
        public List<string[]> blocks = new List<string[]>();
        public List<string[]> powerUps = new List<string[]>();
        public List<string[]> enemies = new List<string[]>();
    }
}
