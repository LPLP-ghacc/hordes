using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviourPunCallbacks
{
    public void OnSettingsClick()
    {
        GameObject.Find("Canvas").transform.Find("mainmenu").gameObject.SetActive(!GameObject.Find("Canvas").transform.Find("mainmenu").gameObject.activeSelf);
        GameObject.Find("Canvas").transform.Find("Settings").gameObject.SetActive(!GameObject.Find("Canvas").transform.Find("Settings").gameObject.activeSelf);
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.CountOfRooms == 0)
            CreateRoom();
        else
            PhotonNetwork.JoinRoom("_GLOBAL");
        
    }

    public void CreateRoom()
    {
        RoomOptions roomOp = new RoomOptions();
        roomOp.MaxPlayers = 0;
        PhotonNetwork.CreateRoom("_GLOBAL", roomOp);
    }

    //lol
    private IEnumerator TryCreateRoomAgain()
    {
        yield return new WaitForSeconds(2);

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LEVEL1HUB UNBAKED");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        TryCreateRoomAgain();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void DebugMenu(int setting)
    {
        switch (setting)
        {
            case 0:
                Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, true, Screen.resolutions[Screen.resolutions.Length - 1].refreshRate);
                GameObject.FindGameObjectWithTag("UI").GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height);
                break;

        }
    }
}
