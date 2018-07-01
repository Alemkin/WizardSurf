using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WizardSurf.Desktop.Screens;

namespace WizardSurf.Desktop {
  public class DebugPanel : BaseScreen {
    private SpriteFont font;

    private Vector2 secondsTitlePosition;
    private Vector2 frameratePosition;
    private Vector2 velocityPosition;

    public float wizardVelocity = 0f;

    private int currentFramerate;

    public DebugPanel(Game1 game) : base(game) {
      secondsTitlePosition = new Vector2(20, 5);
      frameratePosition = new Vector2(20, 25);
      velocityPosition = new Vector2(20, 45);
    }

    public override void LoadContent() {
      font = game.Content.Load<SpriteFont>("Font");
    }

    public override void UnloadContent() {
    }

    public override void Update(GameTime gameTime) {
      UpdateFramerate(gameTime);
    }

    private void UpdateFramerate(GameTime gameTime) {
      var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
      var floatFramerate = 1.0f / deltaTime;
      currentFramerate = (int)Math.Round(floatFramerate);
    }


    public override void Draw(GameTime gameTime) {
      game.spriteBatch.DrawString(font, "Elapsed Time: " + gameTime.TotalGameTime.Seconds.ToString() + " seconds", secondsTitlePosition, Color.White);
      game.spriteBatch.DrawString(font, "Framerate: " + currentFramerate.ToString(), frameratePosition, Color.White);
      game.spriteBatch.DrawString(font, "Current Velocity: " + wizardVelocity.ToString(), velocityPosition, Color.White);
    }
  }
}
