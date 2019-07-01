using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    static SceneManager instance = null;

    public GameObject Dealer;

    public List<Enemy> enemies;

    // Start is called before the first frame update
    public static SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void skillButton ()
    {
        Dealer.GetComponent<Dealer>().CallSkill();
    }
}
