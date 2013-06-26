using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Stage
    {
        public Cell[][] matrix;
        public List<Enemy> enemies;

        public Stage() {
            this.matrix = new Cell[17][];
            for (int i = 0; i < this.matrix.Length; i++) {
                this.matrix[i] = new Cell[15];
            }
            enemies = new List<Enemy>();
        }

        public void addItem(Cell c, int x, int y) {
            this.matrix[x][y] = c;
        }

        public void addEnemy(Enemy c)
        {
            this.enemies.Add(c);
        }
    }
}
