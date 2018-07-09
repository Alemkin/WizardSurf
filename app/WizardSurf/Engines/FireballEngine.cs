using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.Spawners;
using WizardSurf.Desktop.Engines;

namespace WizardSurf.Desktop.Engines {
  public class FireballEngine : BaseEntity {
    public List<Fireball> fireballs;
    private WallSpawner spawner = new WallSpawner();
    private List<Fireball> fireballRemovals = new List<Fireball>();
    private List<Fireball> fireballAdditions = new List<Fireball>();
    private List<ParticleEngine> particleEngines = new List<ParticleEngine>();
    private List<Texture2D> particleTextures = new List<Texture2D>();
    private SoundEffect fireballSplashSound;

    public FireballEngine(Game1 game) : base(game) {
      fireballs = new List<Fireball>();
      fireballs.AddRange(spawner.Spawn(game, 15));
    }

    public override void LoadContent() {
      fireballs.ForEach(f => f.LoadContent());
      particleTextures.Add(game.Content.Load<Texture2D>("star"));
      fireballSplashSound = game.Content.Load<SoundEffect>("fireball_splash");
    }

    public override void UnloadContent() {
      fireballs.ForEach(f => f.UnloadContent());
    }

    public override void Update(GameTime gameTime) {
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
      ClearLists();
    }

    private void ClearLists() {
      fireballRemovals.Clear();
      fireballAdditions.Clear();      
    }

    public void CheckWizardCollisions(Wizard wizard, GameTime gameTime) {
      fireballs.ForEach(f => {
        if (f == null) return;
        var radiusAdded = f.radius + wizard.radius;
        if (Vector2.Distance(f.GetPosition(), wizard.GetPosition()) < radiusAdded - 30f) {
          wizard.ApplyHealth(-f.damage);

          fireballRemovals.Add(f);

          Fireball newFireball = spawner.Spawn(game);
          newFireball.LoadContent();
          fireballAdditions.Add(newFireball);

          BuildFireballSplash(gameTime, f.GetPosition());
          PlaySplashSoundEffect();
        }
      });
      fireballs.RemoveAll(f => fireballRemovals.Contains(f));
      fireballs.AddRange(fireballAdditions);
      ClearLists();
    }

    private void BuildFireballSplash(GameTime gameTime, Vector2 pos) {
      var particleEngine = new ParticleEngine(game, particleTextures, pos, Fireball.BuildPalette(), 50);
      particleEngine.Update(gameTime);
      particleEngines.Add(particleEngine);
    }

    private void PlaySplashSoundEffect() {
      fireballSplashSound.Play();
    }

    public override void Draw(GameTime gameTime) {
      fireballs.ForEach(f => f.Draw(gameTime));
      particleEngines.ForEach(p => {
        p.KillExpiredParticles(gameTime);
        p.Draw(gameTime);
      });
    }
  }
}
