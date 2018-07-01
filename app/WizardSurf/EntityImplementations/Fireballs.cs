using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.EntityImplementations {
  public class Fireballs : BaseEntity {
    //TODO list of fireballs, randomized
    // TODO add abstract spawner
    public Fireball fireball;

    public Fireballs(Game1 game) : base(game) {
      var position =
        new Vector2(game.graphics.GraphicsDevice.Viewport.Width,
         game.graphics.GraphicsDevice.Viewport.Height / 2f);
      fireball = new Fireball(game, new Vector2(-5, 0), position, 0f);
    }

    public override void LoadContent() {
      if (fireball != null) {
        fireball.LoadContent();
      }
    }

    public override void UnloadContent() {
      if (fireball != null) {
        fireball.UnloadContent();
      }
    }

    public override void Update(GameTime gameTime) {
      if (fireball != null) {
        fireball.Update(gameTime);
        if (fireball.offScreen) {
          fireball.UnloadContent();
          fireball = null;
        }
      }
    }

    public Vector2 GetFireballPosition() {
      return fireball == null ? new Vector2(0f, 0f) : fireball.position;
    }

    public override void Draw(GameTime gameTime) {
      if (fireball != null) {
        fireball.Draw(gameTime);        
      }
    }
  }
}
