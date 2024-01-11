using UnityEngine;

namespace Combat
{
    public interface IPunchable
    {
        public bool ReceivePunch(Vector2 v);
    }
}