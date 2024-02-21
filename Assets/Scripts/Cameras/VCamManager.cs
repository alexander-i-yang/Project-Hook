using System;
using System.Collections.Generic;
using Cameras;
using Cinemachine;
using UnityEngine;
using VFX;

namespace World
{
    public class VCamManager : MonoBehaviour
    {
        private Dictionary<int, CinemachineVirtualCamera> _vCams = new();
        public Dictionary<int, CinemachineVirtualCamera> VCams {
            get
            {
                if (_vCams.Count == 0)
                {
                    var vcams = GetComponentsInChildren<CinemachineVirtualCamera>();
                    foreach (var vcam in vcams)
                    {
                        _vCams.Add(vcam.gameObject.layer, vcam);
                    }
                }
                return _vCams;
            }
        }

        private void Awake()
        {
            DefaultFollow f = FindObjectOfType<DefaultFollow>();
            SetFollow(f.transform);
        }

        public void SetFollow(Transform p)
        {
            foreach (var v in VCams) v.Value.Follow = p;
        }

        public void SetNoise(NoiseSettings dNoiseProfile)
        {
            foreach (var v in VCams)
                v.Value.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = dNoiseProfile;
        }

        public void SetConfiner(Collider2D boundingShape)
        {
            foreach (var v in VCams)
                v.Value.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = boundingShape;
        }

        public CinemachineVirtualCamera GetVCam(int layer) => VCams[layer];
        public Vector2 GetVCamOffset(int layer) => VCams[layer].GetCinemachineComponent<CinemachinePixelTransposer>().Offset;
    }
}