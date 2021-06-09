using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace TGC.MonoGame.TP
{

    class SoundManager
    {
        static SoundEffect Boost;
        static SoundEffect BoostStop;
        static SoundEffect Laser;
        static SoundEffect LaserL;
        static SoundEffect LaserR;
        static SoundEffect TurretLaserL;
        static SoundEffect TurretLaserR;
        static SoundEffect TieExplosionL;
        static SoundEffect TieExplosionR;
        static SoundEffect TurretExplosionL;
        static SoundEffect TurretExplosionR;
        static SoundEffect Distant1L;
        static SoundEffect Distant1R;
        static SoundEffect Distant2L;
        static SoundEffect Distant2R;
        static SoundEffect Distant3L;
        static SoundEffect Distant3R;
        static SoundEffect Repair;
        static Song MainTheme;

        public static float MasterVolume = 1f;
        public static float MusicVolume = 0.8f;
        public static float FXVolume = 0.6f;

        float AngleBetweenVectors(Vector3 a, Vector3 b)
        {
            return MathF.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
        }
        
        public SoundManager()
        {
        }


        public static void LoadContent()
        {
            var Game = TGCGame.Instance;
            var Content = Game.Content;
            var ContentFolderSounds = TGCGame.ContentFolderSounds;
            var ContentFolderMusic = TGCGame.ContentFolderMusic;

            Laser = Content.Load<SoundEffect>(ContentFolderSounds + "laser");
            LaserL = Content.Load<SoundEffect>(ContentFolderSounds + "laserL");
            LaserR = Content.Load<SoundEffect>(ContentFolderSounds + "laserR");
            TurretLaserL = Content.Load<SoundEffect>(ContentFolderSounds + "turretLaserL");
            TurretLaserR = Content.Load<SoundEffect>(ContentFolderSounds + "turretLaserR");
            Boost = Content.Load<SoundEffect>(ContentFolderSounds + "boost");
            BoostStop = Content.Load<SoundEffect>(ContentFolderSounds + "boostStop");
            TieExplosionL = Content.Load<SoundEffect>(ContentFolderSounds + "tieExplosionL");
            TieExplosionR = Content.Load<SoundEffect>(ContentFolderSounds + "tieExplosionR");
            TurretExplosionL = Content.Load<SoundEffect>(ContentFolderSounds + "turretExplosionL");
            TurretExplosionR = Content.Load<SoundEffect>(ContentFolderSounds + "turretExplosionR");
            Distant1L = Content.Load<SoundEffect>(ContentFolderSounds + "distant1L");
            Distant1R = Content.Load<SoundEffect>(ContentFolderSounds + "distant1R");
            Distant2L = Content.Load<SoundEffect>(ContentFolderSounds + "distant2L");
            Distant2R = Content.Load<SoundEffect>(ContentFolderSounds + "distant2R");
            Distant3L = Content.Load<SoundEffect>(ContentFolderSounds + "distant3L");
            Distant3R = Content.Load<SoundEffect>(ContentFolderSounds + "distant3R");
            Repair = Content.Load<SoundEffect>(ContentFolderSounds + "repair");

            MainTheme = Content.Load<Song>(ContentFolderMusic + "TheImperialMarch");

            //PlayMusic(MainTheme);
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
        public static SoundEffectInstance PlaySound(Effect effect)
        {
            return PlaySound(effect, 1f);
        }
        public static SoundEffectInstance PlaySound(Effect effect, float vol)
        {
            SoundEffectInstance sound;
            switch(effect)
            {
                case Effect.XwingLaser:
                    sound = Laser.CreateInstance();
                    break;

                case Effect.Boost:
                    sound = Boost.CreateInstance();
                    break;
                case Effect.BoostStop:
                    sound = BoostStop.CreateInstance();
                    break;
                default:
                    return null;
            }

            sound.Volume = vol;
            sound.Play();

            return sound;
        }
        //TODO: Ver por que no funciona?
        //public static void Play3DSound(SoundEffect effect, Vector3 sourcePos)
        //{
        //    var Game = TGCGame.Instance;
        //    var xwing = Game.Xwing;
        //    var sound = effect.CreateInstance();
        //    var emitter = new AudioEmitter() { Position = sourcePos};
        //    var listener = new AudioListener() {
        //        Position = xwing.Position,
        //        Forward = xwing.FrontDirection,
        //        Up = xwing.UpDirection };

        //    Debug.WriteLine("source " + Game.IntVector3ToStr(sourcePos) + " x " + Game.IntVector3ToStr(xwing.Position));
        //    Debug.WriteLine(" e " + Game.IntVector3ToStr(emitter.Position)+ "l" + Game.IntVector3ToStr(listener.Position));

        //    sound.Play();
        //    sound.Apply3D(listener, emitter);
        //}
        // "3D" effect
        
        public static void Play3DSoundAt(Effect effect, Vector3 sourcePos)
        {
            var Game = TGCGame.Instance;

            SoundEffectInstance soundL, soundR;
            float perFxVol;
            switch (effect)
            {
                case Effect.Laser:
                    soundL = LaserL.CreateInstance();
                    soundR = LaserR.CreateInstance();
                    perFxVol = 0.4f;
                    break;
                case Effect.TurretLaser:
                    soundL = TurretLaserL.CreateInstance();
                    soundR = TurretLaserR.CreateInstance();
                    perFxVol = 0.35f;
                    break;
                case Effect.Distant1:
                    soundL = Distant1L.CreateInstance();
                    soundR = Distant1R.CreateInstance();
                    perFxVol = 0.5f;
                    break;
                case Effect.Distant2:
                    soundL = Distant2L.CreateInstance();
                    soundR = Distant2R.CreateInstance();
                    perFxVol = 0.5f;
                    break;
                case Effect.Distant3:
                    soundL = Distant3L.CreateInstance();
                    soundR = Distant3R.CreateInstance();
                    perFxVol = 0.5f;
                    break;
                case Effect.TieExplosion:
                    soundL = TieExplosionL.CreateInstance();
                    soundR = TieExplosionR.CreateInstance();
                    perFxVol = 1f;
                    break;
                case Effect.TurretExplosion:
                    soundL = TurretExplosionL.CreateInstance();
                    soundR = TurretExplosionR.CreateInstance();
                    perFxVol = 1f;
                    break;
                default:
                    return;        
            }
            
            var xwing = Game.Xwing;

            var sourceDir = Vector3.Normalize(xwing.Position - sourcePos);

            var soundAbsYaw = MathF.Atan2(sourceDir.Z, sourceDir.X) - MathHelper.PiOver2;

            if (soundAbsYaw < 0)
                soundAbsYaw += MathHelper.TwoPi;
            soundAbsYaw %= MathHelper.TwoPi;

            var correctedYaw = MathHelper.ToRadians(xwing.Yaw) - MathHelper.PiOver2;
            var angle = correctedYaw - soundAbsYaw;

            var correctedAng = angle - MathHelper.Pi;

            //Pan calculation
            soundL.Volume = (1f + MathF.Sin(correctedAng)) * 0.5f;
            soundR.Volume = (1f + MathF.Cos(correctedAng)) * 0.5f;

            //Vol correction
            soundL.Volume *= perFxVol * FXVolume * MasterVolume;
            soundR.Volume *= perFxVol * FXVolume * MasterVolume;

            
            soundL.Play();
            soundR.Play();

            
            //Debug.WriteLine(
            //                " aYaw " + MathHelper.ToDegrees(soundAbsYaw) +
            //                " xYaw "+ MathHelper.ToDegrees(correctedYaw) +
            //                " a " + MathHelper.ToDegrees(angle) +
            //                " c " + MathHelper.ToDegrees(correctedAng) +
            //                " L " + soundL.Volume +
            //                " R " + soundR.Volume);
        }

        static float distantC = 0;
        static float cMax = 30;
        public static void UpdateRandomDistantSounds(float elapsedTime)
        {
            
            distantC+= elapsedTime * 30;
            
            if(distantC > cMax)
            {
                distantC = 0;

                Random r = new Random();
                cMax = r.Next(30, 300);
                PlayRandomDistantSound();
            }    
        }
        static void PlayRandomDistantSound()
        {
            var Game = TGCGame.Instance;
            var xwing = Game.Xwing;
            Random r = new Random();
            var index = r.Next(0, 2);
            var delta = 100;
            var RandomPos = xwing.Position + new Vector3(r.Next(-delta, delta), 0f, r.Next(-delta, delta));

            if(index == 0)
                Play3DSoundAt(Effect.Distant1, RandomPos);
            if (index == 1)
                Play3DSoundAt(Effect.Distant2, RandomPos);
            if (index == 2)
                Play3DSoundAt(Effect.Distant3, RandomPos);

        }
        public enum Effect
        {
            XwingLaser,
            Laser,
            TurretLaser,
            Boost,
            BoostStop,
            TieExplosion,
            TurretExplosion,
            Distant1,
            Distant2,
            Distant3,
            Repair
        }
    }
}
