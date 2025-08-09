namespace JamGame.UI.Fonts;

public static class FontManager
{
    public static Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

    public static void LoadFonts()
    {
        Fonts.Add("GohuFont", new Font("Assets/Fonts/GohuFont-Powerline.ttf"));
    }
}