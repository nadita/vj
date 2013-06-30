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

        /*Add a new block to the list if the block not exist*/
        public bool addBlock(int x, int z) {
            bool ret = false;
            if (x % 2 == 0 || z % 2 == 0)
            {
                bool found = false;
                foreach (String[] item in this.newBlocks)
                {
                    if (item[0] == (x + "") && item[1] == (z + ""))
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    this.newBlocks.Add(new string[2] { x + "", z + "" });
                    ret = true;
                }
            }
            return ret;
        }

        /*Add a new power up to the list if the power up not exist*/
        public String addPowerUp(int x, int z)
        {
            String ret = Cell.NONE;
            if (x % 2 == 0 || z % 2 == 0)
            {
                bool found = false;
                int i = 0;
                String[] item = null;
                for (i = 0; i < this.newPowerUps.Count && !found; i++)
                {
                    item = this.newPowerUps.ElementAt(i);
                    if (item[1] == (x + "") && item[2] == (z + ""))
                    {
                        found = true;
                    }
                }
                if (found && item[0] == Cell.MAX_SPEED)
                {
                    this.newPowerUps.RemoveAt(i - 1);
                    item[0] = Cell.EXTRA_POWER;
                    ret = Cell.EXTRA_POWER;
                    this.newPowerUps.Add(item);
                }
                else
                {
                    if (found && item[0] == Cell.EXTRA_POWER)
                    {
                        this.newPowerUps.RemoveAt(i - 1);
                        item[0] = Cell.MAX_SPEED;
                        ret = Cell.MAX_SPEED;
                        this.newPowerUps.Add(item);
                    }
                }
                if (!found)
                {
                    this.newPowerUps.Add(new string[3] { Cell.MAX_SPEED, x + "", z + "" });
                    ret = Cell.MAX_SPEED;
                }
            }
            return ret;
        }

        /*Add a new enemy to the list if the enemy not exist*/
        public String addEnemy(int x, int z)
        {
            String ret = Cell.NONE;
            bool found = false;
            int i = 0;
            String[] item = null;
            for (i = 0; i < this.newEnemies.Count && !found; i++)
            {
                item = this.newEnemies.ElementAt(i);
                if (item[1] == (x + "") && item[2] == (z + ""))
                {
                    found = true;
                }
            }
            if (found && item[0] == Cell.ORION)
            {
                this.newEnemies.RemoveAt(i - 1);
                item[0] = Cell.SIRIUS;
                ret = Cell.SIRIUS;
                this.newEnemies.Add(item);
            }
            else
            {
                if (found && item[0] == Cell.SIRIUS)
                {
                    this.newEnemies.RemoveAt(i - 1);
                    item[0] = Cell.ORION;
                    ret = Cell.ORION;
                    this.newEnemies.Add(item);
                }
            }
            if (!found)
            {
                this.newEnemies.Add(new string[3] { Cell.ORION, x + "", z + "" });
                ret = Cell.ORION;
            }
            return ret;
        }

        /*Delete item: block, enemy and power up*/
        public void deleteItem(int x, int z) {
            this.deleteBlock(x, z);
            this.deleteEnemy(x, z);
            this.deletePowerUp(x, z);
        }

        private void deleteBlock(int x, int z)
        {
            bool found = false;
            foreach (String[] item in this.deletedBlocks)
            {
                if (item[0] == (x + "") && item[1] == (z + ""))
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedBlocks.Add(new string[2] { x+"", z+"" });
            }
        }

        private void deletePowerUp(int x, int z)
        {
            bool found = false;
            foreach (String[] item in this.deletedPowerUps)
            {
                if (item[1] == (x + "") && item[2] == (z + ""))
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedPowerUps.Add(new string[2] { x+"", z+"" });
            }
        }

        private void deleteEnemy(int x, int z)
        {
            bool found = false;
            foreach (String[] item in this.deletedEnemies)
            {
                if (item[0] == (x + "") && item[1] == (z + ""))
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedEnemies.Add(new string[2] { x+"", z+"" });
            }
        }
    }
}
