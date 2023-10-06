namespace Quantum._3D_Movement;

public unsafe class PlayerSpawnSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
{
    public void OnPlayerDataSet(Frame f, PlayerRef player)
    {
        var data = f.GetPlayerData(player);
        var prototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
        var entity = f.Create(prototype);

        var playerLink = new PlayerLink()
        {
            Player = player,
        };
        f.Add(entity, playerLink);

        if (f.Unsafe.TryGetPointer<Transform2D>(entity, out var transform2D))
            transform2D->Position.X = 0 + player;
    }
}