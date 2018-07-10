using System;
using System.Collections.Generic;
namespace WizardSurf.Desktop.Engines {
  public class LevelEngine {

    public struct Level {
      public Level(string name, int fireballCount, int maxSpeed, int winConditionSeconds, int lives) {
        Name = name;
        FireballCount = fireballCount;
        MaxSpeed = maxSpeed;
        WinConditionSeconds = winConditionSeconds;
        Lives = lives;
      }
      public string Name { get; set; }
      public int FireballCount { get; set; }
      public int MaxSpeed { get; set; }
      public int WinConditionSeconds { get; set; }
      public int Lives { get; set; }
    }

    private Queue<Level> levels;
    public LevelEngine() {
      //TODO fine tune levels
      var levelList = new List<Level>{
        new Level("Stage Uno", 15, 8, 30, 3),
        new Level("Stage Beta", 15, 8, 60, 3),
        new Level("Stage Right", 15, 8, 100, 3),
        new Level("Emo Stage", 20, 8, 30, 3),
        new Level("Staged Prank", 20, 8, 60, 3),
        new Level("Master Hand", 15, 8, 100, 1)
      };
      levels = new Queue<Level>(levelList);
    }

    public Level GetNextLevel() {
      // TODO return completed game level
      if (levels.Count < 1) return new Level("Free Play", 30, 5, 10000, 1000000);
      return levels.Dequeue();
    }
  }
}
