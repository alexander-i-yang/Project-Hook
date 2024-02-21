using ASK.Helpers;
using ASK.ScreenShake;
using Cinemachine;
using Helpers;
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
        public ScreenShakeDataBurst PunchData;
        public ScreenShakeDataContinuous DiveData;

        public ScreenShakeDataContinuous CurShake;

        [SerializeField] private CinemachineVirtualCamera _mainCam;

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
            _spawnManager.CurrentVCamManager.SetNoise(d.NoiseProfile);
            if (_shakeRoutine != null)
            {
                StopCoroutine(_shakeRoutine);
            }
        
            _shakeRoutine = StartCoroutine(Helper.DelayAction(d.Time, () =>
            {
                _spawnManager.CurrentVCamManager.SetNoise(null);
            }));
        }

        public void ScreenShakePunch() => ScreenShakeBurst(PunchData);
        
        public void ScreenShakeContinuousOn(ScreenShakeDataContinuous d)
        {
            _spawnManager.CurrentVCamManager.SetNoise(d.NoiseProfile);
            CurShake = d;
        }
        
        public void ScreenShakeContinuousOff(ScreenShakeDataContinuous d)
        {
            _spawnManager.CurrentVCamManager.SetNoise(null);
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