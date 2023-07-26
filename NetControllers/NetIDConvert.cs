using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets._NETWORK
{
    public static class NetIDConvert
    {
        public static (string, Sprite, Action) GetAttributesByID(string ID)
        {
            (string, Sprite, Action) value = (null, null, null);

            switch (ID)
            {
                case "001":

                    value = CreateAttributes("testItem", null, delegate ()
                    {
                        Debug.Log("TestITEM");
                    });

                    break;
                case "002":

                    value = CreateAttributes("Flashlight", null, delegate ()
                    {
                        GameObject.FindGameObjectWithTag("Player").GetComponent<RigBuilder>().enabled = true;

                        var item = PhotonNetwork.Instantiate("FlashlightHand", Vector3.zero, Quaternion.identity);

                        item.transform.parent = GameObject.FindGameObjectWithTag("LEFTHAND").transform;
                        item.transform.localPosition = new Vector3(0.1584507f, 0.2797196f, 0.002624063f);
                        item.transform.localRotation = Quaternion.Euler(-49.638f, 106.385f, 50.666f);
                        item.transform.localScale = new Vector3(2.1733f, 2.1733f, 2.1733f);

                        GameObject.FindGameObjectWithTag("LEFTHANDRIG").GetComponent<Rig>().weight = 1f;
                    });

                    break;
                case "003":

                    value = CreateAttributes("First Aid", null, delegate ()
                    {
                        Debug.Log("First Aid");
                    });

                    break;
                case "004":
                    break;
                case "005":
                    break;
                case "006":
                    break;
                case "007":
                    break;
                case "008":
                    break;
                case "009":
                    break;
                case "010":
                    break;
                case "011":
                    break;
                case "012":
                    break;
                case "013":
                    break;
                case "014":
                    break;
            }

            return value;
        }

        private static (string, Sprite, Action) CreateAttributes(string name, Sprite sprite, Action action)
        {
            return (name, sprite, action);
        }
    }
}
