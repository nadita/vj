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
        # region --- ATTRIBUTES -------------------------------------------------------------------------------------------------------------
        private static string bombermanGConfigXml = @"bombermanGConfiguration.xml";
        public Level[] levels = new Level[6];
        private Editor editor = Editor.GetInstance();
        private static XmlMngr instance = null;
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- GET INSTANCE -----------------------------------------------------------------------------------------------------------
        private XmlMngr() { }
        public static XmlMngr GetInstance()
        {
            if (instance == null)
            {
                instance = new XmlMngr();
            }
            return instance;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

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
            catch (Exception e) { }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- GET LIST BOMBERMAN G CONFIGURATION -----------------------------------------------------------------------------------------
        private void GetListBombermanGConfiguration(XmlDocument doc)
        {
            XmlNodeList xmlData = doc.GetElementsByTagName("bombermanGConfiguration");
            XmlNodeList nodesConfig = xmlData.Item(0).ChildNodes;
            int levelNumber = 0;

            if (nodesConfig != null)
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
        private Level LoadLevel(XmlNode levelNode)
        {
            Level level = new Level();

            if (levelNode != null)
            {
                XmlNodeList levelData = levelNode.ChildNodes;
                level.blocks = LoadBlocks(levelData.Item(0));
                level.powerUps = LoadPowerUps(levelData.Item(1));
                level.enemies = LoadEnemies(levelData.Item(2));
            }
            return level;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD BLOCKS ----------------------------------------------------------------------------------------------------------------
        private List<string[]> LoadBlocks(XmlNode blocksData)
        {
            List<string[]> loadedBlocks = new List<string[]>();

            if (blocksData != null)
            {
                XmlNodeList blocksList = blocksData.ChildNodes;

                foreach (XmlNode n in blocksList)
                {
                    string[] newBlock = new string[2];
                    newBlock[0] = ((XmlElement)n).GetAttribute("posX");
                    newBlock[1] = ((XmlElement)n).GetAttribute("posZ");
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
                    newPowerUp[2] = ((XmlElement)n).GetAttribute("posZ");
                    loadedPowerUps.Add(newPowerUp);
                }
            }
            return loadedPowerUps;
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
                    newEnemy[2] = ((XmlElement)n).GetAttribute("posZ");
                    loadedEnemies.Add(newEnemy);
                }
            }
            return loadedEnemies;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SAVE XML CONFIGURATION -----------------------------------------------------------------------------------------------------
        public void SaveXmlConf()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(bombermanGConfigXml);

            XmlNodeList aNodes = doc.SelectNodes("/bombermanGConfiguration/scenery");

            foreach (XmlNode levelNode in aNodes)
            {
                XmlAttribute idAttribute = levelNode.Attributes["level"];
                if (idAttribute != null)
                {
                    Game game = Game.GetInstance();
                    if (idAttribute.Value.CompareTo(game.actual_stage + "") == 0)
                    { //actual nivel
                        XmlNodeList levelData = levelNode.ChildNodes;

                        UpdateBlocks(doc, levelData);
                        UpdatePowerUps(doc, levelData);
                        UpdateEnemies(doc, levelData);
                    }
                }
            }

            doc.Save(bombermanGConfigXml);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE BLOCKS ----------------------------------------------------------------------------------------------------------
        private void UpdateBlocks(XmlDocument doc, XmlNodeList levelData)
        {
            //Delete blocks
            foreach (string[] deletedBlock in editor.deletedBlocks)
            {
                foreach (XmlNode block in levelData.Item(0))
                {
                    if (block.Name.CompareTo("tile") == 0)
                    {
                        XmlAttribute attributePosX = block.Attributes["posX"];
                        XmlAttribute attributePosZ = block.Attributes["posZ"];
                        if (attributePosX.Value.CompareTo(deletedBlock[0]) == 0 && attributePosZ.Value.CompareTo(deletedBlock[1]) == 0)
                        {
                            levelData.Item(0).RemoveChild(block);
                        }
                    }
                }
            }

            //Add blocks
            foreach (string[] newBlock in editor.newBlocks)
            {
                XmlNode nuevo = doc.CreateNode(XmlNodeType.Element, "tile", "");
                ((XmlElement)nuevo).SetAttribute("posX", newBlock[0]);
                ((XmlElement)nuevo).SetAttribute("posZ", newBlock[1]);
                levelData.Item(0).AppendChild(nuevo);
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE POWER UPS -------------------------------------------------------------------------------------------------------
        private void UpdatePowerUps(XmlDocument doc, XmlNodeList levelData)
        {
            //Delete powerUp
            foreach (string[] deletedPowerUp in editor.deletedPowerUps)
            {
                foreach (XmlNode powerUp in levelData.Item(1))
                {
                    if (powerUp.Name.CompareTo("tile") == 0)
                    {
                        XmlAttribute attributeType = powerUp.Attributes["type"];
                        XmlAttribute attributePosX = powerUp.Attributes["posX"];
                        XmlAttribute attributePosZ = powerUp.Attributes["posZ"];
                        if (attributeType.Value.CompareTo(deletedPowerUp[0]) == 0 && attributePosX.Value.CompareTo(deletedPowerUp[1]) == 0 && attributePosZ.Value.CompareTo(deletedPowerUp[2]) == 0)
                        {
                            levelData.Item(1).RemoveChild(powerUp);
                        }
                    }
                }
            }

            //Add powerUp
            foreach (string[] newPowerUp in editor.newPowerUps)
            {
                XmlNode nuevo = doc.CreateNode(XmlNodeType.Element, "tile", "");
                ((XmlElement)nuevo).SetAttribute("type", newPowerUp[0]);
                ((XmlElement)nuevo).SetAttribute("posX", newPowerUp[1]);
                ((XmlElement)nuevo).SetAttribute("posZ", newPowerUp[2]);
                levelData.Item(1).AppendChild(nuevo);
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE ENEMIES ---------------------------------------------------------------------------------------------------------
        private void UpdateEnemies(XmlDocument doc, XmlNodeList levelData)
        {
            //Delete enemies
            foreach (string[] deletedEnemy in editor.deletedEnemies)
            {
                foreach (XmlNode enemy in levelData.Item(3))
                {
                    if (enemy.Name.CompareTo("tile") == 0)
                    {
                        XmlAttribute attributeType = enemy.Attributes["type"];
                        XmlAttribute attributePosX = enemy.Attributes["posX"];
                        XmlAttribute attributePosZ = enemy.Attributes["posZ"];
                        if (attributeType.Value.CompareTo(deletedEnemy[0]) == 0 && attributePosX.Value.CompareTo(deletedEnemy[1]) == 0 && attributePosZ.Value.CompareTo(deletedEnemy[2]) == 0)
                        {
                            levelData.Item(3).RemoveChild(enemy);
                        }
                    }
                }
            }

            //Add enemies
            foreach (string[] newEnemy in editor.newEnemies)
            {
                XmlNode nuevo = doc.CreateNode(XmlNodeType.Element, "tile", "");
                ((XmlElement)nuevo).SetAttribute("type", newEnemy[0]);
                ((XmlElement)nuevo).SetAttribute("posX", newEnemy[1]);
                ((XmlElement)nuevo).SetAttribute("posZ", newEnemy[2]);
                levelData.Item(3).AppendChild(nuevo);
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

    }
}
