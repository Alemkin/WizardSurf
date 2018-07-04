using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.Spawners;

namespace WizardSurf.Desktop.EntityImplementations {
  public class Fireballs : BaseEntity {
    public List<Fireball> fireballs;
    private WallSpawner spawner = new WallSpawner();

    public Fireballs(Game1 game) : base(game) {
      fireballs = new List<Fireball>();
      fireballs.AddRange(spawner.Spawn(game, 18));
    }

    public override void LoadContent() {
      fireballs.ForEach(f => f.LoadContent());
    }

    public override void UnloadContent() {
      fireballs.ForEach(f => f.UnloadContent());
    }

    public override void Update(GameTime gameTime) {
      var fireballRemovals = new List<Fireball>();
      var fireballAdditions = new List<Fireball>();
      fireballs.ForEach(f => f.Update(gameTime));
      fireballs.ForEach(f => {
        if (f.offScreen) {
          fireballRemovals.Add(f);
          Fireball newFireball = spawner.Spawn(game);
          newFireball.LoadContent();
          fireballAdditions.Add(newFireball);
        }
      });
      fireballs.RemoveAll(f => fireballRemovals.Contains(f));
      fireballs.AddRange(fireballAdditions);
    }

    public void CheckWizardCollisions(Wizard wizard) {
      var fireballRemovals = new List<Fireball>();
      var fireballAdditions = new List<Fireball>();
      fireballs.ForEach(f => {
        if (f == null) return;
        var radiusAdded = f.radius + wizard.radius;
        if (Vector2.Distance(f.GetPosition(), wizard.GetPosition()) < radiusAdded - 30f) {
          wizard.ApplyHealth(-f.damage);
          fireballRemovals.Add(f);
          Fireball newFireball = spawner.Spawn(game);
          newFireball.LoadContent();
          fireballAdditions.Add(newFireball);
        }
      });
      fireballs.RemoveAll(f => fireballRemovals.Contains(f));
      fireballs.AddRange(fireballAdditions);
    }

    public override void Draw(GameTime gameTime) {
      fireballs.ForEach(f => f.Draw(gameTime));
    }
  }
}
