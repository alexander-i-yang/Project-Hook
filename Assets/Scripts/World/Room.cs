using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using ASK.Helpers;

namespace World {
    public class Room : MonoBehaviour {
        private Collider2D _roomCollider;
        // public CinemachineVirtualCamera VCam { get; private set; }
        private VCamManager _vcam;

        public VCamManager VCamManager
        {
            get
            {
                if (_vcam == null) _vcam = GetComponentInChildren<VCamManager>();
                return _vcam;
            }
        }

        private CinemachineBrain _cmBrain;

        public bool StopTime = true;
        
        private GameObject _grid;
        private static Coroutine _transitionRoutine;

        //Invoked whenever child objects change. Mostly used for player spawns
        public event Action RecalculateChildren;

        private int _numEnables;
        [SerializeField] private int _maxEnables = 4;

        public delegate void OnRoomTransition(Room roomEntering);
        public static event OnRoomTransition RoomTransitionEvent;

        public Room[] AdjacentRooms;
        // private EndCutsceneManager _endCutsceneManager;

        private void Awake()
        {
            _roomCollider = GetComponent<Collider2D>();
            _cmBrain = FindObjectOfType<CinemachineBrain>(true);

            // _endCutsceneManager = FindObjectOfType<EndCutsceneManager>();
            RecalculateChildren?.Invoke();
            
            _grid = transform.GetChild(0).gameObject;
        }

        /*private void OnEnable()
        {
            EndCutsceneManager.BeegBounceStartEvent += TurnOffStopTime;
        }

        private void OnDisable()
        {
            EndCutsceneManager.BeegBounceStartEvent -= TurnOffStopTime;
        }*/

        void TurnOffStopTime()
        {
            StopTime = false;
        }

        /*private void Update()
        {
            float dist2CameraToRoomCenter = Vector3.SqrMagnitude(Camera.main.transform.position - _roomCollider.transform.position);
            bool shouldEnable = dist2CameraToRoomCenter < _roomCollider.bounds.size.sqrMagnitude * 1.1;
            if (_player.CurrentRoom == this) SetRoomGridEnabled(true);
            if (shouldEnable != _grid.gameObject.activeSelf)
            {
                SetRoomGridEnabled(shouldEnable);
            }
        }*/

        //Source: https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
        public static Bounds OrthograpicBounds(Camera camera)
        {
            float screenAspect = (float) Screen.width / (float) Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        /**
         * Does this room fully contain the bounds of other?
         */
        public bool ContainsCollider(Collider2D other) => _roomCollider.bounds.Contains(other.bounds.min) && _roomCollider.bounds.Contains(other.bounds.max);

        public float GetRoomSize()
        {
            Vector3 dims = _roomCollider.bounds.size;
            return dims.x * dims.y;
        }

        public virtual void TransitionToThisRoom()
        {
            SetRoomGridEnabled(true);
            Reset();
            // FilterLogger.Log(this, $"Transitioned to room: {gameObject.name}");
            if (_transitionRoutine != null)
            {
                StopCoroutine(_transitionRoutine);
            }
            _transitionRoutine = StartCoroutine(TransitionRoutine());
        }

        private IEnumerator TransitionRoutine()
        {
            SwitchCamera();
            bool shouldStopTime = StopTime;
            
            if (shouldStopTime) Time.timeScale = 0f;
            /*
             * This is kinda "cheating". Instead of waiting for the camera to be done switching,
             * we're just waiting for the same amount of time as the blend time between cameras.
             */
            yield return new WaitForSecondsRealtime(_cmBrain.m_DefaultBlend.BlendTime);
            if (shouldStopTime) Time.timeScale = 1f;
            _transitionRoutine = null;
            RoomTransitionEvent?.Invoke(this);
        }

        protected void SwitchCamera()
        {
            //L: Inefficient, but not terrible?
            VCamManager.gameObject.SetActive(true);
            foreach (Room room in RoomList.Rooms)
            {
                if (room != this)
                {
                    room.VCamManager.gameObject.SetActive(false);
                }
            }
        }
        
        public void Reset()
        {
            /*foreach (var r in _resettables)
            {
                if (r != null && r.CanReset()) r.Reset();
            }*/
        }
        
        public void SetRoomGridEnabled(bool setActive)
        {
            _grid.SetActive(setActive);
        }

        private void DestroyAndRecreateGrid()
        {
            GameObject gridObj = _grid.gameObject; 
            var newGrid = Instantiate(
                gridObj,
                gridObj.transform.position,
                Quaternion.identity,
                gridObj.transform.parent
            );

            gridObj.transform.parent = null; //This is so the mechanics in _grid aren't counted in FetchMechanics.
            Destroy(gridObj);
            _grid = newGrid;
            RecalculateChildren?.Invoke();
        }

        public void RoomSetEnable(bool enable)
        {
            if (enable)
            {
                _numEnables++;
                if (_numEnables > _maxEnables)
                {
                    DestroyAndRecreateGrid();
                }
            }
            SetRoomGridEnabled(enable);
            Reset();
        }

        public LogLevel GetLogLevel()
        {
            return LogLevel.Error;
        }

        /*public Door[] CalcAdjacentDoors(Vector2 doorAdjacencyTolerance, LayerMask doorLayerMask)
        {
            if (_roomCollider == null) _roomCollider = GetComponent<Collider2D>();
            var bounds = _roomCollider.bounds;
            Vector2 pointB = (Vector2)bounds.max + doorAdjacencyTolerance;
            Vector2 pointA = (Vector2)bounds.min - doorAdjacencyTolerance;

            var innerDoors = GetComponentsInChildren<Door>();

            var hits = Physics2D.OverlapAreaAll(pointA, pointB, doorLayerMask);
            List<Door> ret = new();
            foreach (var hit in hits)
            {
                Door d = hit.GetComponent<Door>();
                if (d != null && !innerDoors.Contains(d))
                {
                    ret.Add(d);
                }
            }

            return ret.ToArray();
        }*/

        public Room[] CalcAdjacentRooms(Vector2 roomAdjacencyTolerance, LayerMask roomLayerMask)
        {
            if (_roomCollider == null) _roomCollider = GetComponent<Collider2D>();
            var bounds = _roomCollider.bounds;
            Vector2 pointB = (Vector2)bounds.max + roomAdjacencyTolerance;
            Vector2 pointA = (Vector2)bounds.min - roomAdjacencyTolerance;

            var hits = Physics2D.OverlapAreaAll(pointA, pointB, roomLayerMask);
            List<Room> ret = new();
            foreach (var hit in hits)
            {
                Room r = hit.GetComponent<Room>();
                if (r != null && r != this)
                {
                    ret.Add(r);
                }
            }

            return ret.ToArray();
        }
    }
}