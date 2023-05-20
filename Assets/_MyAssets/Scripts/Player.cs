using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    //Stats
    [SerializeField] protected Character4D _character;
    [SerializeField] protected AnimationManager _animation;
    [SerializeField] protected int _strength = 6;
    [SerializeField] protected int _Dexterity = 6;
    [SerializeField] protected int _Intelligence = 6;


    [SerializeField] protected float _Speed = 10;
    [SerializeField] protected int _health;
    [SerializeField] protected BarreDeVie _barreDeVie;

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
    private Animator _shieldAnimator;




    void Start()
    {
        _healthMax = _strength * 10;
        _health = _strength * 10;
        _character.SetDirection(Vector2.down);
        _animation.SetState(CharacterState.Idle);
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _initialAttackRate = _AttackRate;

        _shield = transform.GetChild(0).gameObject;
        _shieldAnimator = _shield.GetComponent<Animator>();
        _barreDeVie.SetMaxHealth(_healthMax);

    }

    // Update is called once per frame
    void Update()
    {
        MouvementsJoueur();
        Attack1();
        Attack2();
        Attack3();
        Attack4();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }
    // CODE POUR FAIRE PERDRE DE LA VIE
    void TakeDamage(int damage)
    {
        _health -= damage;

        _barreDeVie.SetHealth(_health);
    }
    // CODE POUR FAIRE PERDRE DE LA VIE

    protected void Attack1()
    {
        if (Input.GetButton("Attack1") && Time.time > _canAttack1)
        {
            _animation.Attack();
            _canAttack1 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }

    protected void Attack2()
    {
        if (Input.GetButton("Attack2") && Time.time > _canAttack2)
        {
            _animation.Jab();
            _canAttack2 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }
    protected void Attack3()
    {
        if (Input.GetButton("Attack3") && Time.time > _canAttack3)
        {
            _canAttack3 = Time.time + _AttackRate;
            Instantiate(_Attack1Prefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
        }
    }
    protected void Attack4()
    {
        if (Input.GetButton("Attack4") && Time.time > _canAttack4)
        {
            _shield.SetActive(true);
            //AudioSource.PlayClipAtPoint(_Attack1Sound, transform.position);
            StartCoroutine(ShieldRoutine());

        }
    }

    IEnumerator ShieldRoutine()
    {
        yield return new WaitForSeconds(5f);
        _shieldAnimator.SetBool("ShieldActif", false);
        _shield.SetActive(false);
    }



    protected void MouvementsJoueur()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float VertInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizInput, VertInput, 0f);

        //Gestion des animations
        if (horizInput > 0)
        {
            _character.SetDirection(Vector2.right);
        }
        else if (horizInput < 0)
        {
            _character.SetDirection(Vector2.left);
        }
        else if (VertInput > 0)
        {
            _character.SetDirection(Vector2.up);
        }
        else if (VertInput < 0)
        {
            _character.SetDirection(Vector2.down);
        }

        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = direction * _Speed;

        if (horizInput == 0 && VertInput == 0)
        {
            _animation.SetState(CharacterState.Idle);
        }
        else
        {
            _animation.SetState(CharacterState.Run);
        }
    }

    //Methode public
    public void Damage(int damage)
    {
        if (_shield.activeSelf == true)
        {
            _shieldAnimator.SetBool("ShieldActif", false);
            _shield.SetActive(false);
        }
        else
        {
            _animation.Hit();
            _health -= damage;
            /* UIManager _uiManager = FindObjectOfType<UIManager>();
            _uiManager.ChangeLivesDisplayImage(_Health); */
        }
        if (_health < 1)
        {
            _animation.Die();
            _spawnManager.OnPlayerDeath();

        }
    }

}
