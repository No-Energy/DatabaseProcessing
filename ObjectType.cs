using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseProcessing
{
    /// <summary>
    /// where条件类型
    /// </summary>
    public enum WhereObjectType
    {
        等于 = 0,
        不等于=1,
        大于=2,
        小于=3,
        包括=4,
        不包括=5
    }
    /// <summary>
    /// total条件类型
    /// </summary>
    public enum TotalObjectType
    {
        GroupBy=0,
        Sum=1,　　
        Max=2,　　
        Min=3,　　
        Avg=4,
        Count=5
    }
}
