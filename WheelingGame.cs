using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wheeling;

public class WheelingGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Regiment _regiment;

    public WheelingGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1680;
        _graphics.PreferredBackBufferHeight = 1050;
        GraphicsDevice.SetRenderTarget(new RenderTarget2D(GraphicsDevice, 1680, 1050));
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        TexturePack.Load(Content);

        _regiment = new Regiment(
            position: new Vector2(960, 540),
            size: 20,
            frontage: 5);
        _regiment.Select();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        _regiment.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        _regiment.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
