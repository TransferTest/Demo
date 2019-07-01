using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    static SceneManager instance = null;

    public GameObject Dealer;
	public GameObject enemyPrefab;

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
		for (int i = 0; i < 4; i++)
		{
            Instantiate(enemyPrefab, new Vector3(i * 1.44f + 2, 0), Quaternion.identity);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillButton ()
    {
        Dealer.GetComponent<Dealer>().CallSkill();
    }
}
