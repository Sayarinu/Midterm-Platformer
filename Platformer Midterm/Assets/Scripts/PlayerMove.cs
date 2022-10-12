using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerMove : MonoBehaviour
{
    
    public int speed = 10;
    public int jump = 2;
    public Rigidbody2D _rigidbody;
    float xspeed=0;

    [SerializeField]
    float friction=0.15f;
    [SerializeField]
    float accel=0.2f;
    private PlayerInput input; 
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.jump.performed += Jump;
        input.Player.movement.performed += Move;
    }

    // Update is called once per frame
    void Update()
    {
        float val = input.Player.movement.ReadValue<float>();
        
        if(xspeed<speed && (speed*-1)<xspeed){
            xspeed+=val*accel;
        }
        if(val==0){
            if(xspeed>0){
                xspeed-=friction;
            }
            if(xspeed<0){
                xspeed+=friction;
            }
            if(xspeed<friction && (friction*-1)<xspeed){
                xspeed=0;
            }
        }
        _rigidbody.velocity = new Vector2(xspeed,_rigidbody.velocity.y);
        // if(Input.GetButton("Left")){
        //     xspeed-=accel;
        // }
        // if(Input.GetButton("Right")){
        //     xspeed+=accel;
        // }
        // if(!Input.GetButton("Right") && !Input.GetButton("Left")){
        //     if(xspeed>0){
        //         xspeed-=friction;
        //     }
        //     if(xspeed<0){
        //         xspeed+=friction;
        //     }
        //     if(xspeed<0.15 && -0.15<xspeed){
        //         xspeed=0;
        //     }
        // }
        // _rigidbody.velocity = new Vector2(xspeed,0);
        
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed){
            _rigidbody.velocity = new Vector2(xspeed,jump);
        }
    }

    public void Move(InputAction.CallbackContext context){
        float val = context.ReadValue<float>();
       
        
        if(xspeed<speed && (speed*-1)<xspeed){
            xspeed+=val*accel;
        }
       
        _rigidbody.velocity = new Vector2(xspeed,_rigidbody.velocity.y);
        
    }
}
