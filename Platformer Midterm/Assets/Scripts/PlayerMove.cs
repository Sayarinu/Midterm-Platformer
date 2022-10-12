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

    [SerializeField]
    private LayerMask plats;
    [SerializeField]
    float extraHeight = 0.1f;

    [SerializeField]
    float sideBuffer = 0.001f;

    Color rayColor=Color.green;
    private PlayerInput input; 

    private BoxCollider2D boxcollider;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        boxcollider = GetComponent<BoxCollider2D>();
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.jump.performed += Jump;
        input.Player.movement.performed += Move;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded()){
            rayColor = Color.green; 
        }else{
            rayColor = Color.red;
        }
        Debug.DrawRay(boxcollider.bounds.center + new Vector3(boxcollider.bounds.extents.x-sideBuffer,0),Vector2.down*(boxcollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxcollider.bounds.center - new Vector3(boxcollider.bounds.extents.x-sideBuffer,0),Vector2.down*(boxcollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxcollider.bounds.center - new Vector3(boxcollider.bounds.extents.x-sideBuffer,boxcollider.bounds.extents.y + extraHeight),Vector2.right*(boxcollider.bounds.extents.x-sideBuffer)*2, rayColor);
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

    private bool isGrounded(){
        RaycastHit2D ray = Physics2D.BoxCast(boxcollider.bounds.center,boxcollider.bounds.size-new Vector3(sideBuffer,0,0),0f,Vector2.down,extraHeight,plats);
        
       
        return ray.collider!=null;
    }
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && isGrounded()){
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
