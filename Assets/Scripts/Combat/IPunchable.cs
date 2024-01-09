using UnityEngine;

namespace Combat
{
    public interface IPunchable
    {
        public void ReceivePunch(Vector2 v);
    }
}