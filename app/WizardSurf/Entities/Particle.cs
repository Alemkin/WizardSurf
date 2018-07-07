﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WizardSurf.Desktop.Entities {
  public class Particle : BaseEntity {
    public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
    public Vector2 Position { get; set; }        // The current position of the particle        
    public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
    public float Angle { get; set; }            // The current angle of rotation of the particle
    public float AngularVelocity { get; set; }    // The speed that the angle is changing
    public Color Color { get; set; }            // The color of the particle
    public float Size { get; set; }                // The size of the particle
    public int TTL { get; set; }                // The 'time to live' of the particle

    public Particle(Game1 game, Texture2D texture, Vector2 position, Vector2 velocity,
                    float angle, float angularVelocity, Color color, float size, int ttl) : base(game) {
      Texture = texture;
      Position = position;
      Velocity = velocity;
      Angle = angle;
      AngularVelocity = angularVelocity;
      Color = color;
      Size = size;
      TTL = ttl;
    }

    public override void LoadContent() {
      
    }

    public override void UnloadContent() {
      
    }

    public override void Update(GameTime gameTime) {
      TTL--;
      Position += Velocity;
      Angle += AngularVelocity;
    }

    public override void Draw(GameTime gameTime) {
      Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
      origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

      game.spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
          Angle, origin, Size, SpriteEffects.None, 0f);
    }
  }
}
