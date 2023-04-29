using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wheeling;

public class Regiment
{
    private Unit[] _units;
    private Int32 _frontage;
    private Vector2 _position;

    public Regiment(
        Vector2 position, 
        Int32 size,
        Int32 frontage)
    {
        _position = position;
        _frontage = frontage;       
        _units = new Unit[size]; 

        FillRanks();
    }

    private void FillRanks()
    {
        var frontWidth = _frontage * Unit.Width;
        var left = _position.X - (frontWidth / 2f);
        var depth = _units.Length / _frontage;
        var remainder = _units.Length % _frontage;
        var i = 0;

        // If there aren't enough units to fill the back rank,
        // then we'll have a remainder to squeeze into a back rank.
        if (remainder > 0)
        {
            depth++;
        }

        for (var d = 1; d <= depth; d++)
        for (var w = 1; w <= _frontage; w++)
        {
            // If we've filled the ranks, then we're done.
            if (i >= _units.Length)
            {
                break;
            }

            var x = left + (w * Unit.Width);
            var y = _position.Y + (d * Unit.Height);
            var position = new Vector2(x, y);
            var unit = new Unit(position);
            _units[i] = unit;
            i++;
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (var unit in _units)
        {
            unit.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var unit in _units)
        {
            unit.Draw(spriteBatch);
        }
    }
}