using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop;

namespace WizardSurf.Desktop.Screens {
  public class StartScreen : BaseScreen {
    public enum Option { START, QUIT }
    private SpriteFont font;
    private Texture2D background;
    private Rectangle pauseRectangle;

    private Vector2 startOptionPosition;
    private Vector2 quitOptionPosition;

    private float scale = 1.5f;

    private Option selectedOption;

    private string StartString = "Start Game";

    public StartScreen(Game1 game) : base(game) {
      var centerScreen = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - 50f, 
                                     (game.graphics.PreferredBackBufferHeight / 2) - 50f);
      startOptionPosition = new Vector2(centerScreen.X, centerScreen.Y);
      quitOptionPosition = new Vector2(centerScreen.X, centerScreen.Y + 50f);
      pauseRectangle = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      selectedOption = Option.START;
    }

    public override void LoadContent() {
      font = game.Content.Load<SpriteFont>("Font");
      background = game.Content.Load<Texture2D>("pause_background");
    }

    public override void UnloadContent() {
    }

    public override void Update(GameTime gameTime) {
      if (game.CurrentGameState == Game1.GameState.PAUSE) {
        StartString = "Resume";
      } else {
        StartString = "Start Game";
      }

      if (game.inputHelper.IsNewKeyPress(Keys.Down) && selectedOption == Option.START) {
        selectedOption = Option.QUIT;
      } else if (game.inputHelper.IsNewKeyPress(Keys.Up) && selectedOption == Option.QUIT) {
        selectedOption = Option.START;
      }

      if (game.inputHelper.IsNewKeyPress(Keys.Enter)) {
        switch(selectedOption) {
          case Option.START:
            game.CurrentGameState = Game1.GameState.SURFSCREEN;
            break;
          case Option.QUIT:
            game.Exit();
            break;
          default:
            game.CurrentGameState = Game1.GameState.SURFSCREEN;
            break;
        }
      }
    }

    private float alpha = .7f;
    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(background, pauseRectangle, Color.White * alpha);
      game.spriteBatch.DrawString(font, StartString, startOptionPosition, determineColor(Option.START), 0f, new Vector2(0, 0),
                                  scale, SpriteEffects.None, 0f);
      game.spriteBatch.DrawString(font, "Quit", quitOptionPosition, determineColor(Option.QUIT), 0f, new Vector2(0, 0),
                                  scale, SpriteEffects.None, 0f);
    }

    private Color determineColor(Option option) {
      if (selectedOption == option) {
        return Color.BlueViolet;
      }
      return Color.Black;
    }
  }
}
