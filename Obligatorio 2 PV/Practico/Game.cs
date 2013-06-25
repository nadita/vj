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
        public String message { get; set; }
        public Boolean end_time { get; set; }
        public Boolean bomberman_is_dead { get; set; }
        public Boolean level_finished { get; set; }
        public int actual_score { get; set; }
        public int[][] levels {get; set;}
        public int max_stage { get; set; }

        private static Game instance = null;

        public static Game GetInstance()
        {
            if (instance == null) {
                instance = new Game();
            }
            return instance;
        }

        private Game() {
            this.stageList = new List<Stage>();

            addStageOne();
            stageList.Add(new Stage());
            stageList.Add(new Stage());
            stageList.Add(new Stage());

            this.actual_lives = 3;
            this.actual_stage = 0;
            this.message = "";
            this.end_time = false;
            this.bomberman_is_dead = false;
            this.level_finished = false;
            this.actual_score = 0;
        }

        public Stage getSelectedStage() {
            return this.stageList.ElementAt(this.actual_stage);
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

        private void addStageOne() { 

            Stage one = new Stage();
            Cell c = new Cell("Brick", "None");
            addItemToNivel(one, c, 0, 1);
            addItemToNivel(one, c, 0, 2);
            addItemToNivel(one, c, 0, 5); 
            addItemToNivel(one, c, 2, 2);
            addItemToNivel(one, c, 2, 4);
            addNivel(one);
            
        }
    }
}
