using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShowIssueMessage : MonoBehaviour
{
    public Animator UIIssue;

    // Start is called before the first frame update
    void Start()
    {
        UIIssue = GetComponent<Animator>();
    }

    public void StartIssueMessageFade()
    {
        UIIssue.SetBool("CanAnimate", true);
    }
}
