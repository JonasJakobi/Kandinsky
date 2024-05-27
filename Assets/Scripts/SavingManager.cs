using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using System.Linq;
using System.Xml;



public class SavingManager : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] Canvas canvas0;
    [SerializeField] RuleCreationUIManager ruleCreationUIManager;
    private void Start() {
        ruleCreationUIManager = FindObjectOfType<RuleCreationUIManager>();
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
        ShapeRule[] rules = canvas0.rules;
        //remove all rules that have the secondary property
        rules = rules.Where(rule => rule.IsASecondaryRule == false).ToArray();

        SaveRulesToDisk(rules, fileName);
    }
    [ProButton]
    public void LoadRulesFromDisk(string filename)
    {
        string directoryPath = @"C:\Users\Public\Pictures\Sample Pictures\";
        string filePath = Path.Combine(directoryPath, filename + ".json");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(filePath);

            // Convert the JSON string to a rules array
            ShapeRule[] rules = JsonUtility.FromJson<SerializationWrapper<ShapeRule>>(json).Items;

            // Set the rules of the canvas
            canvasManager.SetNewRules(rules);
            ruleCreationUIManager.CreateUIForLoadedRules(rules);

        }
        else
        {
            Debug.Log("File not found");
        }
    }
}