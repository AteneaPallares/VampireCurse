using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float _health;
    private float _maxHealth;
    private AudioManager _audioManager;
    private Canvas _lifeCanvas;
    private EnemiesManager _enemiesManager;
    private float _scoreByKill = 2f;
    private float _round=0;

    private GameObject _player;
    private PlayerController _playerController;
    private NavMeshAgent _enemyNav;
    private Animator _enemyAnim;
    private bool _isWalking=false;
    private float _fireRate=10f;
    private bool _killed = false;
    private bool _isAttacking = false;
    private float _batWait = .2f;
    private int _maxbat = 3;
    private int _batLeft = 3;

    private GameObject _weapon;
    [SerializeField]
    private GameObject _enemyBat;
    private GameObject _enemyBatHolder;
    //private float _enemyBatSpeed=600;

    [SerializeField]
    private Image _life;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _audioManager = GameObject.FindObjectOfType<AudioManager>();
            _enemyAnim = GetComponentInChildren<Animator>();
            _playerController = _player.GetComponent<PlayerController>();
            _enemyNav = transform.GetComponent<NavMeshAgent>();
            _weapon = transform.GetChild(0).gameObject;
            _enemyBatHolder = _weapon.transform.GetChild(0).gameObject;
            _enemiesManager = GameObject.FindObjectOfType<EnemiesManager>();
            _lifeCanvas = GetComponentInChildren<Canvas>();
            _life.fillAmount = _health / _maxHealth;

        }
        else
        {
            _killed = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null && !_killed) 
        {
            _lifeCanvas.transform.LookAt(_player.transform);
            if (_isAttacking)
            {
                if (_batWait <= 0)
                {
                    _batWait = .2f;
                    Fire();
                    _batLeft--;
                }
                _batWait -= Time.deltaTime;
                if (_batLeft == 0)
                {
                    _batWait = 1.3f;
                    _batLeft = _maxbat;
                    _isAttacking = false;
                    _enemyNav.isStopped = false;
                    
                }
                _fireRate = 10f;
            }
            else
            {
                
                _enemyNav.SetDestination(_player.transform.position);
                if (!_isWalking)
                {
                    _isWalking = true;
                    _enemyAnim.SetBool("isWalking", true);
                }

                if (_fireRate < 0)
                {
                    _enemyNav.isStopped = true;
                    _isWalking = false;
                    _isAttacking = true;
                    _enemyAnim.SetBool("isWalking", false);
                    _enemyAnim.SetTrigger("attacking");
                }
                _fireRate -= Time.deltaTime;
            }

        }
        
    }

    public void setHealth(float value)
    {
        _health = value;
        _maxHealth = value;
    }

    void Fire()
    {
        var auxpos = _enemyBatHolder.transform.position;
        auxpos.y = 0;
        GameObject tempBat = Instantiate(_enemyBat, auxpos, _enemyBatHolder.transform.rotation) as GameObject;
    }

    public void HealthUpdate(float damage)
    {
        _health -= damage;
        _life.fillAmount = _health / _maxHealth;
        if (_health <= 0 && !_killed)
        {
            _audioManager.PlayAudio("Noo");
            _enemyAnim.SetBool("isWalking", false);
            _enemyAnim.SetTrigger("killed");
            _killed = true;
            _playerController.UpdateScore(_scoreByKill * _round);
            _enemiesManager.UpdateEnemies();
            Destroy(gameObject, 2f);
        }
    }

    public void SetRound(float value)
    {
        _round = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Garlic")
        {
            Garlic aux = other.GetComponent<Garlic>();
            HealthUpdate(aux.damage);
        }
        Destroy(other.gameObject);
    }
}
