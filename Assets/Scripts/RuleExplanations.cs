using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Generates and displays explanations for the rules.
/// </summary>
public class RuleExplanations : MonoBehaviour
{

    [SerializeField]
    GameObject[] textboxes;
    [Header("UI Configurations")]
    [SerializeField]
    private int hintTextSize = 30;
    [SerializeField]
    private int explanationTextSize = 20;
    

    public void PrintExplanations(ShapeRule[] rules, int verbosity)
    {
        string[] explanations = GetExplanations(rules, verbosity);
        for(int i = 0; i < textboxes.Length; i++)
        {
            if(i < explanations.Length)
            {
                print(explanations[i]);
                textboxes[i].GetComponent<TextMeshProUGUI>().text = explanations[i];
            }
            else
            {
                textboxes[i].GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        
    }
    ///
    /// <summary>
    /// Prints the selected image to the first textbox.
    /// </summary>
    public void PrintSelectedImage(int indexOfImage){
        //convert indexOfImage (0-4) to a,b,c,d,e
        char letter = (char)(indexOfImage + 99);
        //set the text of the first 3 textboxes to nothing
        for(int i = 0; i < 4; i++)
        {
            textboxes[i].GetComponent<TextMeshProUGUI>().text = "";
        }
        textboxes[2].GetComponent<TextMeshProUGUI>().text = "The AI recommends: " + letter + ")";

    }
    /// <summary>
    /// Updates the UI to show the hint. 
    /// </summary>
    public void UpdateUIForHint(){
        textboxes[2].GetComponent<TextMeshProUGUI>().fontSize = hintTextSize;
        textboxes[2].GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
    }
    /// <summary>
    /// Updates the UI to show the explanations. 
    /// </summary>
    public void UpdateUIForExplanations(){
        foreach(GameObject textbox in textboxes){
            textbox.GetComponent<TextMeshProUGUI>().fontSize = explanationTextSize;
            textbox.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
        }
    }
    public static string[] GetExplanations(ShapeRule[] rules, int verbosity)
    {
        
        List<string> answers = new();
        if(verbosity == 0)
        {
            return answers.ToArray();
        }
       foreach(var rule in rules){
            if (rule.IsASecondaryRule)
            {
                continue;
            }
           answers.Add(GetExplanation(rule, verbosity));
            
        }
        return answers.ToArray();
    }

    public static string GetExplanation(ShapeRule rule, int verbosity){
        String answer = "";
        if (rule.positionRules)
            {
                answer= (GeneratePositionRuleExplanation(verbosity, rule.shapeType, rule.color, rule.direction, rule.shapeToPositionTo, rule.colorToPositionTo).TrimEnd() + ".");
            }
        if (rule.minAmountRule)
            {
                answer = (GenerateAmountRuleExplanation(verbosity, rule.shapeType, rule.color, rule.minCount, true).TrimEnd() + ".");
            }
        if (rule.maxAmountRule)
            {
                answer = (GenerateAmountRuleExplanation(verbosity, rule.shapeType, rule.color, rule.maxCount, false).TrimEnd() + ".");
            }
        //Capialize first letter
        answer = char.ToUpper(answer[0]) + answer.Substring(1);
        return answer;

    }
    private static string GenerateAmountRuleExplanation(int verbosity, ShapeType type, ColorType color, int minAmount, bool min)
    {
        string answer = "The rule applies to the ";
        if(verbosity == 1)
        {
            return answer + "amount of certain objects";
        }
        else if(verbosity == 2)
        {
            return answer + (min ? "minimum": "maximum") +  "  amount of certain objects";
        }
        else if(verbosity == 3 || verbosity == 4)
        {
            return answer + (min ? "minimum" : "maximum") + " amount of " + getKandinskyShapeString(type, color);
        }
        //Full Explanation? f.e. "there has to be a minimum of 3 red squares"
        else if (verbosity == 5){
            return (min ? "minimum" : "maximum") + " amount of " + getKandinskyShapeString(type, color) + " is " + minAmount.ToString();
        }
        else{
            return "---- INVALID VERBOSITY, there seems to be some error. ------";
        }
        
    }


    
    private static string GeneratePositionRuleExplanation(int verbosity, ShapeType shape, ColorType color, Direction direction, ShapeType shapeToPositionTo, ColorType colorToPositionTo )
    {
        string answer = "The rule applies to the position of ";

        if (verbosity == 1)
        {
            return answer + "certain objects";
        }
        else if(verbosity == 2)
        {
            return answer + "certain objects in relation to certain other objects";
        }
        else if(verbosity == 3)
        {
            return answer + getKandinskyShapeString(shape, color) + "in relation to certain other objects";
        }
        else if(verbosity == 4)
        {
            return answer + getKandinskyShapeString(shape, color) + "in relation to " + getKandinskyShapeString(shapeToPositionTo, colorToPositionTo) + "";
        }
        //verbosity 5 which is full explanation? f.e. "Red squares have to be above blue triangles."
        else if (verbosity == 5)
        {
            return getKandinskyShapeString(shape, color) + "are " + getDirectionString(direction) + getKandinskyShapeString(shapeToPositionTo, colorToPositionTo);
        }
        else
        return "---- INVALID VERBOSITY, there seems to be some error. ------";
    }

    /// <summary>
    /// Returns the string specifying a shape and color. 
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private static string getKandinskyShapeString(ShapeType shape, ColorType color)
    {
        string answer = "";
        //If it is a specific color we define it before the shape
        if (!color.Equals(ColorType.AllColors))
        { 
            answer = color.ToString().ToLower() + " ";
        }
        //then shape
        if (shape.Equals(ShapeType.AllShapes))
        {
            answer += "objects ";
        }
        else
        {
            answer += shape.ToString() + "s "; // add s for plural.
        }
        //for any / all colors we define at the end.
        if (color.Equals(ColorType.AllColors))
        {
            answer += "of any color";
        }

        return answer;
    }
    private static string getDirectionString(Direction direction)
    {
        switch (direction)
        {
            case Direction.Above:
                return "above ";
            case Direction.Below:
                return "below ";
            case Direction.Left:
                return "to the left of ";
            case Direction.Right:
                return "to the right of ";
            default:
                return "ERROR";
        }
    }
}
