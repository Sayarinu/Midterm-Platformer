using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Users;

[RequireComponent(typeof(PlayerInput))]
public class RebindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference action;
    [SerializeField]
    private bool excludeMouse = true;
   
   
    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions;
    [SerializeField]
    private InputBinding binding;
    [SerializeField]
    private int bindingIndex;
    private string actionName;
    [SerializeField]
    private TMP_Text actionText;
    [SerializeField]
    private Button rebind;
    [SerializeField]
    private TMP_Text rebindText;
    [SerializeField]
    private Button reset;

    private PlayerInput input;
    public InputManager manager;
    public GameObject panel;
    public TMP_Text message;
    public string controlScheme;

    [SerializeField]
    private int controllerBindIndex;

    public TMP_Text controllerBind;
    [SerializeField]
    private Button controllerButton;
    
    

    private void Awake() {
        UpdateUI();
        input = PublicVars.input;
       
       
        
        
    }

    
    private void OnEnable() {
        rebind.onClick.AddListener(() => DoRebind());
        controllerButton.onClick.AddListener(() => DoRebindController());
        reset.onClick.AddListener(() => Reset());

        if(action != null){
            GetBindingInfo();
            UpdateUI();
        }
        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    
    }

    public void Change(InputUser user, InputUserChange change, InputDevice device){
        if(change == InputUserChange.ControlSchemeChanged){
            print("hi");
        }
    }
    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }
    private void OnValidate() {
        if(action == null){
            return;
        }
        GetBindingInfo();
        UpdateUI();
    }
    public void OnControlsChanged()
    {
       print("hi");
    }
    
    private void GetBindingInfo()
    {
        if (action.action != null)
            actionName = action.action.name;

        bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);
        
        if(action.action.bindings[bindingIndex].isPartOfComposite){
            bindingIndex-=1;
        }
    }
     private void UpdateUI()
    {
        if (actionText != null)
            actionText.text = actionName;

        if(rebindText != null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
                controllerBind.text = InputManager.GetBindingName(actionName, controllerBindIndex);
            }
            else{
                rebindText.text = action.action.GetBindingDisplayString(bindingIndex);
                controllerBind.text = action.action.GetBindingDisplayString(controllerBindIndex);
            }
        }
    }

    private void DoRebind()
    {
        if(actionName!="movement"){
            InputManager.rebindComplete += UpdateUI;
            InputManager.rebindCanceled += UpdateUI;
        }else{
            InputManager.rebindComplete -= UpdateUI;
            InputManager.rebindCanceled -= UpdateUI;
        }
        InputManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse, false);
       
    }

    private void DoRebindController()
    {
        if(actionName!="movement"){
            InputManager.rebindComplete += UpdateUI;
            InputManager.rebindCanceled += UpdateUI;
        }else{
            InputManager.rebindComplete -= UpdateUI;
            InputManager.rebindCanceled -= UpdateUI;
        }
        InputManager.StartRebind(actionName, controllerBindIndex, controllerBind, excludeMouse, true);
       
    }

    private void Reset()
    {
        
        InputManager.ResetBinding(actionName, bindingIndex);
        InputManager.ResetBinding(actionName, controllerBindIndex);
        UpdateUI();
    }
}
