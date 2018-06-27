using System;
using Microsoft.Xna.Framework;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.Screens {
  public abstract class BaseScreen : Entity {
    protected Game1 game;

    public BaseScreen(Game1 game) {
      this.game = game;
    }

    public abstract void LoadContent();

    public abstract void UnloadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);
  }
}
