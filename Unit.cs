using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wheeling;

public class Unit
{
    private Vector2 _position;
    private Single _rotation;
    private Single _moveInterval = 100f;
    private Double _lastMoveTime;
    public static Int32 Width = TexturePack.Unit.Width;
    public static Int32 Height = TexturePack.Unit.Height;

    public Unit(Vector2 position)
    {
        _position = position;
    }

    public void Update(GameTime gameTime)
    {
        var totalTime = _lastMoveTime + gameTime.TotalGameTime.TotalMilliseconds;
        if (totalTime > _moveInterval)
        {
            Move();
            _lastMoveTime = 0f;
        }
        else
        {
            _lastMoveTime = totalTime;
        }
    }

    private void Move()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _rotation -= 0.1f;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _rotation += 0.1f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _position.X += (Single)Math.Sin(_rotation) * 5f;
            _position.Y -= (Single)Math.Cos(_rotation) * 5f;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: TexturePack.Unit,
            position: _position,
            sourceRectangle: new Rectangle(0, 0, Width, Height),
            origin: new Vector2(Width / 2f, Height / 2f),
            color: Color.White,
            rotation: _rotation,
            scale: 1f,
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }
}