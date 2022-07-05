using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomManager : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    #region Rules parameters

    public bool separateControls { get; private set; }

    #endregion

    private static RoomManager instance;

    private const byte playerQuitEventCode = 1;
    private string leavingPlayer;
    public static RoomManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }


    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #region ======================= Public : Start  =======================

    public void SetSeparationControlsState(bool state)
    {
        separateControls = state;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(separateControls);
        }
        else
        {
            separateControls = (bool)stream.ReceiveNext();
        }
    }

    public void OnPlayerLeave()
    {
        object[] content = new object[] { PhotonNetwork.NickName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(playerQuitEventCode, content, raiseEventOptions, SendOptions.SendReliable);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MultiplayerMenuScene");
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == playerQuitEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            leavingPlayer = (string)data[0];
            QuitEnum quit = (QuitEnum)data[1];
            if (leavingPlayer != PhotonNetwork.NickName)
            {
                string quitReason;
                switch (quit)
                {
                    case QuitEnum.Dead:
                        quitReason = "has been captured";
                        break;
                    case QuitEnum.Escape:
                        quitReason = "has escaped";
                        break;
                    case QuitEnum.Quit:
                    default:
                        quitReason = "has left the game";
                        break;
                }

                NotificationManager.Instance.DisplayNotification(leavingPlayer + " " + quitReason);
            }
        }
    }

    #endregion ======================= Public : Start  =======================

    #region ======================= Private : Start  =======================

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) //if game secene 
        {
            //Instantiate RoomMangar at (0,0,0) because it's a Empty
            PhotonNetwork.Instantiate(Path.Combine(MultiplayerManager.PhotonPrefabPath, "PlayerManager"), Vector3.zero,
                Quaternion.identity);
        }
    }

    #endregion ======================= Private : Start  =======================
}