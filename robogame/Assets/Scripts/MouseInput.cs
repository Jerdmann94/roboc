// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/mouse.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MouseInput : IInputActionCollection, IDisposable
{
    public InputActionAsset Asset { get; }
    public @MouseInput()
    {
        Asset = InputActionAsset.FromJson(@"{
    ""name"": ""mouse"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""cca2d808-a28d-4ea0-8693-eaeb7222dba5"",
            ""actions"": [
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""a2bcbc95-41e8-4a22-97df-f4da4611fc85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""640f5e66-ee7f-4c49-a32a-6917bad8e7df"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3e760186-dfaa-4d20-ac35-8e8e0e8a0b46"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6c1e768-ba03-4f9b-97e8-9291ef138955"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        _mMouse = Asset.FindActionMap("Mouse", throwIfNotFound: true);
        _mMouseMouseClick = _mMouse.FindAction("MouseClick", throwIfNotFound: true);
        _mMouseMousePosition = _mMouse.FindAction("MousePosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(Asset);
    }

    public InputBinding? bindingMask
    {
        get => Asset.bindingMask;
        set => Asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => Asset.devices;
        set => Asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => Asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return Asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return Asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        Asset.Enable();
    }

    public void Disable()
    {
        Asset.Disable();
    }

    // Mouse
    private readonly InputActionMap _mMouse;
    private IMouseActions _mMouseActionsCallbackInterface;
    private readonly InputAction _mMouseMouseClick;
    private readonly InputAction _mMouseMousePosition;
    public struct MouseActions
    {
        private @MouseInput _mWrapper;
        public MouseActions(@MouseInput wrapper) { _mWrapper = wrapper; }
        public InputAction @MouseClick => _mWrapper._mMouseMouseClick;
        public InputAction @MousePosition => _mWrapper._mMouseMousePosition;
        public InputActionMap get() { return _mWrapper._mMouse; }
        public void enable() { get().Enable(); }
        public void disable() { get().Disable(); }
        public bool Enabled => get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.get(); }
        public void setCallbacks(IMouseActions instance)
        {
            if (_mWrapper._mMouseActionsCallbackInterface != null)
            {
                @MouseClick.started -= _mWrapper._mMouseActionsCallbackInterface.OnMouseClick;
                @MouseClick.performed -= _mWrapper._mMouseActionsCallbackInterface.OnMouseClick;
                @MouseClick.canceled -= _mWrapper._mMouseActionsCallbackInterface.OnMouseClick;
                @MousePosition.started -= _mWrapper._mMouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= _mWrapper._mMouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= _mWrapper._mMouseActionsCallbackInterface.OnMousePosition;
            }
            _mWrapper._mMouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseClick.started += instance.OnMouseClick;
                @MouseClick.performed += instance.OnMouseClick;
                @MouseClick.canceled += instance.OnMouseClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnMouseClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
