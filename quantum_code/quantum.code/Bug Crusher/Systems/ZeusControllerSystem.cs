using Photon.Deterministic;

namespace Quantum.Bug_Crusher.Systems;

public unsafe class ZeusControllerSystem :  SystemMainThreadFilter<ZeusControllerSystem.ZeusFilter>
{
    public struct ZeusFilter
    {
        public EntityRef Entity;
        public GameMaster* GameMaster;
    }

    public override void Update(Frame f, ref ZeusFilter filter)
    {
        if(filter.GameMaster->IsZeus == true)
            CollectInput(f, ref filter);
    }

    private void CollectInput(Frame f, ref ZeusFilter filter)
    {
        Input input = default;
        if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            input = *f.GetPlayerInput(playerLink->Player);
        if (input.Jump.WasPressed)
        {
            TestFunc(f);
        }
    }

    private void TestFunc(Frame f)
    {
        var filtered = f.Filter<TestWall>();

        while (filtered.Next(out var entity, out var t))
        {
            f.Signals.OnForceAdded(entity);          
        }
    }
}