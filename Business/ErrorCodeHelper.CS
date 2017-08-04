using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XMS.Core;

namespace Business
{
    // 定义系统中抛出的所有业务异常相关的自定义错误码。
    // 使用示例：	throw ErrorCodeHelper.MemberNotExists.ToException(new {A=1, B="a"});
    internal class ErrorCodeHelper
    {
    }
}