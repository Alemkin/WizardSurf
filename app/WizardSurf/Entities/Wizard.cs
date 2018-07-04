﻿using System;
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
    private SpriteFont font;

    private Texture2D idleTexture;
    private Texture2D flyingTexture;
    private Texture2D dyingTexture;
    public Vector2 position;
    private float velocity = 5f;
    private float acceleration = .001f;
    private float maxVelocity = 10f;
    public float radius;

    private float currentLife = 15f;

    private Vector2 scale = new Vector2(.25f, .25f);

    public Wizard (Game1 game) : base(game) {
      position = game.screenCenter;
      CurrentState = State.IDLE;
    }

    public override void LoadContent() {
      font = game.Content.Load<SpriteFont>("Font");
      // 521px wide x 464px high
      idleTexture = game.Content.Load<Texture2D>("fairy_idle");
      // 536px wide x 501px high
      flyingTexture = game.Content.Load<Texture2D>("fairy_fly");
      //695px wide x 640px high
      dyingTexture = game.Content.Load<Texture2D>("fairy_die");
      radius = (500 * scale.X) / 2;
      idleSpriteSheet = new Spritesheet.Spritesheet(idleTexture).WithGrid((521,464),(0,0),(0,0));
      flyingSpriteSheet = new Spritesheet.Spritesheet(flyingTexture).WithGrid((536, 501), (0, 0), (0, 0));
      dyingSpriteSheet = new Spritesheet.Spritesheet(dyingTexture).WithGrid((695, 640), (0, 0), (0, 0));
      CreateAnimations();
    }

    private void CreateAnimations() {
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
      game.debugPanel.wizardLife = currentLife;

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

    private float alpha = .95f;
    public override void Draw(GameTime gameTime) {
       if (CurrentState == State.IDLE) {
        game.spriteBatch.Draw(idleAnimation, position, Color.White * alpha, 0, scale, 0);
      } else if (CurrentState == State.LEFT) {
        game.spriteBatch.Draw(movingLeftAnimation, position, Color.White * alpha, 0, scale, 0);
      } else if (CurrentState == State.RIGHT) {
        game.spriteBatch.Draw(movingAnimation, position, Color.White * alpha, 0, scale, 0);
      } else if (CurrentState == State.DESTROYING) {
        game.spriteBatch.Draw(dyingAnimation, position, Color.White * alpha, 0, scale, 0);
      }
      game.spriteBatch.DrawString(font, "X", GetPosition(), Color.White);
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
      var theWidth = (500 * scale.X) / 2;
      var theHeight = (501 * scale.Y) / 2;
      return new Vector2(position.X + theWidth, position.Y + theHeight);
    }

    //TODO add collision handler instead
    public void ApplyHealth(float healthEffect) {
      currentLife = Math.Max(currentLife + healthEffect, 0f);
    }

  }
}
