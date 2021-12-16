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
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""e771843b-2f5b-47c8-936a-a03a2c9896cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""a759ae00-339e-4175-a808-8466a9836ee0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapWeapon"",
                    ""type"": ""Value"",
                    ""id"": ""2ef64698-9891-44f1-86fa-79955c70c59c"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""f147bdb8-8ac5-4de1-8f38-3be6273b7211"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMap"",
                    ""type"": ""Button"",
                    ""id"": ""f69b05e8-7111-4dd8-b07a-353f3072f985"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""2a6b7f1a-6983-4cdf-973d-d57b8b8da6b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowStats"",
                    ""type"": ""Value"",
                    ""id"": ""f6aa32b8-d332-4aa2-abb1-53779c7422d1"",
                    ""expectedControlType"": ""Analog"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""e02affe8-a52f-4080-ab5c-b7ad26874a16"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c097d7bd-9bf5-477b-abaf-66940249e18a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cad65d42-1db5-4224-80d4-6cb698c705ab"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwapWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c010e76-18e3-4f44-a041-b98894861dac"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17359a6c-2f37-4dc4-8d58-732c2d5e8fcf"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ToggleMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5a71f5f-8c33-406a-89aa-ea069815530e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27d95d9b-bc61-4066-b147-f189dd80d094"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ShowStats"",
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
        m_KeyboardControls_Reload = m_KeyboardControls.FindAction("Reload", throwIfNotFound: true);
        m_KeyboardControls_Interact = m_KeyboardControls.FindAction("Interact", throwIfNotFound: true);
        m_KeyboardControls_SwapWeapon = m_KeyboardControls.FindAction("SwapWeapon", throwIfNotFound: true);
        m_KeyboardControls_Melee = m_KeyboardControls.FindAction("Melee", throwIfNotFound: true);
        m_KeyboardControls_ToggleMap = m_KeyboardControls.FindAction("ToggleMap", throwIfNotFound: true);
        m_KeyboardControls_Sprint = m_KeyboardControls.FindAction("Sprint", throwIfNotFound: true);
        m_KeyboardControls_ShowStats = m_KeyboardControls.FindAction("ShowStats", throwIfNotFound: true);
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
    private readonly InputAction m_KeyboardControls_Reload;
    private readonly InputAction m_KeyboardControls_Interact;
    private readonly InputAction m_KeyboardControls_SwapWeapon;
    private readonly InputAction m_KeyboardControls_Melee;
    private readonly InputAction m_KeyboardControls_ToggleMap;
    private readonly InputAction m_KeyboardControls_Sprint;
    private readonly InputAction m_KeyboardControls_ShowStats;
    public struct KeyboardControlsActions
    {
        private @PlayerInput m_Wrapper;
        public KeyboardControlsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_KeyboardControls_Shoot;
        public InputAction @Movement => m_Wrapper.m_KeyboardControls_Movement;
        public InputAction @Reload => m_Wrapper.m_KeyboardControls_Reload;
        public InputAction @Interact => m_Wrapper.m_KeyboardControls_Interact;
        public InputAction @SwapWeapon => m_Wrapper.m_KeyboardControls_SwapWeapon;
        public InputAction @Melee => m_Wrapper.m_KeyboardControls_Melee;
        public InputAction @ToggleMap => m_Wrapper.m_KeyboardControls_ToggleMap;
        public InputAction @Sprint => m_Wrapper.m_KeyboardControls_Sprint;
        public InputAction @ShowStats => m_Wrapper.m_KeyboardControls_ShowStats;
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
                @Reload.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnReload;
                @Interact.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnInteract;
                @SwapWeapon.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSwapWeapon;
                @SwapWeapon.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSwapWeapon;
                @SwapWeapon.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSwapWeapon;
                @Melee.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMelee;
                @Melee.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMelee;
                @Melee.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnMelee;
                @ToggleMap.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnToggleMap;
                @ToggleMap.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnToggleMap;
                @ToggleMap.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnToggleMap;
                @Sprint.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnSprint;
                @ShowStats.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShowStats;
                @ShowStats.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShowStats;
                @ShowStats.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnShowStats;
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
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @SwapWeapon.started += instance.OnSwapWeapon;
                @SwapWeapon.performed += instance.OnSwapWeapon;
                @SwapWeapon.canceled += instance.OnSwapWeapon;
                @Melee.started += instance.OnMelee;
                @Melee.performed += instance.OnMelee;
                @Melee.canceled += instance.OnMelee;
                @ToggleMap.started += instance.OnToggleMap;
                @ToggleMap.performed += instance.OnToggleMap;
                @ToggleMap.canceled += instance.OnToggleMap;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @ShowStats.started += instance.OnShowStats;
                @ShowStats.performed += instance.OnShowStats;
                @ShowStats.canceled += instance.OnShowStats;
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
        void OnReload(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSwapWeapon(InputAction.CallbackContext context);
        void OnMelee(InputAction.CallbackContext context);
        void OnToggleMap(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnShowStats(InputAction.CallbackContext context);
    }
}
