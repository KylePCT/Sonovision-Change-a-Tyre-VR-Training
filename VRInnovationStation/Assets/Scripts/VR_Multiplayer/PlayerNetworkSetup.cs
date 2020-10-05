using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRigGameobject; //XR Rig under the VR Character.
    public GameObject MainAvatarGameobject;

    public GameObject AvatarHeadGameobject;
    public GameObject AvatarBodyGameobject;

    public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerName_Text;

    // Start is called before the first frame update
    void Start()
    {
        //Setup the player.

        //Is the player setup the local player?
        if (photonView.IsMine)
        {
            LocalXRRigGameobject.SetActive(true);
            
            gameObject.GetComponent<MovementController>().enabled = true; //Allows the local rig to be moved.
            gameObject.GetComponent<AvatarInputConverter>().enabled = true;

            AvatarHeadGameobject.layer = 30;
            AvatarBodyGameobject.layer = 31;

            //Get the Avatar selection data for the correct model to be displayed.
            object avatarSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                Debug.Log("Avatar Selection Number: <" + (int)avatarSelectionNumber + ">.");

                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int)avatarSelectionNumber);
            }

            ////Set the local Avatar's head and body to their own layers so the camera in the scene can cull them and not see them.
            //SetLayerRecursively(AvatarHeadGameobject, 12);
            //SetLayerRecursively(AvatarBodyGameobject, 12);
            AvatarHeadGameobject.SetActive(false); //Just remove the head incase, LayerRecursive is buggy.

            //Set up teleportation for local player.
            //Will look for teleportation areas when the rig is instantiated and assign them.
            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if (teleportationAreas.Length > 0)
            {
                Debug.Log("Found <" + teleportationAreas.Length + "> teleportation areas.");
            }

            foreach (var item in teleportationAreas)
            {
                item.teleportationProvider = LocalXRRigGameobject.GetComponent<TeleportationProvider>();
            }

            //Added through scripting so there is only one audio listener in the scene.
            MainAvatarGameobject.AddComponent<AudioListener>();
        }

        //Is the player the remote player?
        else
        {
            LocalXRRigGameobject.SetActive(false);

            //Turn off the movement for the remote player => stops all rigs following the local player and lets everyone control their own rig.
            gameObject.GetComponent<MovementController>().enabled = false;
            gameObject.GetComponent<AvatarInputConverter>().enabled = false;

            ////Set the Avatar's head and body to layer 'Default', which allows them to be seen by Main Cameras in the scene. => Allows remote players to be seen by the local player.

            AvatarHeadGameobject.layer = 0;
            AvatarBodyGameobject.layer = 0;

            AvatarHeadGameobject.SetActive(true); //Just enable the head incase, LayerRecursive is buggy.
        }

        //Shows player names for all users.
        if (PlayerName_Text != null)
        {
            PlayerName_Text.text = photonView.Owner.NickName;
        }
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    //Set layers through code.
    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    //Instantiate and setup the player model for all clients.
    [PunRPC] //Executed for all remote players.
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        //Instantiate as child of LocalXRRig.
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], LocalXRRigGameobject.transform);

        //Access InputConvertr and Holder.
        AvatarInputConverter avatarInputConverter = transform.GetComponent<AvatarInputConverter>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();

        //Set the pieces for Avatar Transforms.
        SetUpAvatarGameobject(avatarHolder.HeadTransform, avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform, avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        //Set as child of the main Avatar transform and integrate.
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }
}
