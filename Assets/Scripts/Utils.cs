using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Contains a view general helpful scripts. 
/// </summary>
public class Utils 
{
    /// <summary>
    /// use Instead of .Equals() to check for equality. On that topic, TODO override .Equals() to this implementation. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool AreShapesEqual(ShapeType a, ShapeType b)
    {
        return a.Equals(b) || a.Equals(ShapeType.AllShapes) || b.Equals(ShapeType.AllShapes);
    }
    /// <summary>
    /// use Instead of .Equals() to check for equality. On that topic, TODO override .Equals() to this implementation. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool AreColorsEqual(ColorType a, ColorType b)
    {
        return a.Equals(b) || a.Equals(ColorType.AllColors) || b.Equals(ColorType.AllColors);
    }
    /// <summary>
    /// Find the KadinskyShape with the largest scale value among the array.
    /// </summary>
    /// <param name="shapes"></param>
    /// <returns></returns>
    public static float FindLargest(KadinskyShape[] shapes)
    {
        float largest = 0;
        foreach (KadinskyShape shape in shapes)
        {
            if (shape.size > largest)
            {
                largest = shape.size;
            }
        }
        return largest;
        
    }

    public static int BinominalCoefficient(int n, int k)
    {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }
    public static int Factorial(int n)
    {
        if (n == 0)
        {
            return 1;
        }
        else
        {
            return n * Factorial(n - 1);
        }

    }

    public static Direction OppositeDirection(Direction d){
        switch(d){
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.Above:
                return Direction.Below;
            case Direction.Below:
                return Direction.Above;
            default:
                Debug.LogError("OppositeDirection() called with invalid direction");
                return Direction.Above;
        }
    }

    public static Direction StringToDirection(string s){
        switch(s){
            case "Left":
                return Direction.Left;
            case "Right":
                return Direction.Right;
            case "Above":
                return Direction.Above;
            case "Below":
                return Direction.Below;
            default:
                Debug.LogError("StringToDirection() called with invalid string. Returning Direction.Above");
                return Direction.Above;
        }
    }
    public static ColorType StringToColorType(string s){
        //check if string is in enum ColorType
        if(System.Enum.IsDefined(typeof(ColorType), s)){
            return (ColorType)System.Enum.Parse(typeof(ColorType), s);
        }
        else{
            Debug.LogError("StringToColorType() called with invalid string, returning AllColors");
            return ColorType.AllColors;
        }
    }
    public static ShapeType StringToShapeType(string s){
        //check if string is in enum ShapeType
        if(System.Enum.IsDefined(typeof(ShapeType), s)){
            return (ShapeType)System.Enum.Parse(typeof(ShapeType), s);
        }
        else{
            Debug.LogError("StringToShapeType() called with invalid string, returning AllShapes");
            return ShapeType.AllShapes;
        }
    }
}
