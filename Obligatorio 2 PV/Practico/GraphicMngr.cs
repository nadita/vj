﻿using System;
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
        public int STAGE_BACKGROUND = 0;
        public int LEVELS = 0;
        public int[] SELECT_LEVEL_FRAMES = new int[9];
        public int[] SELECTED_LEVELS_FRAMES = new int[8];
        public int BLOCK_LEVEL = 0;
        public int PLAY_BUTTON = 0;
        public int MESSAGE = 0;

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

    }
}