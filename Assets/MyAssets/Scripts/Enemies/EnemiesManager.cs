using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyBases;
    [SerializeField]
    private GameObject _enemy;
    private UIController _uIController;
    private float _enemies=0;
    private float _round = 0;
    private float _enemiesHealth = 15;
    private static EnemiesManager _instance;
    private float _waitTime = 5f;
    private AudioManager _audioManager;
    private float _waiting = 5f;
    private bool _creating = false;
    private int _x = -1;
    private int _i = 2;



    private void Awake()
    {
        if (_instance != null && _instance != this) 
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _uIController = GameObject.Find("UI").GetComponent<UIController>();
        _uIController.ShowMessage("Matar o Morir");
        _audioManager = GameObject.FindObjectOfType<AudioManager>();

    }


    // Update is called once per frame
    void Update()
    {
        
        if (_enemies == 0 && _waitTime<=0 && _creating==false)
        {
            _enemiesHealth += 5;
            _round++;
            _uIController.ShowMessage("Ronda " + _round);
            _audioManager.PlayAudio("Round");
            _creating = true;
            _x = _enemyBases.Length-1;
        }
        else
        {

            if (_x >= 0)
            {
                if (_i > 0)
                {
                    if (_waiting <= 0)
                    {
                        CreateEnemy(_enemyBases[_x]);
                        _waiting = 5f;
                        _i--;
                    }
                }
                if (_i == 0)
                {
                    _i = 2;
                    _x--;
                }
            }
            else
            {
                _creating = false;
            }
        }
        _waitTime -= Time.deltaTime;
        _waiting -= Time.deltaTime;
    }

    public void UpdateEnemies()
    {
        _enemies--;
        _waitTime = 5f;
    }
    void CreateEnemy(GameObject baseLocation)
    {
        GameObject tempEnemy = Instantiate(_enemy, baseLocation.transform.position, _enemy.transform.rotation) as GameObject;
        tempEnemy.GetComponent<EnemyController>().setHealth(_enemiesHealth);
        tempEnemy.GetComponent<EnemyController>().SetRound(_round);
        _enemies++;
    }
}
