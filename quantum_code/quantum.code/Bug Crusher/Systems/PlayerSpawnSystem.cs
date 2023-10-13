using System.Diagnostics;
using Photon.Deterministic;

namespace Quantum.Bug_Crusher.Systems;

public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
{
    public void OnPlayerDataSet(Frame f, PlayerRef player)
    {
        var data = f.GetPlayerData(player);
        EntityPrototype prototype = f.FindAsset<EntityPrototype>(IsMasterClient(player) ? data.ZeusPrototype.Id : data.CharacterPrototype.Id);

        var entity = f.Create(prototype);
        var playerLink = new PlayerLink()
        {
            Player = player,
        };
        var gameMaster = new GameMaster()
        {
            IsZeus = IsMasterClient(player),
        };
        f.Add(entity, playerLink);
        f.Add(entity, gameMaster);

        if (!IsMasterClient(player))
        {
            var movementConfig = new MovementConfig()
            {
                JumpHeight = 3,
                MoveSpeed = 10,
                IsGrounded = true,
                GroundHeight = FP._0_50
            };
            f.Add(entity, movementConfig);
        }
        
        if (f.Unsafe.TryGetPointer<Transform2D>(entity, out var transform2D))
        {
            if(IsMasterClient(player))
                transform2D->Position = new FPVector2(-3, FP.FromString("1.5"));

            else
                transform2D->Position = new FPVector2(1, 1);

        }
    }

    private bool IsMasterClient(PlayerRef player)
    {
        return player._index == 1;
       // return false;
    }
}