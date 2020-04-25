using System;
using System.Collections.Generic;
using KKSFramework.DesignPattern;
using KKSFramework.GameSystem.GlobalText;
using UniRx.Async;
using Zenject;

public class ProjectInstall : MonoInstaller
{
    private static readonly List<Type> ViewModelTypes = new List<Type> ();

    public override void InstallBindings ()
    {
        BindViewmodel ();
    }

    private void BindViewmodel ()
    {
        ViewModelTypes.ForEach (type => { Container.Bind (type).AsSingle (); });
    }

    public static void InstallViewmodel ()
    {
        ViewModelTypes.ForEach (type =>
        {
            var viewmodel = (ViewModelBase) ProjectContext.Instance.Container.Resolve (type);
            viewmodel.Initialize ();
        });
    }
}