using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wheeling;

public class Regiment
{
    private Unit[] _units;
    private Int32 _frontage;
    private Int32 _depth;
    private Vector2 _position;
    private Vector2[] _selectedCoords;
    private Single _moveInterval = 100f;
    private Double _lastMoveTime;
    public Boolean Selected { get; private set; }
    public Single Rotation => _units[0].Rotation;

    public Regiment(
        Vector2 position, 
        Int32 size,
        Int32 frontage)
    {
        _position = position;
        _frontage = frontage;       
        _units = new Unit[size]; 

        _selectedCoords = new Vector2[]
        {
            // Front-Left
            new Vector2(_position.X - (Width / 2f), _position.Y),
            // Front-Right
            new Vector2(_position.X + (Width / 2f), _position.Y),
            // Back-Left
            new Vector2(_position.X - (Width / 2f), _position.Y + Height),
            // Back-Right
            new Vector2(_position.X + (Width / 2f), _position.Y + Height)
        };

        FillRanks();
    }

    public Int32 Width => _frontage * Unit.Width;
    public Int32 Height => _depth * Unit.Height;

    public void Select() => Selected = true;

    private void FillRanks()
    {
        var frontWidth = _frontage * Unit.Width;
        var left = _position.X - (frontWidth / 2f) - Unit.Width;
        var depth = _units.Length / _frontage;
        var remainder = _units.Length % _frontage;
        var i = 0;

        // If there aren't enough units to fill the back rank,
        // then we'll have a remainder to squeeze into a back rank.
        if (remainder > 0)
        {
            depth++;
        }

        _depth = depth;

        for (var d = 1; d <= _depth; d++)
        for (var w = 1; w <= _frontage; w++)
        {
            // If we've filled the ranks, then we're done.
            if (i >= _units.Length)
            {
                break;
            }

            var x = left + (w * Unit.Width);
            var y = _position.Y + (d * Unit.Height) - Unit.Height;
            var position = new Vector2(x, y);
            var unit = new Unit(position);
            _units[i] = unit;
            i++;
        }
    }

    private void StepBackwards()
    {
        _position.X -= (Single)Math.Sin(Rotation) * 1.25f;
        _position.Y += (Single)Math.Cos(Rotation) * 1.25f;

        for (var i = 0; i < _selectedCoords.Length; i++)
        {
            var selectedCoord = _selectedCoords[i];
            var x = selectedCoord.X - (Single)Math.Sin(Rotation) * 1.25f;
            var y = selectedCoord.Y + (Single)Math.Cos(Rotation) * 1.25f;
            var position = new Vector2(x, y);
            _selectedCoords[i] = position;
        }

        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            var x = unit.Position.X - (Single)Math.Sin(Rotation) * 1.25f;
            var y = unit.Position.Y + (Single)Math.Cos(Rotation) * 1.25f;
            var position = new Vector2(x, y);
            unit.Move(position);
        }
    }

    private void AdvanceForwards()
    {
        _position.X += (Single)Math.Sin(Rotation) * 5f;
        _position.Y -= (Single)Math.Cos(Rotation) * 5f;

        for (var i = 0; i < _selectedCoords.Length; i++)
        {
            var selectedCoord = _selectedCoords[i];
            var x = selectedCoord.X + (Single)Math.Sin(Rotation) * 5f;
            var y = selectedCoord.Y - (Single)Math.Cos(Rotation) * 5f;
            var position = new Vector2(x, y);
            _selectedCoords[i] = position;
        }

        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            var x = unit.Position.X + (Single)Math.Sin(Rotation) * 5f;
            var y = unit.Position.Y - (Single)Math.Cos(Rotation) * 5f;
            var position = new Vector2(x, y);
            unit.Move(position);
        }
    }

    private void Wheel(Unit centre, Single rotation)
    {
        var rotationMatrix = Matrix.CreateRotationZ(rotation);
        
        var relativeRegimentPosition = _position - centre.Position;
        var rotatedRegimentPosition = Vector2.Transform(relativeRegimentPosition, rotationMatrix);
        var newRegimentPosition = centre.Position + rotatedRegimentPosition;
        _position = newRegimentPosition;
        
        for (var i = 0; i < _selectedCoords.Length; i++)
        {
            var relativeSelectedPosition = _selectedCoords[i] - centre.Position;
            var rotatedSelectedPosition = Vector2.Transform(relativeSelectedPosition, rotationMatrix);
            var newSelectedPosition = centre.Position + rotatedSelectedPosition;
            _selectedCoords[i] = newSelectedPosition;
        }

        for (var i = 0; i < _units.Length; i++)
        {
            var unit = _units[i];
            unit.Rotate(rotation);

            if (unit == centre)
            {
                continue;
            }

            var relativePosition = unit.Position - centre.Position;
            var rotatedPosition = Vector2.Transform(relativePosition, rotationMatrix);
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
        if (Keyboard.GetState().IsKeyDown(Keys.Left)
            || Keyboard.GetState().IsKeyDown(Keys.A))
        {
            WheelLeft();
            return;
        }
        
        if (Keyboard.GetState().IsKeyDown(Keys.Right)
            || Keyboard.GetState().IsKeyDown(Keys.D))
        {
            WheelRight();
            return;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up)
            || Keyboard.GetState().IsKeyDown(Keys.W))
        {
            AdvanceForwards();
            return;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down)
            || Keyboard.GetState().IsKeyDown(Keys.S))
        {
            StepBackwards();
            return;
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

        if (Selected)
        {
            DrawSelected(spriteBatch);
        }
    }

    private void DrawSelected(SpriteBatch spriteBatch)
    {   
        spriteBatch.Draw(
            texture: TexturePack.Pixel,
            position: _selectedCoords[0],
            sourceRectangle: null,
            origin: new Vector2(0, 0),
            color: Color.Yellow,
            rotation: Rotation,
            scale: new Vector2(Width, 2),
            effects: SpriteEffects.None,
            layerDepth: 1f);
            
        spriteBatch.Draw(
            texture: TexturePack.Pixel,
            position: _selectedCoords[1],
            sourceRectangle: null,
            origin: new Vector2(0, 0),
            color: Color.Yellow,
            rotation: Rotation,
            scale: new Vector2(2, Height),
            effects: SpriteEffects.None,
            layerDepth: 1f);
            
        spriteBatch.Draw(
            texture: TexturePack.Pixel,
            position: _selectedCoords[2],
            sourceRectangle: null,
            origin: new Vector2(0, -(Height / 2)),
            color: Color.Yellow,
            rotation: Rotation,
            scale: new Vector2(Width, 2),
            effects: SpriteEffects.None,
            layerDepth: 1f);
            
        spriteBatch.Draw(
            texture: TexturePack.Pixel,
            position: _selectedCoords[3],
            sourceRectangle: null,
            origin: new Vector2(Width / 2, 0),
            color: Color.Yellow,
            rotation: Rotation,
            scale: new Vector2(2, Height),
            effects: SpriteEffects.None,
            layerDepth: 1f);
    }
}