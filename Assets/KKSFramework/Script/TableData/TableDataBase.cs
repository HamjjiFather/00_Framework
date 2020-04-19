using System;
using System.Collections.Generic;

namespace KKSFramework.TableData
{
    /// <summary>
    /// 모든 테이블 데이터 텍스트에서 받아오는 데이터의 베이스 클래스.
    /// </summary>
    [Serializable]
    public abstract class TableDataBase
    {
        #region Fields & Property
        public abstract void SetData (List<string> myData);

        #endregion
    }
}