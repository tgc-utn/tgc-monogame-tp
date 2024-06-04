using WarSteel.Managers;
using WarSteel.UIKit;

public class PrimaryBtn : Button
{
    public PrimaryBtn(string text) : base(ContentRepoManager.Instance().GetTexture("UI/primary-btn"), 0.5f, text) { }
}