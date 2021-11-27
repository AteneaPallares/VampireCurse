using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private static GameOverUI _instance;
    private PlayerController _player;
    [SerializeField]
    private Text _score;
    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        _player = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
        _score.text = "Juego Terminado\nPuntos totales: " + _player.score;
        _player.score = 0;
    }

    // Update is called once per frame
    void Update()
        
    {
        //_player.score = 0;
    }
}
