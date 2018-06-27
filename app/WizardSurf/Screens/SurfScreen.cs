using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.Screens {
  public class SurfScreen : BaseScreen {
    //TODO add font
    private Wizard wizard;

    public SurfScreen(Game1 game) : base(game) {
      wizard = new Wizard(game);
    }

    public override void LoadContent() {
      wizard.LoadContent();
    }

    public override void UnloadContent() {
      wizard.UnloadContent();
    }

    public override void Update(GameTime gameTime) {
      wizard.Update(gameTime);
    }

    public override void Draw(GameTime gameTime) {
      wizard.Draw(gameTime);
      //TODO draw fraemrate
      //TODO draw current velocity
    }
  }
}
