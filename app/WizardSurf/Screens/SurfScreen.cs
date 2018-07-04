using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.EntityImplementations;

namespace WizardSurf.Desktop.Screens {
  public class SurfScreen : BaseScreen {
    private Texture2D background;
    Rectangle skyRectangle;
    private Wizard wizard;
    private Fireballs fireballs;
    private Song song;

    //TODO add Game Over Screen
    public SurfScreen(Game1 game) : base(game) {
      skyRectangle = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      wizard = new Wizard(game);
      fireballs = new Fireballs(game);
    }

    public override void LoadContent() {
      background = game.Content.Load<Texture2D>("background");
      song = game.Content.Load<Song>("wizard");
      wizard.LoadContent();
      fireballs.LoadContent();
    }

    public override void UnloadContent() {
      wizard.UnloadContent();
      fireballs.UnloadContent();
    }

    private bool playingSong = false;
    private bool paused = false;
    public override void Update(GameTime gameTime) {
      if (playingSong == false) {
        MediaPlayer.Play(song);
        playingSong = true;
      }
      if (paused == true) {
        MediaPlayer.Resume();
        paused = false;
      }
      if (game.CurrentGameState == Game1.GameState.PAUSE) {
        MediaPlayer.Pause();
        paused = true;
      }
      wizard.Update(gameTime);
      CheckIfFireballCollision(gameTime);
    }

    private void CheckIfFireballCollision(GameTime gameTime) {
      fireballs.Update(gameTime);
      fireballs.CheckWizardCollisions(wizard);
      if (wizard.CurrentState == BaseEntity.State.DESTROYED) {
        wizard.UnloadContent();
      }
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(background, skyRectangle, Color.White);
      wizard.Draw(gameTime);
      fireballs.Draw(gameTime);
    }
  }
}
