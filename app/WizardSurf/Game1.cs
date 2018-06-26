using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop {
  public class Game1 : Game {
    public GraphicsDeviceManager graphics;
    public InputHelper inputHelper;
    public Vector2 screenCenter;
    public SpriteBatch spriteBatch;

    //TODO move into a Screen and manage there, load screen here, add menu for levels
    private Wizard wizard;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;
      Window.Title = "Wizard Surf";
      Window.AllowUserResizing = true;
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
      spriteBatch = new SpriteBatch(GraphicsDevice);
      inputHelper = new InputHelper();
      wizard = new Wizard(this);
      base.Initialize();
    }

    protected override void LoadContent() {
      wizard.LoadContent();
    }

    protected override void UnloadContent() {
    }

    protected override void Update(GameTime gameTime) {
      inputHelper.Update();
      if (inputHelper.IsNewKeyPress(Keys.Escape)) { Exit(); }
      wizard.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);
      wizard.Draw(gameTime);
      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
