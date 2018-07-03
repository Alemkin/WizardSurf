using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spritesheet;

namespace WizardSurf.Desktop.Entities {
  public class Wizard : BaseEntity {

    //Build particle engine and add particles under wizard
    private Spritesheet.Spritesheet idleSpriteSheet;
    private Spritesheet.Spritesheet flyingSpriteSheet;
    private Spritesheet.Spritesheet dyingSpriteSheet;
    private Animation idleAnimation;
    private Animation movingAnimation;
    private Animation movingLeftAnimation;
    private Animation dyingAnimation;

    private Texture2D idleTexture;
    private Texture2D flyingTexture;
    private Texture2D dyingTexture;
    public Vector2 position;
    private float velocity = 5f;
    private float acceleration = .01f;
    private float maxVelocity = 10f;
    public float radius;

    private float currentLife = 10f;

    private Vector2 scale = new Vector2(.3f, .3f);

    public Wizard (Game1 game) : base(game) {
      position = game.screenCenter;
      CurrentState = State.IDLE;
    }

    public override void LoadContent() {
      // 521px wide x 464px high
      idleTexture = game.Content.Load<Texture2D>("fairy_idle");
      // 536px wide x 501px high
      flyingTexture = game.Content.Load<Texture2D>("fairy_fly");
      //695px wide x 640px high
      dyingTexture = game.Content.Load<Texture2D>("fairy_die");
      radius = (500 / 2) * scale.X;
      idleSpriteSheet = new Spritesheet.Spritesheet(idleTexture).WithGrid((521,464),(0,0),(0,0));
      flyingSpriteSheet = new Spritesheet.Spritesheet(flyingTexture).WithGrid((536, 501), (0, 0), (0, 0));
      dyingSpriteSheet = new Spritesheet.Spritesheet(dyingTexture).WithGrid((695, 640), (0, 0), (0, 0));
      CreateAnimations();
    }

    private void CreateAnimations() {
      //idleAnimation = spriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0),
      //                                        (0, 1), (1, 1), (2, 1), (3, 1),
      //                                        (0, 2), (1, 2), (2, 2), (3, 2),
      //                                        (0, 3), (1, 3), (2, 3), (3, 3));

      //movingAnimation = spriteSheet.CreateAnimation((4, 0), (5, 0), (6, 0), (7, 0),
      //                                        (4, 1), (5, 1), (6, 1), (7, 1),
      //                                        (4, 2), (5, 2), (6, 2), (7, 2),
      //                                        (4, 3), (5, 3), (6, 3), (7, 3));

      //dyingAnimation = spriteSheet.CreateAnimation((4, 4), (5, 4), (6, 4), (7, 4),
      //(4, 5), (5, 5), (6, 5), (7, 5),
      //(4, 6), (5, 6), (6, 6), (7, 6),
      //(4, 7), (5, 7), (6, 7), (7, 7));
      idleAnimation = idleSpriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0), (4, 0));
      movingAnimation = flyingSpriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0), (4, 0));
      dyingAnimation = dyingSpriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0), (4, 0));

      movingLeftAnimation = movingAnimation.FlipX();

      idleAnimation.Start(Repeat.Mode.Loop);
      movingAnimation.Start(Repeat.Mode.Loop);
      movingLeftAnimation.Start(Repeat.Mode.Loop);
    }

    public override void UnloadContent() {
      idleTexture.Dispose();
      flyingTexture.Dispose();
      dyingTexture.Dispose();
    }

    private int deathTicker = 0;
    public override void Update(GameTime gameTime) {
      game.debugPanel.wizardVelocity = velocity;

      HandleWizardDeath();
      UpdateAnimations(gameTime);
      HandleKeyboardInput();
    }

    private void UpdateAnimations(GameTime gameTime) {
      dyingAnimation.Update(gameTime);
      idleAnimation.Update(gameTime);
      movingAnimation.Update(gameTime);
      movingLeftAnimation.Update(gameTime);
    }

    private void HandleWizardDeath() {
      if (currentLife <= 0 && CurrentState != State.DESTROYING && CurrentState != State.DESTROYED) {
        CurrentState = State.DESTROYING;
        dyingAnimation.Start(Repeat.Mode.Once);
      }

      //Waiting for death animation to finish
      if (CurrentState == State.DESTROYING) {
        if (deathTicker++ >= 200) {
          CurrentState = State.DESTROYED;
        }
      }
    }

    public override void Draw(GameTime gameTime) {
       if (CurrentState == State.IDLE) {
        game.spriteBatch.Draw(idleAnimation, position, Color.White, 0, scale, 0);
      } else if (CurrentState == State.LEFT) {
        game.spriteBatch.Draw(movingLeftAnimation, position, Color.White, 0, scale, 0);
      } else if (CurrentState == State.RIGHT) {
        game.spriteBatch.Draw(movingAnimation, position, Color.White, 0, scale, 0);
      } else if (CurrentState == State.DESTROYING) {
        game.spriteBatch.Draw(dyingAnimation, position, Color.White, 0, scale, 0);
      }
    } 

    private void HandleKeyboardInput() {
      if (CurrentState == State.DESTROYING || CurrentState == State.DESTROYED) {
        return;
      }
      //TODO cap and reset when let go of keyboard key
      velocity = Math.Min(velocity + acceleration, maxVelocity);

      CurrentState = State.IDLE;
      if (game.inputHelper.IsKeyDown (Keys.Right)) {
        CurrentState = State.RIGHT;
        position.X += velocity;
      } else if (game.inputHelper.IsKeyDown (Keys.Left)) {
        CurrentState = State.LEFT;
        position.X -= velocity;
      }

      if (game.inputHelper.IsKeyDown(Keys.Down)) {
        position.Y += velocity;
      } else if (game.inputHelper.IsKeyDown(Keys.Up)) {
        position.Y -= velocity;
      }
    }

    public Vector2 GetPosition() {
      var theWidth = (536 / 2) * scale.X;
      var theHeight = (501 / 2) * scale.Y;
      return new Vector2(position.X + theWidth, position.Y + theHeight);
    }

    public void ApplyHealth(float healthEffect) {
      currentLife = Math.Max(currentLife + healthEffect, 0);
    }

  }
}
