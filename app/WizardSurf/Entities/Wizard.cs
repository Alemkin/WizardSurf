using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WizardSurf.Desktop.Entities {
  public class Wizard : BaseEntity {

    private Texture2D texture;
    private Vector2 position;
    private float velocity = 5f;
    private float acceleration = .01f;
    private float maxVelocity = 15f;

    public Wizard (Game1 game) : base(game) {
      position = game.screenCenter;
    }
    
    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("mage");
    }

    public override void UnloadContent() {
    }
    
    public override void Update(GameTime gameTime) {
      HandleKeyboardInput();
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(texture, position, Color.White);
    }

    private void HandleKeyboardInput() {
      //TODO cap and reset when let go of keyboard key
      velocity += acceleration;
      var currentVelocity = Math.Min(velocity, maxVelocity);
      if (game.inputHelper.IsKeyDown (Keys.Right)) {
        position.X += currentVelocity;
      } else if (game.inputHelper.IsKeyDown (Keys.Left)) {
        position.X -= currentVelocity;
      }

      if (game.inputHelper.IsKeyDown(Keys.Down)) {
        position.Y += currentVelocity;
      } else if (game.inputHelper.IsKeyDown(Keys.Up)) {
        position.Y -= currentVelocity;
      }
    }
  }
}
