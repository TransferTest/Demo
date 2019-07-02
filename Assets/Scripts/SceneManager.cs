using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyClass { Dealer, Healer, DealTank, Tanker, Supporter }


public class SceneManager : MonoBehaviour
{
    static SceneManager instance = null;

	public GameObject enemyPrefab;

    public List<Ally> allies = new List<Ally>();
    public Vector3[] positions = new Vector3[5];
    public List<Enemy> enemies = new List<Enemy>();

    public GameObject Dealer;
    public GameObject Healer;
    public GameObject DealTank;
    public GameObject Tanker;
    public GameObject Supporter;
    Dealer dealer;
    Healer healer;
    DealTank dealTank; //각 직업별 클래스 만든 후 Ally type을 수정 필요
    Tanker tanker;
    Supporter supporter;

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
        dealTank = DealTank.GetComponent<DealTank>();
        tanker = Tanker.GetComponent<Tanker>();
        supporter = Supporter.GetComponent<Supporter>();
        allies.Add(dealer);
        allies.Add(healer);
        allies.Add(dealTank);
        allies.Add(tanker);
        allies.Add(supporter);

        //SwapAllyIndex(0, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3 GetRightEnemyPosition(int order)
    {
        return new Vector3(order * 1.44f + 2, -2.32f);
    }

    public void EnemyDied(Enemy enemy)
    {
        SceneManager.Instance.enemies.Remove(enemy);
        //RearrangeEnemies();
    }

    /*public void RearrangeEnemies()
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
    }*/

    public void SkillButton (string className)
    {
        Ally ally = GetAlly(className);
        if (ally != null)
            ally.CallSkill();
    }

    public Ally GetAlly(AllyClass class_)
    {
        switch (class_)
        {
            case AllyClass.Dealer:
                return dealer;
            case AllyClass.Healer:
                return healer;
            case AllyClass.DealTank:
                return dealTank;
            case AllyClass.Tanker:
                return tanker;
            case AllyClass.Supporter:
                return supporter;
            default:
                return null;
        }
    }
    public Ally GetAlly(string className)
    {
        return GetAlly(StringToAllyClass(className));
    }
    public static AllyClass StringToAllyClass(string str)
    {
        return (AllyClass)Enum.Parse(typeof(AllyClass), str);
    }
    // Move all allies to the right position
    // Sync actual position with allies list 
    public void MoveAll ()
    {
        Debug.Log(allies.Count);
        for(int i = 0; i < allies.Count; i++)
        {
            allies[i].MoveToPosition(positions[i]);
        }
    }

    public void SwapAllyIndex (int i1, int i2)
    {
        Ally temp = allies[i1];
        allies[i1] = allies[i2];
        allies[i2] = temp;

        allies[i1].MoveToPosition(positions[i1]);
        allies[i2].MoveToPosition(positions[i2]);
    }
}
