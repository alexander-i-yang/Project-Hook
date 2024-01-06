using A2DK.Phys;
using ASK.Core;

namespace Phys.PhysObjStateMachine
{
    public abstract partial class PhysObjStateMachine
    {
        public abstract class PhysObjState<M, S, I, P> : State<M, S, I>
            where M : PhysObjStateMachine<M, S, I, P>
            where S : PhysObjState<M, S, I, P>
            where I : PhysObjStateInput 
            where P : PhysObj
        {
        }
    }
}