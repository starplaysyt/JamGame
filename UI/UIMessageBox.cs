namespace JamGame.UI;

public class UIMessageBox : IControl
{
    public bool IsVisible { get; set; }
    
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; }
}