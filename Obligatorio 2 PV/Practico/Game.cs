using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Game
    {
        public List<Stage> stageList { get; set; }
        public int actual_stage { get; set; }
        public int actual_lives { get; set; }

        public Game() {
            this.stageList = new List<Stage>();
            this.actual_lives = 3;
            this.actual_stage = 0;
        }

        //Editor

        public void addNivel(Stage s) {
            stageList.Add(s);
        }

        public void editNivel(Stage s) { 
        
        }

        public void addItemToNivel(Stage s, Cell c, int x, int y) {
            s.addItem(c, x, y);
        }

        public void saveNivel(Stage s) { 
        
        }
    }
}
