namespace JamGame.UI;

public class UIMessageBox : IUIControl
{
    public IUIParent? Parent { get; set; }
    public bool IsVisible { get; set; }
    
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; }
}