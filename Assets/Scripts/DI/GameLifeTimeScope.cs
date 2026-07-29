using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] ItemsDataBase itemManager;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IInventory, Inventory>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<ItemsPlacer>();

        builder.RegisterInstance(itemManager);
        builder.Register<ICraftService, CraftService>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<PerlinNoiseMap>();
    }
}
