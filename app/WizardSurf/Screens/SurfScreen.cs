using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WizardSurf.Desktop.Entities;
using WizardSurf.Desktop.Engines;
using static WizardSurf.Desktop.Engines.LevelEngine;

namespace WizardSurf.Desktop.Screens {
  public class SurfScreen : BaseScreen {
    private Texture2D background;
    private SpriteFont font;
    Rectangle skyRectangle;
    private Wizard wizard;
    private FireballEngine fireballs;
    private Song song;
    private Song dedSong;
    private Song dedSongCont;
    private Song victorySong;
    private Vector2 secondsTitlePosition;
    private Vector2 stageTitlePosition;

    private int transparency = 150;
    private Texture2D gameOverTexture;
    private Color [] gameOverColor = new Color [1];
    private Rectangle gameOverColorRectangle;
    private Vector2 centerScreen;
    private int totalFramesSpentInGame = 0;

    private string levelName;

    private int winConditionSeconds;
    // Add rolling credits after death
    // TODO add pause in before starting game
    public SurfScreen(Game1 game, Level level) : base(game) {
      levelName = level.Name;
      winConditionSeconds = level.WinConditionSeconds;
      skyRectangle = new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      wizard = new Wizard(game, level.Lives);
      fireballs = new FireballEngine(game, level.FireballCount, level.MaxSpeed);
      font = game.Content.Load<SpriteFont>("Font");
      gameOverColorRectangle =
        new Rectangle(0, 0, game.graphics.PreferredBackBufferWidth, game.graphics.PreferredBackBufferHeight);
      gameOverTexture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      gameOverColor [0] = Color.FromNonPremultiplied(255, 255, 255, transparency);
      gameOverTexture.SetData<Color>(gameOverColor);
      centerScreen = new Vector2((game.graphics.PreferredBackBufferWidth / 2) - 50f,
                                 (game.graphics.PreferredBackBufferHeight / 2) - 50f);
      secondsTitlePosition = new Vector2((game.graphics.GraphicsDevice.Viewport.Width / 2) - 100f, 20f);
      stageTitlePosition = new Vector2((game.graphics.GraphicsDevice.Viewport.Width) - 250f, 20f);
    }

    public override void LoadContent() {
      background = game.Content.Load<Texture2D>("background");
      song = game.Content.Load<Song>("wizard");
      dedSong = game.Content.Load<Song>("ded");
      dedSongCont = game.Content.Load<Song>("dedcont");
      victorySong = game.Content.Load<Song>("victory");
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
      totalFramesSpentInGame++;
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
      if (HasWon(gameTime) == false 
          && WizardIsBeingDestroyed() == false 
          && WizardIsDestroyed() == false) {
        fireballs.CheckWizardCollisions(wizard, gameTime);
      }
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

    private bool playingVictorySong = false;
    private void PlayVictory() {
      if (playingVictorySong == false) {
        MediaPlayer.Play(victorySong);
        MediaPlayer.IsRepeating = true;
        playingVictorySong = true;
      }      
    }

    private int countdownToNextLevel = Game1.DEFAULT_LEVEL_FRAME_COUNT;
    public override void Draw(GameTime gameTime) {
      game.spriteBatch.Draw(background, skyRectangle, Color.White);
      game.spriteBatch.DrawString(font, levelName,
                                  stageTitlePosition, Color.GreenYellow, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
      game.spriteBatch.DrawString(font, BuildSurviveMessage(gameTime), 
                                  secondsTitlePosition, Color.GreenYellow, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
      if (HasWon(gameTime) == false) {
        wizard.Draw(gameTime);
      }
      fireballs.Draw(gameTime);
      if (WizardIsBeingDestroyed()) {
        PlayDed();
      }
      if (WizardIsDestroyed()) {
        PlayDed();
        game.spriteBatch.Draw(gameOverTexture, gameOverColorRectangle, Color.Black);
        game.spriteBatch.DrawString(font, "You DED", centerScreen, Color.Red);
      }
      if (WizardIsBeingDestroyed() == false
          && WizardIsDestroyed() == false
          && HasWon(gameTime)) {
        PlayVictory();
        game.spriteBatch.Draw(gameOverTexture, gameOverColorRectangle, Color.Chartreuse);
        game.spriteBatch.DrawString(font, "You WERN - " + levelName, centerScreen, Color.White);
        game.spriteBatch.DrawString(font, NextStageMessage(), new Vector2(centerScreen.X, centerScreen.Y + 30f), Color.White);
        countdownToNextLevel--;
      }
    }

    public string NextStageMessage() {
      return "Next Stage in: " + Math.Max(countdownToNextLevel / 60, 0) + " seconds";
    }

    public bool HasWon(GameTime gameTime) {
      if (WizardIsBeingDestroyed() || WizardIsDestroyed()) return false;
      return GetTotalSecondsSpentInGame() > winConditionSeconds;
    }

    private bool WizardIsBeingDestroyed() {
      return wizard.CurrentState == BaseEntity.State.DESTROYING;
    }

    private bool WizardIsDestroyed() {
      return wizard.CurrentState == BaseEntity.State.DESTROYED;
    }

    public int GetTotalSecondsSpentInGame() {
     return totalFramesSpentInGame / 60;
    }

    private string BuildSurviveMessage(GameTime gameTime) {
      if (HasWon(gameTime) || WizardIsBeingDestroyed() || WizardIsDestroyed()) {
        return "";
      }        
      return "Survive " + GetTimeLeftToWin() + " more second(s)!";
    }

    public int GetTimeLeftToWin() {
      return Math.Max(winConditionSeconds - GetTotalSecondsSpentInGame(), 0);
    }
  }
}
