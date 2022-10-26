using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference action;
    [SerializeField]
    private bool excludeMouse = true;
    [Range(0,10)]
    [SerializeField]
    private int selectedBinding;
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

    public InputManager manager;
    public GameObject panel;
    public TMP_Text message;

    private void Awake() {
        UpdateUI();
        
    }
    private void OnEnable() {
        rebind.onClick.AddListener(() => DoRebind());
        reset.onClick.AddListener(() => Reset());

        if(action != null){
            GetBindingInfo();
            UpdateUI();
        }
        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
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
    private void GetBindingInfo()
    {
        if (action.action != null)
            actionName = action.action.name;

        if(action.action.bindings.Count > selectedBinding)
        {
            binding = action.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
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
            }
            else
                rebindText.text = action.action.GetBindingDisplayString(bindingIndex);
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
        InputManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse);
       
    }

    private void Reset()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}
