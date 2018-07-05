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
    private SpriteFont font;
    Rectangle skyRectangle;
    private Wizard wizard;
    private Fireballs fireballs;
    private Song song;
    private Song dedSong;
    private Song dedSongCont;


    private int transparency = 150;
    private Texture2D gameOverTexture;
    private Color [] gameOverColor = new Color [1];
    private Rectangle gameOverColorRectangle;
    private Vector2 centerScreen;

    //TODO add win condition, e.g. after 100 seconds, you win
    public SurfScreen(Game1 game) : base(game) {
      skyRectangle = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      wizard = new Wizard(game);
      fireballs = new Fireballs(game);
      font = game.Content.Load<SpriteFont>("Font");
      gameOverColorRectangle =
        new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      gameOverTexture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      gameOverColor [0] = Color.FromNonPremultiplied(255, 255, 255, transparency);
      gameOverTexture.SetData<Color>(gameOverColor);
      centerScreen = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - 50f,
                                     (game.graphics.PreferredBackBufferHeight / 2) - 50f);
    }

    public override void LoadContent() {
      background = game.Content.Load<Texture2D>("background");
      song = game.Content.Load<Song>("wizard");
      dedSong = game.Content.Load<Song>("ded");
      dedSongCont = game.Content.Load<Song>("dedcont");
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
        MediaPlayer.IsRepeating = true;
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

    private bool playingDedSong = false;
    private bool playingDedSongCont = false;
    private int playingDedFrameCount = 0;
    private void PlayDed() {
      if (playingDedSong == false) {
        MediaPlayer.Play(dedSong);
        MediaPlayer.IsRepeating = false;
        playingDedSong = true;
      }
      if (playingDedSong && !playingDedSongCont) {
        if (playingDedFrameCount++ > 720) {
          MediaPlayer.Play(dedSongCont);
          playingDedSongCont = true;
        }
      }
    }

    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(background, skyRectangle, Color.White);
      wizard.Draw(gameTime);
      fireballs.Draw(gameTime);
      if (wizard.CurrentState == BaseEntity.State.DESTROYING) {
        PlayDed();
      }
      if (wizard.CurrentState == BaseEntity.State.DESTROYED) {
        PlayDed();
        game.spriteBatch.Draw(gameOverTexture, gameOverColorRectangle, Color.Black);
        game.spriteBatch.DrawString(font, "You DED", centerScreen, Color.Red);
      }
    }
  }
}
