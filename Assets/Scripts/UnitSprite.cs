using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSprite : MonoBehaviour
{
    Unit unit;
    private void Start()
    {
        unit = gameObject.GetComponentInParent<Unit>();
    }
    void OnMouseDown()
    {
        Debug.Log(unit.GetType().ToString() + " MouseDown detected");
        SceneManager.Instance.OnClickUnit(unit);
    }
}
