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
    
    public AudioController(ContentManager Content)
    {
        BackgroundSong = Content.Load<Song>(ConfigurationManager.AppSettings["ContentFolderMusic"] + "Silly");
        JumpSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "bounce1");
        RiseSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "rise");
        PowerUpSound = Content.Load<SoundEffect>(ConfigurationManager.AppSettings["ContentFolderSounds"] + "a1-8bit");


        MediaPlayer.IsRepeating = true;
        PlayMusic();
    }

    public static void PlayMusic()
    {
        MediaPlayer.Play(BackgroundSong);
    }

    public static void PlayJump()
    {
        JumpSound.Play();
    }

    public static void PlayRise()
    {
        RiseSound.Play();
    }
    
    public static void PlayPowerUp()
    {
        PowerUpSound.Play();
    }
}