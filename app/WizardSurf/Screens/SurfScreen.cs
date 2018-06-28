using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.EntityImplementations;

namespace WizardSurf.Desktop.Screens {
  public class SurfScreen : BaseScreen {
    private Texture2D skyBackground;
    Rectangle skyRectangle;
    private Wizard wizard;
    private Fireballs fireballs;

    public SurfScreen(Game1 game) : base(game) {
      skyRectangle = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      wizard = new Wizard(game);
      fireballs = new Fireballs(game);
    }

    public override void LoadContent() {
      skyBackground = game.Content.Load<Texture2D>("sky");
      wizard.LoadContent();
      fireballs.LoadContent();
    }

    public override void UnloadContent() {
      wizard.UnloadContent();
      fireballs.UnloadContent();
    }

    public override void Update(GameTime gameTime) {
      wizard.Update(gameTime);
      fireballs.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(skyBackground, skyRectangle, Color.White);
      wizard.Draw(gameTime);
      fireballs.Draw(gameTime);
      //TODO draw current velocity
    }
  }
}
