namespace Quantum.Bug_Crusher.Systems;
public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.PlayerFilter>
{
    public struct PlayerFilter
    {
        public EntityRef Entity;
        public CharacterController2D* CharacterController;
        public GameMaster* GameMaster;
    }
 
    public override void Update(Frame f, ref PlayerFilter filter)
    {
        if(f.Unsafe.TryGetPointer(filter.Entity, out GameMaster* gameMaster))
            if(gameMaster->IsZeus == false)
                CollectInput(f, ref filter);
    }

    private void CollectInput(Frame f, ref PlayerFilter filter)
    {
        Input input = default;
        if(f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
            input = *f.GetPlayerInput(playerLink->Player);
        if(input.Jump.WasPressed)
            Log.Debug("Jumping.....");
    }
}