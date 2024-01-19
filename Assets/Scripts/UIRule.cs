using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
public class UIRule : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    TextMeshProUGUI text;
    public UnityEvent OnRuleDeleted = new UnityEvent();


    public void SetRule(ShapeRule rule){
        text.text = RuleExplanations.GetExplanation(rule, 5);
    }
    //Delete when button pressed
    public void DeleteRule(){
        OnRuleDeleted.Invoke();
        Destroy(gameObject);
    }

}
