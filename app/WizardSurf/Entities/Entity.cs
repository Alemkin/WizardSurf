using Microsoft.Xna.Framework;
namespace WizardSurf.Desktop.Entities {
  public interface Entity {
    void LoadContent();
    void UnloadContent();
    void Update(GameTime gameTime);
    void Draw(GameTime gameTime);
  }
}
