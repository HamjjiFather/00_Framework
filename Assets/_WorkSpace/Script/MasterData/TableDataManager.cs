// ExcelExporter로 자동 생성된 파일.

using System.Collections.Generic;
using System.Linq;
using KKSFramework.TableData;
using Cysharp.Threading.Tasks;

public class TableDataManager : Singleton<TableDataManager>
{
    public readonly Dictionary<int, TableDataBase> TotalDataDict = new Dictionary<int, TableDataBase> ();

	public Dictionary<int, Template> TemplateDict = new Dictionary<int, Template> ();


    public async UniTask LoadTableDatas ()
    {
		TemplateDict = (await ReadCSVData.Instance.LoadCSVData<Template> ("TableData", nameof (Template))).ToDictionary (x => x.Id, x => x);



    }
}
