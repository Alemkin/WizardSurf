using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.EntityImplementations {
  public class Fireballs : BaseEntity {
    //TODO list of fireballs, randomized
    // TODO add abstract spawner
    public Fireball fireball;
    public List<Fireball> fireballs;

    public Fireballs(Game1 game) : base(game) {
      var position =
        new Vector2(game.graphics.GraphicsDevice.Viewport.Width,
         game.graphics.GraphicsDevice.Viewport.Height / 2f);
      fireball = new Fireball(game, new Vector2(-5, 0), position, 0f);

      var fireball1 = new Fireball(game, new Vector2(-5, 0), position, 0f);
      var fireball2 = new Fireball(game, new Vector2(5, 0), position, 0f);
      var fireball3 = new Fireball(game, new Vector2(0, 5), position, 0f);
      var fireball4 = new Fireball(game, new Vector2(0, -5), position, 0f);

      var fireball6 = new Fireball(game, new Vector2(5, 5), position, 0f);
      var fireball5 = new Fireball(game, new Vector2(-5, 5), position, 0f);
      var fireball7 = new Fireball(game, new Vector2(5, -5), position, 0f);
      var fireball8 = new Fireball(game, new Vector2(-5, -5), position, 0f);

      fireballs = new List<Fireball>() {
        fireball1, fireball2, fireball3, fireball4, fireball5, fireball6, fireball7, fireball8
      };
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
          //fireball.UnloadContent();
          //fireball = null;
          fireball.offScreen = false;
          fireball.position = fireball.initialPosition;
        }
      }
    }

    public Vector2 GetFireballPosition() {
      return fireball == null 
        ? new Vector2(0f, 0f) 
          : fireball.GetPosition();
    }

    public float GetFireballRadius() {
      return fireball == null ? 0f : fireball.radius;
    }

    public override void Draw(GameTime gameTime) {
      if (fireball != null) {
        fireball.Draw(gameTime);        
      }
    }
  }
}
