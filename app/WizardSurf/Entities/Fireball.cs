using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardSurf.Desktop.Entities {
  public class Fireball : BaseEntity {

    private Texture2D texture;
    public Vector2 position;
    //TODO implement rotation
    private float rotation;
    private Vector2 velocity;
    //TODO collision detector
    //TODO use particle engine to enhance fireball

    public Boolean offScreen = false;
    public Fireball(Game1 game, Vector2 velocity, Vector2 startPosition, float rotation) : base(game) {
      position = startPosition;
      this.rotation = rotation;
      this.velocity = velocity;
    }

    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("fireball");
      origin = new Vector2(texture.Width / 2, texture.Height / 2);
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
    }

    public override void Draw(GameTime gameTime) {
      //TODO add rectangle, and location, and rotation
      game.spriteBatch.Draw(texture, position, Color.White);
    }

    public void SetVelocity(Vector2 velocity) {
      this.velocity = velocity;
    }

    public void SetRotation(float rotation) {
      this.rotation = rotation;
    }
  }
}
