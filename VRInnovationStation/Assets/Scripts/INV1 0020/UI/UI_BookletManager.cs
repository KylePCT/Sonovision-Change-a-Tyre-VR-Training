using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_BookletManager : MonoBehaviour
{
    private GameObject CanvasCheckObj = null;

    [Header("Page Lists")]
    public UI_Instruction[] InstructionPages;
    public List<GameObject> InstructionObjects;
    public GameObject[] InstructionCanvases;

    [Header("Page Single References")]
    public GameObject IntroductionPage;

    [Header("Page History")]
    private GameObject PreviousPage;
    public List<GameObject> PageHistory;
    public GameObject CurrentPage;

    private void OnEnable()
    {
        CanvasCheckObj = gameObject;
    }

    void Awake()
    {
        //Finds all the instruction pages and adds them to the InstructionObjects list.
        InstructionPages = FindObjectsOfType<UI_Instruction>();
        foreach (UI_Instruction instruction in InstructionPages)
        {
            InstructionObjects.Add(instruction.gameObject);
        }

        InstructionCanvases = GameObject.FindGameObjectsWithTag("InstructionCanvas");

        PopulatePages();
    }

    void Start()
    {
        //Turn off all the instructional pages.
        foreach (GameObject InstructionObject in InstructionCanvases)
        {
            InstructionObject.SetActive(false);
        }

        //Only activate the introduction page.
        IntroductionPage.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Page Navigation

    //Runs through this reference checker until a canvas component is found, which is then set as the active current page.
    //Originally from L3Harris ICG.2.1.0 UI_ButtonManager.cs and has been revamped.
    //+++++ REQUIRES EACH PAGE TO HAVE A SEPERATE CANVAS COMPONENT.
    public void FindCanvasReference()
    {
        if (CurrentPage == null)
        {
            if (CanvasCheckObj.GetComponent<Canvas>() != null)
            {
                CurrentPage = CanvasCheckObj;
            }
            else
            {
                CanvasCheckObj = CanvasCheckObj.transform.parent.gameObject;
                FindCanvasReference();
            }
        }
    }

    public void SetPreviousPage(GameObject InstructionObject)
    {
        //If the page can be returned to, add it to the history.
        if (InstructionObject.GetComponent<UI_Instruction>().CanBeReturnedTo == true)
        {
            PageHistory.Add(InstructionObject);
        }
    }

    //Gets the previous page by looking through the PageHistory list and getting the previous page to the current.
    public GameObject GetPreviousPage()
    {
        int Index = PageHistory.Count;
        PreviousPage = PageHistory[Index - 1];
        return PreviousPage;
    }

    //Removes the page before the current from the PageHistory list.
    public void RemovePreviousPage()
    {
        int Index = PageHistory.Count;
        PageHistory.RemoveAt(Index - 1);
    }

    //Hides the game object the current button and canvas is on and then shows the target canvas.
    public void ShowNextPage(GameObject NextPage)
    {
        //If the currentpage is not null, set it to not be active. If it is, get the canvas reference and then set to not be active.
        if (CurrentPage != null)
        {
            CurrentPage.SetActive(false);
        }
        else
        {
            FindCanvasReference();
            CurrentPage.SetActive(false);
        }

        //Set the PreviousPage method.
        CurrentPage.SetActive(false);
        SetPreviousPage(CurrentPage);
        CurrentPage = NextPage;
        NextPage.SetActive(true);
    }
    #endregion

    public void PopulatePages()
    {
        for (int i = 0; i < InstructionCanvases.Length; i++)
        {
            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Title").GetComponent<TextMeshProUGUI>().text = InstructionPages[i].InstructionName;
            //set description
            //set image
            //instantiate these at some point then add this stuff
        }
    }
}