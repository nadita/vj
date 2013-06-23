using System;

using bb = Blitz3DSDK;

namespace Practico
{
    static class MainClass
    {
        //--GAME CONSTS-------------------------------------------------------------------
        //--------------------------------------------------------------------------------
        static int CAMERA               = 0;
        
        //--------------------------------------------------------------------------------

        static int BOMBER               = 0;

        //--METHODS-----------------------------------------------------------------------
        //--------------------------------------------------------------------------------

        [STAThread]
        static void Main()
        {

            bb.BeginBlitz3D();
            bb.Graphics3D(Constants.WIDTH, Constants.HEIGHT, 32, bb.GFX_WINDOWED);

            createStage();
            createBomberman();
            createLights();
            createCamera();
            
            while (bb.KeyDown(bb.KEY_ESCAPE) == 0)
            {
                bb.UpdateWorld();

                updateCameraPos();

                bb.RenderWorld();
                
                bb.FlushKeys();

                bb.Color(255, 0, 0);

                bb.Flip();
            }

            bb.EndBlitz3D();
        }


        //--------------------------------------------------------------------------------


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
            bb.PositionEntity(CAMERA, 0, 2, 0);
            bb.CameraClsColor(CAMERA, 189, 224, 251);

            bb.CameraRange(CAMERA, 0.1f, 200);
            
        }

        //--------------------------------------------------------------------------------

        static void updateCameraPos() //
        {
            if (bb.KeyDown(bb.KEY_UP) == 1)
            {
                bb.MoveEntity(CAMERA, 0, 0, 0.5f);
            }

            if (bb.KeyDown(bb.KEY_DOWN) == 1)
            {
                bb.MoveEntity(CAMERA, 0, 0, -0.5f);
            }

            if (bb.KeyDown(bb.KEY_LEFT) == 1)
            {
                bb.TurnEntity(CAMERA, 0, 0.5f, 0);
            }

            if (bb.KeyDown(bb.KEY_RIGHT) == 1)
            {
                bb.TurnEntity(CAMERA, 0, -0.5f, 0);
            }

            if (bb.KeyDown(bb.KEY_A) == 1)
            {
                bb.MoveEntity(CAMERA, 0, 0.5f, 0);
            }

            if (bb.KeyDown(bb.KEY_S) == 1)
            {
                bb.MoveEntity(CAMERA, 0, -0.5f, 0);
            }

            if (bb.KeyDown(bb.KEY_1) == 1)
            {
                bb.RotateEntity(CAMERA, 30, 0, 0);
            }

            if (bb.KeyDown(bb.KEY_2) == 1)
            {
                bb.RotateEntity(CAMERA, 90, 0, 0);
                bb.PositionEntity(CAMERA, 0, 20, 0);
            }
        }

        //--------------------------------------------------------------------------------

        public static void createBomberman() //
        {
            BOMBER = bb.LoadMesh("Images//Bomberman//bomber.b3d");
            bb.PositionEntity(BOMBER, 0, 0, 0);
            bb.RotateEntity(BOMBER, 0, 90, 0);
            bb.ScaleEntity(BOMBER, 0.7f, 1, 0.7f);
        }
    }
}
