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
    //TODO add background music
    //TODO on updates check location of wizard + fireballs and see if any collisions, and apply damage

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
      CheckIfFireballCollision(gameTime);
    }

    private void CheckIfFireballCollision(GameTime gameTime) {
      //TODO fix this when adding random fireballs, and add function to Fireballs.cs
      fireballs.Update(gameTime);
      if (Vector2.Distance(fireballs.GetFireballPosition(), wizard.position) < 30f) {
        fireballs.fireball.UnloadContent();
        fireballs.fireball = null;
        wizard.ApplyHealth(-10f);
      }
      if (wizard.CurrentState == BaseEntity.State.DESTROYED) {
        wizard.UnloadContent();
      }
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(skyBackground, skyRectangle, Color.White);
      wizard.Draw(gameTime);
      fireballs.Draw(gameTime);
    }
  }
}
