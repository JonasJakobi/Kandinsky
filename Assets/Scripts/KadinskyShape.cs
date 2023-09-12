using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// An abstract representation of a kandinsky shape. It doesnt exist as a gameobject but just as data. 
/// Rules etc. are calculated using these and later in Canvas they are converted to GameObjects that contain the actual shape and color values. 
/// (This is done to make it easier to calculate rules and to make it easier to change the rules as well as the visuals later on, as they are seperated)
/// </summary>
[System.Serializable]
public class KadinskyShape 
{
    public ShapeType shape;
    public ColorType color;
    public float size;
    public Vector2 position;


    

    public override string ToString()
    {
        return "Shape: " + shape + " Color: " + color + " Size: " + size + " Position: " + position;
    }
}
[System.Serializable]
/// <summary>
/// Enum specifying the possible shapes of a kadinsky shape
/// </summary>
public enum ShapeType
{
    Square, Triangle, Circle, AllShapes
}
[System.Serializable]
public enum ColorType
{
    Red, Green, Blue,  AllColors
}




