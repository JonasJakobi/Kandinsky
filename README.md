# Kandinsky Generator
> A program for creating Kandinsky Patterns, generating Kandinsky Figures, and creating images to be used user studies.

## Installation

This project runs on the **Unity Editor Version 2021.2.7f1**. Due to the project building on WebGL, the **WebGL Build Support** module has to be added to the Editor. Open the ``/Assets/Scenes/StandardKandinskyScene`` and press play or Build the project to some directory and open the ``index.html`` file in the build folder. 


## Project Structure & Usage
The project is housed inside the StandardKandinskyScene. The logic is mostly attached to the *Canvas Manager* GameObject.

### Controls

**Tab** - Open / Close Rule Configuration Window.


**S** - Generate new Image.


**D** - Save image as png file.


**O** - View current file path for saving images.


### Configuring Rules through the Provided Interface


To add a new rule, configure to what kind of objects the rule should apply by setting the color and shape dropdowns. They can not both be set to AllColors and AllShapes. Then, the type of rule can be specified by the dropdown which will reveal the other options for that rule. The rule then has to be added to the list of current rules by pressing *Add Rule* . After adding every rule, press *Export Rules* on the right or delete unwanted rules with the *x* button next to it in the list. 

- *Reverse One Canvas* will ensure that one of the five images will have reversed rules. 

- The *Show Hint* Button toggles between showing a hint which tells which image is reversed or showing explanations describing the rules. 
When choosing explanations, *Diagnosticity* can be used to change how much information these statements give, with 0 disabling them and 5 being a full descrption of the rules. 
When choosing the hint, it can also be set to be wrong and point out one of the images that is not reversed. 



## Kandinsky Concepts Explained

**Kandinsky Figure**:  
A Kandinsky Figure is a square image containing $n$ Kandinsky Objects, each defined by their color, shape, position, and size. 

**Statement / Rule:**

A statement about a Kandinsky Figure $k$ can be represented in logical notation as
> $s(k) \rightarrow B$  

where $B$ is a boolean variable taking values of either $1$ or $0$ or as a natural language statement which can be true or false.
In our current implementation there are 3 possible types of rules: 
- Minimum Amount: There has to be atleast $x$ amount of the specified type of object in each figure.
- Maximum Amount: There can be no more than $x$ amount of the specified type of object. 
- Relative Positioning: Our specified type of object has to be below/ above/ to the right/ to the left of another type of object. 

These rules can be applied to object of a specific color and shape (e.g. red squares) or just one of the two variables (e.g. "all shapes that are red" or "a square of any color").

**Kandinsky Pattern**:  
A Kandinsky Pattern is the subset of all Kandinsky Figures which follow its rules. For example, if we define the rule "Minimum amount of red shapes is 2" then every image that has more then two red objects belongs to this pattern. A more mathematically precise definition would be:

Let the Kandinsky Pattern $K$ be the subset of all Kandinsky Figures $k$ for which the rules $s(k)$ are always true, where $s(k)$ represents the ground truth of $K$. Mathematically, this can be expressed as:

> $ K = \{ k \mid \forall k \in K, \, s(k) = 1 \}$
