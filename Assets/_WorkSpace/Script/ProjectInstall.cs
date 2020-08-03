using System;
using System.Collections.Generic;
using KKSFramework.DesignPattern;
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

    public static void InitViewmodel ()
    {
        ViewmodelTypes.ForEach (type =>
        {
            var viewmodel = (ViewModelBase) ProjectContext.Instance.Container.Resolve (type);
            viewmodel.Initialize ();
        });
    }
    
    
    public static void InitLocalDataViewmodel ()
    {
        ViewmodelTypes.ForEach (type =>
        {
            var viewmodel = (ViewModelBase) ProjectContext.Instance.Container.Resolve (type);
            viewmodel.InitAfterLoadLocalData ();
        });
    }
    
    
    public static void InitTableDataViewmodel ()
    {
        ViewmodelTypes.ForEach (type =>
        {
            var viewmodel = (ViewModelBase) ProjectContext.Instance.Container.Resolve (type);
            viewmodel.InitAfterLoadTableData ();
        });
    }
}