using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleSheets;


public class ConfigMenu : MonoBehaviour
{
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private UIDocument document;
    [SerializeField] private StyleSheet styleSheet;

    RuleFields ruleFields;
    private struct RuleFields{
        public DropdownField color;
        public DropdownField shape;
        public Toggle minAmountRule;
        public TextField minAmount;
        public Toggle maxAmountRule;
        public TextField maxAmount;
        public Toggle directionRule;
        public DropdownField direction;
        public DropdownField otherShape;
        public DropdownField otherColor;
    }

    public List<ShapeRule> result;
    // Start is called before the first frame update
    void Start()
    {
        result = new List<ShapeRule>();
        Generate();
    }
    /*
    private void OnValidate() {
        if(Application.isPlaying) return;

        Generate();
    }
    */

    private void OnEnable() {
        Generate();
    }

    void Generate(){
        var root = document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(styleSheet);
        ruleFields = new RuleFields();
        var quitButton = new Button();
        quitButton.text = "Quit Application";
        quitButton.AddToClassList("button");
        quitButton.clicked += () => Application.Quit();
        root.Add(quitButton);
        GenerateRuleBox(root);
        
       //Generate button to print all values
        var button = new Button();
        button.text = "Add new rule";
        button.AddToClassList("button");

        button.clicked += () => result.Add(CreateShapeRuleFromFields());
        root.Add(button);
        
        var button2 = new Button();
        button2.text = "Export Rules";
        button2.AddToClassList("button");

        button2.clicked += () => canvasManager.SetNewRules(result.ToArray());
        root.Add(button2);
    }

    ShapeRule CreateShapeRuleFromFields(){
        ShapeType shape = Utils.StringToShapeType(ruleFields.shape.value);
        ColorType color = Utils.StringToColorType(ruleFields.color.value);
        bool minAmountRule = ruleFields.minAmountRule.value;
        int minAmount = 0;
        if(int.TryParse(ruleFields.minAmount.text, out minAmount) == false){
            minAmount = 0;
        }
        bool maxAmountRule = ruleFields.maxAmountRule.value;
        int maxAmount = 0;
        if(int.TryParse(ruleFields.maxAmount.text, out maxAmount) == false){
            maxAmount = 0;
        }
        bool directionRule = ruleFields.directionRule.value;
        Direction direction = Utils.StringToDirection(ruleFields.direction.value);
        ShapeType otherShape = Utils.StringToShapeType(ruleFields.otherShape.value);
        ColorType otherColor = Utils.StringToColorType(ruleFields.otherColor.value);
        return ShapeRuleCreator.CreateShapeRule(shape, color, minAmountRule, minAmount, maxAmountRule, maxAmount, directionRule, direction, otherShape, otherColor);

    }

    //Method that creates box for rule generation. Dropdown for shape and color
    void GenerateRuleBox(VisualElement root){
        
        var ruleBox = new VisualElement();
        ruleBox.AddToClassList("ruleBox");
        

        String[] shapeNames = Enum.GetNames(typeof(ShapeType));
        Array.Resize(ref shapeNames, shapeNames.Length - 1);
        var shapeDropdown = CreateDropDown("Shape?", new List<string>(shapeNames), "dropdown");
        ruleBox.Add(shapeDropdown);
        ruleFields.shape = shapeDropdown;

        String[] colorNames = Enum.GetNames(typeof(ColorType));
        Array.Resize(ref colorNames, colorNames.Length - 1);
        var colorDropdown = CreateDropDown("Color?", new List<string>(colorNames), "dropdown");
        ruleBox.Add(colorDropdown);
        ruleFields.color = colorDropdown;
        
        var minToggle = CreateToggle("Min Amount Rule?", "toggle");
        ruleBox.Add(minToggle);
        ruleFields.minAmountRule = minToggle;

        var minAmount = CreateIntegerField("Min Amount", "integerField");
        ruleBox.Add(minAmount);
        ruleFields.minAmount = minAmount;

        var maxToggle = CreateToggle("Max Amount Rule?", "toggle");
        ruleBox.Add(maxToggle);
        ruleFields.maxAmountRule = maxToggle;

        var maxAmount = CreateIntegerField("Max Amount", "integerField");
        ruleBox.Add(maxAmount);
        ruleFields.maxAmount = maxAmount;

        


        //Direction Toggle
        var directionToggle = CreateToggle("Direction Rule?", "toggle");
        ruleBox.Add(directionToggle);
        ruleFields.directionRule = directionToggle;
        //Direction dropdown
        string[] directionNames = Enum.GetNames(typeof(Direction));
  
        var directionDropdown = CreateDropDown("Direction?", new List<string>(directionNames), "dropdown");
        ruleBox.Add(directionDropdown);
        ruleFields.direction = directionDropdown;
        //Text to signify that the following shape and color is the one that will be affected by the direction rule
        ruleBox.Add(new Label("The other object is.."));
        //Shape dropdown
        var otherShapeDropdown = CreateDropDown("Shape?", new List<string>(shapeNames), "dropdown");
        ruleBox.Add(otherShapeDropdown);
        ruleFields.otherShape = otherShapeDropdown;
        //Color dropdown
        var otherColorDropdown = CreateDropDown("Color?", new List<string>(colorNames), "dropdown");
        ruleBox.Add(otherColorDropdown);
        ruleFields.otherColor = otherColorDropdown;


        root.Add(ruleBox);

    }



    DropdownField CreateDropDown(String title, List<string> options, String classList){
        var dropdown = new DropdownField(options, title);
        dropdown.AddToClassList(classList);
        return dropdown;
    }
    Toggle CreateToggle(String title, String classList){
        var toggle = new Toggle(title);
        toggle.AddToClassList(classList);
        return toggle;
    }

    TextField CreateIntegerField(String title, String classList){
        var integerField = new TextField(title);
        integerField.AddToClassList(classList);
        return integerField;
    }

  
}
