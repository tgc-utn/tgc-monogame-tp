using System.ComponentModel;
using System.Configuration;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace MonoGamers.Audio;

public class AudioController
{
    static Song BackgroundSong { get; set; }
    static SoundEffect JumpSound { get; set; }
    static SoundEffect RiseSound { get; set; }
    static SoundEffect PowerUpSound { get; set; }
    static SoundEffect CheerSound { get; set; }

    static bool StopPlaying { get; set; }
    
    public AudioController(ContentManager Content)
    {
        BackgroundSong = Content.Load<Song>(ConfigurationManager.AppSettings["ContentFolderMusic"] + "Silly");
        JumpSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "bounce1");
        RiseSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "rise");
        PowerUpSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "a1-8bit");
        CheerSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "Cheering");
        StopPlaying = false;

        MediaPlayer.IsRepeating = true;
        PlayMusic();
    }

    public static void PlayMusic()
    {
        MediaPlayer.Play(BackgroundSong);
    }

    public static void StopMusic()
    {
        MediaPlayer.Stop();
    }

    public static void StopSoundEffects()
    {
        StopPlaying = true;
    }

    public static void RestoreSoundEffects()
    {
        StopPlaying = false;
    }

    public static void PlayJump()
    {
        if(!StopPlaying) JumpSound.Play();
    }

    public static void PlayRise()
    {
        if(!StopPlaying) RiseSound.Play();
    }
    
    public static void PlayPowerUp()
    {
        if (!StopPlaying) PowerUpSound.Play();
    }
    
    public static void PlayCheer()
    {
        if(!StopPlaying) CheerSound.Play();
    }
}