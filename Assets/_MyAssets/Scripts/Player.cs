using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    
    //Stats
    
    [SerializeField] protected int _Strength = 6;    
    [SerializeField] protected int _Dexterity = 6;
    [SerializeField] protected int _Constitution = 6;
    [SerializeField] protected int _Intelligence = 6;
    
    
    [SerializeField] protected float _Speed = 1;
    [SerializeField] protected int _Health = 10;
    
    [SerializeField] protected float _AttackRate = 1.0f;
    
    [SerializeField] protected GameObject _Attack1Prefab = default;
    [SerializeField] protected GameObject _Attack2Prefab = default;
    [SerializeField] protected GameObject _Attack3Prefab = default;
    [SerializeField] protected GameObject _Attack4Prefab = default;
    [SerializeField] protected GameObject _DeathPrefab = default;
    [SerializeField] protected AudioClip _Attack1Sound = default;
    [SerializeField] protected AudioClip _Attack2Sound = default;
    [SerializeField] protected AudioClip _Attack3Sound = default;
    [SerializeField] protected AudioClip _Attack4Sound = default;
    [SerializeField] protected AudioClip _DeathSound = default;
    private SpawnManager _spawnManager;
    private float _canAttack1 = -1f;
    private float _canAttack2 = -1f;
    private float _canAttack3 = -1f;
    private float _canAttack4 = -1f;

    private float _CDAttack1 = 5f;
    private float _CDAttack2 = 10f;
    private float _CDAttack3 = 15f;
    private float _CDAttack4 = 20f;
    private float _initialAttackRate;
    private int _healthMax;
    private float _speedMax;

    private bool _isAttackSpeedBuffed = false;
    private GameObject _shield;
    private Animator _animator;



    void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _initialAttackRate= _AttackRate;
        _shield = transform.GetChild(0).gameObject;
        _animator = GetComponent<Animator>();
        _healthMax = _Health * _Constitution;
    }
    // Update is called once per frame
    void Update()
    {
        MouvementsJoueur();
        Attack1();
        Attack2();
        Attack3();
        Attack4();
    }

    protected void Attack1()
    {
        if(Input.GetKey("Attack1") && Time.time > _canAttack1)
        {
            _canAttack1 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }

    protected void Attack2()
    {
        if(Input.GetKey("Attack2") && Time.time > _canAttack2)
        {
            _canAttack2 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }
    protected void Attack3()
    {
        if(Input.GetKey("Attack3") && Time.time > _canAttack3)
        {
            _canAttack3 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }
    protected void Attack4()
    {
        if(Input.GetKey("Attack4") && Time.time > _canAttack4)
        {
         // Calcul CD
         // _canAttack4 = Time.time + (_CDAttack4-(_Intelligence*0.02f);
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }
    
    

    protected void MouvementsJoueur()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float VertInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizInput, VertInput, 0f);
        
        //Gestion des animations
        if (horizInput > 0)
        {
            _animator.SetBool("TurnRight", true);
            _animator.SetBool("TurnLeft", false);
        }
        else if (horizInput < 0)
        {
            _animator.SetBool("TurnRight", false);
            _animator.SetBool("TurnLeft", true);
        }
        else
        {
            _animator.SetBool("TurnLeft", false);
            _animator.SetBool("TurnRight", false);
        }
        
        transform.Translate(direction * Time.deltaTime * _Speed);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 4.5f), 0f);
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0f);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0f);
        }
    }

    //Methode public
    public void Damage()
    {
        if(_shield.activeSelf==true){
            _shield.SetActive(false);
        }
        else{
            --_Health;
            UIManager _uiManager = FindObjectOfType<UIManager>();
            _uiManager.ChangeLivesDisplayImage(_Health);
        }
        if (_Health < 1)
        {
            Instantiate(_DeathPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
           
        }
    }

}
