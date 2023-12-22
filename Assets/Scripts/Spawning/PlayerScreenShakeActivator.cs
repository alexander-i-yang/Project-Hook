using ASK.Helpers;
using ASK.ScreenShake;
using Cinemachine;
using Player;
using UnityEditor;
using UnityEngine;

namespace Spawning
{
    [RequireComponent(typeof(PlayerSpawnManager))]
    [RequireComponent(typeof(PlayerDeathManager))]
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerScreenShakeActivator : ScreenShakeActivator
    {
        private PlayerSpawnManager _spawnManager;
        private PlayerDeathManager _deathManager;
        
        public ScreenShakeDataBurst DeathData;
        public ScreenShakeDataContinuous DiveData;

        public ScreenShakeDataContinuous CurShake;

        private void Awake()
        {
            _spawnManager = GetComponent<PlayerSpawnManager>();
            _deathManager = GetComponent<PlayerDeathManager>();
        }

        private void OnEnable()
        {
            // Room.RoomTransitionEvent += SwitchRooms;
            _deathManager.OnDeath += DeathScreenShake;
        }
        
        private void OnDisable()
        {
            // Room.RoomTransitionEvent -= SwitchRooms;
            _deathManager.OnDeath -= DeathScreenShake;
        }

        private Coroutine _shakeRoutine;
        public void ScreenShakeBurst(ScreenShakeDataBurst d)
        {
            // base.ScreenShakeBurst(_spawnManager.CurrentVCam, d);
            var c = _spawnManager.CurrentVCam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
            c.m_NoiseProfile = d.NoiseProfile;
            print(c.m_NoiseProfile);

            if (_shakeRoutine != null)
            {
                StopCoroutine(_shakeRoutine);
            }
        
            _shakeRoutine = StartCoroutine(Helper.DelayAction(d.Time, () =>
            {
                c.m_NoiseProfile = null;
            }));
        }
        
        public void ScreenShakeContinuousOn(ScreenShakeDataContinuous d)
        {
            base.ScreenShakeContinuousOn(_spawnManager.CurrentVCam, d);
            CurShake = d;
        }
        
        public void ScreenShakeContinuousOff(ScreenShakeDataContinuous d)
        {
            base.ScreenShakeContinuousOff(_spawnManager.CurrentVCam, d);
            CurShake = null;
        }

        private void DeathScreenShake() => ScreenShakeBurst(DeathData);

        /*public void SwitchRooms(Room r)
        {
            //Note: since CurShake doesn't get set to null from a burst shake,
            //there may be a bug caused by diving into lava
            //But it works just fine
            if (CurShake != null)
            {
                if (_spawnManager.CurrentRoom != null)
                {
                    // print("SC off");
                    base.ScreenShakeContinuousOff(
                        _spawnManager.CurrentRoom.VCam,
                        CurShake
                    );
                }
                // print("SC on");
                /*base.ScreenShakeContinuousOn(
                    r.VCam,
                    CurShake
                )#1#;
            }
        }*/
    }
}