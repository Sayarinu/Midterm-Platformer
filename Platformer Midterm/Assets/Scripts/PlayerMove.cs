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

    [SerializeField]
    private bool inWater = false;

    [SerializeField]
    private float gravityScale;

    [SerializeField]
    private float swimTime=0;
    [SerializeField]
    private float currentTime=0;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        boxcollider = GetComponent<BoxCollider2D>();
        gravityScale = _rigidbody.gravityScale;
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
        }else{
            dashAvailable = false;
        }
        
        
    }

    private void FixedUpdate() {
        currentTime += Time.deltaTime;
    }
    private bool isGrounded(){
        RaycastHit2D ray = Physics2D.BoxCast(boxcollider.bounds.center,boxcollider.bounds.size-new Vector3(sideBuffer,0,0),0f,Vector2.down,extraHeight,plats);
        
       
        return ray.collider!=null;
    }
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && (isGrounded() || (inWater && currentTime - swimTime > 0.2))){
            swimTime = currentTime;
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
        
        gravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
        _rigidbody.gravityScale=gravityScale;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.layer==4){
            
            if(!inWater){
                speed/=2;
                accel/=2;
                friction/=2;
                dashspeed/=2;
                dashFriction/=2;
                jump/=3;
                xspeed/=2;
                gravityScale/=4;
                _rigidbody.gravityScale=gravityScale;
                _rigidbody.velocity= new Vector2(xspeed,_rigidbody.velocity.y/2);
            }
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer==4){
            
            if(inWater){
                speed*=2;
                accel*=2;
                friction*=2;
                dashspeed*=2;
                dashFriction*=2;
                jump*=3;
                gravityScale*=4;
                xspeed*=2;
                _rigidbody.gravityScale=gravityScale;
                if(_rigidbody.velocity.y<0){
                    _rigidbody.velocity= new Vector2(xspeed,_rigidbody.velocity.y);
                }else{
                    _rigidbody.velocity= new Vector2(xspeed,_rigidbody.velocity.y*4);
                }
            }
            inWater = false;
        }
    }
}
