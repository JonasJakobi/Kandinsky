using System;
using System.Collections;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Use together with AllRulesViewUI to display all rules after a hint has been given.
/// </summary>
public class RulesAfterHintView : MonoBehaviour
{
    AllRulesViewUI allRulesViewUI;
    [SerializeField] private WarningText statusText;



    private void OnEnable() {
        allRulesViewUI = GetComponent<AllRulesViewUI>();
        ShapeRule[] rules = SavingManager.Instance.CreateRulesFromFileContent(Resources.Load<TextAsset>("realall").text);
        allRulesViewUI.SetRules(rules);
        allRulesViewUI.HideRuleButtonsAndPlus();
        DisableallAfterHint(rules[12], 1); //12 for positional
        

    }

    public void DisableallAfterHint(ShapeRule rule, int verbosity){
        var allAfter = RulesAmountCalculator.RulesAfterHint(rule, verbosity);
        var amount = allAfter.Length;
        statusText.SetWarningText( amount + "/36 possible rules", -1, false);
        allRulesViewUI.DisableAllBut(allAfter);

    }


}