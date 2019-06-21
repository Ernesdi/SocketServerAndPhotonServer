using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommon
{
    //传输的参数Code,传输的是字典类型的数据，ParameterCode区分键值。
    public enum ParameterCode : byte
    {
        Username,
        Password,
        PosX, PosY, PosZ,
        CurrentOnlineNameExcuteSelf,
        Position
    }
}
