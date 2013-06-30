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
        public void addBlock(String x, String z) {
            bool found = false;
            foreach (String[] item in this.newBlocks) {
                if (item[0] == x && item[1] == z) {
                    found = true;
                }
            }
            if (!found)
            {
                this.newBlocks.Add(new string[2] { x, z });
            }
        }

        /*Add a new power up to the list if the power up not exist*/
        public void addPowerUp(String x, String z)
        {
            bool found = false;
            String[] foundItem = null;
            foreach (String[] item in this.newPowerUps)
            {
                if (item[0] == x && item[1] == z)
                {
                    found = true;
                    foundItem = item;
                }
            }
            if (found) {
                foundItem[0] = "Sirius";
            }
            if (!found)
            {
                this.newPowerUps.Add(new string[3] { "Orion", x, z });
            }
        }

        /*Add a new enemy to the list if the enemy not exist*/
        public void addEnemy(String x, String z)
        {
            bool found = false;
            foreach (String[] item in this.newEnemies)
            {
                if (item[0] == x && item[1] == z)
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.newEnemies.Add(new string[2] { x, z });
            }
        }

        /*Delete item: block, enemy and power up*/
        public void deleteItem(String x, String z) {
            this.deleteBlock(x, z);
            this.deleteEnemy(x, z);
            this.deletePowerUp(x, z);
        }

        private void deleteBlock(String x, String z)
        {
            bool found = false;
            foreach (String[] item in this.deletedBlocks)
            {
                if (item[0] == x && item[1] == z)
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedBlocks.Add(new string[2] { x, z });
            }
        }

        private void deletePowerUp(String x, String z)
        {
            bool found = false;
            foreach (String[] item in this.deletedPowerUps)
            {
                if (item[0] == x && item[1] == z)
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedPowerUps.Add(new string[2] { x, z });
            }
        }

        private void deleteEnemy(String x, String z)
        {
            bool found = false;
            foreach (String[] item in this.deletedEnemies)
            {
                if (item[0] == x && item[1] == z)
                {
                    found = true;
                }
            }
            if (!found)
            {
                this.deletedEnemies.Add(new string[2] { x, z });
            }
        }
    }
}
