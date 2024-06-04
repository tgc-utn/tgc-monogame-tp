using System.Collections.Generic;
using WarSteel.Scenes;
using WarSteel.UIKit;

public abstract class UIScreen
{
    private string _name;
    private List<UI> _uiElems;

    public string Name => _name;
    public List<UI> UIElems => _uiElems;

    public UIScreen(string name)
    {
        _name = name;
        _uiElems = new List<UI>();
    }

    // adds ui elements
    public abstract void Render(Scene scene);

    // removes all ui elements
    public void Remove()
    {
        _uiElems.ForEach(elem => elem.toDestroy = true);
    }

    public void RemoveUIElem(UI ui)
    {
        _uiElems.Remove(ui);
    }

    public void AddUIElem(UI ui)
    {
        _uiElems.Add(ui);
    }
}