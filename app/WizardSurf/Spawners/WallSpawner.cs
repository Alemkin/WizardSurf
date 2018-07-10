using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WizardSurf.Desktop.Entities;

namespace WizardSurf.Desktop.Spawners {
  public class WallSpawner {
    public enum Wall { WEST = 0, NORTH = 1, EAST = 2 }

    private int speedMax;
    public WallSpawner(int speedMax = 8) {
      this.speedMax = speedMax;
    }

    //TODO add chance to spawn power ups
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
      var wall = Game1.random.Next(3);
      var speed = (double)Game1.random.Next(4, speedMax);
      var angle = (double)CalculateAngle(wall);
      var radians = ConvertToRadians(angle);
      var velocity = new Vector2(CalculateX(speed, radians), CalculateY(speed, radians));
      var position = CalculatePosition(game, wall);
      return new Fireball(game, velocity, position);
    }

    private double ConvertToRadians(double angle) {
      return Math.PI * (angle / 180);
    }

    private Vector2 CalculatePosition(Game1 game, int wall) {
      switch (wall) {
        case (int)Wall.WEST:
          return new Vector2(0, Game1.random.Next(game.graphics.PreferredBackBufferHeight + 1));
        case (int)Wall.NORTH:
          return new Vector2(Game1.random.Next(game.graphics.PreferredBackBufferWidth + 1), 0);
        case (int)Wall.EAST:
          return new Vector2(game.graphics.PreferredBackBufferWidth, Game1.random.Next(game.graphics.PreferredBackBufferHeight + 1));
        default:
          return new Vector2(0, 0);
      }
    }

    private float CalculateY(double speed, double angle) {
      return (float)(speed * Math.Sin(angle));
    }

    private float CalculateX(double speed, double angle) {
      return (float)(speed * Math.Cos(angle));
    }

    private int CalculateAngle(int wall) {
      switch (wall) {
        case (int)Wall.WEST:
          return Game1.random.Next(-90, 90);
        case (int)Wall.NORTH:
          return Game1.random.Next(0, 180);
        case (int)Wall.EAST:
          return Game1.random.Next(90, 270);
        default:
          return 0;
      }
    }
  }
}
