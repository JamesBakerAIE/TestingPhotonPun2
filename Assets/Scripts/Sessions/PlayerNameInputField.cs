using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


namespace Networking.LobbyServices.UI
{
    /// <summary>
    /// Player name input field. Let the user input their name.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerUserNamePrefKey = "UserName";

        #endregion


        #region MonoBehaviour CallBacks

        void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if(inputField != null)
            {
                if(PlayerPrefs.HasKey(playerUserNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerUserNamePrefKey);
                    inputField.text = defaultName;
                }
            }
            else
            {
                Debug.LogError("A TextMeshPro Input Field is not attached to this object");
            }


            PhotonNetwork.NickName = defaultName;

        }

        void Update()
        {

        }

        #endregion


        #region Public Methods

        public void SetUserName(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogError("Username is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerUserNamePrefKey, value);
        }

        #endregion




    }

}
