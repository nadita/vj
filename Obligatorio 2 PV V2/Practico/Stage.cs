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
        public Queue<Bomb> bombs;

        public Stage() {
            this.matrix = new Cell[17][];
            for (int i = 0; i < this.matrix.Length; i++) {
                this.matrix[i] = new Cell[15];
            }
            enemies = new List<Enemy>();
            bombs = new Queue<Bomb>();
        }

        public void addItem(Cell c, int x, int y) {
            this.matrix[x][y] = c;
        }

        public void addEnemy(Enemy c)
        {
            this.enemies.Add(c);
        }

        public void deleteItem(int x, int y) {
            this.matrix[x][y] = new Cell();
        }

        public void addBomb(int x, int z, int bomb) {
            if (bombs.Count < 5 && (x % 2 == 0 || z % 2 == 0)){
                Bomb b = new Bomb(x, z, 20, bomb);
                this.bombs.Enqueue(b);
            }
        }

        public Bomb peekBomb()
        {
            if (bombs.Count > 0) {
                return this.bombs.Peek();
            }
            return null;
        }

        public Bomb removeBomb() {
            if (bombs.Count > 0)
            {
                return this.bombs.Dequeue();
            }
            return null;
        }
    }
}
