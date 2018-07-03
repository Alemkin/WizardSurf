using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardSurf.Desktop.Entities {
  public class Fireball : BaseEntity {

    public Texture2D texture;
    private float scale = 1f;
    public Vector2 position;
    public Vector2 initialPosition;
    //TODO implement rotation
    private float rotation;
    private Vector2 velocity;
    //TODO use particle engine to enhance fireball
    public float radius;
    public float damage = 5f;

    public Boolean offScreen = false;
    public Fireball(Game1 game, Vector2 velocity, Vector2 startPosition, float rotation) : base(game) {
      position = startPosition;
      initialPosition = startPosition;
      this.rotation = rotation;
      this.velocity = velocity;
    }

    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("fireball");
      radius = texture.Height / 2 * scale;
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
      game.spriteBatch.Draw(texture, position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
    }

    public Vector2 GetPosition() {
      return new Vector2(position.X + ((texture.Width / 2) * scale) , position.Y + ((texture.Height / 2) * scale));
    }

    public void SetVelocity(Vector2 velocity) {
      this.velocity = velocity;
    }

    public void SetRotation(float rotation) {
      this.rotation = rotation;
    }
  }
}
