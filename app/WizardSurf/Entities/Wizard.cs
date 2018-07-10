using System;
using System.Collections.Generic;
using WizardSurf.Desktop.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spritesheet;

namespace WizardSurf.Desktop.Entities {
  public class Wizard : BaseEntity {

    private Spritesheet.Spritesheet idleSpriteSheet;
    private Spritesheet.Spritesheet flyingSpriteSheet;
    private Spritesheet.Spritesheet dyingSpriteSheet;
    private Animation idleAnimation;
    private Animation movingAnimation;
    private Animation movingLeftAnimation;
    private Animation dyingAnimation;
    private SpriteFont font;
    private ParticleEngine particleEngine;

    //TODO add hurt animation
    //TODO add special moves
    private Texture2D idleTexture;
    private Texture2D flyingTexture;
    private Texture2D dyingTexture;
    public Vector2 position;
    private float velocity = 5f;
    private float acceleration = .001f;
    private float maxVelocity = 10f;
    public float radius;

    private int lives = 3;

    private Vector2 scale = new Vector2(.25f, .25f);

    public Wizard (Game1 game, int lives) : base(game) {
      position = game.screenCenter;
      CurrentState = State.IDLE;
      this.lives = lives;
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

      List<Texture2D> textures = new List<Texture2D>();
      textures.Add(game.Content.Load<Texture2D>("diamond"));
      particleEngine = new ParticleEngine(game, textures, GetBottomPos(), BuildPalette(), 1);
    }

    private Vector2 GetBottomPos() {
      var currentPos = GetPosition();
      currentPos.Y += (480 * scale.Y) / 2;
      return currentPos;      
    }
    private List<ParticleEngine.RGBA> BuildPalette() {
      var alpha = .95f;
      var palette = new List<ParticleEngine.RGBA>();
      palette.Add(new ParticleEngine.RGBA(115, 195, 130, alpha));
      palette.Add(new ParticleEngine.RGBA(70, 150, 90, alpha));
      palette.Add(new ParticleEngine.RGBA(130, 110, 90, alpha));
      palette.Add(new ParticleEngine.RGBA(88, 70, 60, alpha));
      palette.Add(new ParticleEngine.RGBA(0, 0, 22, alpha));
      return palette;
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
      game.debugPanel.wizardLife = lives;

      HandleWizardDeath();
      UpdateAnimations(gameTime);
      HandleKeyboardInput();

      particleEngine.EmitterLocation = GetBottomPos();
      particleEngine.Update(gameTime);
    }

    private void UpdateAnimations(GameTime gameTime) {
      dyingAnimation.Update(gameTime);
      idleAnimation.Update(gameTime);
      movingAnimation.Update(gameTime);
      movingLeftAnimation.Update(gameTime);
    }

    private void HandleWizardDeath() {
      if (lives <= 0 && CurrentState != State.DESTROYING && CurrentState != State.DESTROYED) {
        CurrentState = State.DESTROYING;
        dyingAnimation.Start(Repeat.Mode.Once);
      }

      //Waiting for death animation to finish
      if (CurrentState == State.DESTROYING) {
        if (WillPutOutOfBoundsDown() == false) position.Y += velocity;
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
      if (CurrentState != State.DESTROYING && CurrentState != State.DESTROYED) {
        game.spriteBatch.DrawString(font, "X", GetPosition(), Color.Black);
        particleEngine.Draw(gameTime);
      }
    } 

    private void HandleKeyboardInput() {
      if (CurrentState == State.DESTROYING || CurrentState == State.DESTROYED) {
        return;
      }
      //TODO cap and reset when let go of keyboard key
      velocity = Math.Min(velocity + acceleration, maxVelocity);

      CurrentState = State.IDLE;
      if (game.inputHelper.IsKeyDown(Keys.Down) && WillPutOutOfBoundsDown() == false) {
        CurrentState = State.RIGHT;
        position.Y += velocity;
      } else if (game.inputHelper.IsKeyDown(Keys.Up) && WillPutOutOfBoundsUp() == false) {
        CurrentState = State.LEFT;
        position.Y -= velocity;
      }

      if (game.inputHelper.IsKeyDown (Keys.Right) && WillPutOutOfBoundsRight() == false) {
        CurrentState = State.RIGHT;
        position.X += velocity;
      } else if (game.inputHelper.IsKeyDown (Keys.Left) && WillPutOutOfBoundsLeft() == false) {
        CurrentState = State.LEFT;
        position.X -= velocity;
      }
    }

    private bool WillPutOutOfBoundsLeft() {
      return (position.X - velocity) < 5f;
    }

    private bool WillPutOutOfBoundsRight() {
      return (position.X + velocity) > game.graphics.PreferredBackBufferWidth - 100f;
    }

    private bool WillPutOutOfBoundsUp() {
      return (position.Y - velocity) < 5f;
    }

    private bool WillPutOutOfBoundsDown() {
      return (position.Y + velocity) > game.graphics.PreferredBackBufferHeight - 180f;
    }

    public Vector2 GetPosition() {
      if (CurrentState == State.DESTROYED || CurrentState == State.DESTROYING) return new Vector2(-5000, -5000);
      var theWidth = (500 * scale.X) / 2;
      var theHeight = (501 * scale.Y) / 2;
      return new Vector2(position.X + theWidth, position.Y + theHeight);
    }

    //TODO add collision handler instead
    public void ApplyHealth(int healthEffect) {
      lives = Math.Max(lives + healthEffect, 0);
    }

  }
}
