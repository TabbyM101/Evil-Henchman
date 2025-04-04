
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaytimeInputManager : MonoBehaviour
{
    public static InputSystem_Actions inputActions;
    // you may subscribe to this event if your script would like to know whenever action map layers are enabled or
    // disabled utilizing this utility class
    public static event Action<InputActionMap> actionMapChange;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Disables all action maps and enables only the given action map.
    /// </summary>
    /// <param name="actionMap">The action map to enable.</param>
    public static void EnableActionMapOnly(InputActionMap actionMap)
    {
        
        if (actionMap.enabled)
        {
            return;
        }
        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
        Debug.Log("Enable " + actionMap.name + " action map.");
    }

    /// <summary>
    /// Disables all action maps and enables only the list of the given action maps.
    /// </summary>
    /// <param name="actionMaps">The action maps to enable</param>
    public static void EnableActionMapOnly(List<InputActionMap> actionMaps)
    {
        inputActions.Disable();
        foreach (var actionMap in actionMaps)
        {
            if (actionMap.enabled) { continue; }
            actionMapChange?.Invoke(actionMap);
            actionMap.Enable();
            Debug.Log("Enable " + actionMap.name + " action map.");
        }
        
    }

    public static void SetActionMapEnabled(InputActionMap actionMap, bool enabled)
    {
        if (enabled && !actionMap.enabled)
        {
            actionMapChange?.Invoke(actionMap);
            actionMap.Enable();
            Debug.Log("Enable " + actionMap.name + " action map.");
        }
        else if (!enabled && actionMap.enabled)
        {
            actionMap.Disable();
        }
    }

    /// <summary>
    /// Disables all action maps.
    /// </summary>
    public static void DisableAllActionMaps()
    {
        if (inputActions != null)
        {
            foreach (var actionMap in inputActions)
            {
                actionMap.Disable();
            }
        }
    }

    public static void EnableAllActionMaps()
    {
         if (inputActions != null)
        {
            foreach (var actionMap in inputActions)
            {
                actionMap.Enable();
            }
        }
    }

    public static InputSystem_Actions GetInputAsset()
    {
        return inputActions;
    }
}

public enum ActionMapLayers{
    Player,
    UI,
    Camera,
    Menu,
    Developer
}
