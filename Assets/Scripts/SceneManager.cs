using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    static SceneManager instance = null;

	public GameObject enemyPrefab;

    public List<Ally> allies = new List<Ally>();
    public List<Enemy> enemies = new List<Enemy>();

    public GameObject Dealer;
    public GameObject Healer;
    Dealer dealer;
    Healer healer;

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
            Enemy newEnemy = Instantiate(enemyPrefab, GetRightEnemyPosition(i), Quaternion.identity).GetComponent<Enemy>();
            newEnemy.order = i;
            enemies.Add(newEnemy);
		}
        dealer = Dealer.GetComponent<Dealer>();
        healer = Healer.GetComponent<Healer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetRightEnemyPosition(int order)
    {
        return new Vector3(order * 1.44f + 2, 0);
    }

    public void EnemyDied(Enemy enemy)
    {
        SceneManager.Instance.enemies.Remove(enemy);
        RearrangeEnemies();
    }

    public void RearrangeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];
            if (enemy.order != i)
            {
                enemy.MoveToPosition(GetRightEnemyPosition(i));
                enemy.order = i;
            }
        }
    }

    public void SkillButton ()
    {
        healer.CallSkill();
    }
}
