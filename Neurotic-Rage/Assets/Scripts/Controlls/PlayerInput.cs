// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controlls/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""KeyboardControls"",
            ""id"": ""831c6626-6f01-4b41-aab6-26114b0c2509"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Value"",
                    ""id"": ""6b97c69f-e1e0-48af-8cbb-6f1d3e12c37f"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""74455640-7d4c-48bb-8110-af654424fb4c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Direction"",
                    ""id"": ""4746717d-e23a-4ef2-871e-5bdb058051fe"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""de973c4f-736d-409a-83d0-f3ec3711224f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fed05c57-537e-487c-a80a-0af0e352c123"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bcf5b6ff-7a89-40b9-acc0-e30c5f87b74e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e1734376-303e-4968-b540-60033b77ecb5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""65f75164-3da1-416e-9b8a-e10922f53c7d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8a144c28-90f2-4707-825f-4a4be87639bd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller Xbox"",
            ""bindingGroup"": ""Controller Xbox"",
            ""devices"": []
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // KeyboardControls
        m_KeyboardControls = asset.FindActionMap("KeyboardControls", throwIfNotFound: true);
        m_KeyboardControls_Shoot = m_KeyboardControls.FindAction("Shoot", throwIfNotFound: true);
        m_KeyboardControls_Movement = m_KeyboardControls.FindAction("Movement", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // KeyboardControls
    private readonly InputActionMap m_KeyboardControls;
    private IKeyboardControlsActions m_KeyboardControlsActionsCallbackInterface;
    private readonly InputAction m_KeyboardControls_Shoot;
    private readonly InputAction m_KeyboardControls_Movement;
    public struct KeyboardControlsActions
    {
        private @PlayerInput m_Wrapper;
        public KeyboardControlsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_KeyboardControls_Shoot;
        public InputAction @Movement => m_Wrapper.m_KeyboardControls_Movement;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardControlsActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardControlsActions instance)
        {
            if (m_Wrapper.m_KeyboardControlsActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShoot;
                @Movement.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_KeyboardControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public KeyboardControlsActions @KeyboardControls => new KeyboardControlsActions(this);
    private int m_ControllerXboxSchemeIndex = -1;
    public InputControlScheme ControllerXboxScheme
    {
        get
        {
            if (m_ControllerXboxSchemeIndex == -1) m_ControllerXboxSchemeIndex = asset.FindControlSchemeIndex("Controller Xbox");
            return asset.controlSchemes[m_ControllerXboxSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IKeyboardControlsActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
}
