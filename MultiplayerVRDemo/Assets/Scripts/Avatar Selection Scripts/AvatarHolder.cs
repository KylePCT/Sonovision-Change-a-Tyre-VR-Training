using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarHolder : MonoBehaviour
{

    public Transform MainAvatarTransform;
    public Transform HeadTransform;
    public Transform BodyTransform;
    public Transform HandLeftTransform;
    public Transform HandRightTransform;

    private void Start()
    {
        //Setting the layer of avatar head to AvatarLocalHead layer so that it does not block the view of the local VR Player
        SetLayerRecursively(HeadTransform.gameObject, 11);

        //Setting the layer of avatar body to AvatarLocalBody layer so that it does not block the view of the local VR Player
        SetLayerRecursively(BodyTransform.gameObject, 12);
    }
    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
