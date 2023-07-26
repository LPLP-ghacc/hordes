using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkingDamageSystem : MonoBehaviour
{
    [Header("Player fields")]
    public PlayerFields fields;

    [Header("UI")]
    public TextMeshProUGUI healthTextBox;

    [Header("Main weapon")]
    public GameObject weapon;

    public GameObject[] decals;

    [Space]
    public bool isPlayerDie = false;

    public int selfHp = 100;

    public float sendDelay;

    public GameObject originCamRoot;

    public GameObject onSendDamageCamRoot;

    private PhotonView player;
    private GameObject self;
    private Animator animator;

    public GameObject[] weapons;

    void Start()
    {
        player = this.GetComponent<PhotonView>();

        if(player.IsMine)
        {
            animator = fields.GetComponent<PlayerFields>().NETWORK_PLAYER_ANIMATOR;

            self = this.gameObject;
        }
    }

    public void Update()
    {
        if (player.IsMine)
        {
            CheckDie();
            ChangeWeapon();

            healthTextBox.text = $"{selfHp}";
        }
    }

    #region ChangeWeapon

    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.RPC("SetWeapon", RpcTarget.All, 0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.RPC("SetWeapon", RpcTarget.All, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.RPC("SetWeapon", RpcTarget.All, 2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.RPC("SetWeapon", RpcTarget.All, 3);
        }
    }

    [PunRPC]
    public void SetWeapon(int weapon)
    {
        if(weapon <= weapons.Length)
        {
            weapons[weapon].SetActive(true);

            foreach(var obj in weapons)
            {
                if(obj != weapons[weapon])
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    #endregion

    public void ReceiveDamage(int damage, string playername)
    {
        if (player.IsMine)
        {
            Debug.Log(selfHp + $" taken damage from {playername}");
            animator.StopPlayback();
            animator.Play("recieve", 0, 1);
            //animator.SetTrigger("resieve");

            if (selfHp > damage) selfHp -= damage; 
            else selfHp = 0;
        }
    }

    IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private void CheckDie()
    {
        if (selfHp <= 0)
        {
            Debug.Log($"player: {self.gameObject.name} was die!");
            isPlayerDie = true;
            animator.SetTrigger("dead");
            Respawn(3f);
        }

        if(selfHp < 50)
        {
            player.RPC("InitBloodEff", RpcTarget.All, selfHp);
        }
        else if (selfHp < 25)
        {
            player.RPC("InitBloodEff", RpcTarget.All, selfHp);
        }
    }

    [PunRPC]
    public void InitBloodEff(int hp)
    {
        if(hp < 50)
        {
            decals[0].SetActive(true);
        }
        else if(hp < 25)
        {
            decals[1].SetActive(true);
        }
    }
}
