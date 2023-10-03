using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RuleExplanations : MonoBehaviour
{

    [SerializeField]
    GameObject[] textboxes;


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
    public static string[] GetExplanations(ShapeRule[] rules, int verbosity)
    {
        
        List<string> answers = new();
        if(verbosity == 0)
        {
            return answers.ToArray();
        }
       foreach(var rule in rules){
           
            if (rule.positionRules)
            {
                answers.Add(GeneratePositionRuleExplanation(verbosity, rule.shapeType, rule.color, rule.direction, rule.shapeToPositionTo, rule.colorToPositionTo).TrimEnd() + ".");
            }
            if (rule.minAmountRule)
            {
                answers.Add(GenerateAmountRuleExplanation(verbosity, rule.shapeType, rule.color, rule.minCount, true).TrimEnd() + ".");
            }
            if (rule.maxAmountRule)
            {
                answers.Add(GenerateAmountRuleExplanation(verbosity, rule.shapeType, rule.color, rule.maxCount, false).TrimEnd() + ".");
            }
        }
        return answers.ToArray();
    }
    private static string GenerateAmountRuleExplanation(int verbosity, ShapeType type, ColorType color, int minAmount, bool min)
    {
        string answer = "The rule applies to the ";
        if(verbosity == 1)
        {
            return answer + "amount of certain shapes";
        }
        else if(verbosity == 2)
        {
            return answer + (min ? "minimum": "maximum") +  "  amount of certain shapes";
        }
        else if(verbosity == 3 || verbosity == 4)
        {
            return answer + (min ? "minimum" : "maximum") + " amount of " + getKandinskyShapeString(type, color);
        }
        //Full Explanation? f.e. "there has to be a minimum of 3 red squares"
        else if (verbosity == 5){
            return (min ? "minimum" : "maximum") + " amount of " + getKandinskyShapeString(type, color) + "is " + minAmount.ToString();
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
            return answer + "certain shapes";
        }
        else if(verbosity == 2)
        {
            return answer + "certain shapes in relation to certain other shapes";
        }
        else if(verbosity == 3)
        {
            return answer + getKandinskyShapeString(shape, color) + " in relation to certain other shapes";
        }
        else if(verbosity == 4)
        {
            return answer + getKandinskyShapeString(shape, color) + " in relation to " + getKandinskyShapeString(shapeToPositionTo, colorToPositionTo) + "";
        }
        //verbosity 5 which is full explanation? f.e. "Red squares have to be above blue triangles."
        else if (verbosity == 5)
        {
            return getKandinskyShapeString(shape, color) + "are" + getDirectionString(direction) + getKandinskyShapeString(shapeToPositionTo, colorToPositionTo);
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
            answer += "Shapes";
        }
        else
        {
            answer += shape.ToString() + "s "; // add s for plural.
        }
        //for any / all colors we define at the end.
        if (color.Equals(ColorType.AllColors))
        {
            answer += "of any color ";
        }

        return answer;
    }
    private static string getDirectionString(Direction direction)
    {
        switch (direction)
        {
            case Direction.Above:
                return "above";
            case Direction.Below:
                return "below";
            case Direction.Left:
                return "to the left of";
            case Direction.Right:
                return "to the right of";
            default:
                return "ERROR";
        }
    }
}
