using System.Collections.Generic;
using System.Linq;
using KKSFramework.ResourcesLoad;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.TableData
{
    /// <summary>
    /// CSV데이터를 받아와 리턴.
    /// </summary>
    public class ReadCSVData : Singleton<ReadCSVData>
    {
        /// <summary>
        /// CSV 데이터 리턴.
        /// 데이터 클래스를 List<object> 형식으로 받아올 수 있게 구현해주어야 함.
        /// </summary>
        /// <returns></returns>
        public async UniTask<List<T>> LoadCSVData<T> (string path) where T : TableDataBase, new ()
        {
            var csvDataBases = new List<T> ();
            var res = await ResourcesLoadHelper
                .GetResourcesAsync<TextAsset> (ResourceRoleType._Data, ResourcesType.TSV, path);
            var csvEnum = res.text.Split ('\n').GetEnumerator ();

            csvEnum.MoveNext ();

            while (csvEnum.MoveNext ())
            {
                var datas = csvEnum.Current?.ToString ();
                if (string.IsNullOrEmpty (datas)) continue;
                var tempListValues = datas.Split ('\t').ToList ();
                var dataBase = new T ();
                dataBase.SetData (tempListValues);
                csvDataBases.Add (dataBase);
            }

            await UniTask.WaitWhile (() => csvEnum.MoveNext ());
            return csvDataBases;
        }
    }
}