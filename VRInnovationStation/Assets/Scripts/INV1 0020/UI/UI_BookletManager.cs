using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class UI_BookletManager : MonoBehaviour
{
    [Header("Page Lists")]
    public UI_Instruction[] InstructionPages;
    public List<GameObject> InstructionObjects;
    public GameObject[] InstructionCanvases;

    [SerializeField]
    [Header("Page Single References")]
    public GameObject Tablet;
    public GameObject CanvasParent;
    public GameObject CanvasCheckObj;
    public GameObject IntroductionPage;

    [SerializeField]
    [Header("Page History")]
    private GameObject PreviousPage;
    public List<GameObject> PageHistory;
    public GameObject CurrentPage;

    private void OnEnable()
    {
    }

    void Awake()
    {

        //Finds all the instruction pages and adds them to the InstructionObjects list.
        InstructionPages = FindObjectsOfType<UI_Instruction>();
        //Sort the pages in Ascending order by their name.
        InstructionPages = InstructionPages.OrderBy(c => c.name).ToArray();

        foreach (UI_Instruction instruction in InstructionPages)
        {
            GameObject NewCanvas = Instantiate(Resources.Load("InstructionCanvas")) as GameObject;
            InstructionObjects.Add(instruction.gameObject);
        }

        InstructionCanvases = GameObject.FindGameObjectsWithTag("InstructionCanvas");

        IntroductionPage = InstructionCanvases[0];


        for (int i = 0; i < (InstructionCanvases.Length); i++)
        {
            if (InstructionPages[i].InstructionNumberSuffix == null)
            {
                InstructionCanvases[i].name = "UI_Canvas_" + InstructionPages[i].InstructionNumber + "_" + InstructionPages[i].InstructionNameShort;
            }
            else
            {
                InstructionCanvases[i].name = "UI_Canvas_" + InstructionPages[i].InstructionNumber + InstructionPages[i].InstructionNumberSuffix + "_" + InstructionPages[i].InstructionNameShort;
            }

            if (i != 0) //If the index is not 0, set the next pages.
            {
                InstructionPages[i - 1].NextInstruction = InstructionCanvases[i];
            }

            InstructionCanvases[i].gameObject.transform.SetParent(CanvasParent.gameObject.transform, false);
            InstructionCanvases[i].gameObject.transform.position = CanvasParent.gameObject.transform.position;
        }

        PopulatePages();
        CurrentPage = IntroductionPage;

        InstructionCanvases = InstructionCanvases.OrderBy(c => c.name).ToArray();
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
        //CurrentPage = IntroductionPage;
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
                Debug.Log("<color=cyan>[UI_BookletManager.cs]</color> Couldn't find canvas. Re-running with parent of " + CanvasCheckObj + ".");
                CanvasCheckObj = CanvasCheckObj.transform.GetChild(0).gameObject;
                FindCanvasReference();
            }
        }
    }

    //InstructionObject is CurrentPage here!
    public void SetPreviousPage(GameObject InstructionObject)
    {
        Debug.Log("<color=cyan>[UI_BookletManager.cs]</color> Previous Page / CanBeReturned is " + InstructionObject.GetComponentInChildren<UI_HoldHistoryBool>().CanBeReturnedToWithBack + ".");
        //If the page can be returned to, add it to the history.
        if (InstructionObject.GetComponentInChildren<UI_HoldHistoryBool>().CanBeReturnedToWithBack == true)
        {
            PageHistory.Add(InstructionObject);
        }

        else
        {
            InstructionObject = null;
        }
    }

    //Gets the previous page by looking through the PageHistory list and getting the previous page to the current.
    public GameObject GetPreviousPage()
    {
        int Index = PageHistory.Count;
        PreviousPage = PageHistory[Index - 1];
        Debug.Log("<color=cyan>[UI_BookletManager.cs] Previous Page is: </color> " + PreviousPage);
        return PreviousPage;
    }

    //Removes the page before the current from the PageHistory list.
    public void RemovePreviousPage()
    {
        int Index = PageHistory.Count;
        PageHistory.RemoveAt(Index - 1);
    }

    //Finds the last active page amd assigns it.
    public void ShowPreviousPage()
    {
        Debug.Log("<color=cyan>[UI_BookletManager.cs] </color>Showing previous page: " + GetPreviousPage());
        CurrentPage.SetActive(false);
        RemovePreviousPage();
        GetPreviousPage().SetActive(true);
    }

    //Hides the game object the current button and canvas is on and then shows the target canvas.
    public void ShowNextPage(GameObject NextPage)
    {
        SetPreviousPage(CurrentPage);

        CurrentPage.SetActive(false);
        CurrentPage = NextPage;
        CurrentPage.SetActive(true);

        //If the currentpage is not null, set it to not be active. If it is, get the canvas reference and then set to not be active.
        if (CurrentPage != null)
        {
            NextPage.SetActive(true);
            CurrentPage.SetActive(false);
        }
        else
        {
            NextPage.SetActive(true);
            FindCanvasReference();
            CurrentPage.SetActive(false);
        }

        //Set the PreviousPage method.
        NextPage.SetActive(true);
        CurrentPage = NextPage;
        //maybe clear listener from button idk
    }
    #endregion

    public void PopulatePages()
    {
        for (int i = 0; i < (InstructionCanvases.Length); i++)
        {
            Debug.Log("<color=cyan>[UI_BookletManager.cs] </color>" + InstructionPages[i].NextInstruction);
            int temp_i = i;
            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Title").GetComponent<TextMeshProUGUI>().text = InstructionPages[temp_i].InstructionName;
            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/ScrollArea/TextContainer/Description").GetComponent<TextMeshProUGUI>().text = InstructionPages[temp_i].InstructionDescription;

            if (InstructionPages[temp_i].InstructionImageGuide != null)
            {
                InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Image").GetComponent<Image>().sprite = InstructionPages[temp_i].InstructionImageGuide;
            }
            else
            {
                InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Image").gameObject.SetActive(false);
            }

            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Back").GetComponent<Button>().onClick.AddListener(() => Debug.Log("<color=cyan>[UI_BookletManager.cs] </color>Previous page clicked: " + PageHistory[0]));
            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Back").GetComponent<Button>().onClick.AddListener(() => ShowPreviousPage());

            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Forward").GetComponent<Button>().onClick.AddListener(() => Debug.Log("<color=cyan>[UI_BookletManager.cs] </color>Next page clicked: " + InstructionPages[temp_i].NextInstruction));
            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel/Forward").GetComponent<Button>().onClick.AddListener(() => ShowNextPage(InstructionPages[temp_i].NextInstruction));

            InstructionCanvases[i].gameObject.transform.Find("InstructionPanel").GetComponent<UI_HoldHistoryBool>().CanBeReturnedToWithBack = InstructionPages[temp_i].CanBeReturnedToWithBack;

            Debug.Log("Canvas: <" + InstructionCanvases[i].name + "> populated.");
        }
    }
}