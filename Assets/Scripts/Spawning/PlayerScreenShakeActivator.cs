using ASK.ScreenShake;
using Player;
using UnityEditor;
using UnityEngine;

namespace Spawning
{
    [RequireComponent(typeof(PlayerSpawnManager))]
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerScreenShakeActivator : ScreenShakeActivator
    {
        private PlayerSpawnManager _spawnManager;
        private PlayerCore _core;
        
        public ScreenShakeDataBurst DeathData;
        public ScreenShakeDataContinuous DiveData;

        public ScreenShakeDataContinuous CurShake;

        private void Awake()
        {
            _spawnManager = GetComponent<PlayerSpawnManager>();
        }

        private void OnEnable()
        {
            // Room.RoomTransitionEvent += SwitchRooms;
            _core.DeathManager.OnDeath += DeathScreenShake;
        }
        
        private void OnDisable()
        {
            // Room.RoomTransitionEvent -= SwitchRooms;
            _core.DeathManager.OnDeath -= DeathScreenShake;
        }

        public void ScreenShakeBurst(ScreenShakeDataBurst d)
        {
            base.ScreenShakeBurst(_spawnManager.CurrentVCam, d);
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