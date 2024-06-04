using WarSteel.Managers;
using WarSteel.UIKit;

public class SecondaryBtn : Button
{
    public SecondaryBtn(string text) : base(ContentRepoManager.Instance().GetTexture("UI/secondary-btn"), 0.5f, text) { }
}