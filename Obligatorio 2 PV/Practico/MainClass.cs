using System;

using bb = Blitz3DSDK;

namespace Practico
{
    static class MainClass
    {
        [STAThread]
        static void Main()
        {
            GameMngr game = GameMngr.GetInstance();
            game.Play();
        }
    }
}
