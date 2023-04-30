using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wheeling;

public class Regiment
{
    private Unit[] _units;
    private Int32 _frontage;
    private Vector2 _position;
    private Single _moveInterval = 100f;
    private Double _lastMoveTime;

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

    private void StepBackwards()
    {
        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            var x = unit.Position.X - (Single)Math.Sin(unit.Rotation) * 1.25f;
            var y = unit.Position.Y + (Single)Math.Cos(unit.Rotation) * 1.25f;
            var position = new Vector2(x, y);
            unit.Move(position);
        }
    }

    private void AdvanceForwards()
    {
        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            var x = unit.Position.X + (Single)Math.Sin(unit.Rotation) * 5f;
            var y = unit.Position.Y - (Single)Math.Cos(unit.Rotation) * 5f;
            var position = new Vector2(x, y);
            unit.Move(position);
        }
    }

    private void Wheel(Unit centre, Single rotation)
    {
        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            unit.Rotate(rotation);

            if (unit == centre)
            {
                continue;
            }

            var relativePosition = unit.Position - centre.Position;
            var rotatedPosition = Vector2.Transform(relativePosition, Matrix.CreateRotationZ(rotation));
            var newPosition = centre.Position + rotatedPosition;            
            unit.Move(newPosition);
        }
    }

    private void WheelLeft()
    {
        var centre = _units[0];
        var rotation = -0.0314f;
        Wheel(centre, rotation);
    }

    private void WheelRight()
    {
        var centre = _units[_frontage - 1];
        var rotation = 0.0314f;
        Wheel(centre, rotation);
    }

    private void HandleManeuvers()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            WheelLeft();
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            WheelRight();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            AdvanceForwards();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            StepBackwards();
        }
    }

    public void Update(GameTime gameTime)
    {
        var totalTime = _lastMoveTime + gameTime.TotalGameTime.TotalMilliseconds;
        if (totalTime > _moveInterval)
        {
            HandleManeuvers();
            _lastMoveTime = 0f;
        }
        else
        {
            _lastMoveTime = totalTime;
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