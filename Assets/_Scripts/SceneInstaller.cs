using TrainSystem;
using Tunnels;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GenerationTunnel _generationTunnel;
     private Train train;
    public override void InstallBindings()
    {
        Container.Bind<GenerationTunnel>().FromInstance(_generationTunnel);
        
    }

    
}