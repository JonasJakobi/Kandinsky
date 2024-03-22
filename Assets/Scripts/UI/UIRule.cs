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
        //verbosity of 4 is the full explanation of the rule 
        text.text = RuleExplanations.GetExplanation(rule, 4);
    }
    //Delete when button pressed
    public void DeleteRule(){
        OnRuleDeleted.Invoke();
        Destroy(gameObject);
    }

}
