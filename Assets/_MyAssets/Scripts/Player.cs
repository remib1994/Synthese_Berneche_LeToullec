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
    private float _canAttack = -1f;
    private float _initialAttackRate;
    private int _healthMax;
    private float _speedMax;

    //private bool _isAttackSpeedBuffed = false;
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
    }

    protected void Attack1()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > _canAttack)
        {
            _canAttack = Time.time + _AttackRate;
            //Jouer le son de l'attaque
            AudioSource.PlayClipAtPoint(_Attack1Sound, Camera.main.transform.position, 0.3f);
            Instantiate(_Attack1Prefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
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
