using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine.Events;
using System;
/// <summary>
/// Takes User Input and generates new Kadinsky Images / takes screenshots, etc. 
/// Manages the 5 canvases and their rules.
/// </summary>
public class CanvasManager : MonoBehaviour
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WebGLTakeScreenshot();
    #endif
    [Header("CONFIGURATIONS")]
    [SerializeField]
    string directory = "C:\\Users\\Public\\Pictures\\Sample Pictures\\";
    [SerializeField]
    string fileName = "Kadinsky";
    [SerializeField]
    int currentScreenshotNumber = 1;
    
    [SerializeField]
    [Header("Reverse one of the 5 canvases rules?")]
    private bool reverseOneCanvas = false;
    [Header("If true, hint about the correct image will be shown, if false, show AI explanations.")]
    public bool toggleHint = true;

    
    [Header("If we are have hints on, this flag will make the hint always be wrong.")]
    public bool toogleHintIsWrong = false;
    [Header("Verbosity for Explanations(1-4). Set to 0 for None.")]
    public int verbosity;



    [Header("REFERENCES")]
    [SerializeField]
    Canvas[] canvases;

    [SerializeField]
    RuleExplanations explanations;
    [SerializeField]    
    GameObject ruleCreatorUI;
    
    private void Start()
    {

        currentScreenshotNumber = 1;
        UpdateCanvases();
        
    }
    /// <summary>
    /// Adds secondary positional rule. Update all canvases to match canvas 0 and reverse rules if needed.
    /// </summary>
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
            if(i == 4 && reverseOneCanvas){
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
        HandleUserInputs();
    }

    private void HandleUserInputs(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            ruleCreatorUI.SetActive(!ruleCreatorUI.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.O)){
            Application.OpenURL(directory);
        }
        //Only allow image generation and screenshotting if rule creator menu is not active
        if(!ruleCreatorUI.activeSelf){
            if(Input.GetKeyDown(KeyCode.S)){
                GenerateNewImage();
            }
            if(Input.GetKeyDown(KeyCode.D)){
                StartCoroutine(TakeScreenshot(currentScreenshotNumber));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void GenerateNewImage()
    {
        Debug.Log("Trying to generate new image nr " + currentScreenshotNumber);
        //shuffle canvas positions as hints / reversed canvases are always the same ones. 
        ShuffleCanvases();
       //Generate all Kandinsky Images
        foreach(var c in canvases)
        {
            c.TryGenerateShapes(c.amountOfShapes, true);
            c.DrawShapes();
        }

        if(toggleHint){
            CreateHints();
        }
        //Create Explanations instead of hint
        else{
            explanations.UpdateUIForExplanations();
            explanations.PrintExplanations(canvases[0].rules, verbosity);
        }
        //Print Amount of Rules
        Debug.Log("TotalAmountOfRules: " + RulesAmountCalculator.CalculateAmountOfRulesOverall());
        Debug.Log("Amount of Rules after rule 1 and verbosity: " + RulesAmountCalculator.CalculateRulesPossibleAfterHint(canvases[0].rules[0], verbosity));
    }
    private void CreateHints(){
        if(!reverseOneCanvas){
                Debug.LogError("ReverseOneCanvas is false, so the hint will just show a random image.");
            }
            explanations.UpdateUIForHint();
            if(toogleHintIsWrong){
                explanations.PrintSelectedImage(canvases[0].positionIndex); //just pick any canvas with correct rules (0-3). positions on screen get shuffled anyway.
            }
            else{
                explanations.PrintSelectedImage(canvases[4].positionIndex); //canvas 4 is the one with opposite rules.
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
        var screenshotName = "";
        if(toggleHint){
            screenshotName = fileName + "-" + positionIndexToLetter(canvases[4].positionIndex)  + "-nr" + nr + ".png";
        }
        else{
            screenshotName = fileName + nr + ".png";
        }
        
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
        positions = positions.OrderBy(x => UnityEngine.Random.value).ToList();
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
    

   

    public void SetReverseOneCanvas(bool value){
        reverseOneCanvas = value;
        UpdateCanvases();
    }
    public void SetToggleHint(bool value){
        toggleHint = value;
    }
    public void SetToggleHintIsWrong(bool value){
        toogleHintIsWrong = value;
    }

    public void SetVerbosityFromString(string input){
        int value = 0;
        if(int.TryParse(input, out value)){
            verbosity = value;
        }
        else{
            Debug.LogError("Could not parse verbosity value");
        }
    }

}
