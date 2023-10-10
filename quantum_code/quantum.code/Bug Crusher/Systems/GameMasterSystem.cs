namespace Quantum.Bug_Crusher.Systems;

public unsafe class GameMasterSystem :  SystemMainThreadFilter<GameMasterSystem.ZeusFilter>
{
    public struct ZeusFilter
    {
        public EntityRef Entity;
        public GameMaster* GameMaster;
    }

    public override void Update(Frame f, ref ZeusFilter filter)
    {
        if(f.Unsafe.TryGetPointer(filter.Entity, out GameMaster* gameMaster))
            if(gameMaster->IsZeus == true)
                CollectInput(f, ref filter);
    }

    private void CollectInput(Frame f, ref ZeusFilter filter)
    {
        Input input = default;
        if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            input = *f.GetPlayerInput(playerLink->Player);
        
        if(input.Jump.WasPressed)
            Log.Debug("Adding Force...............");
                       
    }
}