using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerMove : MonoBehaviour
{
    
    public int speed = 10;
    public int dashspeed = 20;
    public int jump = 2;
    public Rigidbody2D _rigidbody;
    float xspeed=0;

    public static bool canSwim;
    public static bool canDash;

    AudioSource _audioSource;

    public AudioClip enemyDeathSound;
    public AudioClip deathSound;

    public AudioClip dashSound;

    public AudioClip takeDamageSound;

    public AudioClip walkSound;

    public AudioClip jumpSound;

    public AudioClip swimJumpSound;

    public AudioClip collectibleSound;
    

    [SerializeField]
    float friction=0.15f;
    [SerializeField]
    float accel=0.2f;
    [SerializeField]
    float dashFriction=1f;

    [SerializeField]
    private LayerMask plats;

    [SerializeField]
    private LayerMask enemies;
    [SerializeField]
    float extraHeight = 0.1f;

    [SerializeField]
    float sideBuffer = 0.001f;

    Color rayColor=Color.green;
    public PlayerInput input; 

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
    private float hitTime=0;
    [SerializeField]
    private bool invincible=false;
    [SerializeField]
    private float currentTime=0;
    
    



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting");
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        boxcollider = GetComponent<BoxCollider2D>();
        gravityScale = _rigidbody.gravityScale;
        input = PublicVars.input;
        input.Player.Enable();
        input.Player.jump.performed += Jump;
        input.Player.movement.performed += Move;
        input.Player.dash.performed += Dash;
        input.Player.die.performed += DieInput;
    
        // set spawn as first checkpoint
        PublicVars.currentCheckpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded(plats,sideBuffer)){

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
            if(isGrounded(plats,sideBuffer)){
                if(xspeed>speed){
                    xspeed-=dashFriction;
                }
                if((speed*-1)>xspeed){
                    xspeed+=dashFriction;
                }
            }
            _rigidbody.velocity = new Vector2(xspeed,_rigidbody.velocity.y);
            if(_rigidbody.velocity.y<-10){
                _rigidbody.velocity = new Vector2(xspeed,-10);
            }
        }else{
            dashAvailable = false;
        }
        
        
    }

    private void FixedUpdate() {
        currentTime += 1;
        if(currentTime - hitTime > 50){
            invincible=false;
        }
    }
    private bool isGrounded(LayerMask layer, float buffer){
        RaycastHit2D ray = Physics2D.BoxCast(boxcollider.bounds.center,boxcollider.bounds.size-new Vector3(buffer,0,0),0f,Vector2.down,extraHeight,layer);
        
       
        return ray.collider!=null;
    }
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && (isGrounded(plats,sideBuffer) || (inWater && currentTime - swimTime > 10))){
            swimTime = currentTime;
            _rigidbody.velocity = new Vector2(xspeed,jump);
            if (!inWater) {
                _audioSource.PlayOneShot(jumpSound);
            } else {
                _audioSource.PlayOneShot(swimJumpSound);
            }
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
            _audioSource.PlayOneShot(dashSound);
        }
    }

    

    public void DieInput(InputAction.CallbackContext context){
        Die();
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
            if(PublicVars.canSwim){
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
            }else if(!invincible){
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            if(isDashing){
                _audioSource.PlayOneShot(enemyDeathSound);
                Destroy(other.gameObject);
            }else if (isGrounded(enemies,0) && !isGrounded(plats,sideBuffer) && !inWater){
                _audioSource.PlayOneShot(enemyDeathSound);
                Destroy(other.gameObject);
                if(input.Player.jump.IsPressed()){
                    _rigidbody.velocity = new Vector2(xspeed,jump);
                }else{
                    _rigidbody.velocity = new Vector2(xspeed,jump/2);
                }
                hitTime = currentTime - 49;
                invincible = true;
            }else if(!invincible){
                hitTime=currentTime;
                invincible = true;
                PublicVars.playerHealth-=1;
                if (PublicVars.playerHealth != 0) {
                    _audioSource.PlayOneShot(takeDamageSound);
                }
                if(PublicVars.playerHealth<=0){
                    _audioSource.PlayOneShot(deathSound);
                    PublicVars.playerHealth=PublicVars.maxHealth;
                    // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    Die();
                }
            } 
        }
        if (other.CompareTag("DeathBox") && !invincible) {
            Die();
        }
        if (other.CompareTag("Collect")) {
            _audioSource.PlayOneShot(collectibleSound);
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

    private void Die() {
        transform.position = PublicVars.currentCheckpoint;
        _rigidbody.velocity = Vector2.zero;
        PublicVars.playerHealth = 3;
    }

    public void RemoveInput(){
        input.Player.jump.performed -= Jump;
        input.Player.movement.performed -= Move;
        input.Player.dash.performed -= Dash;
        input.Player.die.performed -= DieInput;
    }
}
