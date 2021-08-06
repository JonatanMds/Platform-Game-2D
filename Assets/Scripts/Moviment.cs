using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moviment : MonoBehaviour
{


    private Rigidbody2D _rb;

    [Header("Moviment Player or eixo X")]
    [SerializeField] private float _speed = 10F;
    [SerializeField]private float _maxSpeed;
    [SerializeField]private float _linearDrag; //Desaceleração
    private float _horizontal;
    private bool _direction => (_rb.velocity.x > 0f && _horizontal < 0f) || (_rb.velocity.x < 0f && _horizontal > 0f);

    [Header("Moviment Player or eixo Y")]
    [SerializeField]private float _jumpForce = 10;
    [SerializeField]private LayerMask _groundlayer;
    [SerializeField]private float _larguraDoRaycast = 0.8f;
    [SerializeField]private float _airLinearDrag = 2.5f; 
    [SerializeField]private float _fallMultiplier = 8f; //Velocidade da queda, almenta a gravidade na queda
    [SerializeField]private float _lowJumpFallMultiplier = 5f; //Aumenta ou diminui a gravidade na hora do salto
    private float _vertical; 
    private bool _canJump => Input.GetButtonDown("Jump") && _onGrounded;
    private bool _onGrounded;



    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_horizontal);
        moviment();
        if(_canJump) jump();   
    }

    private void FixedUpdate() {
        checkCollision();
        
        if(_onGrounded){
            ApplyLinearDrag();
        }else{
            ApplyAirLinearDrag();
        }
        FallMultiplier();
    }

    void moviment(){
        _horizontal = Input.GetAxis("Horizontal");
        _rb.AddForce(new Vector2(_horizontal, 0f)*_speed);

        if(Mathf.Abs(_rb.velocity.x) > _maxSpeed){
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x)*_maxSpeed, _rb.velocity.y);
        }
    }

    void jump(){
        //_vertical = Input.GetAxisRaw("Vertical");
        _rb.velocity = new Vector2(_rb.velocity.x,0f);
        _rb.AddForce(Vector2.up*_jumpForce, ForceMode2D.Impulse);
    }


    void ApplyLinearDrag(){
        if(Mathf.Abs(_horizontal) < 0.4f || _direction){
            _rb.drag = _linearDrag;
        }else{
            _rb.drag = 0;
        }

    }
    void ApplyAirLinearDrag(){
        
        _rb.drag = _linearDrag;
    }

    void FallMultiplier(){
        if(_rb.velocity.y < 0){
            _rb.gravityScale = _fallMultiplier;
        }else if(_rb.velocity.y > 0 && !Input.GetButtonDown("Jump")){
            _rb.gravityScale = _lowJumpFallMultiplier;
        }else{
            _rb.gravityScale = 1;
        }
    }

    void checkCollision(){
        _onGrounded = Physics2D.Raycast(transform.position,Vector2.down,_larguraDoRaycast,_groundlayer);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position,transform.position+Vector3.down*_larguraDoRaycast);  

    }

    
}
