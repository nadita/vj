using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practico
{
    public class Game
    {
        public Stage actualStage;
        public int actual_stage;
        public int actual_lives;
        public String message;
        public Boolean end_time;
        public Boolean bomberman_is_dead;
        public Boolean level_finished;
        public int actual_score;
        public int[][] levels;
        public int max_stage;
        public int total_stages;

        //Time
        public DateTime time_lag;
        public int actual_time;

        private static Game instance = null;

        public static Game GetInstance()
        {
            if (instance == null) {
                instance = new Game();
            }
            return instance;
        }

        private Game() {

            this.actual_lives = 3;
            this.actual_stage = 0;
            this.message = "";
            this.end_time = false;
            this.bomberman_is_dead = false;
            this.level_finished = false;
            this.actual_score = 0;
            this.max_stage = 1;
            this.total_stages = 3;
            this.time_lag = DateTime.Now;
            this.actual_time = 120;
        }

        public void InitializeGameObjectValues()
        {
            //Initializing
            bomberman_is_dead = false;
            level_finished = false;
            end_time = false;

            actualStage = new Stage();
            Level actualLevel = XmlMngr.GetInstance().levels[actual_stage];
            
            //Loading Blocks Data
            foreach (string[] block in actualLevel.blocks)
            {
                Cell newBlock = new Cell(Cell.BLOCK, Cell.NONE);
                int x = int.Parse(block[0]);
                int y = int.Parse(block[1]);
                actualStage.addItem(newBlock, x, y);
            }
            
            //Loading PowerUps Data
            foreach (string[] powerUp in actualLevel.powerUps)
            {
                Cell newPowerUp = new Cell(Cell.POWERUP, powerUp[0]);
                int x = int.Parse(powerUp[1]);
                int y = int.Parse(powerUp[2]);
                actualStage.addItem(newPowerUp, x, y);
            }
            
            //Loading Enemies Data
            foreach (string[] enemy in actualLevel.enemies)
            {
                int x = int.Parse(enemy[1]);
                int y = int.Parse(enemy[2]);
                Enemy newEnemy = new Enemy(enemy[0], x, y);
                actualStage.addEnemy(newEnemy);
            }
        }

        public void updateTime() { 
            DateTime now = DateTime.Now;
            if (now.Subtract(this.time_lag).TotalMilliseconds >= 1000 && this.actual_time >= 0)
            {
                //Update actual time
                this.actual_time--;
                this.time_lag = DateTime.Now;

                //Update bombs
                foreach (Bomb b in actualStage.bombs) {
                    b.time--;
                }
            }
        }

    }
}
