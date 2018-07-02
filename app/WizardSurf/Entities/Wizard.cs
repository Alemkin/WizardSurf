using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spritesheet;

namespace WizardSurf.Desktop.Entities {
  public class Wizard : BaseEntity {

    //Build particle engine and add particles under wizard
    private Spritesheet.Spritesheet spriteSheet;
    private Animation idleAnimation;
    private Animation movingAnimation;
    private Animation movingRightAnimation;
    private Animation dyingAnimation;

    private Texture2D texture;
    public Vector2 position;
    private float velocity = 5f;
    private float acceleration = .01f;
    private float maxVelocity = 5f;

    //TODO add life and collisions
    private float currentLife = 10f;

    private Vector2 scale = new Vector2(1f, 1f);

    public Wizard (Game1 game) : base(game) {
      position = game.screenCenter;
      CurrentState = State.IDLE;
    }

    public override void LoadContent() {
      texture = game.Content.Load<Texture2D>("wizardspritesheet");
      origin = new Vector2(texture.Width / 2, texture.Height / 2);
      spriteSheet = new Spritesheet.Spritesheet(texture).WithGrid((64,64),(0,0),(0,0));
      CreateAnimations();
    }

    private void CreateAnimations() {
      idleAnimation = spriteSheet.CreateAnimation((0, 0), (1, 0), (2, 0), (3, 0),
                                              (0, 1), (1, 1), (2, 1), (3, 1),
                                              (0, 2), (1, 2), (2, 2), (3, 2),
                                              (0, 3), (1, 3), (2, 3), (3, 3));

      movingAnimation = spriteSheet.CreateAnimation((4, 0), (5, 0), (6, 0), (7, 0),
                                              (4, 1), (5, 1), (6, 1), (7, 1),
                                              (4, 2), (5, 2), (6, 2), (7, 2),
                                              (4, 3), (5, 3), (6, 3), (7, 3));

      dyingAnimation = spriteSheet.CreateAnimation((4, 4), (5, 4), (6, 4), (7, 4),
                                              (4, 5), (5, 5), (6, 5), (7, 5),
                                              (4, 6), (5, 6), (6, 6), (7, 6),
                                                   (4, 7), (5, 7), (6, 7), (7, 7));
      movingRightAnimation = movingAnimation.FlipX();

      idleAnimation.Start(Repeat.Mode.Loop);
      movingAnimation.Start(Repeat.Mode.Loop);
      movingRightAnimation.Start(Repeat.Mode.Loop);
    }

    public override void UnloadContent() {
      texture.Dispose();
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
      movingRightAnimation.Update(gameTime);
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
        game.spriteBatch.Draw(movingAnimation, position, Color.White, 0, scale, 0);        
      } else if (CurrentState == State.RIGHT) {
        game.spriteBatch.Draw(movingRightAnimation, position, Color.White, 0, scale, 0);
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

    public void ApplyHealth(float healthEffect) {
      currentLife = Math.Max(currentLife + healthEffect, 0);
    }

  }
}
