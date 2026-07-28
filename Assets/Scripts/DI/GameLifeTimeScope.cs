using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IInventory, Inventory>(Lifetime.Singleton);
        builder.RegisterComponentInHierarchy<ItemsPlacer>();
    }
}
