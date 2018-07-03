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
    public List<Fireball> fireballs;

    public Fireballs(Game1 game) : base(game) {
      var position =
        new Vector2(game.graphics.GraphicsDevice.Viewport.Width,
                    game.graphics.GraphicsDevice.Viewport.Height / 2f);
      var fireball1 = new Fireball(game, new Vector2(-3, 0), position, 0f);

      position = new Vector2(0, game.graphics.GraphicsDevice.Viewport.Height / 2f);
      var fireball2 = new Fireball(game, new Vector2(2, 0), position, 0f);

      position = new Vector2(game.graphics.GraphicsDevice.Viewport.Width / 2f,0);
      var fireball3 = new Fireball(game, new Vector2(0, 5), position, 0f);

      position = new Vector2(game.graphics.GraphicsDevice.Viewport.Width / 2f, 
                             game.graphics.GraphicsDevice.Viewport.Height);
      var fireball4 = new Fireball(game, new Vector2(0, -4), position, 0f);

      position = new Vector2(0, 0);
      var fireball6 = new Fireball(game, new Vector2(3, 4), position, 0f);

      position = new Vector2(game.graphics.GraphicsDevice.Viewport.Width,0f);
      var fireball5 = new Fireball(game, new Vector2(-3, 3), position, 0f);

      position = new Vector2(0, game.graphics.GraphicsDevice.Viewport.Height);
      var fireball7 = new Fireball(game, new Vector2(3, -5), position, 0f);

      position = new Vector2(game.graphics.GraphicsDevice.Viewport.Width,
                             game.graphics.GraphicsDevice.Viewport.Height);
      var fireball8 = new Fireball(game, new Vector2(-4, -5), position, 0f);

      fireballs = new List<Fireball>() {
        fireball1, fireball2, fireball3, fireball4,
        fireball5, fireball6, fireball7, fireball8
      };
    }

    public override void LoadContent() {
      fireballs.ForEach(f => f.LoadContent());
    }

    public override void UnloadContent() {
      fireballs.ForEach(f => f.UnloadContent());
    }

    public override void Update(GameTime gameTime) {
      fireballs.ForEach(f => f.Update(gameTime));
      fireballs.ForEach(f => {
        if (f.offScreen) {
          //fireball = null;
          f.offScreen = false;
          f.position = f.initialPosition;
        }
      });
    }

    public void CheckWizardCollisions(Wizard wizard) {
      var fireballRemovals = new List<Fireball>();
      fireballs.ForEach(f => {
        if (f == null) return;
        var radiusAdded = f.radius + wizard.radius;
        if (Vector2.Distance(f.GetPosition(), wizard.GetPosition()) < radiusAdded - 60f) {
          wizard.ApplyHealth(-f.damage);
          fireballRemovals.Add(f);
        }
      });
      fireballs.RemoveAll(f => fireballRemovals.Contains(f));
    }

    public override void Draw(GameTime gameTime) {
      fireballs.ForEach(f => f.Draw(gameTime));
    }
  }
}
