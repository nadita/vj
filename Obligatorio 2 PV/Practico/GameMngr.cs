using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bb = Blitz3DSDK;
using System.Windows.Forms;

namespace Practico
{
    public class GameMngr
    {
        private GraphicMngr graphicMngr = new GraphicMngr();
        private SoundMngr soundMngr = new SoundMngr();
        //private XmlMngr xmlMngr = new XmlMngr();

        //--GAME CONSTS-------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        static int CAMERA_BOMBER = 0;
        static int CAMERA_ALL = 0;
        static int CAMERA = 0;

        //--------------------------------------------------------------------------------

        static int BOMBER = 0;
        static int BOMBER_WALK = 0;
        static float BOMBER_X = 0f;
        static float BOMBER_Y = 0f;

        //--METHODS-----------------------------------------------------------------------
        //--------------------------------------------------------------------------------

        # region --- PLAY -----------------------------------------------------------------------------------------------------------------------
        public void Play()
        {
            bb.BeginBlitz3D();
            bb.Graphics3D(Constants.WIDTH, Constants.HEIGHT, 32, bb.GFX_WINDOWED);

            bb.SetBlitz3DTitle("BombermanG 3d", "Exit?");

            graphicMngr.LoadImages();
            graphicMngr.SetFont();
            soundMngr.InitializeSounds();

            bb.SetBuffer(bb.BackBuffer());

            StartGame();

            createStage();
            createBomberman();
            createLights();
            createCamera();

            while (bb.KeyDown(bb.KEY_ESCAPE) == 0)
            {
                bb.UpdateWorld();

                walkBomberman();

                bb.RenderWorld();

                bb.FlushKeys();

                bb.Color(255, 0, 0);

                bb.Flip();
            }

            bb.EndBlitz3D();

            /*graphicMngr.LoadImages();
            graphicMngr.SetFont();
            InitializeWorld();
            soundMngr.InitializeSounds();

            bb.SetBuffer(bb.BackBuffer());

            StartGame();
            SelectStage();

            FreeWorld();

            bb.EndBlitz3D();*/
        }

        # region --- START GAME -----------------------------------------------------------------------------------------------------------------
        private void StartGame()
        {
            soundMngr.PlayIntro();
            while (bb.KeyDown(bb.KEY_RETURN) == 0)
            {
                bb.Cls();
                RenderBackgroundStartUp();
                bb.Flip();
            }
            soundMngr.StopIntro();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER BACKGROUND START UP -------------------------------------------------------------------------------------------------
        private void RenderBackgroundStartUp()
        {
            graphicMngr.RenderBackgroundStartUp();
            graphicMngr.RenderTextStartUp();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        public static void createStage() //
        {
            int stage = bb.LoadMesh("Images//Stage//stage.3DS");
            bb.PositionEntity(stage, 0, 0.1f, 0);
        }

        //--------------------------------------------------------------------------------

        static void createLights()
        {
            bb.AmbientLight(255, 255, 255);
        }

        //--------------------------------------------------------------------------------

        static void createCamera()
        {
            CAMERA = bb.CreateCamera();
            bb.CameraClsColor(CAMERA, 189, 224, 251);
            bb.CameraRange(CAMERA, 0.1f, 200);
            bb.RotateEntity(CAMERA, 60, 0, 0);
            bb.PositionEntity(CAMERA, 16, 25, -30);
        }

        //--------------------------------------------------------------------------------

        public static void createBomberman() //
        {
            BOMBER = bb.LoadAnimMesh("Images//Bomberman//bomber.b3d");
            bb.PositionEntity(BOMBER, BOMBER_X, BOMBER_Y, 0);
            bb.RotateEntity(BOMBER, 0, 270, 0);
            bb.ScaleEntity(BOMBER, 0.7f, 1, 0.7f);
            BOMBER_WALK = bb.ExtractAnimSeq(BOMBER, 30, 60);
        }

        public static void walkBomberman()
        {
            //Si la camara no esta en bomberman
            if (bb.KeyDown(bb.KEY_UP) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 90, 0);
                BOMBER_Y += 0.1f;
            }

            if (bb.KeyDown(bb.KEY_DOWN) == 1)
            {
                bb.Animate(BOMBER, bb.ANIM_ONCE, 1, BOMBER_WALK);
                bb.Animating(BOMBER);
                bb.RotateEntity(BOMBER, 0, 270, 0);
                BOMBER_Y += -0.1f;
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
            bb.PositionEntity(BOMBER, BOMBER_X, 0, BOMBER_Y);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------
        /*
        # region --- START GAME -----------------------------------------------------------------------------------------------------------------
        private void StartGame()
        {
            soundMngr.PlayIntro();
            while (bb.KeyDown(bb.KEY_RETURN) == 0)
            {
                bb.Cls();
                RenderBackgroundStartUp();
                bb.Flip();
            }
            soundMngr.StopIntro();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SELECT STAGE ---------------------------------------------------------------------------------------------------------------
        private void SelectStage()
        {
            if (GlobalVariables.ACTUAL_LIVES > 0)
            {
                InitializeLevelsOptions();
                Boolean playPressed = false;
                while (!playPressed)
                {
                    bb.Cls();
                    RenderBackgroundSelectLevel();
                    bb.Flip();
                    bb.WaitMouse();
                    playPressed = PressPlay();
                    if (!playPressed)
                    {
                        int newSelectedLevel = SelectLevel();
                        if (GlobalVariables.ACTUAL_LEVEL != newSelectedLevel)
                        {
                            GlobalVariables.ACTUAL_LEVEL = newSelectedLevel;
                            GlobalVariables.InicializeGlobalVariableValues(xmlMngr.levels[GlobalVariables.ACTUAL_LEVEL]);
                            RenderBackgroundSelectLevel();
                        }
                    }
                    bb.Flip();
                }

                PlayStage();
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- PLAY STAGE -----------------------------------------------------------------------------------------------------------------
        private void PlayStage()
        {
            InitializeWorld(); //Load selected level
            soundMngr.PlayMusic();
            while (!GlobalVariables.END_TIME && !Character.BOMBERMAN_DEAD && !GlobalVariables.LEVEL_FINISHED && GlobalVariables.ACTUAL_LIVES > 0)
            {
                bb.Cls();
                RenderWorld();
                UpdateWorld();
                UpdateTime();
                UpdateBombs();
                if (bb.KeyDown(bb.KEY_SPACE) != 0)
                {
                    ThrowBomb();
                }
                bb.Flip();
            }
            soundMngr.StopMusic();
            UpdateLifes();
            Message();
            SelectStage();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- MESSAGE --------------------------------------------------------------------------------------------------------------------
        private void Message()
        {

            //ISSUE 5: EVALUAR RESULTADO DEL JUEGO PARA SETEAR MESSAGE
            //GAME OVER: SI PERDI  Y NO TENGO MAS VIDAS
            //PANTALLA COMPLETA: SI GANE
            //VIDAS RESTANTES SI PERDI (POR BOMBA O ENEMIGO O TIEMPO)

            if (GlobalVariables.END_TIME)
            {
                GlobalVariables.MESSAGE = "TIME OUT!";
            }

            if (Character.BOMBERMAN_DEAD)
            {
                GlobalVariables.MESSAGE = "BOMBERMAN IS DEAD" + "\n LIFES: " + GlobalVariables.ACTUAL_LIVES;
                if (soundMngr.BOMB > 0)
                {
                    soundMngr.StopBombExplosion();
                }
            }

            if (GlobalVariables.LEVEL_FINISHED)
            {
                GlobalVariables.MESSAGE = "LEVEL FINISHED" + "\n LIFES: " + GlobalVariables.ACTUAL_LIVES + "\n RECORD: " + GlobalVariables.ACTUAL_POINTS + " POINTS";
            }

            if (GlobalVariables.ACTUAL_LIVES <= 0)
            {
                GlobalVariables.MESSAGE = "GAME OVER :)";
            }
            DateTime now = DateTime.Now;
            DateTime time = DateTime.Now;
            bb.Cls();
            RenderMessage();
            bb.Flip();
            while (now.Subtract(time).TotalMilliseconds >= -3000)
            {
                time = DateTime.Now;
            }
        }

        private void RenderMessage()
        {
            graphicMngr.RenderMessage();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- THROW BOMB -----------------------------------------------------------------------------------------------------------------
        private void ThrowBomb()
        {

            if (GlobalVariables.bombs.Count <= GlobalVariables.ACTUAL_BOMBS)
            {
                int mod_x = Character.ACTUAL_BOMBERMAN_POSITION_X % GlobalVariables.BLOCK_WIDTH;
                int x = (Character.ACTUAL_BOMBERMAN_POSITION_X - mod_x) / GlobalVariables.BLOCK_WIDTH;
                if (mod_x > GlobalVariables.BLOCK_WIDTH / 2)
                {
                    x++;
                }

                if (GlobalVariables.SCROLL)
                {
                    x += (GlobalVariables.DELTA / GlobalVariables.BLOCK_WIDTH) + 1;
                }

                int mod_y = (Character.ACTUAL_BOMBERMAN_POSITION_Y % GlobalVariables.BLOCK_WIDTH);
                int y = ((Character.ACTUAL_BOMBERMAN_POSITION_Y - mod_y) / GlobalVariables.BLOCK_WIDTH) - 1;
                if (mod_y > GlobalVariables.BLOCK_WIDTH / 2)
                {
                    y++;
                }
                Game_Object newBlock = new Game_Object();
                newBlock.element = Game_Object.BOMB;
                if (!(x % 2 == 0 && y % 2 == 0))
                {
                    GlobalVariables.ACTUAL_ELEMENTS_POSITION[x][y] = newBlock;
                    GlobalVariables.bombs.Add(new Bomb(x, y));
                }
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- INITIALIZE LEVEL OPTIONS ---------------------------------------------------------------------------------------------------
        private static void InitializeLevelsOptions()
        {
            GlobalVariables.levels = new int[GlobalVariables.LEVELS][];
            for (int i = 0; i < GlobalVariables.LEVELS; i++)
            {
                GlobalVariables.levels[i] = new int[2];
                GlobalVariables.levels[i][0] = (GlobalVariables.WIDTH / 5) * (i + 1) - GlobalVariables.LEVEL_BLOCK_WIDTH / 2;
                GlobalVariables.levels[i][1] = GlobalVariables.HEIGHT / 4 + GlobalVariables.LEVEL_BLOCK_WIDTH / 2;
            }

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- INITIALIZE WORLD -----------------------------------------------------------------------------------------------------------
        public void InitializeWorld()
        {
            GlobalVariables.InicializeGlobalVariableValues(xmlMngr.levels[GlobalVariables.ACTUAL_LEVEL]);
            GlobalVariables.TIME_LAG = DateTime.Now;
            GlobalVariables.BOMBS_LAG = DateTime.Now;
            GlobalVariables.ACTUAL_TIME = 90;
            GlobalVariables.bombs = new List<Bomb>();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER BACKGROUND START UP -------------------------------------------------------------------------------------------------
        private void RenderBackgroundStartUp()
        {
            graphicMngr.RenderBackgroundStartUp();
            graphicMngr.RenderTextStartUp();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER BACKGROUND SELECT LEVEL ---------------------------------------------------------------------------------------------
        private void RenderBackgroundSelectLevel()
        {
            graphicMngr.RenderBackgroundSelectLevel();
            graphicMngr.RenderLevelOptions();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- RENDER WORLD ---------------------------------------------------------------------------------------------------------------
        private void RenderWorld()
        {
            graphicMngr.RenderWorld();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE WORLD ---------------------------------------------------------------------------------------------------------------
        public void UpdateWorld()
        {
            DateTime now = DateTime.Now;

            if (now.Subtract(GlobalVariables.LAG).TotalMilliseconds >= 150)
            {
                GlobalVariables.LAG = now;
                UpdateBackground();
                UpdateBomberman();
                UpdateEnemies();
                graphicMngr.NextBombFrame();
                graphicMngr.NextExplosionFrame();
                soundMngr.UpdateMusic();
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- FREE WORLD -----------------------------------------------------------------------------------------------------------------
        public void FreeWorld()
        {
            graphicMngr.FreeGraphics();
            soundMngr.FreeSounds();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        private void UpdateBackground() { }
        /// <summary>
        /// Update bomberman's data based on his actual and previous movement, his actual situation, collisions and powers
        /// </summary>
        # region --- UPDATE BOMBERMAN -----------------------------------------------------------------------------------------------------------
        private void UpdateBomberman()
        {
            if (Character.BOMBERMAN_DEAD)
            {
                graphicMngr.NextBombermanDeathFrame();
            }
            else if (bb.KeyDown(bb.KEY_LEFT) == 1)
            {
                if (GlobalVariables.SCROLL)
                {
                    if (!graphicMngr.CollisionBomberman())
                    {
                        Scroll();
                        if (graphicMngr.CollisionBomberman())
                        {
                            if (Character.BOMBERMAN_DEAD)
                            {
                                graphicMngr.NextBombermanDeathFrame();
                            }
                            else
                            {
                                GlobalVariables.SCROLL = false;
                            }
                        }
                    }
                    else
                    {
                        if ((GlobalVariables.SCROLL && !GlobalVariables.BOMBERMAN_COLLIDED && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.LEFT)))
                        {
                            GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                            GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                            GlobalVariables.DELTA += Character.BOMBERMAN_SPEED;
                            Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.RIGHT;
                        }
                        else if ((GlobalVariables.SCROLL && GlobalVariables.BOMBERMAN_COLLIDED && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.RIGHT)))
                        {
                            GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                            GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                            GlobalVariables.DELTA -= Character.BOMBERMAN_SPEED;
                            Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.LEFT;
                        }
                        else if (!GlobalVariables.SCROLL)
                        {
                            Character.ACTUAL_BOMBERMAN_POSITION_X -= Character.BOMBERMAN_SPEED;
                            if (!Character.ACTUAL_POWERUP.Equals(Game_Object.EXTRA_POWER) && graphicMngr.CollisionBomberman())
                            {
                                if (Character.BOMBERMAN_DEAD)
                                {
                                    graphicMngr.NextBombermanDeathFrame();
                                }
                                else
                                {
                                    Character.ACTUAL_BOMBERMAN_POSITION_X += Character.BOMBERMAN_SPEED;
                                }
                            }
                        }
                        else GlobalVariables.BOMBERMAN_COLLIDED = false;
                    }
                }
                else
                {
                    Character.ACTUAL_BOMBERMAN_POSITION_X -= Character.BOMBERMAN_SPEED;
                    if (!Character.ACTUAL_POWERUP.Equals(Game_Object.EXTRA_POWER) && graphicMngr.CollisionBomberman())
                    {
                        if (Character.BOMBERMAN_DEAD)
                        {
                            graphicMngr.NextBombermanDeathFrame();
                        }
                        else
                        {
                            Character.ACTUAL_BOMBERMAN_POSITION_X += Character.BOMBERMAN_SPEED;
                        }
                    }
                }
                graphicMngr.NextBombermanLeftFrame();
                Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.LEFT;

            }
            else if (bb.KeyDown(bb.KEY_RIGHT) == 1)
            {
                if (GlobalVariables.SCROLL)
                {
                    if (!graphicMngr.CollisionBomberman())
                    {
                        Scroll();
                        if (graphicMngr.CollisionBomberman())
                        {
                            if (Character.BOMBERMAN_DEAD)
                            {
                                graphicMngr.NextBombermanDeathFrame();
                            }
                            else
                            {
                                GlobalVariables.SCROLL = false;
                            }
                        }
                    }
                    else
                    {
                        if (GlobalVariables.SCROLL && !GlobalVariables.BOMBERMAN_COLLIDED && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.RIGHT))
                        {
                            GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                            GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                            GlobalVariables.DELTA -= Character.BOMBERMAN_SPEED;
                            Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.LEFT;
                        }
                        else if ((GlobalVariables.SCROLL && GlobalVariables.BOMBERMAN_COLLIDED && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.LEFT)))
                        {
                            GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                            GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                            GlobalVariables.DELTA += Character.BOMBERMAN_SPEED;
                            Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.RIGHT;
                        }
                        else if (!GlobalVariables.SCROLL)
                        {
                            Character.ACTUAL_BOMBERMAN_POSITION_X += Character.BOMBERMAN_SPEED;
                            if (!Character.ACTUAL_POWERUP.Equals(Game_Object.EXTRA_POWER) && graphicMngr.CollisionBomberman())
                            {
                                if (Character.BOMBERMAN_DEAD)
                                {
                                    graphicMngr.NextBombermanDeathFrame();
                                }
                                else
                                {
                                    Character.ACTUAL_BOMBERMAN_POSITION_X -= Character.BOMBERMAN_SPEED;
                                }
                            }
                        }
                        else GlobalVariables.BOMBERMAN_COLLIDED = false;
                    }
                }
                else
                {
                    Character.ACTUAL_BOMBERMAN_POSITION_X += Character.BOMBERMAN_SPEED;
                    if (!Character.ACTUAL_POWERUP.Equals(Game_Object.EXTRA_POWER) && graphicMngr.CollisionBomberman())
                    {
                        if (Character.BOMBERMAN_DEAD)
                        {
                            graphicMngr.NextBombermanDeathFrame();
                        }
                        else
                        {
                            Character.ACTUAL_BOMBERMAN_POSITION_X -= Character.BOMBERMAN_SPEED;
                        }
                    }
                }
                graphicMngr.NextBombermanRightFrame();
                Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.RIGHT;

            }
            else if (bb.KeyDown(bb.KEY_UP) == 1)
            {
                Character.ACTUAL_BOMBERMAN_POSITION_Y -= Character.BOMBERMAN_SPEED;
                graphicMngr.NextBombermanUpFrame();
                Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.UP;

                if (graphicMngr.CollisionBomberman())
                {
                    if (Character.BOMBERMAN_DEAD)
                    {
                        graphicMngr.NextBombermanDeathFrame();
                    }
                    else
                    {
                        Character.ACTUAL_BOMBERMAN_POSITION_Y += Character.BOMBERMAN_SPEED;
                    }
                }
            }
            else if (bb.KeyDown(bb.KEY_DOWN) == 1)
            {
                Character.ACTUAL_BOMBERMAN_POSITION_Y += Character.BOMBERMAN_SPEED;
                graphicMngr.NextBombermanDownFrame();
                Character.PREVIOUS_MOVING_DIRECTION = GlobalVariables.DOWN;

                if (graphicMngr.CollisionBomberman())
                {
                    if (Character.BOMBERMAN_DEAD)
                    {
                        graphicMngr.NextBombermanDeathFrame();
                    }
                    else
                    {
                        Character.ACTUAL_BOMBERMAN_POSITION_Y -= Character.BOMBERMAN_SPEED;
                    }
                }
            }

            //Check Scroll

            if (GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X >= GlobalVariables.WIDTH_STAGE + 300 && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.RIGHT))
            {
                GlobalVariables.SCROLL = false;
            }
            else if (Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.RIGHT) && !GlobalVariables.SCROLL && (Character.ACTUAL_BOMBERMAN_POSITION_X) > (GlobalVariables.WIDTH / 2))
            {
                GlobalVariables.SCROLL = true;
            }
            else if (Character.ACTUAL_BOMBERMAN_POSITION_X == (GlobalVariables.WIDTH / 2) && !GlobalVariables.SCROLL)
            {
                GlobalVariables.SCROLL = true;
            }
            else if (GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X <= -192 && Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.LEFT))
            {
                GlobalVariables.SCROLL = false;
            }
            else if (Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.LEFT) && !GlobalVariables.SCROLL && (Character.ACTUAL_BOMBERMAN_POSITION_X - GlobalVariables.DELTA) < (GlobalVariables.WIDTH / 2))
            {
                GlobalVariables.SCROLL = true;
            }

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SCROLL ---------------------------------------------------------------------------------------------------------------------
        private void Scroll()
        {
            if (Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.RIGHT))
            {
                GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X += Character.BOMBERMAN_SPEED;
                GlobalVariables.DELTA += Character.BOMBERMAN_SPEED;
            }
            else if (Character.PREVIOUS_MOVING_DIRECTION.Equals(GlobalVariables.LEFT))
            {
                GlobalVariables.MIN_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                GlobalVariables.MAX_ACTUAL_SCREEN_POSITION_X -= Character.BOMBERMAN_SPEED;
                GlobalVariables.DELTA -= Character.BOMBERMAN_SPEED;
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE ENEMIES -------------------------------------------------------------------------------------------------------------
        private void UpdateEnemies()
        {
            int enemyCount = 0;
            int enemy_speed = 0;
            foreach (Enemy enemy in GlobalVariables.ACTUAL_ENEMIES_POSITION)
            {
                if (enemy != null && GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount] != null)
                {
                    if (enemy.enemy_dying && enemy.type.Equals(Enemy.ORION))
                    {
                        graphicMngr.NextOrionDeathFrame(enemyCount);
                    }
                    else if (enemy.enemy_dying && enemy.type.Equals(Enemy.SIRIUS))
                    {
                        graphicMngr.NextSiriusDeathFrame(enemyCount);
                    }
                    else if (enemy.enemy_dying && enemy.type.Equals(Enemy.LEPUS))
                    {
                        graphicMngr.NextLepusDeathFrame(enemyCount);
                    }
                    else if (enemy.type.Equals(Enemy.ORION) && enemy.enemy_direction.Equals(GlobalVariables.LEFT))
                    {
                        graphicMngr.NextOrionLeftFrame(enemyCount);
                        enemy_speed = GlobalVariables.ORION_SPEED;
                    }
                    else if (enemy.type.Equals(Enemy.ORION) && enemy.enemy_direction.Equals(GlobalVariables.RIGHT))
                    {
                        graphicMngr.NextOrionRightFrame(enemyCount);
                        enemy_speed = GlobalVariables.ORION_SPEED;
                    }
                    else if (enemy.type.Equals(Enemy.SIRIUS) && enemy.enemy_direction.Equals(GlobalVariables.LEFT))
                    {
                        graphicMngr.NextSiriusLeftFrame(enemyCount);
                        enemy_speed = GlobalVariables.SIRIUS_SPEED;
                    }
                    else if (enemy.type.Equals(Enemy.SIRIUS) && enemy.enemy_direction.Equals(GlobalVariables.RIGHT))
                    {
                        graphicMngr.NextSiriusRightFrame(enemyCount);
                        enemy_speed = GlobalVariables.SIRIUS_SPEED;
                    }
                    else if (enemy.type.Equals(Enemy.LEPUS) && enemy.enemy_direction.Equals(GlobalVariables.UP))
                    {
                        graphicMngr.NextLepusUpFrame(enemyCount);
                        enemy_speed = GlobalVariables.LEPUS_SPEED;
                    }
                    else if (enemy.type.Equals(Enemy.LEPUS) && enemy.enemy_direction.Equals(GlobalVariables.DOWN))
                    {
                        graphicMngr.NextLepusDownFrame(enemyCount);
                        enemy_speed = GlobalVariables.LEPUS_SPEED;
                    }


                    if (GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount] != null && enemy.enemy_direction.Equals(GlobalVariables.LEFT))
                    {
                        if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X -= (2 * enemy_speed);
                        }
                        else
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X -= enemy_speed;
                        }

                        if (graphicMngr.CollisionEnemy(enemyCount))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].enemy_direction = GlobalVariables.RIGHT;
                            if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X += (2 * enemy_speed);
                            }
                            else
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X += enemy_speed;
                            }
                        }
                    }
                    else if (GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount] != null && enemy.enemy_direction.Equals(GlobalVariables.RIGHT))
                    {
                        if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X += (2 * enemy_speed);
                        }
                        else
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X += enemy_speed;
                        }
                        if (graphicMngr.CollisionEnemy(enemyCount))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].enemy_direction = GlobalVariables.LEFT;
                            if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X -= (2 * enemy_speed);
                            }
                            else
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_X -= enemy_speed;
                            }
                        }
                    }
                    else if (GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount] != null && enemy.enemy_direction.Equals(GlobalVariables.UP))
                    {
                        if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y -= (2 * enemy_speed);
                        }
                        else
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y -= enemy_speed;
                        }
                        if (graphicMngr.CollisionEnemy(enemyCount))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].enemy_direction = GlobalVariables.DOWN;
                            if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y += (2 * enemy_speed);
                            }
                            else
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y += enemy_speed;
                            }
                        }
                    }
                    else if (GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount] != null && enemy.enemy_direction.Equals(GlobalVariables.DOWN))
                    {
                        if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y += (2 * enemy_speed);
                        }
                        else
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y += enemy_speed;
                        }
                        if (graphicMngr.CollisionEnemy(enemyCount))
                        {
                            GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].enemy_direction = GlobalVariables.UP;
                            if (enemy.actual_powerup.Equals(Game_Object.MAX_SPEED))
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y -= (2 * enemy_speed);
                            }
                            else
                            {
                                GlobalVariables.ACTUAL_ENEMIES_POSITION[enemyCount].position_Y -= enemy_speed;
                            }
                        }
                    }
                }
                enemyCount++;
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- PRESS PLAY -----------------------------------------------------------------------------------------------------------------
        private Boolean PressPlay()
        {
            Boolean selected = false;
            int x = bb.MouseX();
            int y = bb.MouseY();

            int x_p = GlobalVariables.WIDTH / 2 - GlobalVariables.PLAY_BUTTON_WIDTH / 2;
            int y_p = GlobalVariables.HEIGHT - GlobalVariables.PLAY_BUTTON_HEIGHT * 3 + GlobalVariables.PLAY_BUTTON_HEIGHT / 2;

            int x_m = GlobalVariables.PLAY_BUTTON_WIDTH;
            int y_m = GlobalVariables.PLAY_BUTTON_HEIGHT;
            if (x >= x_p && x <= x_p + x_m
                && y >= y_p && y <= y_p + y_m)
            {
                selected = true;
            }

            return selected;

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SELECT LEVEL ---------------------------------------------------------------------------------------------------------------
        private int SelectLevel()
        {
            int selected = GlobalVariables.ACTUAL_LEVEL;

            for (int i = 0; i <= GlobalVariables.MAX_LEVEL; i++)
            {
                if (bb.RectsOverlap(GlobalVariables.levels[i][0], GlobalVariables.levels[i][1], GlobalVariables.PLAY_BUTTON_WIDTH, GlobalVariables.PLAY_BUTTON_HEIGHT, bb.MouseX(), bb.MouseY(), 1, 1) == 1)
                {
                    selected = i;
                    break;
                }
            }
            return selected;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE TIME ----------------------------------------------------------------------------------------------------------------

        private void UpdateTime()
        {
            DateTime now = DateTime.Now;
            if (now.Subtract(GlobalVariables.TIME_LAG).TotalMilliseconds >= 1000 && GlobalVariables.ACTUAL_TIME >= 0)
            {
                GlobalVariables.ACTUAL_TIME--;
                GlobalVariables.TIME_LAG = DateTime.Now;
                graphicMngr.RenderStatusBar();

                if (GlobalVariables.ACTUAL_TIME <= 0)
                {
                    GlobalVariables.END_TIME = true;
                }

                if (Character.ACTUAL_POWERUP != Game_Object.NULL)
                {
                    Character.POWERUP_TIME_LEFT--;
                    if (Character.POWERUP_TIME_LEFT <= 0)
                    {
                        Character.ACTUAL_POWERUP = Game_Object.NULL;
                        Character.POWERUP_TIME_LEFT = GlobalVariables.POWERUP_TIME;
                    }
                }
            }


        }

        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE BOMBS ---------------------------------------------------------------------------------------------------------------

        private void UpdateBombs()
        {
            DateTime now = DateTime.Now;
            if (now.Subtract(GlobalVariables.BOMBS_LAG).TotalMilliseconds >= 1000 && GlobalVariables.ACTUAL_TIME >= 0)
            {
                List<Bomb> aux = new List<Bomb>();
                foreach (Bomb b in GlobalVariables.bombs)
                {
                    b.lag--;
                    if (b.lag <= 0)
                    {
                        //Explosion
                        GlobalVariables.ACTUAL_ELEMENTS_POSITION[b.position_X][b.position_Y].element = Game_Object.EXPLOSION;
                        aux.Add(b);
                        soundMngr.PlayBombExplosion();
                    }
                }
                foreach (Bomb b in aux)
                {
                    GlobalVariables.bombs.Remove(b);
                }
                GlobalVariables.BOMBS_LAG = DateTime.Now;

                if (soundMngr.BOMB > 0)
                {
                    soundMngr.BOMB--;
                    if (soundMngr.BOMB <= 0)
                    {
                        soundMngr.StopBombExplosion();
                    }
                }
            }
        }

        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- UPDATE LIVES ---------------------------------------------------------------------------------------------------------------
        private void UpdateLifes()
        {
            //if end time or bomberman is died
            if (GlobalVariables.END_TIME || Character.BOMBERMAN_DEAD)
            {
                GlobalVariables.ACTUAL_LIVES--;
            }
            else
            {
                if (GlobalVariables.LEVEL_FINISHED)
                {
                    GlobalVariables.ACTUAL_LIVES++;
                    if (GlobalVariables.MAX_LEVEL < GlobalVariables.LEVELS)
                    {
                        GlobalVariables.MAX_LEVEL++;
                    }

                    //Update score
                    GlobalVariables.ACTUAL_POINTS += GlobalVariables.ACTUAL_TIME;
                }
            }
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------
        */
    }
}
