using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Instruction : MonoBehaviour
{
    [Header("Identity")] //Allows for instructions to be marked as 2, 2a etc.
    public int InstructionNumber;
    public string InstructionNumberSuffix;

    [Header("Content")] //The content of the instruction.
    public string InstructionName;
    public string InstructionNameShort;
    [TextArea]
    public string InstructionDescription;
    public Sprite InstructionImageGuide;

    [Header("Parameters")] //Parameters to be used in the future.
    public bool CanBeReturnedToWithBack = true;
    [Space(10)]
    public GameObject PreviousInstruction;
    public GameObject NextInstruction;
    [Space(10)]
    public bool IsTaskComplete;
    //how to task it as complete idk
}
