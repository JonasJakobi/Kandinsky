using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
public class UIRule : MonoBehaviour
{
    [Header("Colors & Images")]
    [SerializeField] Color defaultColor;
    [SerializeField] Color disabledColor;


    [SerializeField] Sprite triangleSprite;
    [SerializeField] Sprite squareSprite;
    [SerializeField] Sprite circleSprite;

    [SerializeField] Sprite redObject;
    [SerializeField] Sprite greenObject;
    [SerializeField] Sprite blueObject;

    [SerializeField] Sprite upArrow;
    [SerializeField] Sprite downArrow;
    [SerializeField] Sprite leftArrow;
    [SerializeField] Sprite rightArrow;
    [SerializeField] Sprite moreThan;
    [SerializeField] Sprite lessThan;
    [Header("Internal State")]
    public ShapeRule rule;
    [SerializeField] bool showControlButtons = true;
    [SerializeField] bool isEnabled = true;

    [Header("References")]
    [SerializeField] Button deleteButton;
    [SerializeField] Button editButton;
    [SerializeField] TextMeshProUGUI text;
    public UnityEvent OnRuleDeleted = new UnityEvent();
    [SerializeField] Image leftObjectImage;
    [SerializeField] Image rightObjectImage;
    [SerializeField] Image relationImage;




    public void SetRule(ShapeRule rule){
        //verbosity of 4 is the full explanation of the rule 
        this.rule = rule;
        text.text = RuleExplanations.GetExplanation(rule, 4);
        leftObjectImage.sprite = GetShapeSprite(rule.shapeType, rule.color);
        
        rightObjectImage.sprite = GetShapeSprite(rule.shapeToPositionTo, rule.colorToPositionTo);
        relationImage.sprite = GetRelationSprite(rule);

    }
    //Delete when button pressed
    public void DeleteRule(){
        OnRuleDeleted.Invoke();
        Destroy(gameObject);
    }

    private Sprite GetShapeSprite(ShapeType shapeType, ColorType colorType){
        if(shapeType.Equals(ShapeType.AllShapes)){
            switch(colorType){
                case ColorType.Red:
                    return redObject;
                case ColorType.Green:
                    return greenObject;
                case ColorType.Blue:
                    return blueObject;
                case ColorType.AllColors:
                    return redObject;
            }
        }
        else{
            switch(shapeType){
                case ShapeType.Triangle:
                    return triangleSprite;
                case ShapeType.Square:
                    return squareSprite;
                case ShapeType.Circle:
                    return circleSprite;
            }
        }
        Debug.LogError("No sprite found for shape " + shapeType + " and color " + colorType);
        return null;
    }
    private Sprite GetRelationSprite(ShapeRule rule){
        if(rule.amountRules){
            if(rule.moreThan){
                return moreThan;
            }
            else{
                return lessThan;
            }
        }
        else{
            switch(rule.direction){
                case Direction.Above:
                    return upArrow;
                case Direction.Below:
                    return downArrow;
                case Direction.Left:
                    return leftArrow;
                case Direction.Right:
                    return rightArrow;
            }
        }
        Debug.LogError("No relation sprite found for rule " + rule);
        return null;
    }

    public void HideButtons(){
        GetComponent<Button>().interactable = false;
        deleteButton.gameObject.SetActive(false);
        editButton.gameObject.SetActive(false);
    }

    public void SetIsEnabled(bool isEnabled){
        this.isEnabled = isEnabled;
        if(isEnabled){
            text.color = defaultColor;
        }
        else{
            text.color = disabledColor;
        }
        //if we dont show the buttons at all, no need to disable them
        if(showControlButtons){
            deleteButton.interactable = isEnabled;
            editButton.interactable = isEnabled;
        }
    }

    public void SetRuleToCanvasManager(){
        FindObjectOfType<CanvasManager>().SetNewRules(new ShapeRule[]{rule});
    }
    public void Disable(){
        GetComponent<Image>().color = disabledColor;
    }

}
