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
    DealTank dealTank;
    Tanker tanker;
    Supporter supporter;

    Unit firstClicked;

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
            newEnemy.Init(Enemy.EnemyType.skeleton);
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

    public void UnitDied(Unit unit)
    {
        if (unit is Enemy)
            SceneManager.Instance.enemies.Remove((Enemy)unit);
        else if (unit is Ally)
        {
            SceneManager.Instance.allies.Remove((Ally)unit);
            SyncPosition();
        }
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

    public void OnClickUnit(Unit unit)
    {
        if(firstClicked == null)
        {
            if(unit is Ally)
            {
                firstClicked = unit;
            }
            else
            {
                for (int i = 0; i < allies.Count; i++)
                {
                    allies[i].SetDesignatedTarget(unit);
                }
            }
        }
        else
        {
            if(unit is Ally)
            {
                SwapAllyIndex(firstClicked.order, unit.order);
                //ShiftAllyIndex(firstClicked.order, unit.order);
            }
            else
            {
                firstClicked.SetDesignatedTarget(unit);
            }
            firstClicked = null;
        }
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
    // Swap allies at index I1 and I2
    public void SwapAllyIndex (int i1, int i2)
    {
        /*Ally temp = allies[i1];
        allies[i1] = allies[i2];
        allies[i2] = temp;

        allies[i1].MoveToPosition(positions[i1]);
        allies[i2].MoveToPosition(positions[i2]);*/
        if (i1 == i2)
            return;
        Ally temp = allies[i1];
        MoveToIndex(allies[i2], i1);
        MoveToIndex(temp, i2);
    }
    // Move ally at index SRC to index DST
    // Shift all allies between SRC and DST
    public void ShiftAllyIndex (int src, int dst)
    {
        if (src < 0 || dst < 0 || src >= allies.Count || dst >= allies.Count)
        {
            Debug.Log("Error at ShiftAllyIndex: out of bounds");
        }
        Ally temp = allies[src];
        if (src == dst)
            return;
        else if (src > dst)
        {
            for (int i = src - 1; i >= dst; i--)
            {
                MoveToIndex(allies[i], i + 1);
            }
        }
        else
        {
            for (int i = src + 1; i <= dst; i++)
            {
                MoveToIndex(allies[i], i - 1);
            }
        }
        MoveToIndex(temp, dst);
    }
    // Move ally to INDEX
    // Change both index and order
    private void MoveToIndex (Ally t, int index)
    {
        allies[index] = t;
        t.MoveToPosition(positions[index]);
        t.order = index;
    }
    // Find ally with its order
    // Not used, but leave it for the case that order and index are not matched
    private Ally AllyAtOrder (int order)
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].order == order)
                return allies[i];
        }
        return null;
    }
    // Move allies to match order and actual index
    private void SyncPosition ()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].order == i)
                continue;
            allies[i].order = i;
            allies[i].MoveToPosition(positions[i]);
        }
    }
}
