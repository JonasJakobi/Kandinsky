using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.cyborgAssets.inspectorButtonPro;
using System.Linq;

/// <summary>
/// Main First Screen of the application that shows all possible rules.
/// </summary>
public class AllRulesViewUI : MonoBehaviour{
   


    [Header("References")]
    [SerializeField] GameObject uiParent;
    [SerializeField] GameObject ruleUIPrefab;
    private List<GameObject> rules = new List<GameObject>();


    public void SetRules(ShapeRule[] rules){
        foreach(ShapeRule rule in rules){
            AddRule(rule);
        }
    }
    public void ClearRules(){
        foreach(GameObject rule in rules){
            Destroy(rule);
        }
        rules.Clear();
    }
    [ProButton]
    public void HideRuleButtonsAndPlus(){
        foreach(GameObject rule in rules){
            rule.GetComponent<UIRule>().HideButtons();
        }
        uiParent.transform.GetChild(uiParent.transform.childCount - 1).gameObject.SetActive(false);
    }
    public void AddRule(ShapeRule rule){
        GameObject ruleUI = Instantiate(ruleUIPrefab, uiParent.transform);
        ruleUI.transform.SetParent(uiParent.transform, false);
        ruleUI.transform.SetSiblingIndex(uiParent.transform.childCount - 2); //ensure that the + button is always at the bottom
        ruleUI.GetComponent<UIRule>().SetRule(rule);
        ruleUI.GetComponent<UIRule>().OnRuleDeleted.AddListener(() => RemoveRule(rule));
        rules.Add(ruleUI);
    }

    public void RemoveRule(ShapeRule rule){
        rules.Remove(rules.First(x => x.GetComponent<UIRule>().rule == rule));
    }

    public List<ShapeRule> GetRules(){
        return rules.Select(x => x.GetComponent<UIRule>().rule).ToList();
    }

    public void DisableAllBut(ShapeRule[] rulesToStay){
        foreach(GameObject rule in rules){
            if(!rulesToStay.Contains(rule.GetComponent<UIRule>().rule)){
                rule.GetComponent<UIRule>().Disable();
            }
        }
    }

    private void OnEnable() {

        ShapeRule[] rules = SavingManager.Instance.CreateRulesFromFileContent(Resources.Load<TextAsset>("realall").text);
        SetRules(rules);
    }
    
}