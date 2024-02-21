using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace World
{
    public class VCamManager : MonoBehaviour
    {
        private CinemachineVirtualCamera[] _vCams;
        public CinemachineVirtualCamera[] VCams {
            get
            {
                if (_vCams == null) _vCams = GetComponentsInChildren<CinemachineVirtualCamera>();
                return _vCams;
            }
        }


        public void SetFollow(Transform playerTransform)
        {
            foreach (var v in VCams) v.Follow = playerTransform;
        }

        public void SetNoise(NoiseSettings dNoiseProfile)
        {
            foreach (var v in VCams)
                v.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = dNoiseProfile;
        }

        public void SetConfiner(Collider2D boundingShape)
        {
            foreach (var v in VCams)
                v.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = boundingShape;
        }
    }
}