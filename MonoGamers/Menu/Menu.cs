using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGamers.Audio;

namespace MonoGamers.Menu;

public class Menu
{
    public bool OnMenu { get; set; }
    
    private TGCGame TGCGame { get; set; }
    
    private Button PlayButton { get; set; }
    private Button ExitButton { get; set; }
    private Button MusicEnabledButton { get; set; }
    private Button MusicDisabledButton { get; set; }
    private Button SoundEnabledButton { get; set; }
    private Button SoundDisabledButton { get; set;}
    private Texture2D Title { get; set; }
    
    bool musicButtonPressed = false;
    bool soundButtonPressed = false;

    private ContentManager _contentManager;
    private GraphicsDevice _graphicsDevice;
    
    
    public Menu(ContentManager content,GraphicsDevice graphicsDevice, TGCGame tgcGame)
    {
        _contentManager = content;
        _graphicsDevice = graphicsDevice;
        OnMenu = true;
        TGCGame = tgcGame;

    }

    public void LoadContent()
    {
        var width = _graphicsDevice.Viewport.Width;
        var height = _graphicsDevice.Viewport.Height;
        Title = _contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/title");

        PlayButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/play"), new Vector2(width * 0.375f, height * 0.35f), 1.1f);
        ExitButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/exit"), new Vector2(width * 0.395f, height * 0.55f), 1f);
        MusicEnabledButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/musicOn"), new Vector2(width*0.95f, height*0.01f), 0.8f);
        MusicDisabledButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/musicOff"), new Vector2(width*0.95f, height*0.01f), 0.8f);
        SoundEnabledButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/audioOn"), new Vector2(width*0.9f, height*0.01f), 0.8f);
        SoundDisabledButton = new Button(_contentManager.Load<Texture2D>(ConfigurationManager.AppSettings["ContentFolderTextures"] + "menu/audioOff"), new Vector2(width*0.9f, height*0.01f), 0.8f);
    }

    public void Update(MouseState PreviousMouseState, MouseState MouseState)
    {
        if (PlayButton.IsPressed(PreviousMouseState, MouseState)) { 
            OnMenu = false;
        }
        
        if (ExitButton.IsPressed(PreviousMouseState, MouseState)) 
            TGCGame.Exit();
        
        if (MusicEnabledButton.IsPressed(PreviousMouseState, MouseState)) 
        {
            musicButtonPressed = true;
            soundButtonPressed = false;
        }
        if (SoundEnabledButton.IsPressed(PreviousMouseState, MouseState)) 
        {
            soundButtonPressed = true;
            musicButtonPressed = false;
        }
        if (musicButtonPressed)
        {
            AudioController.PlayMusic();
        }
        else if (MusicDisabledButton.IsPressed(PreviousMouseState, MouseState))
        {
            AudioController.StopMusic();
        }
        if (soundButtonPressed)
        {
            AudioController.RestoreSoundEffects();
        }
        else if (SoundDisabledButton.IsPressed(PreviousMouseState, MouseState))
        {
            AudioController.StopSoundEffects();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Title, new Rectangle(550, 100, Title.Width, Title.Height), Color.White);
        PlayButton.Render(spriteBatch);
        ExitButton.Render(spriteBatch);
        SoundEnabledButton.Render(spriteBatch);
        SoundDisabledButton.Render(spriteBatch);
        MusicEnabledButton.Render(spriteBatch);
        MusicDisabledButton.Render(spriteBatch);
    }
}