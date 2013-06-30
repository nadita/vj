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
        //--ATRIBUTES---------------------------------------------------------------------
       
        private Game game = Game.GetInstance();
        private GraphicMngr graphicMngr = GraphicMngr.GetInstance();
        private SoundMngr soundMngr = SoundMngr.GetInstance();
        private XmlMngr xmlMngr = XmlMngr.GetInstance();
        private static GameMngr instance = null;
        private Boolean pressedPlay = false;
        private Boolean pressedEdit = false;

        //--METHODS-----------------------------------------------------------------------

        # region --- GET INSTANCE -----------------------------------------------------------------------------------------------------------
        private GameMngr() { }
        public static GameMngr GetInstance()
        {
            if (instance == null)
            {
                instance = new GameMngr();
            }
            return instance;
        }
        # endregion -------------------------------------------------------------------------------------------------------------------------
        
        # region --- PLAY -------------------------------------------------------------------------------------------------------------------
        public void Play()
        {
            bb.BeginBlitz3D();
            bb.Graphics3D(Constants.WIDTH, Constants.HEIGHT, 32, bb.GFX_WINDOWED);
            bb.SetBlitz3DTitle("BombermanG 3d", "Exit?");

            InitializeWorld();

            bb.SetBuffer(bb.BackBuffer());

            StartGame();
            SelectStage();
            FreeWorld();

            bb.EndBlitz3D();
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
                while (!pressedPlay && !pressedEdit)
                {
                    bb.Cls();
                    RenderBackgroundSelectLevel();
                    bb.Flip();
                    bb.WaitMouse();
                    PressOption();
                    if (!pressedPlay && !pressedEdit)
                    {
                        int newSelectedLevel = SelectLevel();
                        if (game.actual_stage != newSelectedLevel)
                        {
                            game.actual_stage = newSelectedLevel;
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
            graphicMngr.RenderLevelOptions(game.max_stage, game.levels, game.total_stages, game.actual_stage);
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- INITIALIZE LEVEL OPTIONS ---------------------------------------------------------------------------------------------------
        private void InitializeLevelsOptions()
        {
            game.levels = new int[game.total_stages][];
            for (int i = 0; i < game.total_stages; i++)
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

            for (int i = 0; i < game.max_stage; i++)
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

            InitializeStage();
            
            while (bb.KeyDown(bb.KEY_ESCAPE) == 0)
            {
                UpdateStage();

                bb.UpdateWorld();

                bb.RenderWorld();

                PostRenderMethods();

                bb.FlushKeys();

                bb.Color(255, 0, 0);

                bb.Flip();
            }
            Message();
            SelectStage();
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- EDIT STAGE -----------------------------------------------------------------------------------------------------------------
        public void EditStage(){
            InitializeStage();

            while (bb.KeyDown(bb.KEY_ESCAPE) == 0)
            {
                UpdateEditStage();

                bb.UpdateWorld();

                bb.RenderWorld();

                bb.Text(20, 100, "Bomber X: " + (int)bb.EntityX(graphicMngr.BOMBER_SPHERE));
                bb.Text(20, 140, "Bomber Z: " + (int)bb.EntityZ(graphicMngr.BOMBER_SPHERE));

                AddElement();

                bb.FlushKeys();

                bb.Color(255, 0, 0);

                bb.Flip();
            }
            
            xmlMngr.SaveXmlConf();
            //SelectStage();
        }

        private void AddElement() {

            int x = (int)bb.EntityX(graphicMngr.BOMBER_SPHERE)/2;
            int z = (int)bb.EntityZ(graphicMngr.BOMBER_SPHERE)/2;

            if (bb.KeyDown(bb.KEY_A) == 1)
            {
                if (Editor.GetInstance().addBlock(x, z * (-1)))
                {
                    game.actualStage.addItem(new Cell(Cell.BLOCK, Cell.NONE), x, z * (-1));
                    graphicMngr.CreateBlock(x * Constants.BLOCK_FACTOR, z * Constants.BLOCK_FACTOR);
                }
            }
            else {
                if (bb.KeyDown(bb.KEY_S) == 1)
                {
                    String ret = Editor.GetInstance().addPowerUp(x, z * (-1));
                    if (ret == Cell.MAX_SPEED)
                    {
                        game.actualStage.addItem(new Cell(Cell.BLOCK, Cell.MAX_SPEED), x, z * (-1));
                        graphicMngr.CreateBlock(x * Constants.BLOCK_FACTOR, z * Constants.BLOCK_FACTOR);
                    }
                    else {
                        if (ret == Cell.EXTRA_POWER)
                        {
                            game.actualStage.addItem(new Cell(Cell.BLOCK, Cell.EXTRA_POWER), x, z * (-1));
                            graphicMngr.CreateBlock(x * Constants.BLOCK_FACTOR, z * Constants.BLOCK_FACTOR);
                        }
                    }
                }
                else {
                    if (bb.KeyDown(bb.KEY_D) == 1)
                    {
                        String ret = Editor.GetInstance().addEnemy(x, z * (-1));
                        if (ret == Cell.SIRIUS)
                        {
                            game.actualStage.addItem(new Cell(Cell.ENEMY, Cell.SIRIUS), x, z * (-1));
                            graphicMngr.CreateSirius(x, z * (-1));
                        }
                        else
                        {
                            if (ret == Cell.ORION)
                            {
                                game.actualStage.addItem(new Cell(Cell.ENEMY, Cell.ORION), x, z * (-1));
                                graphicMngr.CreateSirius(x, z * (-1));
                            }
                        }
                    }
                    else {
                        if (bb.KeyDown(bb.KEY_F) == 1)
                        {
                            Editor.GetInstance().deleteItem(x, z);
                        }
                    }
                }
            }
            
        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        # region --- SELECT ACTION --------------------------------------------------------------------------------------------------------------
        private void SelectAction() {

            Boolean playPressed = false;
            Boolean editPressed = false;
            while (!playPressed && !editPressed)
            {
                bb.Cls();
                bb.Flip();
                if (pressedEdit)
                {
                    editPressed = true;
                    EditStage();
                }
                else {
                    if (pressedPlay) {
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

        # region --- PRESS OPTION -----------------------------------------------------------------------------------------------------------------
        private void PressOption()
        {
            int x = bb.MouseX();
            int y = bb.MouseY();

            int x_p = graphicMngr.PLAY_BUTTON_X_POSITION;
            int y_p = graphicMngr.PLAY_BUTTON_Y_POSITION;

            int x_e = graphicMngr.EDIT_BUTTON_X_POSITION;
            int y_e = graphicMngr.EDIT_BUTTON_Y_POSITION;

            int x_m = Constants.PLAY_BUTTON_WIDTH;
            int y_m = Constants.PLAY_BUTTON_HEIGHT;
            if (x >= x_p && x <= x_p + x_m
                && y >= y_p && y <= y_p + y_m)
            {
                pressedPlay = true;
            }
            else if (x >= x_e && x <= x_e + x_m
                && y >= y_e && y <= y_e + y_m)
            {
                pressedEdit = true;
            }

        }
        # endregion -----------------------------------------------------------------------------------------------------------------------------

        public void InitializeWorld() //
        {
            xmlMngr.ReadXmlConf();
            graphicMngr.InitializeGraphics();
            soundMngr.InitializeSounds();
        }

        public void InitializeStage() //
        {
            Game.GetInstance().InitializeGameObjectValues();
            graphicMngr.InitializeStage();
        }

        public void UpdateStage()
        {
            graphicMngr.UpdateKeyBoard(); //en vez de esto, despues habria q llamar a update bomberman
            graphicMngr.UpdateOrion();
            graphicMngr.UpdateSirius();
        }

        public void UpdateEditStage()
        {
            graphicMngr.UpdateKeyBoard();
        }

        public void PostRenderMethods()
        {
            graphicMngr.HUD();
        }

        public void FreeWorld()
        {
            graphicMngr.FreeGraphics();
            soundMngr.FreeSounds();
        }

        public void FreeStage()
        {
            soundMngr.FreeSounds();
        }
    }
}