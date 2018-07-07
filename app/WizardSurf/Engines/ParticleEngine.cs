using System;
using System.Collections.Generic;
using WizardSurf.Desktop.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WizardSurf.Desktop.Engines {
  public class ParticleEngine {
    public Vector2 EmitterLocation { get; set; }
    private List<Particle> particles;
    private List<Texture2D> textures;
    private Game1 game;
    private List<RGBA> palette = new List<RGBA>();

    public ParticleEngine(Game1 game, List<Texture2D> textures, Vector2 location, List<RGBA> palette) {
      this.game = game;
      EmitterLocation = location;
      this.textures = textures;
      this.particles = new List<Particle>();
      this.palette = palette;
    }

    public void Update(GameTime gameTime) {
      var total = 5;

      for (int i = 0; i < total; i++) {
        particles.Add(GenerateNewParticle());
      }

      for (int p = 0; p < particles.Count; p++) {
        particles [p].Update(gameTime);
        if (particles [p].TTL <= 0) {
          particles.RemoveAt(p);
          p--;
        }
      }
    }

    public void Draw(GameTime gameTime) {
      for (int i = 0; i < particles.Count; i++) {
        particles [i].Draw(gameTime);
      }
    }

    public struct RGBA {
      public RGBA(int r, int g, int b, float a) {
        Red = r;
        Green = g;
        Blue = b;
        Alpha = a;
      }
      public int Red { get; set; }
      public int Green { get; set; }
      public int Blue { get; set; }
      public float Alpha { get; set; }
    }

    private Particle GenerateNewParticle() {
      var texture = textures [Game1.random.Next(textures.Count)];
      var position = EmitterLocation;
      var velocity = new Vector2(
              1f * (float)(Game1.random.NextDouble() * 2 - 1),
              1f * (float)(Game1.random.NextDouble() * 2 - 1));
      var angle = 0f;
      var angularVelocity = 0.1f * (float)(Game1.random.NextDouble() * 2 - 1);
      RGBA rgba = palette[Game1.random.Next(palette.Count)];
      Color color = new Color(rgba.Red, rgba.Green, rgba.Blue, Convert.ToInt32(255f * rgba.Alpha));
      var size = (float)Game1.random.NextDouble();
      var ttl = 10 + Game1.random.Next(40);

      return new Particle(game, texture, position, velocity, angle, angularVelocity, color, size, ttl);
    }
  }
}
