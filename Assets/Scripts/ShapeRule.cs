using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Each instance of ShapeRule can be specified to a type or group of kadinskyShapes and then specifies several rules for that type. 
/// </summary>
[System.Serializable]
public class ShapeRule 
{
    [Header("The rule should apply to...")]
    [Tooltip("Shape type that this rule applies to")]
    /// <summary>
    /// Shape type that this rule applies to
    /// </summary>
    public ShapeType shapeType;

    [Tooltip("Color that the rule applies to ")]
    /// <summary>
    /// Color that the rule applies to
    /// </summary>
    public ColorType color;


    [Tooltip("if enabled,  this type has to be in a direction of another type.")]
    [Header("Restrict position relative to other shapes and colors?")]
    /// <summary>
    /// if enabled,  this type has to be in a direction of another type.
    /// </summary>
    public bool positionRules = true;

    public bool amountRules = false;
    public bool moreThan = false;
    [Header("Our shape should be ..... of the other.")]
    [Tooltip("The direction relative to the other that the new shape has to be. ")]
    /// <summary>
    /// The direction relative to the other that the new shape has to be. 
    /// </summary>
    public Direction direction;
    [Header("The other shape is.. ")]
    [Tooltip("The shape type to which to place to ")]
    /// <summary>
    /// The shape type to which to place to 
    /// </summary>
    public ShapeType shapeToPositionTo;
    [Tooltip("the color type to which to place to ")]
    /// <summary>
    /// the color type to which to place to 
    /// </summary>
    public ColorType colorToPositionTo;
    [Header("DONT CHANGE - Gets checked automatically for secondary position rule")]
    public bool IsASecondaryRule = false;



    /// <summary>
    /// Call first to check if rule applies to a certain shape. Returns true if it does.
    /// </summary>
    public bool AppliesTo(KadinskyShape s)
    {
       
        return Utils.AreShapesEqual(shapeType, s.shape) && Utils.AreColorsEqual(color, s.color);
        
    }
    //-----------------The two rule methods 
    // --- Explanation:--- The distinction into two different methods is neccesary.
    // An example why: if we say triangles have to be to the left of squares, the randomly generated triangles are not likely to be,
    // while f.e. all circles are valid so adding this rule will result in no squares and triangles appearing anymore. 
    //Splitting the methods ensures a shape is "locked in" and we keep randomly searching for a position to place it correctly. 
    
    /// <summary>
    /// Rules that, when unsuccessful, demand a new shape or color to be generated. 
    /// </summary>
    /// <param name="shapes">All shapes</param>
    /// <param name="newShape">The new shape to check</param>
    /// <returns></returns>
    public RulesResult checkRulesBeforeShapeAndColor(List<KadinskyShape> shapes, KadinskyShape newShape)
    {
        
        List<RulesResult> results = new List<RulesResult>();
        if (amountRules)
        {
            if (moreThan)
            {
                results.Add(CheckAmountRules(shapes, newShape, false, false));
                results.Add(CheckAmountRules(shapes, newShape, true, true));
            }
            else
            {
                results.Add(CheckAmountRules(shapes, newShape, true, false));
                results.Add(CheckAmountRules(shapes, newShape, false, true));
            }
            
        }
        //if there were no rules to check, just return already.
        if (results.Count == 0)
        {
            return RulesResult.None;
        }
        //For every rule we checked, we have to see if any one was unsuccesful or succesful.( Currently, for one rule,this is overengineered. but if more rules are added, it is necessary.)
        bool everFalse = false;
        bool everTrue = false;
        foreach (var ans in results)
        {
            if (ans.Equals(RulesResult.Mixed)) return ans; // mixed we instantly return because it is already mixed.
            if (ans.Equals(RulesResult.AllTrue)) everTrue = true;
            else if (ans.Equals(RulesResult.AllFalse)) everFalse = true;
        }

        return CreateRulesResult(everTrue, everFalse);
    }

    /// <summary>
    /// Rules, that need to be checked after a new shape and color have been generated and only effect its position.
    /// 
    /// </summary>
    /// <param name="shapes">All shapes</param>
    /// <param name="newShape">the new chape to check </param>
    /// <returns></returns>
    
    public RulesResult checkRulesAfterShapeAndColor(List<KadinskyShape> shapes, KadinskyShape newShape)
    {
        //Check all rules.
        List<RulesResult> results = new List<RulesResult>();
        
        if (positionRules)
        {
            results.Add(CheckPositionRules(shapes, newShape));
        }
        //if there were no rules to check, just return already.
        if(results.Count == 0)
        {
            return RulesResult.None;
        }
        //Get Correct Return value;
        bool everFalse = false;
        bool everTrue = false;
        foreach(var ans in results) //
        {
            if (ans.Equals(RulesResult.Mixed)) return ans; // mixed we instantly return because it is already mixed.
            else if (ans.Equals(RulesResult.AllTrue)) everTrue = true;
            else if(ans.Equals(RulesResult.AllFalse)) everFalse = true;
        }

        return CreateRulesResult(everTrue, everFalse);


    }
    //Positional Rule - applies to 'shapeType' & 'color'. it has to be placed relative to 'shapeToPositionTo' & 'colorToPositionTo' in 'direction'. 
    //--Potentielle Erkl�rungen:
    //Abstrkations level 1: "Die Regel bezieht sich auf.. die Position von bestimmten Figuren."
    //Abstraktions level 2: "Die Regel bezieht sich auf..  die relative Position von bestimmten Figuren zu bestimmten anderen Figuren."
    //Abstraktions level 3: "Die Regel bezieht sich auf.. die relative Position von Figuren der Farbe X und Form Y zu bestimmten anderen Figuren."
    //Vollst�ndige Erkl�rung: "Die Regel bezieht sich auf.. die relative Position von Figuren der Farbe X und Form Y
    //                         zu bestimmten anderen Figuren, die die Farbe A und Form B haben."
    private RulesResult CheckPositionRules(List<KadinskyShape> shapes, KadinskyShape newShape)
    {
        bool everTrue = false;
        bool everFalse = false;
        foreach(var shape in shapes)
        {
            if(!Utils.AreColorsEqual(shape.color, colorToPositionTo)) // 'shape' is not the right color
            {
                continue;
            }
            if(!Utils.AreShapesEqual(shape.shape, shapeToPositionTo))
            {
                continue; 
            }
            //only relevant shapes left, so check if the position rules apply.
            bool result = checkPositionOfOneShape(shape, newShape);
            if (result)
            {
                everTrue = true;
            }
            else
            {
                everFalse = true;
            }

        }
        return CreateRulesResult(everTrue, everFalse);

    }

    private bool checkPositionOfOneShape(KadinskyShape shape,KadinskyShape newShape)
    {
        //Check for all directions
        float padding = 1.8f;
        if (direction.Equals( Direction.Above))
        {
            if (shape.position.y + padding < newShape.position.y) return true;
        }
        else if (direction.Equals(Direction.Below))
        {
            if (shape.position.y - padding > newShape.position.y) return true;
        }
        else if (direction.Equals(Direction.Left))
        {
            if (shape.position.x - padding > newShape.position.x) return true;
        }
        else if (direction.Equals(Direction.Right))
        {
            if (shape.position.x + padding < newShape.position.x) return true;
        }
        return false;
      
        
        
    }
    /// <summary>
    /// Will check if the amount of shapes of a certain type and color is lower then maximum. 
    /// because we want to be able to generate images that do NOT follow the rules, this rule has to be looked at even for shapes that are not of the type specified for this rule. 
    /// Otherwise, the "randomness" will always pick other shapes and colors as our shape and color f.e. "needs to be less then 4", which when it isnt yet, is always true so never false so 
    /// its impossible to generate more, leading to a "dead end".  
    /// </summary>
    /// <param name="shapes"></param>
    /// <param name="newShape"></param>
    /// <param name="maxAmount">true - we are checking for max amount right now</param>
    /// <returns></returns>
    public RulesResult CheckAmountRules(List<KadinskyShape> shapes, KadinskyShape newShape, bool maxAmount, bool secondaryObject)
    {
        int maxCount = 2;
        int minCount = 3;
        var shapeToCheck = secondaryObject ?   shapeToPositionTo : shapeType;
        var colorToCheck = secondaryObject ?   colorToPositionTo : color;
        // Initialize a counter for the number of shapes of the specified type
        int shapeTypeCount = 0;

        // Iterate through the list of shapes in the canvas
        foreach (KadinskyShape shape in shapes)
        {
            // If the current shape is of the specified type, increment the counter
            if (Utils.AreShapesEqual(shape.shape, shapeToCheck))
            {
                if (Utils.AreColorsEqual(shape.color, colorToCheck))
                {
                    //Debug.Log("Found a shape and color that already exists.");
                    shapeTypeCount++;
                }
                    
            }
        }
        if (maxAmount)
        {
            // Check if the number of shapes of the specified type is within the allowed range
            if (shapeTypeCount >= maxCount)
            {
                //Debug.Log("Returnning all false.");
                return RulesResult.AllFalse;
            }
            else
            {
                return RulesResult.AllTrue;
            }
        }
        //we are checking for min amount now..
        else
            {
            //if the min amount hasnt been reached yet, we only allow this shape and color to be generated.
            if (shapeTypeCount < minCount)
            {
                if (Utils.AreShapesEqual(newShape.shape, shapeToCheck) && Utils.AreColorsEqual(newShape.color, colorToCheck))
                {
                    return RulesResult.AllTrue;
                }
                else
                {
                    return RulesResult.AllFalse;
                }
            }
            else
            {
                return RulesResult.AllTrue;
            }
        }
        


    }
    private RulesResult CreateRulesResult(bool everTrue, bool everFalse)
    {
        //Return correct RulesResult:
        if (everTrue && everFalse)
        {
            return RulesResult.Mixed;
        }
        else if (everTrue)
        {
            return RulesResult.AllTrue;
        }
        else if (everFalse)
        {
            return RulesResult.AllFalse;
        }
        else //if no object was ever checked (so everTrue and everFalse are both false) we return None. 
        {
            return RulesResult.None;
        }
    }
   
    
}

[System.Serializable]
public enum RulesResult
{
    AllTrue, AllFalse, Mixed, None
}
[System.Serializable]
public enum Direction
{
    Left, Right, Above, Below
}

