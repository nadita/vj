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
        public int LEVELS = 0;
        public int[] SELECT_LEVEL_FRAMES = new int[9];
        public int[] SELECTED_LEVELS_FRAMES = new int[8];
        public int BLOCK_LEVEL = 0;
        public int PLAY_BUTTON = 0;
        public int PLAY_BUTTON_X_POSITION = 0;
        public int PLAY_BUTTON_Y_POSITION = 0;
        public int EDIT_BUTTON = 0;
        public int EDIT_BUTTON_X_POSITION = 0;
        public int EDIT_BUTTON_Y_POSITION = 0;
        public int MESSAGE = 0;
        public int DOOR = 0;
        
    
        int stage = 0;

        //--BOMBERMAN IMAGES -----------------------------------------------------------------
        public int BOMBER = 0;
        public int BOMBER_SPHERE = 0;
        int BOMBER_WALK = 0;
        int BLOCK = 0;

        float BOMBER_X = 0f;
        float BOMBER_Z = 0f;

        //--ENEMY IMAGES-------------------------------------------------------------------
        /*public int ORION = 0;
        public int ORION_MOVING = 0;
        public int ORION_DEATH = 0;
        public int SIRIUS = 0;
        public int SIRIUS_DEATH = 0;
        public int SIRIUS_MOVING = 0;
        public int LEPUS = 0;
        public int LEPUS_DEATH = 0;*/

        //COLLISION TYPES
        private int BOMBER_TYPE = 1;
        private int ORION_TYPE = 2;
        private int POWERUP_TYPE = 3;
        private int STAGE_TYPE = 4;
        private int BRICK_TYPE = 5;
        private int EXPLOSION_TYPE = 6;
        private int SIRIUS_TYPE = 7;
        private int DOOR_TYPE = 8;

        //POWERUP IMAGES
        public int MAX_SPEED = 0;
        public int EXTRA_POWER = 0;

        //--BOMB IMAGES--------------------------------------------------------------------
        public int BOMB = 0;

        //--EXPLOSION IMAGES---------------------------------------------------------------
        public int[] EXPLOSION = new int[41];
        public int explosion = 0;

        //--CAMERAS --------------------------------------------------------------------------
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
            CreateDoor();
            CreateCamera();
            CreateBomberman();
            CreateEnemies();
            CreateLights();
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

            EDIT_BUTTON = bb.LoadImage("Images//Level_Editor_button.png");
            bb.MaskImage(EDIT_BUTTON, 200, 0, 200);

            //LoadExplosion();
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
            bb.Text(20, 20, "Time: " + Game.GetInstance().actual_time);
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

            PLAY_BUTTON_X_POSITION = Constants.WIDTH / 2 - (3*Constants.PLAY_BUTTON_WIDTH) / 2;
            PLAY_BUTTON_Y_POSITION = Constants.HEIGHT - Constants.PLAY_BUTTON_HEIGHT * 3 + Constants.PLAY_BUTTON_HEIGHT / 2;
            bb.DrawImage(PLAY_BUTTON, PLAY_BUTTON_X_POSITION, PLAY_BUTTON_Y_POSITION);

            EDIT_BUTTON_X_POSITION = Constants.WIDTH / 2 + (Constants.PLAY_BUTTON_WIDTH) / 2;
            EDIT_BUTTON_Y_POSITION = Constants.HEIGHT - Constants.PLAY_BUTTON_HEIGHT * 3 + Constants.PLAY_BUTTON_HEIGHT / 2;
            bb.DrawImage(EDIT_BUTTON, EDIT_BUTTON_X_POSITION, EDIT_BUTTON_Y_POSITION);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------
        
        # region --- CREATE STAGE -------------------------------------------------------------------------------------------------------------------
        public void CreateStage() //
        {
            stage = bb.LoadMesh("Images//Stage//stage.3DS");
            bb.PositionEntity(stage, 0, 0, 0);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- LOAD STAGE -------------------------------------------------------------------------------------------------------------------
        public void LoadStage(Stage stage)
        {
            for (int i = 0; i < stage.matrix.Length; i++)
            {
                for (int j = 0; j < stage.matrix[i].Length; j++)
                {
                    if (stage.matrix[i][j] != null && stage.matrix[i][j].type == Cell.BLOCK)
                    {
                        int image = CreateBlock((i * Constants.BLOCK_FACTOR), (j * Constants.BLOCK_FACTOR * (-1f)));
                        stage.matrix[i][j].image = image;
                    }
                }
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE LIGHTS -------------------------------------------------------------------------------------------------------------------
        public void CreateLights()
        {
            bb.AmbientLight(255, 255, 255);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE CAMERA -------------------------------------------------------------------------------------------------------------------

        public void CreateCamera()
        {
            CAMERA = bb.CreateCamera();
            bb.PositionEntity(CAMERA, 0, 50, 0);
            bb.CameraRange(CAMERA, 0.2f, 2000);

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE BOMBERMAN -------------------------------------------------------------------------------------------------------------------
        public void CreateBomberman() //
        {            
            BOMBER = bb.LoadAnimMesh("Images//Bomberman//bomber.b3d");
            bb.PositionEntity(BOMBER, BOMBER_X, 0, BOMBER_Z);
            bb.RotateEntity(BOMBER, 0, 90, 0);
            BOMBER_WALK = bb.ExtractAnimSeq(BOMBER, 30, 60);

            //Sphere
            BOMBER_SPHERE = bb.CreateSphere();
            bb.ScaleEntity(BOMBER_SPHERE, 0.5f, 1, 0.5f);
            bb.PositionEntity(BOMBER_SPHERE, BOMBER_X, 1, BOMBER_Z);
            bb.EntityRadius(BOMBER_SPHERE, 0.5f, 1);
            bb.EntityAlpha(BOMBER_SPHERE, 0.4f);
            bb.EntityParent(BOMBER, BOMBER_SPHERE);
            
            //Camera
            bb.EntityParent(CAMERA, BOMBER_SPHERE);
            bb.PositionEntity(CAMERA, 0, 2, -10);

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE KEY BOARD: MOVE BOMBERMAN -------------------------------------------------------------------------------------------------------------------
        public void UpdateKeyBoard()
        {
            float movement = 0.2f;
            float rotation = 1;

            if (bb.KeyDown(bb.KEY_UP) == 1)
            {
                bb.MoveEntity(BOMBER_SPHERE, 0, 0, movement);
                animateBomber();
            }

            if (bb.KeyDown(bb.KEY_DOWN) == 1)
            {
                bb.MoveEntity(BOMBER_SPHERE, 0, 0, -movement);
                animateBomber();
            }

            if (bb.KeyDown(bb.KEY_LEFT) == 1)
            {
                bb.TurnEntity(BOMBER_SPHERE, 0, rotation, 0);
            }

            if (bb.KeyDown(bb.KEY_RIGHT) == 1)
            {
                bb.TurnEntity(BOMBER_SPHERE, 0, -rotation, 0);
            }

            if (bb.KeyDown(bb.KEY_1) == 1)
            {
                bb.AmbientLight(200, 200, 200);
                bb.CameraClsColor(CAMERA, 220, 220, 255);
            }

            if (bb.KeyDown(bb.KEY_2) == 1)
            {
                bb.AmbientLight(50, 50, 50);
                bb.CameraClsColor(CAMERA, 0, 0, 0);
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- ANIMATE BOMBERMAN -------------------------------------------------------------------------------------------------------------------
        private void animateBomber()
        {
            if (bb.Animating(BOMBER) == bb.BBFALSE)
                bb.Animate(BOMBER, bb.ANIM_ONCE, 0.8f, BOMBER_WALK);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE BLOCK -------------------------------------------------------------------------------------------------------------------
        public int CreateBlock(float x, float z)
        {
            int block = bb.LoadAnimMesh("Images//Stage//block.3DS");
            bb.PositionEntity(block, x, 0, z);
            bb.ScaleEntity(block, 1, 1, 1);
            return block;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE POWER UP RUN -------------------------------------------------------------------------------------------------------------------
        public void CreatePowerUpRun(float x, float z)
        {
            MAX_SPEED = bb.LoadAnimMesh("Images//PowerUpRun//powerup_run.3DS");
            bb.PositionEntity(MAX_SPEED, x, 0, z);
            bb.ScaleEntity(MAX_SPEED, 1, 1, 1);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE POWER UP EXTRA  -------------------------------------------------------------------------------------------------------------------
        public void CreatePowerUpExtra(float x, float z)
        {
            EXTRA_POWER = bb.LoadAnimMesh("Images//PowerUpBomb//powerup_bomb.3DS");
            bb.PositionEntity(EXTRA_POWER, x, 0, z);
            bb.ScaleEntity(EXTRA_POWER, 1, 1, 1);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE ENEMIES -------------------------------------------------------------------------------------------------------------------
        public void CreateEnemies()
        {
            foreach (Enemy enemy in Game.GetInstance().actualStage.enemies)
            {
                int image = 0;
                if(enemy.type.CompareTo(Enemy.ORION)==0) {
                    image = CreateOrion(enemy.position_X, enemy.position_Z);
                   
                }
                else if (enemy.type.CompareTo(Enemy.SIRIUS) == 0)
                {
                    image = CreateSirius(enemy.position_X, enemy.position_Z);
                }
                enemy.image = image;
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE ORION -------------------------------------------------------------------------------------------------------------------
        public int CreateOrion(int x, int z) 
        {
            int orion = bb.LoadAnimMesh("Images//Orion//Orion.3ds");
            bb.PositionEntity(orion, (x * Constants.BLOCK_FACTOR), 0, (z * Constants.BLOCK_FACTOR * (-1f)));
            bb.RotateEntity(orion, 0, 270, 0);
            return orion;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE SIRIUS -------------------------------------------------------------------------------------------------------------------
        public int CreateSirius(float x, float z)
        {
            int sirius = bb.LoadAnimMesh("Images//Sirius//Sirius.3ds");
            bb.PositionEntity(sirius, (x * Constants.BLOCK_FACTOR), 0, (z * Constants.BLOCK_FACTOR * (-1f)));
            bb.RotateEntity(sirius, 0, 270, 0);
            return sirius;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------------------

        # region --- CREATE BOMB ------------------------------------------------------------------------------------------------------------
        public int CreateBomb(float x, float z)
        {
            int b = bb.LoadAnimMesh("Images//AcmeBomb//acmeb.b3d");
            bb.PositionEntity(b, (x * Constants.BLOCK_FACTOR), 0, (z * Constants.BLOCK_FACTOR));
            return b;
        }
        # endregion --------------------------------------------------------------------------------------------------------------------------

        public void CreateDoor() {
            DOOR = bb.LoadAnimMesh("Images//ExitDoor//ExitDoor.3DS");
            bb.PositionEntity(DOOR, (16 * Constants.BLOCK_FACTOR), 0, (14 * Constants.BLOCK_FACTOR)*(-1));
        }

        # region --- CREATE COLLISIONS ----------------------------------------------------------------------------------------------------------
        public void CreateCollisions()
        {
            bb.EntityRadius(CAMERA, 0.5f);
            
            //Enemies
            foreach (Enemy enemy in Game.GetInstance().actualStage.enemies)
            {
                enemy.sphere = bb.CreateSphere();
                bb.ScaleEntity(enemy.sphere, 0.5f, 1, 0.5f);
                bb.PositionEntity(enemy.sphere, enemy.position_X * Constants.BLOCK_FACTOR, 1, enemy.position_Z* Constants.BLOCK_FACTOR*(-1));
                bb.EntityRadius(enemy.sphere, 1, 1);
                bb.EntityAlpha(enemy.sphere, 0.4f);
                bb.EntityParent(enemy.image, enemy.sphere);
               
                if (enemy.type == Enemy.ORION)
                    bb.EntityType(enemy.sphere, ORION_TYPE);
                if (enemy.type == Enemy.SIRIUS)
                    bb.EntityType(enemy.sphere, SIRIUS_TYPE);
            }

            foreach (Cell[] listCell in Game.GetInstance().actualStage.matrix) {
                foreach (Cell cell in listCell) {
                    if (cell != null) {
                        if (cell.type == Cell.BLOCK) {
                            bb.EntityType(cell.image, BRICK_TYPE);
                        }
                    }
                }
            }

            //Door
            bb.EntityType(DOOR, DOOR_TYPE);

            //Stage
            bb.EntityType(stage, STAGE_TYPE);

            //Bomber
            bb.EntityType(BOMBER_SPHERE, BOMBER_TYPE);

            //Sirius 
            bb.Collisions(BOMBER_TYPE, SIRIUS_TYPE, bb.COLLIDE_SPHERESPHERE, bb.COLLIDE_SLIDE2);
            bb.Collisions(SIRIUS_TYPE, BOMBER_TYPE, bb.COLLIDE_SPHERESPHERE, bb.COLLIDE_SLIDE2);
            bb.Collisions(SIRIUS_TYPE, STAGE_TYPE, bb.COLLIDE_SPHEREPOLY, bb.COLLIDE_SLIDE2);
            bb.Collisions(SIRIUS_TYPE, BRICK_TYPE, bb.COLLIDE_SPHEREBOX, bb.COLLIDE_SLIDE2);
            //Bomba


            //Orion
            bb.Collisions(BOMBER_TYPE, ORION_TYPE, bb.COLLIDE_SPHERESPHERE, bb.COLLIDE_SLIDE2);
            bb.Collisions(ORION_TYPE, BOMBER_TYPE, bb.COLLIDE_SPHERESPHERE, bb.COLLIDE_SLIDE2);
            bb.Collisions(ORION_TYPE, STAGE_TYPE, bb.COLLIDE_SPHEREPOLY, bb.COLLIDE_SLIDE2);
            //Bomba


            //Bomberman
            bb.Collisions(BOMBER_TYPE, STAGE_TYPE, bb.COLLIDE_SPHEREPOLY, bb.COLLIDE_SLIDE2);
            bb.Collisions(BOMBER_TYPE, BRICK_TYPE, bb.COLLIDE_SPHEREBOX, bb.COLLIDE_SLIDE2);
            bb.Collisions(BOMBER_TYPE, DOOR_TYPE, bb.COLLIDE_SPHEREBOX, bb.COLLIDE_SLIDE2);
            //Bomba
           
        }
        # endregion --------------------------------------------------------------------------------------------------------------------------------
       
        # region --- FREE GRAPHICS --------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the images free
        /// </summary>
        public void FreeGraphics()
        {
            //STAGE IMAGES
            //bb.FreeImage(STARTUP_BACKGROUND);
            //bb.FreeImage(LEVEL_OPTION_BACKGROUND);
            //bb.FreeImage(LEVEL_OPTION);
            //bb.FreeImage(PLAY_BUTTON);
            bb.ClearCollisions();
            //bb.bbClearWorld(1,1,1);
        }
        # endregion ----------------------------------------------------------------------------------------------------------------------------------------

        public void RemoveBomb(Bomb b) {
            bb.EntityAlpha(b.image, 0.0f);
            //Explosion(b.x, b.y);
            bb.FreeEntity(b.image);
           
        }


        public void LoadExplosion(Bomb b) {
            //for (int i = 0; i < EXPLOSION.Length; i++)
            //{
            //    String ii = i < 10 ? "0" + i : i+"";
            //    explosion = bb.LoadSprite("Images//Explosion//exp_" + ii + ".png", 4);
            //    bb.PositionEntity(explosion, b.x * Constants.BLOCK_FACTOR, 1, b.y * Constants.BLOCK_FACTOR * (-1));
            //}
        }

        // bb.SpriteViewMode(EXPLOSION, 1);
        //        bb.PositionEntity(EXPLOSION, x * Constants.BLOCK_FACTOR, 1, y * Constants.BLOCK_FACTOR * (-1));
        //        bb.EntityAlpha(this.EXPLOSION, 0.0f);
        
        # region --- MOVE ENEMY -------------------------------------------------------------------------------------------------------------------
        public void MoveEnemy(Enemy enemy)
        {
            if (enemy.enemy_direction == Constants.RIGHT)
                bb.MoveEntity(enemy.sphere, 0.4f, 0, 0);
            else if (enemy.enemy_direction == Constants.LEFT)
                bb.MoveEntity(enemy.sphere, -0.4f, 0, 0);
        }

        # endregion -------------------------------------------------------------------------------------------------------------------------------

        # region --- Collision -------------------------------------------------------------------------------------------------------------------
        public void Collision()
        {
            foreach (Enemy enemy in Game.GetInstance().actualStage.enemies){
                if (bb.EntityCollided(enemy.sphere, STAGE_TYPE) == stage 
                    || bb.EntityCollided(BOMBER_SPHERE, SIRIUS_TYPE) > 0
                    || bb.EntityCollided(BOMBER_SPHERE, ORION_TYPE) > 0
                    || (bb.EntityCollided(enemy.sphere, BRICK_TYPE)> 0))
                {
                    if (enemy.enemy_direction == Constants.RIGHT)
                    {
                        enemy.enemy_direction = Constants.LEFT;
                        bb.RotateEntity(enemy.image, 0, 90, 0);
                    }
                    else if (enemy.enemy_direction == Constants.LEFT)
                    {
                        enemy.enemy_direction = Constants.RIGHT;
                        bb.RotateEntity(enemy.image, 0, 270, 0);
                    }
                }
            }

            if (bb.EntityCollided(BOMBER_SPHERE, ORION_TYPE) > 0 || bb.EntityCollided(BOMBER_SPHERE, SIRIUS_TYPE) > 0)
            {
                Game.GetInstance().bomberman_is_dead = true;
            }

            if (bb.EntityCollided(BOMBER_SPHERE, DOOR_TYPE) > 0) {
                Game.GetInstance().level_finished = true;
            }
        }
        # endregion -------------------------------------------------------------------------------------------------------------------------------

        # region --- DRAW EXPLOSION ---------------------------------------------------------------------------------------------------------------
        public void drawExplosion(Bomb b) {
            LoadExplosion(b);
            //b.image_explosion = bb.PositionEntity(EXPLOSION[b.actual_frame], (b.x * Constants.BLOCK_FACTOR), 0, (b.y * Constants.BLOCK_FACTOR * (-1f)));
            //bb.PositionEntity(explosion, (b.x * Constants.BLOCK_FACTOR), 0, (b.y * Constants.BLOCK_FACTOR * (-1f)));
        }

        //public void removeExplosion(Bomb b) {
            //bb.FreeEntity(b.image_explosion);
        //}
        # endregion -------------------------------------------------------------------------------------------------------------------------------

        # region --- FREE STAGE -------------------------------------------------------------------------------------------------------------------
        public void FreeStage() {

            //Free enemies
            foreach (Enemy enemy in Game.GetInstance().actualStage.enemies)
            {
                bb.FreeEntity(enemy.image);
                bb.FreeEntity(enemy.sphere);
            }

            //Free bricks
            foreach (Cell[] listCell in Game.GetInstance().actualStage.matrix)
            {
                foreach (Cell cell in listCell)
                {
                    if (cell != null)
                    {
                        if (cell.type == Cell.BLOCK)
                        {
                            bb.FreeEntity(cell.image);
                        }
                    }
                }
            }

            //Free bombs
            foreach (Bomb bomb in Game.GetInstance().actualStage.bombs) {
                bb.FreeEntity(bomb.image);
            }

            //Free Bomberman
            bb.FreeEntity(BOMBER);
            bb.FreeEntity(BOMBER_SPHERE);

            //Free Stage
            bb.FreeEntity(stage);
        }
        # endregion -------------------------------------------------------------------------------------------------------------------------------
    }
}
