// ExcelExporter로 자동 생성된 파일.

using System;
using System.Collections.Generic;
using KKSFramework.TableData;

public class Template : TableDataBase
{
    /// <summary>
    /// 인덱스
    /// </summary>
    public int Id;



    public Template ()
    {
    }


    public override void SetData (List<string> myData)
    {
        var rowIndex = 0;
		Id = int.Parse(myData[rowIndex++]);

    }
}
