using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Manages the UI for creating rules.
/// </summary>
public class RuleCreationUIManager : MonoBehaviour
{
    public CanvasManager canvasManager;
    public UIRuleSelector uIRuleSelector;
    public GameObject ruleUIPrefab;
    public GameObject currentRulesUIParent;
    public TMP_Dropdown shapeDropdown;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown whichRuleDropdown;
    public TMP_InputField minInputField;
    public TMP_InputField maxInputField;
    public TMP_Dropdown positioningDropdown;
    public TMP_Dropdown otherShapeDropdown;
    public TMP_Dropdown otherColorDropdown;
    public Button addRuleButton;
    [SerializeField]
    private List<ShapeRule> rules = new List<ShapeRule>();

    // TODO: Add method to update UI showing current rules

    private void Start() {
        //Populate the dropdowns with the enum values

        shapeDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(ShapeType))));
        //Set the last element to All Shapes
        shapeDropdown.options[shapeDropdown.options.Count - 1].text = "All Shapes";
        colorDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(ColorType))));
        colorDropdown.options[colorDropdown.options.Count - 1].text = "All Colors";
        positioningDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(Direction))));
        otherShapeDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(ShapeType))));
        otherShapeDropdown.options[otherShapeDropdown.options.Count - 1].text = "All Shapes";
        otherColorDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(ColorType))));
        otherColorDropdown.options[otherColorDropdown.options.Count - 1].text = "All Colors";
        //Set the kind of rule to minimum amount by default to update UI accordingly
        uIRuleSelector.ChangeRuleElements(0);
    }

    public void AddRule(){
        //call ShapeRuleCreator.CreateShapeRule with the values from the UI
        ShapeRule newRule = ShapeRuleCreator.CreateShapeRule(
            (ShapeType)shapeDropdown.value,
            (ColorType)colorDropdown.value,
            whichRuleDropdown.value == 0 || whichRuleDropdown.value == 1 ? true : false, //Amount Rule
             whichRuleDropdown.value == 0 ? true : false, //MoreThan Rule
            whichRuleDropdown.value == 2 ? true : false,
            (Direction)positioningDropdown.value,
            (ShapeType)otherShapeDropdown.value,
            (ColorType)otherColorDropdown.value
        );
        if(ShapeRuleCreator.IsRuleValid(newRule) == false){
            Debug.LogError("Rule is not valid");
            return;
        }
        rules.Add(newRule);
        //create a new rule UI element
        GameObject newRuleUI = Instantiate(ruleUIPrefab, currentRulesUIParent.transform);
        //set the rule of the rule UI element
        newRuleUI.GetComponent<UIRule>().SetRule(newRule);
        //Subscribe to the delete event
        newRuleUI.GetComponent<UIRule>().OnRuleDeleted.AddListener(() => RemoveRule(newRule));

    }

    public void CreateUIForLoadedRules(ShapeRule[] loadedRules){
        foreach(ShapeRule rule in loadedRules){
            rules.Add(rule);
            GameObject newRuleUI = Instantiate(ruleUIPrefab, currentRulesUIParent.transform);
            newRuleUI.GetComponent<UIRule>().SetRule(rule);
            newRuleUI.GetComponent<UIRule>().OnRuleDeleted.AddListener(() => RemoveRule(rule));
        }
    }

    public void ExportRules(){
        canvasManager.SetNewRules(rules.ToArray());
    }
 

    public void RemoveRule(ShapeRule rule){
        rules.Remove(rule);
    }

    private int TryParseInputField(TMP_InputField inputField){
        int result = 0;
        if(int.TryParse(inputField.text, out result) == false){
            result = 0;
        }
        return result;
    }
}

