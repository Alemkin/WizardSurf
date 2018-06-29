using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spritesheet;

namespace WizardSurf.Desktop.Entities {
  //TODO load spritesheet
  //TODO use sheet to animate wizard
  public class Wizard : BaseEntity {

    //Build particle engine and add particles under wizard
    private Spritesheet.Spritesheet spriteSheet;
    private Animation animation;
    //TODO add attacking animation
    //TODO add moving animation, flipping x
    private Texture2D texture;
    private Vector2 position;
    private float velocity = 5f;
    private float acceleration = .01f;
    private float maxVelocity = 15f;

    private int aY = 0;

    public Wizard (Game1 game) : base(game) {
      position = game.screenCenter;
    }
    
    public override void LoadContent() {
      //texture = game.Content.Load<Texture2D>("mage");
      //32 x 32 sprites, animated 10 over, 5 animations down
      texture = game.Content.Load<Texture2D>("CrazyWizard");
      spriteSheet = new Spritesheet.Spritesheet(texture).WithGrid((32,32),(0,0),(0,0));
      animation =
        spriteSheet.CreateAnimation((0, aY), (1, aY), (2, aY), (3, aY), (4, aY), (5, aY), (6, aY), (7, aY), (8, aY), (9, aY));
      animation.Start(Repeat.Mode.Loop);
    }

    public override void UnloadContent() {
    }
    
    public override void Update(GameTime gameTime) {
      animation.Update(gameTime);
      HandleKeyboardInput();
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(animation, position, Color.White, 0, new Vector2(3, 3), 0);
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
