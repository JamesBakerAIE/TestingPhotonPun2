using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

namespace Client.Player.CameraControls
{
    public class CameraShake : MonoBehaviourPun
    {
        #region Static Fields

        public static CameraShake Instance { get; private set; }

        #endregion

        #region Private Fields

        private CinemachineVirtualCamera virtualCamera = null;
        private CinemachineBasicMultiChannelPerlin basicMultiChannelPerlin = null;
        private float shakeTimer = 0;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (photonView.IsMine == false)
            {
                return;
            }
            Instance = this;
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            if(photonView.IsMine == false)
            {
                return;
            }

            Instance = this;
            if (basicMultiChannelPerlin == null)
            {
                basicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }

            basicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
        }

        private void Update()
        {
            if (photonView.IsMine == false)
            {
                return;
            }

            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;

                if(shakeTimer <= 0)
                {
                    if(basicMultiChannelPerlin == null)
                    {
                        basicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    }

                    basicMultiChannelPerlin.m_AmplitudeGain = 0;
                }
            }
        }

        #endregion
    }

}
