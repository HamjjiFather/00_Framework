using System;
using System.Collections.Generic;
using KKSFramework.DesignPattern;
using KKSFramework.GameSystem.GlobalText;
using UniRx.Async;
using Zenject;

public class ProjectInstall : MonoInstaller
{
    private static readonly List<Type> ViewmodelTypes = new List<Type> ();

    public override void InstallBindings ()
    {
        BindViewmodel ();
    }

    private void BindViewmodel ()
    {
        // ViewModelTypes.Add (typeof(Any Viewmodel Types));
        ViewmodelTypes.ForEach (type => { Container.Bind (type).AsSingle (); });
    }

    public static void InstallViewmodel ()
    {
        ViewmodelTypes.ForEach (type =>
        {
            var viewmodel = (ViewModelBase) ProjectContext.Instance.Container.Resolve (type);
            viewmodel.Initialize ();
        });
    }
}