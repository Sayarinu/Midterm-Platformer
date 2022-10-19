using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerMove : MonoBehaviour
{
    
    public int speed = 10;
    public int dashspeed = 20;
    public int jump = 2;
    public Rigidbody2D _rigidbody;
    float xspeed=0;

    [SerializeField]
    float friction=0.15f;
    [SerializeField]
    float accel=0.2f;
    [SerializeField]
    float dashFriction=1f;

    [SerializeField]
    private LayerMask plats;
    [SerializeField]
    float extraHeight = 0.1f;

    [SerializeField]
    float sideBuffer = 0.001f;

    Color rayColor=Color.green;
    private PlayerInput input; 

    private BoxCollider2D boxcollider;
    
    [SerializeField]
    private bool dashAvailable = true;

    [SerializeField]
    private bool isDashing = false;

    [SerializeField]
    private int facingRight = 1;



    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        boxcollider = GetComponent<BoxCollider2D>();
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.jump.performed += Jump;
        input.Player.movement.performed += Move;
        input.Player.dash.performed += Dash;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded()){

            rayColor = Color.green; 
            dashAvailable = true;
        }else{
            rayColor = Color.red;
        }
        Debug.DrawRay(boxcollider.bounds.center + new Vector3(boxcollider.bounds.extents.x-sideBuffer,0),Vector2.down*(boxcollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxcollider.bounds.center - new Vector3(boxcollider.bounds.extents.x-sideBuffer,0),Vector2.down*(boxcollider.bounds.extents.y + extraHeight), rayColor);
        Debug.DrawRay(boxcollider.bounds.center - new Vector3(boxcollider.bounds.extents.x-sideBuffer,boxcollider.bounds.extents.y + extraHeight),Vector2.right*(boxcollider.bounds.extents.x-sideBuffer)*2, rayColor);
        float val = input.Player.movement.ReadValue<Vector2>().x;
        if(!isDashing){
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
            if(xspeed>0){
                facingRight = 1;
            }else if(xspeed<0){
                facingRight = -1;
            }
            if(isGrounded()){
                if(xspeed>speed){
                    xspeed-=dashFriction;
                }
                if((speed*-1)>xspeed){
                    xspeed+=dashFriction;
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
        }else{
            dashAvailable = false;
        }
        
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
        float val = input.Player.movement.ReadValue<Vector2>().x;
        if(xspeed<speed && (speed*-1)<xspeed){
            xspeed+=val*accel;
        }
        _rigidbody.velocity = new Vector2(xspeed,_rigidbody.velocity.y);
        
    }

    public void Dash(InputAction.CallbackContext context){
        Vector2 vec = input.Player.movement.ReadValue<Vector2>();
        if(dashAvailable){
            print("dash");
            StartCoroutine("DashTime");
            dashAvailable = false;
        }
    }

    IEnumerator DashTime(){
        print("actual");
        Vector2 vec = input.Player.movement.ReadValue<Vector2>();
        isDashing = true;
        _rigidbody.velocity = new Vector2(0,0);
        _rigidbody.AddForce(vec.normalized*dashspeed,ForceMode2D.Impulse);
        xspeed=vec.normalized.x*dashspeed;
        if(vec==Vector2.zero){
            _rigidbody.AddForce(new Vector2(facingRight*dashspeed,0),ForceMode2D.Impulse);
            xspeed=facingRight*dashspeed;
        }
        
        float gravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        _rigidbody.gravityScale=gravityScale;
    }
}
