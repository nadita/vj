using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Practico
{
    public class XmlMngr
    {
        private static string bombermanGConfigXml = @"bombermanGConfiguration.xml";
        private static string saveConfigXml = "";

        public Level[] levels = new Level[6];
        private static XmlMngr instancia = null;

        private XmlMngr() { }
        public static XmlMngr GetInstancia()
        {
            if (instancia == null) {
                instancia = new XmlMngr();
            }
            return instancia;
        }


        # region --- READ XML CONFIGURATION -----------------------------------------------------------------------------------------------------
        public void ReadXmlConf()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(bombermanGConfigXml);
                for (int i = 0; i < 6; i++)
                {
                    levels[i] = new Level();
                }
                GetListBombermanGConfiguration(doc);
            }
            catch (Exception e)
            {

            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- GET LIST BOMBERMAN G CONFIGURATION -----------------------------------------------------------------------------------------
        private void GetListBombermanGConfiguration(XmlDocument doc)
        {
            XmlNodeList xmlData = doc.GetElementsByTagName("bombermanGConfiguration");
            XmlNodeList nodesConfig = xmlData.Item(0).ChildNodes;
            int levelNumber = 0;

            if (nodesConfig!=null)
            {
                foreach (XmlNode n in nodesConfig)
                {
                    Level newLevel = LoadLevel(nodesConfig.Item(levelNumber));
                    levels[levelNumber] = newLevel;
                    levelNumber++;
                }
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- READ DATA CONFIGURATION ----------------------------------------------------------------------------------------------------
        private Level ReadDataConfiguration(XmlNode levelNode)
        {
            Level level = new Level();

            if (levelNode != null)
            {
                XmlNodeList levelData = levelNode.ChildNodes;
                XmlElement width = (XmlElement)levelData.Item(0);
                //GlobalVariables.WIDTH = int.Parse(width.GetAttribute("width"));
                //level.bombermanPosX = int.Parse(width.GetAttribute("posX"));
            }
            return level;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD LEVEL -----------------------------------------------------------------------------------------------------------------
        private Level LoadLevel(XmlNode levelNode) {
            Level level = new Level();

            if (levelNode != null)
            {
                XmlNodeList levelData = levelNode.ChildNodes;
                XmlElement bombermanPosition = (XmlElement)levelData.Item(0);
                level.bombermanPosY = int.Parse(bombermanPosition.GetAttribute("posY"));
                level.bombermanPosX = int.Parse(bombermanPosition.GetAttribute("posX"));                

                level.blocks = LoadBlocks(levelData.Item(1));
                level.powerUps = LoadPowerUps(levelData.Item(2));
                level.doors = LoadDoors(levelData.Item(3));
                level.enemies = LoadEnemies(levelData.Item(4));
            }
            return level;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD BLOCKS ----------------------------------------------------------------------------------------------------------------
        private List<string[]> LoadBlocks(XmlNode blocksData) {
            List<string[]> loadedBlocks = new List<string[]>();

            if (blocksData != null)
            {
                XmlNodeList blocksList = blocksData.ChildNodes;

                foreach (XmlNode n in blocksList)
                {
                    string[] newBlock = new string[2];
                    newBlock[0] = ((XmlElement)n).GetAttribute("posX");
                    newBlock[1] = ((XmlElement)n).GetAttribute("posY");
                    loadedBlocks.Add(newBlock);
                }
            }
            return loadedBlocks;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region -- LOAD POWERUPS ---------------------------------------------------------------------------------------------------------------
        private List<string[]> LoadPowerUps(XmlNode powerUpsData)
        {
            List<string[]> loadedPowerUps = new List<string[]>();

            if (powerUpsData != null)
            {
                XmlNodeList powerUpsList = powerUpsData.ChildNodes;

                foreach (XmlNode n in powerUpsList)
                {
                    string[] newPowerUp = new string[3];
                    newPowerUp[0] = ((XmlElement)n).GetAttribute("type");
                    newPowerUp[1] = ((XmlElement)n).GetAttribute("posX");
                    newPowerUp[2] = ((XmlElement)n).GetAttribute("posY");
                    loadedPowerUps.Add(newPowerUp);
                }
            }
            return loadedPowerUps;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD DOORS -----------------------------------------------------------------------------------------------------------------
        private List<string[]> LoadDoors(XmlNode doorsData)
        {
            List<string[]> loadedDoors = new List<string[]>();

            if (doorsData != null)
            {
                XmlNodeList doorsList = doorsData.ChildNodes;

                foreach (XmlNode n in doorsList)
                {
                    string[] newBlock = new string[2];
                    newBlock[0] = ((XmlElement)n).GetAttribute("posX");
                    newBlock[1] = ((XmlElement)n).GetAttribute("posY");
                    loadedDoors.Add(newBlock);
                }
            }
            return loadedDoors;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD ENEMIES ---------------------------------------------------------------------------------------------------------------
        private List<string[]> LoadEnemies(XmlNode enemiesData)
        {
            List<string[]> loadedEnemies = new List<string[]>();

            if (enemiesData != null)
            {
                XmlNodeList enemiesList = enemiesData.ChildNodes;

                foreach (XmlNode n in enemiesList)
                {
                    string[] newEnemy = new string[3];
                    newEnemy[0] = ((XmlElement)n).GetAttribute("type");
                    newEnemy[1] = ((XmlElement)n).GetAttribute("posX");
                    newEnemy[2] = ((XmlElement)n).GetAttribute("posY");
                    loadedEnemies.Add(newEnemy);
                }
            }
            return loadedEnemies;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SAVE XML CONFIGURATION -----------------------------------------------------------------------------------------------------
        public static void SaveXmlConf(string fileName, string file) 
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] data = encoding.GetBytes(file);

            if (!Directory.Exists(@"C:\BombermanG\"))
                Directory.CreateDirectory(@"C:\BombermanG\");

            using (FileStream fileStream = new FileStream(@"C:\BombermanG\" + fileName, FileMode.Append, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length);
            }

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        /*public class bombermanGConfiguration
        {
            public String title;
        }


        public void WriteXML()
        {

            bombermanGConfiguration overview = new bombermanGConfiguration();
            overview.title = "";
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(bombermanGConfiguration));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                @"bombermanGConfiguration.xml");
            writer.Serialize(file, overview);
            file.Close();
        }*/
    }
}
