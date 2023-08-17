using Microsoft.Xna.Framework.Input;

namespace D3D.Managers;

public interface IInputManager
{
    bool IsKeyDown(Keys keys, bool canRepeat = true);

    bool IsKeyUp(Keys keys, bool canRepeat = true);
}