using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using AOT;


/// <summary>
/// This class is responsible for saving and loading rules to and from disk.
/// Can also be used to load all rules.
/// </summary>
public class SavingManager : Singleton<SavingManager>
{
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] Canvas canvas0;
    [SerializeField] AllRulesViewUI ruleCreationUIManager;
    private void Start() {
        ruleCreationUIManager = FindObjectOfType<AllRulesViewUI>();
    }
    public void SaveRulesToDisk(ShapeRule[] rules, string fileName)
    {
        string directoryPath = @"C:\Users\Public\Pictures\Sample Pictures\";


        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Save rules to disk
        string filePath = Path.Combine(directoryPath, fileName + ".json");

        // Convert the rules array to a JSON string
        string json = JsonUtility.ToJson(new SerializationWrapper<ShapeRule> { Items = rules }, true);

        // Write the JSON string to the file
        File.WriteAllText(filePath, json);
    }

    [System.Serializable]
    private class SerializationWrapper<T>
    {
        public T[] Items;
    }
    [ProButton]
   public void SaveCurrentRulesToDisk(string fileName)
    {
        ShapeRule[] rules = ruleCreationUIManager.GetRules().ToArray();
        //remove all rules that have the secondary property

        SaveRulesToDisk(rules, fileName);
    }
    [ProButton]
    public ShapeRule[] LoadRulesFromDisk(string filename)
    {
        string directoryPath = @"C:\Users\Public\Pictures\Sample Pictures\";
        string filePath = Path.Combine(directoryPath, filename + ".json");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            return CreateRulesFromFileContent(File.ReadAllText(filePath));

        }
        else
        {
            Debug.Log("File not found");
            return null;
        }
    }
    public ShapeRule[] CreateRulesFromFileContent(string content)
    {
        // Read the JSON string from the file
            string json = content;

            // Convert the JSON string to a rules array
            ShapeRule[] rules = JsonUtility.FromJson<SerializationWrapper<ShapeRule>>(json).Items;

            // Set the rules of the canvas
            //canvasManager.SetNewRules(rules);
            ruleCreationUIManager.SetRules(rules);
            return rules;
    }
    public ShapeRule[] LoadAllRules(){
        return CreateRulesFromFileContent(Resources.Load<TextAsset>("realall").text);
    }

}