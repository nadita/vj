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
        private Game game = Game.GetInstancia();
        private GraphicMngr graphicMngr = GraphicMngr.GetInstancia();
        private SoundMngr soundMngr = SoundMngr.GetInstancia();
        private XmlMngr xmlMngr = XmlMngr.GetInstancia();
        private static GameMngr instancia = null;

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

        static int BLOCK = 0;

        //--METHODS-----------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        

        private GameMngr() {}
        public static GameMngr GetInstancia()
        {
            if (instancia == null) {
                instancia = new GameMngr();
            }
            return instancia;
        }


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
            SelectStage();

            

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

        # endregion -----------------------------------------------------------------------------------------------------------------------------

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
            if (game.actual_lives > 0)
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
                        if (game.actual_stage != newSelectedLevel)
                        {
                            game.actual_stage = newSelectedLevel;
                            //GlobalVariables.InicializeGlobalVariableValues(xmlMngr.levels[GlobalVariables.ACTUAL_LEVEL]);
                            //RenderBackgroundSelectLevel();
                        }
                    }
                    bb.Flip();
                }
                SelectAction();
                bb.EndBlitz3D();
            }
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
            graphicMngr.RenderLevelOptions(game.max_stage, game.levels, game.stageList.Count, game.actual_stage);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- INITIALIZE LEVEL OPTIONS ---------------------------------------------------------------------------------------------------
        private void InitializeLevelsOptions()
        {
            game.levels = new int[game.stageList.Count][];
            for (int i = 0; i < game.stageList.Count; i++)
            {
                game.levels[i] = new int[2];
                game.levels[i][0] = (Constants.WIDTH / 5) * (i + 1) - Constants.LEVEL_BLOCK_WIDTH / 2;
                game.levels[i][1] = Constants.HEIGHT / 4 + Constants.LEVEL_BLOCK_WIDTH / 2;
            }

        }

        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SELECT LEVEL ---------------------------------------------------------------------------------------------------------------
        private int SelectLevel()
        {
            int selected = game.actual_stage;

            for (int i = 0; i < game.stageList.Count; i++)
            {
                if (bb.RectsOverlap(game.levels[i][0], game.levels[i][1], Constants.PLAY_BUTTON_WIDTH, Constants.PLAY_BUTTON_HEIGHT, bb.MouseX(), bb.MouseY(), 1, 1) == 1)
                {
                    selected = i;
                    break;
                }
            }
            return selected;
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- PLAY STAGE -----------------------------------------------------------------------------------------------------------------
        private void PlayStage() {
            createStage();
            loadStage();
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

           
            Message();
            SelectStage();
        }
        /*private void PlayStage()
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
        }*/
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- PLAY STAGE -----------------------------------------------------------------------------------------------------------------
        public void EditStage(){
        
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SELECT ACTION --------------------------------------------------------------------------------------------------------------
        private void SelectAction() {

            Boolean playPressed = false;
            while (!playPressed)
            {
                bb.Cls();
                graphicMngr.RenderMessage("Press E to Edit or P to Play!");
                bb.Flip();
                bb.FlushKeys();
                bb.WaitKey();
                if (bb.KeyHit(bb.KEY_E) >= 1)
                {
                    playPressed = true;
                    EditStage();
                }
                else {
                    if (bb.KeyHit(bb.KEY_P) >= 1) {
                        playPressed = true;
                        PlayStage();
                    }
                }
            }
           
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------
      
        # region --- MESSAGE --------------------------------------------------------------------------------------------------------------------
        private void Message()
        {

            //ISSUE 5: EVALUAR RESULTADO DEL JUEGO PARA SETEAR MESSAGE
            //GAME OVER: SI PERDI  Y NO TENGO MAS VIDAS
            //PANTALLA COMPLETA: SI GANE
            //VIDAS RESTANTES SI PERDI (POR BOMBA O ENEMIGO O TIEMPO)

            if (game.end_time)
            {
                game.message = "TIME OUT!";
            }

            if (game.bomberman_is_dead)
            {
                game.message = "BOMBERMAN IS DEAD" + "\n LIFES: " + game.actual_lives;
                if (soundMngr.BOMB > 0)
                {
                    soundMngr.StopBombExplosion();
                }
            }

            if (game.level_finished)
            {
                game.message = "LEVEL FINISHED" + "\n LIFES: " + game.actual_lives + "\n RECORD: " + game.actual_score + " POINTS";
            }

            if (game.actual_lives <= 0)
            {
                game.message = "GAME OVER :)";
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
            graphicMngr.RenderMessage(game.message);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------


        public void createStage() //
        {
            int stage = bb.LoadMesh("Images//Stage//stage.3DS");
            bb.PositionEntity(stage, 0, 0.1f, 0);
        }

        public void loadStage() {
            Stage stage = game.stageList.ElementAt(game.actual_stage);

            for (int i = 0; i < stage.matrix.Length; i++) {
                for (int j = 0; j < stage.matrix[i].Length; j++) {
                    if (stage.matrix[i][j] != null && stage.matrix[i][j].type == "Brick") {
                        createBlock(i, (j * Constants.BLOCK_FACTOR));
                    }
                }
            }
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
            bb.PositionEntity(BOMBER, BOMBER_X, 0, BOMBER_Y);
            bb.RotateEntity(BOMBER, 0, 270, 0);
            bb.ScaleEntity(BOMBER, 0.7f, 1, 0.7f);
            BOMBER_WALK = bb.ExtractAnimSeq(BOMBER, 30, 60);
        }

        public static void createBlock(float x, float y) {
            BLOCK = bb.LoadAnimMesh("Images//Stage//block.3DS");
            bb.PositionEntity(BLOCK, x, 0, y);
            //bb.RotateEntity(BLOCK, 0, 270, 0);
            bb.ScaleEntity(BLOCK, 0.7f, 1, 0.7f);
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
        
        # region --- PRESS PLAY -----------------------------------------------------------------------------------------------------------------
        private Boolean PressPlay()
        {
            Boolean selected = false;
            int x = bb.MouseX();
            int y = bb.MouseY();

            int x_p = Constants.WIDTH / 2 - Constants.PLAY_BUTTON_WIDTH / 2;
            int y_p = Constants.HEIGHT - Constants.PLAY_BUTTON_HEIGHT * 3 + Constants.PLAY_BUTTON_HEIGHT / 2;

            int x_m = Constants.PLAY_BUTTON_WIDTH;
            int y_m = Constants.PLAY_BUTTON_HEIGHT;
            if (x >= x_p && x <= x_p + x_m
                && y >= y_p && y <= y_p + y_m)
            {
                selected = true;
            }

            return selected;

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------
    }
}