using UnityEngine;
using Zenject;

public class MatchMakingInstaller : MonoInstaller<MatchMakingInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IMatchMaker>().FromInstance(FindObjectOfType<PhotonMatchMaker>());
    }
}