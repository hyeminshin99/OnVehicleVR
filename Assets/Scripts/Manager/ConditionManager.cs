using UnityEngine;
using KUsystem.Utils;

public class ConditionManager : MonoBehaviour
{
    public enum Condition
    {
        Condition1,
        Condition2,
        Condition3
    }

    public Condition currentCondition;
    public GameObject AObject;

    void Update()
    {
        // 조건에 따른 동작
        switch (currentCondition)
        {
            case Condition.Condition1:
                HandleCondition1();
                break;
            case Condition.Condition2:
                HandleCondition2();
                break;
            case Condition.Condition3:
                HandleCondition3();
                break;
        }
    }

    void HandleCondition1()
    {

        // 추가 동작: AObject 비활성화
        if (AObject != null)
        {
            AObject.SetActive(false);
        }
    }

    void HandleCondition2()
    {

        // 추가 동작: AObject 활성화
        if (AObject != null)
        {
            AObject.SetActive(true);
        }
    }

    void HandleCondition3()
    {
        Debug.Log("Condition3 동작");
    }
}
