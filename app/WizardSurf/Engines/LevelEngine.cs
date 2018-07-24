using System;
using System.Collections.Generic;
namespace WizardSurf.Desktop.Engines {
  public class LevelEngine {
    // TODO add sprite to level
    public struct Level {
      public Level(string name, string background, int fireballCount, int maxSpeed, int winConditionSeconds, int lives) {
        Name = name;
        Background = background;
        FireballCount = fireballCount;
        MaxSpeed = maxSpeed;
        WinConditionSeconds = winConditionSeconds;
        Lives = lives;
      }
      public string Name { get; set; }
      public string Background { get; set; }
      public int FireballCount { get; set; }
      public int MaxSpeed { get; set; }
      public int WinConditionSeconds { get; set; }
      public int Lives { get; set; }
    }

    //TODO add hard mode, where lives do not reset, etc
    private Queue<Level> levels;
    public LevelEngine() {
      //TODO fine tune levels
      var levelList = new List<Level>{
        new Level("Stage Uno", "background", 15, 8, 30, 3),
        new Level("Stage Beta", "background", 15, 8, 60, 3),
        new Level("Stage Right", "background2", 15, 8, 100, 3),
        new Level("Emo Stage", "background2", 20, 8, 30, 3),
        new Level("Staged Prank", "background3", 20, 8, 60, 3),
        new Level("Master Hand", "background3", 15, 8, 100, 1)
      };
      levels = new Queue<Level>(levelList);
    }

    public Level GetNextLevel() {
      // TODO Add secret ending after this level
      if (levels.Count < 1) return new Level("Can U Die?", "background", 40, 6, 5000, 10000);
      return levels.Dequeue();
    }
  }
}
