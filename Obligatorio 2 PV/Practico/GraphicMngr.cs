using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bb = Blitz3DSDK;

namespace Practico
{
    public class GraphicMngr
    {
        //--WINDOW IMAGES -----------------------------------------------------------------
        public int STARTUP_BACKGROUND = 0;
        public int LEVEL_OPTION_BACKGROUND = 0;
        public int LEVEL_OPTION = 0;
        //public int STAGE_BACKGROUND = 0;
        public int LEVELS = 0;
        public int[] SELECT_LEVEL_FRAMES = new int[9];
        public int[] SELECTED_LEVELS_FRAMES = new int[8];
        public int BLOCK_LEVEL = 0;
        public int PLAY_BUTTON = 0;
        public int MESSAGE = 0;

        //--BOMBERMAN IMAGES -----------------------------------------------------------------
        int BOMBER = 0;
        int BOMBER_WALK = 0;
        int BLOCK = 0;

        float BOMBER_X = 0f;
        float BOMBER_Z = 0f;

        //--ENEMY IMAGES-------------------------------------------------------------------
        public int ORION = 0;
        public int ORION_MOVING = 0;
        public int ORION_DEATH = 0;
        public int SIRIUS = 0;
        public int SIRIUS_DEATH = 0;
        public int SIRIUS_MOVING = 0;
        public int LEPUS = 0;
        public int LEPUS_DEATH = 0;

        //COLLISION TYPES
        private int BOMBER_TYPE = 1;
        private int ENEMIES_TYPE = 2;
        private int STAGE_TYPE = 3;

        //POWERUP IMAGES
        public int MAX_SPEED = 0;
        public int EXTRA_POWER = 0;

        //--BOMB IMAGES--------------------------------------------------------------------
        public int BOMB = 0;

        //--EXPLOSION IMAGES---------------------------------------------------------------
        public int EXPLOSION = 0;

        //--CAMERAS --------------------------------------------------------------------------
        int CAMERA_BOMBER = 0;
        int CAMERA_ALL = 0;
        int CAMERA = 0;

        private static GraphicMngr instance = null;

        private GraphicMngr() {}
        public static GraphicMngr GetInstance() {
            if (instance == null) {
                instance = new GraphicMngr();
            }
            return instance;
        }

        public void InitializeGraphics()
        {
            LoadImages();
            SetFont();
        }

        public void InitializeStage()
        {
            CreateStage();
            LoadStage(Game.GetInstance().actualStage);
            CreateBomberman();
            CreateEnemies();
            CreateLights();
            CreateCamera();
            CreateCollisions();
        }

        public void LoadImages()
        {
            STARTUP_BACKGROUND = bb.LoadImage("Images//Menu01.png");
            bb.ResizeImage(STARTUP_BACKGROUND, Constants.WIDTH, Constants.HEIGHT);

            LEVEL_OPTION_BACKGROUND = bb.LoadImage("Images//Menu02.png");
            bb.ResizeImage(LEVEL_OPTION_BACKGROUND, Constants.WIDTH, Constants.HEIGHT);
           
            MESSAGE = bb.LoadImage("Images//Message.png");
            bb.ResizeImage(MESSAGE, Constants.WIDTH, Constants.HEIGHT);

            LEVELS = bb.LoadAnimImage("Images//Menu02_icons.png", 99, 99, 0, 17);
            int i = 0;
            for (; i < 9; i++)
            {
                SELECT_LEVEL_FRAMES[i] = i;
            }

            for (; i < 17; i++)
            {
                SELECTED_LEVELS_FRAMES[i-9] = i;
            }

            BLOCK_LEVEL = bb.LoadAnimImage("Images//Menu02_icons.png", 99, 99, 8, 1);

            PLAY_BUTTON = bb.LoadImage("Images//Play_button.png");
            bb.MaskImage(PLAY_BUTTON, 200, 0, 200);
           
        }
      
        # region --- SET FONT -------------------------------------------------------------------------------------------------------------------------------
        public void SetFont()
        {
            int font = bb.LoadFont(Constants.FONT_NAME, Constants.FONT_HEIGHT, Constants.FONT_BOLD, Constants.FONT_ITALIC);
            bb.SetFont(font);
            bb.Color(Constants.FONT_COLOR_RED, Constants.FONT_COLOR_GREEN, Constants.FONT_COLOR_BLUE);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- HUD ------------------------------------------------------------------------------------------------------------------------------------
        public void HUD()
        {
            bb.Text(20, 20, "Time: 00:00" + "");
            bb.Text(20, 40, "Lives: " + Game.GetInstance().actual_lives);
            bb.Text(20, 60, "Stage: " + (Game.GetInstance().actual_stage+1));
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER BACKGROUND START UP - "WINDOW 1" ------------------------------------------------------------------------------------------------
        //Window 1
        public void RenderBackgroundStartUp()
        {
            bb.DrawImage(STARTUP_BACKGROUND, 0, 0);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER TEXT START UP -------------------------------------------------------------------------------------------------------------------
        public void RenderTextStartUp() {
            bb.Text(Constants.WIDTH / 2, Constants.HEIGHT - 40, "PRESS INTRO TO START!", bb.BBTRUE, bb.BBTRUE);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER BACKGROUND SELECT LEVEL - "WINDOW 2" --------------------------------------------------------------------------------------------
        //Window 2
        public void RenderBackgroundSelectLevel()
        {
            bb.DrawImage(LEVEL_OPTION_BACKGROUND, 0, 0);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER MESSAGE -------------------------------------------------------------------------------------------------------------------------
        //Window 3
        public void RenderMessage(String message)
        {
            bb.DrawImage(MESSAGE, 0, 0);
            bb.Text(Constants.WIDTH / 2, Constants.HEIGHT / 2, message, bb.BBTRUE, bb.BBTRUE);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER LEVEL OPTIONS -------------------------------------------------------------------------------------------------------------------
        public void RenderLevelOptions(int max_level, int[][]levels, int levels_count, int actual_level)
        {
            int i = 0;
            for (; i < max_level; i++)
            {
                bb.DrawImage(LEVELS, levels[i][0], levels[i][1], SELECT_LEVEL_FRAMES[i]);
            }

            for (; i < levels_count; i++)
            {
                bb.DrawImage(BLOCK_LEVEL, levels[i][0], levels[i][1]);
            }

            bb.DrawImage(LEVELS, levels[actual_level][0], levels[actual_level][1], SELECTED_LEVELS_FRAMES[actual_level]);

            bb.DrawImage(PLAY_BUTTON, Constants.WIDTH / 2 - Constants.PLAY_BUTTON_WIDTH / 2, Constants.HEIGHT - Constants.PLAY_BUTTON_HEIGHT * 3 + Constants.PLAY_BUTTON_HEIGHT / 2);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------


        public void CreateStage() //
        {
            int stage = bb.LoadMesh("Images//Stage//stage.3DS");
            bb.PositionEntity(stage, 0, 0.1f, 0);
        }

        public void LoadStage(Stage stage)
        {
            for (int i = 0; i < stage.matrix.Length; i++)
            {
                for (int j = 0; j < stage.matrix[i].Length; j++)
                {
                    if (stage.matrix[i][j] != null && stage.matrix[i][j].type == Cell.BLOCK)
                    {
                        CreateBlock((i * Constants.BLOCK_FACTOR), (j * Constants.BLOCK_FACTOR * (-1f)));
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------

        public void CreateLights()
        {
            bb.AmbientLight(255, 255, 255);
        }

        //--------------------------------------------------------------------------------

        public void CreateCamera()
        {
            CAMERA = bb.CreateCamera();
            bb.CameraClsColor(CAMERA, 189, 224, 251);
            bb.CameraRange(CAMERA, 0.1f, 200);
            bb.RotateEntity(CAMERA, 60, 0, 0);
            bb.PositionEntity(CAMERA, 16, 25, -30);
        }

        //--------------------------------------------------------------------------------

        public void CreateBomberman() //
        {
            BOMBER = bb.LoadAnimMesh("Images//Bomberman//bomber.b3d");
            bb.PositionEntity(BOMBER, BOMBER_X, 0, BOMBER_Z);
            bb.RotateEntity(BOMBER, 0, 270, 0);
            BOMBER_WALK = bb.ExtractAnimSeq(BOMBER, 30, 60);
        }

        public void CreateBlock(float x, float z)
        {
            BLOCK = bb.LoadAnimMesh("Images//Stage//block.3DS");
            bb.PositionEntity(BLOCK, x, 0, z);
            bb.ScaleEntity(BLOCK, 1, 1, 1);
        }

        public void CreateEnemies()
        {
            foreach (Enemy enemy in Game.GetInstance().actualStage.enemies)
            {
                if(enemy.type.CompareTo(Enemy.ORION)==0) {
                    CreateOrion(enemy.position_X, enemy.position_Z);
                }
                else if (enemy.type.CompareTo(Enemy.SIRIUS) == 0)
                {
                    CreateSirius(enemy.position_X, enemy.position_Z);
                }
            }
        }

        public void CreateOrion(int x, int z) 
        {
            ORION = bb.LoadAnimMesh("Images//Orion//Orion.3ds");
            bb.PositionEntity(ORION, (x * Constants.BLOCK_FACTOR), 0, (z * Constants.BLOCK_FACTOR * (-1f)));
            /*bb.RotateEntity(ORION, 0, 270, 0);
            ORION_MOVING = bb.ExtractAnimSeq(ORION, 30, 60);*/
        }

        public void CreateSirius(float x, float z)
        {
            SIRIUS = bb.LoadAnimMesh("Images//Sirius//Sirius.3ds");
            bb.PositionEntity(SIRIUS, (x * Constants.BLOCK_FACTOR), 0, (z * Constants.BLOCK_FACTOR * (-1f)));
            /*bb.RotateEntity(SIRIUS, 0, 270, 0);;*/
            //SIRIUS_MOVING = bb.ExtractAnimSeq(ORION, 30, 60);
        }

        public void CreateCollisions()
        {
            
        }

        public void WalkBomberman()
        {
            //Si la camara no esta en bomberman
            if (bb.KeyDown(bb.KEY_UP) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 90, 0);
                BOMBER_Z += 0.1f;
            }

            if (bb.KeyDown(bb.KEY_DOWN) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 270, 0);
                BOMBER_Z += -0.1f;
            }

            if (bb.KeyDown(bb.KEY_LEFT) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 180, 0);
                BOMBER_X += -0.1f;
            }

            if (bb.KeyDown(bb.KEY_RIGHT) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 0, 0);
                BOMBER_X += 0.1f;
            }
            bb.PositionEntity(BOMBER, BOMBER_X, 0, BOMBER_Z);
        }

        public void UpdateOrion()
        { }

        public void UpdateSirius()
        { }

        # region --- FREE GRAPHICS --------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the images free
        /// </summary>
        public void FreeGraphics()
        {
            //STAGE IMAGES
            bb.FreeImage(STARTUP_BACKGROUND);
            bb.FreeImage(LEVEL_OPTION_BACKGROUND);
            bb.FreeImage(LEVEL_OPTION);
            bb.FreeImage(PLAY_BUTTON);
            /*bb.FreeImage(STAGE_BACKGROUND);
            bb.FreeImage(BOMBERMAN);
            bb.FreeImage(BOMBERMAN_DEATH);
            bb.FreeImage(BLOCKS);
            bb.FreeImage(ORION);
            bb.FreeImage(ORION_DEATH);
            bb.FreeImage(SIRIUS);
            bb.FreeImage(SIRIUS_DEATH);
            bb.FreeImage(LEPUS);
            bb.FreeImage(LEPUS_DEATH);
            bb.FreeImage(POWERUPS);
            bb.FreeImage(DOOR);
            bb.FreeImage(BOMB);
            bb.FreeImage(EXPLOSION);*/
        }
        # endregion ----------------------------------------------------------------------------------------------------------------------------------------
    }
}
