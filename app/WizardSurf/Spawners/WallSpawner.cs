using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.Spawners {
  public class WallSpawner {
    public enum Wall { WEST = 0, NORTH = 1, EAST = 2 }
    private Random random = new Random();

    public WallSpawner() {
    }

    public List<Fireball> Spawn(Game1 game, int number) {
      var fireballs = new List<Fireball>();
      for (int i = 0; i < number; i++) {
        fireballs.Add(CreateRandomFireball(game));
      }
      return fireballs;
    }

    public Fireball Spawn(Game1 game) {
      return CreateRandomFireball(game);
    }

    private Fireball CreateRandomFireball(Game1 game) {
      var wall = random.Next(3);
      var speed = (double)random.Next(4, 8);
      var angle = (double)CalculateAngle(wall);
      var velocity = new Vector2(CalculateX(speed, angle), CalculateY(speed, angle));
      var position = CalculatePosition(game, wall);
      return new Fireball(game, velocity, position);
    }

    private Vector2 CalculatePosition(Game1 game, int wall) {
      switch (wall) {
        case (int)Wall.WEST:
          return new Vector2(0, random.Next(game.graphics.PreferredBackBufferHeight + 1));
        case (int)Wall.NORTH:
          return new Vector2(random.Next(game.graphics.PreferredBackBufferWidth + 1), 0);
        case (int)Wall.EAST:
          return new Vector2(game.graphics.PreferredBackBufferWidth, random.Next(game.graphics.PreferredBackBufferHeight + 1));
        default:
          return new Vector2(0, 0);
      }
    }

    private float CalculateY(double speed, double angle) {
      return (float)(speed * Math.Cos(angle));
    }

    private float CalculateX(double speed, double angle) {
      return (float)(speed * Math.Sin(angle));
    }

    //TODO fix angles
    private int CalculateAngle(int wall) {
      switch (wall) {
        case (int)Wall.WEST:
          return random.Next(-180, 0);
        case (int)Wall.NORTH:
          return random.Next(180, 360);
        case (int)Wall.EAST:
          return random.Next(90, 270);
        default:
          return 0;
      }
    }
  }
}
