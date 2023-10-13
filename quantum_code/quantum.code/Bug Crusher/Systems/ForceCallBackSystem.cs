using Photon.Deterministic;

namespace Quantum.Bug_Crusher.Systems;

public unsafe class ForceCallBackSystem : SystemSignalsOnly, ISignalOnForceAdded
{
    public void OnForceAdded(Frame f, EntityRef entity)
    {
        if (f.Unsafe.TryGetPointer(entity, out TestWall* testWall))
        {
            if (f.Unsafe.TryGetPointer(entity, out PhysicsBody2D* pb))
            {
                pb->AddLinearImpulse(new FPVector2(0, testWall->ForceAmount));       
            }
        }
    }
}