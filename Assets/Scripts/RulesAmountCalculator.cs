
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class is responsible for calculating the amount of rules possible in the application.
/// Currently the number is not entirely correct in one case.
/// </summary>
public class RulesAmountCalculator : MonoBehaviour {


     public static int CalculateAmountOfRulesOverall()
    {
        //The only rules we want are: Is x over / left of / right of/ below y
        // more x then y, less x then y
       //x and y have to be only a shape or only a color. something like "red squares ..." is not allowed

        int amountOfRelations = 6;
        int shapeAmount  = System.Enum.GetValues(typeof(ShapeType)).Length - 1;
        int colorAmount = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        int totalRules = CalculateTotalRuleAmount(amountOfRelations, shapeAmount, colorAmount);
        return totalRules;


    }

    public static int CalculateTotalRuleAmount (int amountOfRelations, int shapeAmount, int colorAmount){
        if(shapeAmount < 2 && colorAmount < 2){
            return amountOfRelations;
        }
        else if(shapeAmount < 2){
            return amountOfRelations * colorAmount;
        }
        else if(colorAmount < 2){
            return amountOfRelations * shapeAmount;
        }
        else{
            return amountOfRelations *( Utils.BinominalCoefficient(shapeAmount, 2) + Utils.BinominalCoefficient(colorAmount, 2) );
        }
        
    }
    /// <summary>
    /// THIS ONE IS CURRENTLY INCORRECT. 
    /// </summary>
    /// <param name="rule"></param>
    /// <param name="verbosity"></param>
    /// <returns></returns>
    public static int CalculateRulesPossibleAfterHint(ShapeRule rule, int verbosity){
        int amountOfRelations = 6;
        int shapeAmount  = System.Enum.GetValues(typeof(ShapeType)).Length - 1;
        int colorAmount = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        if (verbosity == 1){
            if(rule.positionRules){
                amountOfRelations = 4;
            }
            else{
                amountOfRelations = 2; // min and max amount
            }
        }
        if (verbosity == 2){
            if(rule.positionRules){
                amountOfRelations = 4;
                if(rule.shapeType == ShapeType.AllShapes){
                    shapeAmount = 0;
                }
                else{
                    colorAmount = 0;
                }

            }
            //Min/max Rules --> will now just be more than or less than
            else if (rule.amountRules){
                amountOfRelations = 2;
                if(rule.shapeType == ShapeType.AllShapes){
                    shapeAmount = 0;
                }
                else{
                    colorAmount = 0;
                }
            }
        }
        if (verbosity == 3){
            if(rule.positionRules){
                //verbosity 3 tells both objects so only need the amount of variations.
                amountOfRelations = 4;
                
                shapeAmount = 0;
                colorAmount = 0;
            }
            else if (rule.amountRules){
                amountOfRelations = 2; //PROBLEM : do we
                shapeAmount = 0;
                colorAmount = 0;
            }
        }
        return CalculateTotalRuleAmount(amountOfRelations, shapeAmount, colorAmount);
       
    }
    /// <summary>
    /// Returns all rules that are possible after a hint has been given. 
    /// Hint is given in the form of a rule that it hints to and the verbosity of the hint.
    /// </summary>
    /// <param name="rule"></param>
    /// <param name="verbosity"></param>
    /// <returns></returns>
    public static ShapeRule[] RulesAfterHint(ShapeRule rule, int verbosity){
        ShapeRule[] all = SavingManager.Instance.LoadAllRules();
        List<ShapeRule> rules = new List<ShapeRule>();
        rules.AddRange(all);
        if(verbosity == 1){
            if(rule.positionRules){
                rules = rules.Where(r => r.positionRules).ToList();
            }
            else{
                rules = rules.Where(r => r.amountRules).ToList();
            }
        }
        if(verbosity == 2){
            rules = (rule.positionRules) ?  rules.Where(r => r.positionRules).ToList() : rules.Where(r => r.amountRules).ToList();
            if(rule.shapeType != ShapeType.AllShapes){
                rules = rules.Where(r => r.shapeType == rule.shapeType).ToList();
            }
            else{
                rules = rules.Where(r => r.color == rule.color).ToList();
            }
        }

        if(verbosity == 3){
            rules = (rule.positionRules) ?  rules.Where(r => r.positionRules).ToList() : rules.Where(r => r.amountRules).ToList();
            if(rule.shapeType != ShapeType.AllShapes){
                rules = rules.Where(r => r.shapeType == rule.shapeType).ToList();
            }
            else{
                rules = rules.Where(r => r.color == rule.color).ToList();
            }
            if(rule.shapeToPositionTo != ShapeType.AllShapes){
                rules = rules.Where(r => r.shapeToPositionTo == rule.shapeToPositionTo).ToList();
            }
            else{
                rules = rules.Where(r => r.colorToPositionTo == rule.colorToPositionTo).ToList();
            }
        }
        return rules.ToArray();
    }



    
}