using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bb = Blitz3DSDK;

namespace Practico
{
    public class SoundMngr
    {
        //--MUSIC AND SOUNDS EFFECTS------------------------------------------------------
        //--------------------------------------------------------------------------------

        public int BOMB_EXPLOSION = 0;
        public int MUSIC = 0;
        public int INTRO = 0;
        public bool SOUND_IS_PLAYED = true;
        public int CHANNEL_MUSIC = 0;
        public int CHANNEL_INTRO = 0;
        public int CHANNEL_BOMB = 0;
        public float MUSIC_VOLUMEN = 0.5F;
        public int BOMB = 0;
        private static SoundMngr instance = null;

        private SoundMngr() { }
        public static SoundMngr GetInstance()
        {
            if (instance == null) {
                instance = new SoundMngr();
            }
            return instance;
        }


        public void InitializeSounds()
        {
            LoadSounds();
        }

        private void LoadSounds()
        {
            MUSIC = bb.LoadSound("Sounds/music02.mp3");
            INTRO = bb.LoadSound("Sounds/music.wav");
            BOMB_EXPLOSION = bb.LoadSound("Sounds/bomb.wav");
            bb.LoopSound(MUSIC);
            bb.LoopSound(INTRO);
            bb.LoopSound(BOMB_EXPLOSION);
        }

        public void PlayMusic()
        {
            CHANNEL_MUSIC = bb.PlaySound(MUSIC);
        }

        public void StopMusic()
        {
            bb.StopChannel(CHANNEL_MUSIC);
        }

        public void PlayIntro()
        {
            CHANNEL_INTRO = bb.PlaySound(INTRO);
        }

        public void StopIntro()
        {
            bb.StopChannel(CHANNEL_INTRO);
        }

        public void PlayBombExplosion()
        {
            CHANNEL_BOMB = bb.PlaySound(BOMB_EXPLOSION);
            BOMB = 2;
        }

        public void StopBombExplosion()
        {
            bb.StopChannel(CHANNEL_BOMB);
            BOMB = 0;
        }

        public void UpdateMusic()
        {
            bb.ChannelVolume(CHANNEL_MUSIC, MUSIC_VOLUMEN);
        }

        public void FreeSounds()
        {
            //bb.FreeSound(MUSIC);
            //bb.FreeSound(BOMB_EXPLOSION);
            //bb.FreeSound(INTRO);
        }
    }
}
