using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public int speed = 5;
    public int jump = 2;
    public Rigidbody2D _rigidbody;
    float xspeed=0;
    float friction=0.15f;

    float accel=0.2f;

    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        PlayerInput input = new PlayerInput();
        input.Player.Enable();
        input.Player.jump.performed += Jump;
        input.Player.movement.performed += Move;
    }

    // Update is called once per frame
    void Update()
    {
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
        float val = 0f;
        Debug.Log(context.ReadValue<float>());
        xspeed+=val*speed;
    
        _rigidbody.velocity = new Vector2(xspeed,_rigidbody.velocity.y);
        
    }
}
