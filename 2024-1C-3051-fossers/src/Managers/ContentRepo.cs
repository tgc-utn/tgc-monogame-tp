using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace WarSteel.Managers;

public class ContentRepoManager
{
    private const string ContentFolder3D = "Models/";
    private const string ContentFolderEffects = "Effects/";
    private const string ContentFolderAudio = "Audio/";
    private const string ContentFolderSpriteFonts = "SpriteFonts/";
    private const string ContentFolderTextures = "Textures/";

    private ContentManager _manager;

    private static ContentRepoManager _INSTANCE = null;

    public ContentManager Manager
    {
        get => _manager;
    }

    public static void SetUpInstance(ContentManager manager)
    {
        _INSTANCE = new ContentRepoManager
        {
            _manager = manager
        };
    }

    public static ContentRepoManager Instance() => _INSTANCE;

    public Effect GetEffect(string effect)
    {
        return _manager.Load<Effect>(ContentFolderEffects + effect);
    }

    public Model GetModel(string model)
    {
        return _manager.Load<Model>(ContentFolder3D + model);
    }

    public Texture2D GetTexture(string texture)
    {
        return _manager.Load<Texture2D>(ContentFolderTextures + texture);
    }

    public TextureCube GetTextureCube(string texture)
    {
        return _manager.Load<TextureCube>(ContentFolderTextures + texture);
    }

    public SpriteFont GetSpriteFont(string font)
    {
        return _manager.Load<SpriteFont>(ContentFolderSpriteFonts + font);
    }

    public SoundEffect GetSoundEffect(string audio)
    {
        return _manager.Load<SoundEffect>(ContentFolderAudio + audio);
    }

    public Song GetSong(string song)
    {
        return _manager.Load<Song>(ContentFolderAudio + song);
    }
}
