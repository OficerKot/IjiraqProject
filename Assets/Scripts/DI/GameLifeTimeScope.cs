using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [Header("Игра")]
    [SerializeField] MenuManager menuManager;    
    [Header("Предметы")]
    [SerializeField] ItemsDataBase itemManager;
    [Header("Домино")]
    [SerializeField] DominoManager dominoManager;
    [SerializeField] DominoConfig dominoConfig;


    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IInventory, Inventory>(Lifetime.Singleton);
 
        builder.RegisterInstance(itemManager);
        builder.RegisterInstance(dominoManager);
        builder.RegisterInstance(menuManager);
        builder.RegisterInstance(dominoConfig);    

        builder.Register<DominoFactory>(Lifetime.Singleton);
        builder.Register<SigilsState>(Lifetime.Singleton);
        builder.Register<HandManager>(Lifetime.Singleton);
        builder.Register<DominoPool>(Lifetime.Singleton);
        builder.Register<DominoProtecter>(Lifetime.Singleton);
        builder.Register<ObeliskManager>(Lifetime.Singleton);
        builder.Register<RoadManager>(Lifetime.Singleton);
        builder.Register<SignsManager>(Lifetime.Singleton);
        builder.Register<LayerSorter>(Lifetime.Singleton);

        builder.Register<ICraftService, CraftService>(Lifetime.Singleton);

        builder.RegisterComponentInHierarchy<ItemsPlacer>();
        builder.RegisterComponentInHierarchy<PerlinNoiseMap>();
        builder.RegisterComponentInHierarchy<GameManager>();
        builder.RegisterComponentInHierarchy<SigilsMenu>();
        builder.RegisterComponentInHierarchy<UISelectionPanel>();
        builder.RegisterComponentInHierarchy<UIInventory>();
        builder.RegisterComponentInHierarchy<CharacterMovement>(); //он вообще не должен быть MonoBehaviour, потом исправить
    }
}
