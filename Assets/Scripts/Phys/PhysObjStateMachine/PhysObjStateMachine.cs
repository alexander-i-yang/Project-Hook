using A2DK.Phys;
using ASK.Core;

namespace Phys.PhysObjStateMachine
{
    public abstract partial class PhysObjStateMachine<M, S, I, P> : StateMachine<M, S, I>
        where M : PhysObjStateMachine<M, S, I, P>
        where S : PhysObjStateMachine.PhysObjState<M, S, I, P>
        where I : PhysObjStateInput
        where P : PhysObj
    {
        private P _physObj;
        public P MyPhysObj
        {
            get
            {
                if (_physObj == null) _physObj = GetComponent<P>();
                return _physObj;
            }
        }
    }
}