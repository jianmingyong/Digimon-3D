using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace D3D.Managers.Implementations;

public sealed class InputManager : IInputManager
{
    private KeyboardState _previousKeyboardState;
    private KeyboardState _currentKeyboardState;

    private MouseState _previousMouseState;
    private MouseState _currentMouseState;

    public InputManager(Game game)
    {
        GamePad.InitDatabase();
        
        game.Services.AddService(typeof(InputManager), this);
        game.Services.AddService(typeof(IInputManager), this);
    }
    
    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }

    public bool IsKeyDown(Keys keys, bool canRepeat = true)
    {
        if (canRepeat)
        {
            return _currentKeyboardState.IsKeyDown(keys);
        }

        return _currentKeyboardState.IsKeyDown(keys) && !_previousKeyboardState.IsKeyDown(keys);
    }
    
    public bool IsKeyUp(Keys keys, bool canRepeat = true)
    {
        if (canRepeat)
        {
            return _currentKeyboardState.IsKeyUp(keys);
        }

        return _currentKeyboardState.IsKeyUp(keys) && !_previousKeyboardState.IsKeyUp(keys);
    }
}