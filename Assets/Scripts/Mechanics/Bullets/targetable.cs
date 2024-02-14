using UnityEngine;


    public class targetable : MonoBehaviour
    {
        [SerializeField]
        objectType type = new objectType();

        public objectType GetType(){
            return type;
        }
    }

    public enum objectType{
        Player,
        Wall,
        Other
    };