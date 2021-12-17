// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Controlls/Secondplayer.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Secondplayer : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Secondplayer()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Secondplayer"",
    ""maps"": [
        {
            ""name"": ""KeyboardControls"",
            ""id"": ""62d4b877-4d44-486a-9f07-63c82430f457"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Value"",
                    ""id"": ""844d9891-8fd7-4747-9b19-75703b06d699"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ae1e216d-4fc4-42ae-9c72-f0e987b0a089"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""29b7acf0-f5c6-487a-8502-6eed4a481467"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""dbcc3c04-472a-4d2c-bb13-3b70ba79fb71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwapWeapon"",
                    ""type"": ""Value"",
                    ""id"": ""30c7e629-f019-47e8-9bb2-f0cb1da2aed0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""09289344-fe02-4895-8967-8fcba0152d02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMap"",
                    ""type"": ""Button"",
                    ""id"": ""66c7348b-87f1-4131-8b0f-710d96d8b699"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""4a482858-621b-4d3b-9941-e7c4bd93eaa7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShowStats"",
                    ""type"": ""Button"",
                    ""id"": ""0d640bd2-4b35-4149-aa8c-852ace7b229d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""4a85f4f8-366d-4499-a175-71a3ec8acb40"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8991ae5c-7bac-41af-9d5e-1568e6d64fdc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""197fe8a6-2cf5-442c-ace7-c0b932f08ab5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74b940c7-609d-4f83-8fbf-631e423f634a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8527ed7-3dec-4967-a519-674fe4dbe83f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41a2d3cf-086c-4645-9bc2-63e987f3fab7"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""SwapWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01997d6d-3e68-4006-8f2c-b111f6af3e99"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""241c0d3d-556c-4cc4-9d76-acf695e18111"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""ToggleMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d8cf635-baac-4dd7-b8ac-e1d1b522a09e"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9b39a9f-7fa9-4682-bd4e-15bf66e72fa1"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""ShowStats"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0214e027-2769-4656-88a7-e582b8e8b580"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller Xbox;Secondplayer"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Secondplayer"",
            ""bindingGroup"": ""Secondplayer"",
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
        m_KeyboardControls_Aim = m_KeyboardControls.FindAction("Aim", throwIfNotFound: true);
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
    private readonly InputAction m_KeyboardControls_Aim;
    public struct KeyboardControlsActions
    {
        private @Secondplayer m_Wrapper;
        public KeyboardControlsActions(@Secondplayer wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_KeyboardControls_Shoot;
        public InputAction @Movement => m_Wrapper.m_KeyboardControls_Movement;
        public InputAction @Reload => m_Wrapper.m_KeyboardControls_Reload;
        public InputAction @Interact => m_Wrapper.m_KeyboardControls_Interact;
        public InputAction @SwapWeapon => m_Wrapper.m_KeyboardControls_SwapWeapon;
        public InputAction @Melee => m_Wrapper.m_KeyboardControls_Melee;
        public InputAction @ToggleMap => m_Wrapper.m_KeyboardControls_ToggleMap;
        public InputAction @Sprint => m_Wrapper.m_KeyboardControls_Sprint;
        public InputAction @ShowStats => m_Wrapper.m_KeyboardControls_ShowStats;
        public InputAction @Aim => m_Wrapper.m_KeyboardControls_Aim;
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
                @Aim.started -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_KeyboardControlsActionsCallbackInterface.OnAim;
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
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public KeyboardControlsActions @KeyboardControls => new KeyboardControlsActions(this);
    private int m_SecondplayerSchemeIndex = -1;
    public InputControlScheme SecondplayerScheme
    {
        get
        {
            if (m_SecondplayerSchemeIndex == -1) m_SecondplayerSchemeIndex = asset.FindControlSchemeIndex("Secondplayer");
            return asset.controlSchemes[m_SecondplayerSchemeIndex];
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
        void OnAim(InputAction.CallbackContext context);
    }
}
