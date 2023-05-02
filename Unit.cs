using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wheeling;

public class Unit
{
    private Vector2 _position;
    private Single _rotation;
    public static Int32 Width = TexturePack.Unit.Width;
    public static Int32 Height = TexturePack.Unit.Height;
    public Vector2 Position => _position;
    public Single Rotation => _rotation;

    public Unit(Vector2 position)
    {
        _position = position;
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Rotate(Single rotation)
    {
        _rotation += rotation;
    }

    public void Move(Vector2 position)
    {
        _position = position;
    }

    private void Move()
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: TexturePack.Unit,
            position: _position,
            sourceRectangle: new Rectangle(0, 0, Width, Height),
            origin: new Vector2(0, 0),
            color: Color.White,
            rotation: _rotation,
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }
}