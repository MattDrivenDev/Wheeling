using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Wheeling;

public static class TexturePack
{
    public static Texture2D Unit { get; private set; }
    public static Texture2D Pixel { get; private set; }

    public static void Load(ContentManager content)
    {
        Unit = content.Load<Texture2D>("wheeling-unit");
        Pixel = content.Load<Texture2D>("pixel");
    }
}