using Microsoft.Xna.Framework.Input;

namespace D3D.Managers.Implementations;

public sealed class InputManager
{
    private KeyboardState _previousKeyboardState;
    private KeyboardState _currentKeyboardState;

    private MouseState _previousMouseState;
    private MouseState _currentMouseState;

    public InputManager()
    {
        GamePad.InitDatabase();
    }
    
    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        _previousMouseState = _currentMouseState;
        _currentMouseState = Mouse.GetState();
    }
}