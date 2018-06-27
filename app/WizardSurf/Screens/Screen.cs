using System;
namespace WizardSurf.Desktop {
  public interface Screen {
    void LoadContent();
    void UnloadContent();
    void Draw(GameTime gameTime);
    void Update(GameTime gameTime);
  }
}
