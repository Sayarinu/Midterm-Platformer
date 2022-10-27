using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static PlayerInput inputActions;

    public static string control;
    public int help;

    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    public TMP_Text message = null;
    public GameObject panel;

    private void Start()
    {
        inputActions = PublicVars.input;
        message = null;
        control = null;
    }

    public static void StartRebind(string actionName, int bindingIndex, TMP_Text statusText, bool excludeMouse, bool controller)
    {
        InputAction action = inputActions.asset.FindAction(actionName);
        if (action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
                DoRebind(action, firstPartIndex, statusText, true, excludeMouse, controller);
        }
        else
            DoRebind(action, bindingIndex, statusText, false, excludeMouse, controller);
    }
   
    private static void DoRebind(InputAction actionToRebind, int bindingIndex, TMP_Text statusText, bool allCompositeParts, bool excludeMouse, bool controller)
    {
        if (actionToRebind == null || bindingIndex < 0){
            return;
        }
        

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            
            operation.Dispose();

            if(allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count){
                    
                    if(actionToRebind.bindings[nextBindingIndex].isPartOfComposite){
                        DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse, controller);
                    }
                }else{
                    if(controller){    
                        statusText.text = GetBindingName(actionToRebind.name, 5);
                    }else{
                        statusText.text = GetBindingName(actionToRebind.name, 0);
                    }
                }
            }
            if(bindingIndex+1 < actionToRebind.bindings.Count){
                if(!actionToRebind.bindings[bindingIndex+1].isPartOfComposite&&!actionToRebind.bindings[bindingIndex+1].isComposite){
                    statusText.text = actionToRebind.GetBindingDisplayString(bindingIndex);
                }
                else if(actionToRebind.bindings[bindingIndex+1].isComposite){
                    if(controller){
                        statusText.text = GetBindingName(actionToRebind.name, 5);
                    }else{
                        
                        statusText.text = GetBindingName(actionToRebind.name, 0);
                    
                    }
                }
            }
            
            SaveBindingOverride(actionToRebind);
            rebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/backspace");
        
        if(actionToRebind.bindings[bindingIndex].isPartOfComposite){
            statusText.text = $"Waiting for {actionToRebind.bindings[bindingIndex].name} input...";
        }else{
            statusText.text = $"Waiting for {rebind.expectedControlType} input...";
        }
        
        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start(); //actually starts the rebinding process
    }

    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if(inputActions==null){
            inputActions = PublicVars.input;
        }
        InputAction action = inputActions.FindAction(actionName);
        
        return action.GetBindingDisplayString(bindingIndex);
    }

    private static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        

        InputAction action = inputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = inputActions.asset.FindAction(actionName);

        if(action == null || action.bindings.Count <= bindingIndex)
        {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            print(bindingIndex);
            for (int i = bindingIndex+1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; i++){
                print("hi");
                action.RemoveBindingOverride(i);
            }
        }
        else{
            print("no");
            action.RemoveBindingOverride(bindingIndex);
        }

        SaveBindingOverride(action);
    }

    

}