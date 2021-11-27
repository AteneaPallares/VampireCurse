using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image _life;
    [SerializeField]
    private Text _score;
    [SerializeField]
    private Text _health;
    [SerializeField]
    private Text _message;
    private float _showTime=3f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_showTime < 0)
        {
            _message.text = "";
        }
        _showTime -= Time.deltaTime;

    }

    public void UpdateLifeBar(float life)
    {
        _life.fillAmount = life / 100;
        _health.text = (life).ToString();
    }

    public void UpdateScore(float score)
    {
        _score.text = score.ToString();
        
    }

    public void ShowMessage(string message)
    {
        _message.text = message;
        _showTime = 3f;
    }

}
