
using JetBrains.Annotations;
using UnityEngine;

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
            else{
                amountOfRelations = 1;

            }
        }
        if (verbosity == 3){
            if(rule.positionRules){
                //verbosity 3 tells both objects so only need the amount of variations.
                amountOfRelations = 4;
                
                shapeAmount = 0;
                colorAmount = 0;
            }
            else{
                amountOfRelations = 1; //PROBLEM : do we
            }
        }
        return CalculateTotalRuleAmount(amountOfRelations, shapeAmount, colorAmount);
       
    }


    
}