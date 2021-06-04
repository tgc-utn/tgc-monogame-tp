using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace TGC.MonoGame.TP
{
    class SoundManager
    {
        public static float MusicVolume = 0.8f;
        public SoundManager()
        {
        }


        public static void PlayMusic(Song song)
        {
            StopMusic();
            MediaPlayer.Volume = MusicVolume;
            MediaPlayer.Play(song);
        }
        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        public static void StopSound(SoundEffectInstance instance)
        {
            if(instance != null && instance.State.Equals(SoundState.Playing))
                instance.Stop();

        }
        public static SoundEffectInstance PlaySound(SoundEffect effect)
        {
            return PlaySound(effect, 1f);
        }
        public static SoundEffectInstance PlaySound(SoundEffect effect, float vol)
        {
            var instance = effect.CreateInstance();
            instance.Volume = vol;
            instance.Play();

            return instance;
        }
        public static void Play3DSoundAt(SoundEffect effect, Vector3 sourcePos)
        {
            Play3DSoundAt(effect, sourcePos, 1f);
        }
        public static void Play3DSoundAt(SoundEffect effect, Vector3 sourcePos, float vol)
        {
            var Game = TGCGame.Instance;
            
            var instance = effect.CreateInstance();
            
            var dest = new AudioListener();
            var source = new AudioEmitter();

            //dest.Position = Game.Xwing.Position;
            //source.Position = sourcePos;


            dest.Position = sourcePos;
            source.Position = Game.Xwing.Position;

            instance.Volume = vol;
            //instance.Apply3D(dest, source);
            instance.Pan = 0.8f;
            instance.Play();
        }

        //public enum SoundEffect
        //{
        //    Laser,
        //    TurretLaser,
        //    Boost,
        //    TieExplosion,
        //    TurretExplosion
        //}
    }
}
