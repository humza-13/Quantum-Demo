using System.Diagnostics;
using Photon.Deterministic;

namespace Quantum.Bug_Crusher.Systems;

public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
{
    public void OnPlayerDataSet(Frame f, PlayerRef player)
    {
        EntityPrototype prototype = default;
        var data = f.GetPlayerData(player);

       if(IsMasterClient(player))
            prototype = f.FindAsset<EntityPrototype>(data.ZeusPrototype.Id);
        else
            prototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
        
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

        if (f.Unsafe.TryGetPointer<Transform2D>(entity, out var transform2D))
        {
            if(IsMasterClient(player))
                transform2D->Position = new FPVector2(3, 1);
            else
                transform2D->Position = new FPVector2(0, 0);

        }
    }

    private bool IsMasterClient(PlayerRef player)
    {
        if (player._index == 1) 
            return true;
        
        return false;
        
    }
}