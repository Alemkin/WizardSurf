using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardSurf.Desktop.Entities {
  public class Fireball : BaseEntity {

    //TODO create lower class called moving entity, to support of types
    public Texture2D texture;
    private float scale = 1f;
    public Vector2 position;
    public Vector2 initialPosition;
    private float rotation = 0f;
    private Vector2 velocity;
    //TODO use particle engine to enhance fireball
    public float radius;
    public float damage = 5f;
    private SpriteFont font;

    public Boolean offScreen = false;
    public Fireball(Game1 game, Vector2 velocity, Vector2 startPosition) : base(game) {
      position = startPosition;
      initialPosition = startPosition;
      this.velocity = velocity;
    }

    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("fireball");
      radius = texture.Height / 2 * scale;
      origin = new Vector2(texture.Width / 2, texture.Height / 2);
      font = game.Content.Load<SpriteFont>("Font");
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
    }

    private float alpha = .9f;
    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(texture, position, null, Color.White * alpha, rotation, origin, scale, SpriteEffects.FlipHorizontally, 0f);
      game.spriteBatch.DrawString(font, "X", GetPosition(), Color.Black);
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
