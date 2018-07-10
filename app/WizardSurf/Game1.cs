using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.Screens;
using WizardSurf.Desktop.Engines;

namespace WizardSurf.Desktop {
  public class Game1 : Game {
    public GraphicsDeviceManager graphics;
    public InputHelper inputHelper;
    public Vector2 screenCenter;
    public SpriteBatch spriteBatch;
    public static Random random = new Random();
    private LevelEngine levelEngine = new LevelEngine();

    public enum GameState { START, SURFSCREEN, PAUSE }
    public GameState CurrentGameState { get; set; }

    // TODO add screen manager that will handle all of the screens via menus
    //TODO add score tracker for levels completed, and lives lost per level
    public SurfScreen surfScreen;
    public Entity startScreen;

    public DebugPanel debugPanel;

    public bool GameOver { get; set; }

    public static int DEFAULT_LEVEL_FRAME_COUNT = 15 * 60;
    private int frameCountToNextLevel = DEFAULT_LEVEL_FRAME_COUNT;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;
      Window.Title = "Wizard";
      Content.RootDirectory = "Content";
      CurrentGameState = GameState.START;
      GameOver = false;
    }

    protected override void Initialize() {
      screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
      spriteBatch = new SpriteBatch(GraphicsDevice);
      inputHelper = new InputHelper();
      surfScreen = new SurfScreen(this, levelEngine.GetNextLevel());
      debugPanel = new DebugPanel(this);
      startScreen = new StartScreen(this);
      base.Initialize();
    }

    protected override void LoadContent() {
      startScreen.LoadContent();
      surfScreen.LoadContent();
      debugPanel.LoadContent();
    }

    protected override void UnloadContent() {
      startScreen.UnloadContent();
      surfScreen.UnloadContent();
      debugPanel.UnloadContent();
    }

    protected override void Update(GameTime gameTime) {
      base.Update(gameTime);
      inputHelper.Update();
      if (CurrentGameState == GameState.SURFSCREEN) {
        if (inputHelper.IsNewKeyPress(Keys.Escape)) {
          CurrentGameState = GameState.PAUSE;
          surfScreen.Update(gameTime);
          debugPanel.Update(gameTime);
        }
      }

      if (CurrentGameState == GameState.START || CurrentGameState == GameState.PAUSE) {
        startScreen.Update(gameTime);
      }
      if (CurrentGameState == GameState.SURFSCREEN) {
        surfScreen.Update(gameTime);
        debugPanel.Update(gameTime);
        if (surfScreen.HasWon(gameTime) && GameOver == false) {
          if (frameCountToNextLevel-- < 1) {
            frameCountToNextLevel = DEFAULT_LEVEL_FRAME_COUNT;
            var nextLevel = levelEngine.GetNextLevel();
            if (nextLevel.Lives == 0) {
              GameOver = true;
            } else {
              surfScreen = new SurfScreen(this, nextLevel);
              surfScreen.LoadContent();
            }
          }
        }
      }

    }

    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.LavenderBlush);
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                        DepthStencilState.None, RasterizerState.CullCounterClockwise);

      if (CurrentGameState == GameState.START || CurrentGameState == GameState.PAUSE) {
        startScreen.Draw(gameTime);
      }
      if (CurrentGameState == GameState.SURFSCREEN) {
        surfScreen.Draw(gameTime);
        debugPanel.Draw(gameTime);
      }

      spriteBatch.End();
      base.Draw(gameTime);
    }
  }
}
