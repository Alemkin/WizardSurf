using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.Screens;

namespace WizardSurf.Desktop {
  public class Game1 : Game {
    public GraphicsDeviceManager graphics;
    public InputHelper inputHelper;
    public Vector2 screenCenter;
    public SpriteBatch spriteBatch;

    // TODO add menu screen
    // TODO add screen manager that will handle all of the screens via menus
    public Entity surfScreen;

    public DebugPanel debugPanel;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 640;
      graphics.PreferredBackBufferHeight = 480;
      Window.Title = "Wizard";
      Window.AllowUserResizing = true;
      Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
      screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
      spriteBatch = new SpriteBatch(GraphicsDevice);
      inputHelper = new InputHelper();
      surfScreen = new SurfScreen(this);
      debugPanel = new DebugPanel(this);
      base.Initialize();
    }

    protected override void LoadContent() {
      surfScreen.LoadContent();
      debugPanel.LoadContent();
    }

    protected override void UnloadContent() {
      surfScreen.UnloadContent();
      debugPanel.UnloadContent();
    }

    protected override void Update(GameTime gameTime) {
      inputHelper.Update();
      if (inputHelper.IsNewKeyPress(Keys.Escape)) { Exit(); }
      surfScreen.Update(gameTime);
      debugPanel.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                        DepthStencilState.None, RasterizerState.CullCounterClockwise);
      surfScreen.Draw(gameTime);
      debugPanel.Draw(gameTime);
      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
