using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
public class CanvasManager : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void WebGLTakeScreenshot();
    [SerializeField]
    string directory = "C:\\Users\\Public\\Pictures\\Sample Pictures\\";
    [SerializeField]
    string fileName = "Kadinsky";
    [SerializeField]
    int currentScreenshotNumber = 1;
    [SerializeField]
    Canvas[] canvases;

    [SerializeField]
    RuleExplanations explanations;
    [SerializeField]    
    GameObject RuleCreatorUI;
    [Header("If true, hint about the correct image will be shown, if false, show AI explanations.")]
    public bool toggleHint = true;
    [Header("Verbosity for Explanations(1-4). Set to 0 for None.")]
    public int verbosity;
    private void Start()
    {
        CalculateAmountOfRules();
        currentScreenshotNumber = 1;
        UpdateCanvases();
        
    }
    private void UpdateCanvases(){
        //Create Secondary Positional Rule
        ShapeRuleCreator.AddSecondaryPositionRuleToCanvas(canvases[0]);
        for(int i = 1; i < 5; i++)//go through canvas copy 1-3 and apply rules and amountofShapes from canvas 0.
        {
            canvases[i].amountOfShapes = canvases[0].amountOfShapes;
            //update min and amx size and tries
            canvases[i].minSize = canvases[0].minSize;
            canvases[i].maxSize = canvases[0].maxSize;
            canvases[i].maxTriesCanvas = canvases[0].maxTriesCanvas;
            canvases[i].maxTriesShape = canvases[0].maxTriesShape;

            canvases[i].rules = ShapeRuleCreator.CopyRules(canvases[0].rules);
            //Reverse Rules for last canvas
            if(i == 4){
                Debug.Log("Reversing Rules");
                ShapeRuleCreator.ReverseRules(canvases[i]);
            }
        }
    }
    public void SetNewRules(ShapeRule[] rules){
        canvases[0].rules = rules;
        UpdateCanvases();
    }
  
    private void Update()
    {
        //Simple first input system. no ui, just button input.
        if (Input.GetKeyDown(KeyCode.S))
        {
            MakeKandinskyPatterns();

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(TakeScreenshot(currentScreenshotNumber));
            
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RuleCreatorUI.SetActive(!RuleCreatorUI.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Application.OpenURL(directory);
        }
    }
    public void MakeKandinskyPatterns()
    {
        //Create 5 images, 4 with rules applying and 5th with rules not applying.

        Debug.Log("Trying to generate new image nr " + currentScreenshotNumber);
        ShuffleCanvases();
       //Generate all Kandinsky Images
        foreach(var c in canvases)
        {
            c.TryGenerateShapes(c.amountOfShapes, true);
            c.DrawShapes();
        }


        if(toggleHint){
            explanations.PrintSelectedImage(canvases[4].positionIndex);
        }
        else{
            explanations.PrintExplanations(canvases[0].rules, verbosity);
        }
        
        

        




    }
    /// <summary>
    ///  Takes a screenshot of the Camera. (Hide UI first if dealing with UI
    ///  Generate all GameObjects first (DrawShapes());
    /// </summary>
    private IEnumerator TakeScreenshot(int nr)
    {
        #if UNITY_EDITOR
        Debug.Log("Editor screenshot!");
        if (!System.IO.Directory.Exists(directory))
            System.IO.Directory.CreateDirectory(directory);

        var screenshotName = fileName + "-" + positionIndexToLetter(canvases[4].positionIndex)  + "-nr" + nr + ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(directory, screenshotName));

        Debug.Log("new file saved to: " + directory + screenshotName);
        currentScreenshotNumber++;
        #endif
        #if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("Webgl screenshot!");
        yield return new WaitForEndOfFrame();
        WebGLTakeScreenshot();
        #endif

        yield return new WaitForEndOfFrame();
    }

    private void ShuffleCanvases(){
        //Save all 5 positions of canvases and shuffle them 
        List<int> positions = new List<int>();
        foreach(var c in canvases)
        {
            positions.Add(c.positionIndex);
        }
        //Shuffle array
        positions = positions.OrderBy(x => Random.value).ToList();
        //Apply new positions
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].positionIndex = positions[i];
            
            canvases[i].transform.position = new Vector3(canvases[i].positionIndex * 12, canvases[i].transform.position.y, 0);

        }
    }

    private string positionIndexToLetter(int a)
    {
        switch (a)
        {
            case -2:
                return "A";
            case -1:
                return "B";
            case 0:
                return "C";
            case 1:
                return "D";
            case 2:
                return "E";

            default:
                return "A";
        }

    }
    

    public void CalculateAmountOfRules()
    {
        //The only rules we want are: Is x over / left of / right of y
        // more x then y, less x then y
       //x and y can both be any color, any shape, or any combination of both
        int totalRules = 0;
        int n1  = System.Enum.GetValues(typeof(ShapeType)).Length - 1;
        int n2 = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        totalRules = 6 * (Utils.BinominalCoefficient(n1, 2) + Utils.BinominalCoefficient(n2, 2));
     
        
        Debug.Log("Total Rules: " + totalRules);


    }


}
