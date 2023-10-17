using System;
using Photon.Deterministic;
using Quantum.Physics2D;

namespace Quantum.Bug_Crusher.Systems;
public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.PlayerFilter>
{
    public struct PlayerFilter
    {
        public EntityRef Entity;
        public GameMaster* GameMaster;
        public MovementConfig* MovementConfig;
        public PhysicsBody2D* PhysicsBody2D;
        public Transform2D* Transform2D;

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
        
        GroundCheck(f, ref filter);
        HandleMovement(f, ref filter, input);
        HandleJump(f, ref filter, input);
        
    }

    private void HandleMovement(Frame f, ref PlayerFilter filter, Input input) =>
        filter.PhysicsBody2D->AddLinearImpulse( new FPVector2(
            (input.Direction.X * filter.MovementConfig->MoveSpeed * f.DeltaTime), FP._0));

    private void HandleJump(Frame f, ref PlayerFilter filter, Input input)
    {
        if(input.Jump.WasPressed && filter.MovementConfig->IsGrounded)
            filter.PhysicsBody2D->AddLinearImpulse(FPVector2.Up * filter.MovementConfig->JumpHeight);
    }

    private void GroundCheck(Frame f, ref PlayerFilter filter)
    {
        #region Debug
        Draw.Line(filter.Transform2D->Position,
            new FPVector2(filter.Transform2D->Position.X, filter.Transform2D->Position.Y - filter.MovementConfig->GroundHeight));
        #endregion
        var isGrounded = f.Physics2D.Linecast(filter.Transform2D->Position, 
           new FPVector2(filter.Transform2D->Position.X , filter.Transform2D->Position.Y - filter.MovementConfig->GroundHeight)).HasValue;
       filter.MovementConfig->IsGrounded = isGrounded;
    }
    
} 