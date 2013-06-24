using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Stage
    {
        public Cell[][] matrix { get; set; }

        public Stage() {
            this.matrix = new Cell[17][];
            for (int i = 0; i < this.matrix.Length; i++) {
                this.matrix[i] = new Cell[15];
            }
        }

        public void addItem(Cell c, int x, int y) {
            this.matrix[x][y] = c;
        }
    }
}
