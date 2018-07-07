using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Engines;

namespace WizardSurf.Desktop.Entities {
  public class Fireball : BaseEntity {

    public Texture2D texture;
    private float scale = 1f;
    public Vector2 position;
    public Vector2 initialPosition;
    private float rotation = 0f;
    private Vector2 velocity;
    public float radius;
    public float damage = 5f;
    private SpriteFont font;
    private ParticleEngine particleEngine;

    public Boolean offScreen = false;
    public Fireball(Game1 game, Vector2 velocity, Vector2 startPosition) : base(game) {
      position = startPosition;
      initialPosition = startPosition;
      this.velocity = velocity;
    }

    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("fireball");
      radius = (texture.Height / 2 * scale) - 10f;
      origin = new Vector2(texture.Width / 2, texture.Height / 2);
      font = game.Content.Load<SpriteFont>("Font");
      List<Texture2D> textures = new List<Texture2D>();
      textures.Add(game.Content.Load<Texture2D>("star"));
      particleEngine = new ParticleEngine(game, textures, position, BuildPalette());
    }

    private List<ParticleEngine.RGBA> BuildPalette() {
      var alpha = .8f;
      var palette = new List<ParticleEngine.RGBA>();
      palette.Add(new ParticleEngine.RGBA(253, 207, 88, alpha));
      palette.Add(new ParticleEngine.RGBA(117, 118, 118, alpha));
      palette.Add(new ParticleEngine.RGBA(242, 125, 12, alpha));
      palette.Add(new ParticleEngine.RGBA(128, 9, 9, alpha));
      palette.Add(new ParticleEngine.RGBA(240, 127, 19, alpha));
      return palette;
    }

    public override void UnloadContent() {
      texture.Dispose();
    }

    public override void Update(GameTime gameTime) {
      if (position.X < -150 
          || position.X > game.graphics.GraphicsDevice.Viewport.Width + 50f
          || position.Y < -150
          || position.Y > game.graphics.GraphicsDevice.Viewport.Height + 50f) {
        offScreen = true;
      }
      position.X += velocity.X;
      position.Y += velocity.Y;
      //TODO more gravity
      velocity.Y += 0.01f;
      rotation = (float)Math.Atan2(velocity.Y, velocity.X);
      particleEngine.EmitterLocation = position;
      particleEngine.Update(gameTime);
    }

    private float alpha = .9f;
    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(texture, position, null, Color.White * alpha, rotation, origin, scale, SpriteEffects.FlipHorizontally, 0f);
      game.spriteBatch.DrawString(font, "X", GetPosition(), Color.Black);
      particleEngine.Draw(gameTime);
    }

    public Vector2 GetPosition() {
      return position;
    }

    public void SetVelocity(Vector2 velocity) {
      this.velocity = velocity;
    }

    public void SetRotation(float rotation) {
      this.rotation = rotation;
    }
  }
}
