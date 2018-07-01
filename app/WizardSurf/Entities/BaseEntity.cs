using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace WizardSurf.Desktop.Entities {
  public abstract class BaseEntity : Entity {
    public enum State {
      IDLE, LEFT, RIGHT, UP, DOWN, ATTACKING, DESTROYING, DESTROYED
    }
    public State CurrentState { get; set; }
    protected Game1 game;
    public Vector2 origin;

    public BaseEntity(Game1 game) {
      this.game = game;
    }

    public abstract void LoadContent();

    public abstract void UnloadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
  }
}
