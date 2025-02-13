using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
/// <summary>
/// This class is responsible for creating shaperules at runtime. This means: 
/// -rversing shape rules automatically
/// -creating the second shape rule to a positional rule. (as two are necessary for logic to work)
/// </summary>
public class ShapeRuleCreator 
{
    public static void AddSecondaryPositionRuleToCanvas(Canvas c){
        //Search for all positional rules
        List<ShapeRule> positionalRules = new List<ShapeRule>();
        foreach(ShapeRule rule in c.rules){
            if(rule.positionRules){
                positionalRules.Add(rule);
            }
        }
        ShapeRule[] newRules = new ShapeRule[positionalRules.Count];
        //Create secondary rules
        for(int i = 0; i < positionalRules.Count; i++){
            newRules[i] = CreateSecondaryPositionRule(positionalRules[i]);
        }
        //create new array with all rules
        ShapeRule[] allRules = new ShapeRule[c.rules.Length + newRules.Length];
        for(int i = 0; i < c.rules.Length; i++){
            allRules[i] = c.rules[i];
        }
        for(int i = 0; i < newRules.Length; i++){
            allRules[i + c.rules.Length] = newRules[i];
        }
        c.rules = allRules;

    } 
    /// <summary>
    /// Creates the secondary, mandatory rule that makes a positional rule work. 
    /// </summary>
    /// <param name="positionRule"></param>
    /// <returns></returns>
    private static ShapeRule CreateSecondaryPositionRule(ShapeRule positionRule){
        ShapeType shapeA = positionRule.shapeType;
        ColorType colorA = positionRule.color;

        ShapeType shapeB = positionRule.shapeToPositionTo;
        ColorType colorB = positionRule.colorToPositionTo;

        ShapeRule newRule = new ShapeRule();
        newRule.shapeType = shapeB;
        newRule.color = colorB;
        newRule.shapeToPositionTo = shapeA;
        newRule.colorToPositionTo = colorA;
        newRule.positionRules = true;
        newRule.direction = Utils.OppositeDirection(positionRule.direction);
        newRule.IsASecondaryRule = true;
        return newRule;
    }

    public static void ReverseRules(Canvas c){
        //Every positional rule gets direction reversed. 
        // every max rule turns to min and increase amount by 1.
        // every min rule turns to max and decrease amount by 1.

        //Iterate through all rules
        for(int i = 0; i< c.rules.Length; i++){
            ShapeRule rule = CopyRule(c.rules[i]);

            //If rule is positional, reverse direction
            if(rule.positionRules){
                rule.direction = Utils.OppositeDirection(rule.direction);
            }
            //If rule is max, turn to min and increase amount by 1
            if(rule.amountRules){
                rule.moreThan = !rule.moreThan;
            }
            c.rules[i] = CopyRule(rule);
        }
        
    }
    /// <summary>
    /// Creates a shape rule with the given parameters
    /// </summary>
    public static ShapeRule CreateShapeRule(ShapeType shape, ColorType color,bool amountRules, bool moreThan, bool directionRule, Direction direction, ShapeType otherShape, ColorType otherColor){
        ShapeRule rule = new ShapeRule();
        rule.shapeType = shape;
        rule.color = color;
        rule.amountRules = amountRules;
        rule.moreThan = moreThan;
        rule.positionRules = directionRule;
        rule.direction = direction;
        rule.shapeToPositionTo = otherShape;
        rule.colorToPositionTo = otherColor;
        return rule;
    } 

    public static ShapeRule[] CopyRules(ShapeRule[] r){
        ShapeRule[] newRules = new ShapeRule[r.Length];
        for(int i = 0; i < r.Length; i++){
            newRules[i] = CopyRule(r[i]);
        }
        return newRules;
    }
    public static ShapeRule CopyRule(ShapeRule r){
        ShapeRule newRule = new ShapeRule();
        newRule.shapeType = r.shapeType;
        newRule.color = r.color;
        newRule.amountRules = r.amountRules;
        newRule.moreThan = r.moreThan;
        newRule.positionRules = r.positionRules;
        newRule.direction = r.direction;
        newRule.shapeToPositionTo = r.shapeToPositionTo;
        newRule.colorToPositionTo = r.colorToPositionTo;
        return newRule;
    }

    public static bool IsRuleValid(ShapeRule r){
        if(r.color == ColorType.AllColors && r.shapeType == ShapeType.AllShapes){
            return false;
        }
        if(r.positionRules){
            if(r.colorToPositionTo == ColorType.AllColors && r.shapeToPositionTo == ShapeType.AllShapes){
                return false;
            }
            if(r.color == r.colorToPositionTo && r.shapeType == r.shapeToPositionTo){
                return false;
            }
        }
        return true;
    }


}
