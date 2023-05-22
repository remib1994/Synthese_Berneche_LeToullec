using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Player _player;
 
    [SerializeField] private string _nom = default;
    private int _damage;

    private void Awake()
    {
        //_uiManager = FindObjectOfType<UIManager>();
    }
    void Start()
    {
        _damage = _player.Strength * 1;
        StartCoroutine(DestroyThrust());
    }
    
    IEnumerator DestroyThrust()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && _nom == "Player")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            //enemy.Damage(_damage);
        }
    }
}
