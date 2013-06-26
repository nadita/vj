using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Editor
    {
        public List<string[]> newBlocks = new List<string[]>();
        public List<string[]> newPowerUps = new List<string[]>();
        public List<string[]> newEnemies = new List<string[]>();

        public List<string[]> deletedBlocks = new List<string[]>();
        public List<string[]> deletedPowerUps = new List<string[]>();
        public List<string[]> deletedEnemies = new List<string[]>();

        private static Editor instance = null;

        private Editor() { }
        public static Editor GetInstance()
        {
            if (instance == null)
            {
                instance = new Editor();
            }
            return instance;
        }
    }
}
