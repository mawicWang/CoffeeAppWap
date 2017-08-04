using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace PublicResource
{
    public delegate TTarget CopyDelegate<TTarget, TSource>(TSource objSource); //委托
    public class CopyObject<TTarget, TSource>
    {
        private static CopyDelegate<TTarget, TSource> objCopyDelegate = null;

        /// <summary>
        /// 对象赋值
        /// </summary>
        /// <param name="objSource">原对象</param>
        /// <returns>返回的对象</returns>
        public static TTarget Copy(TSource objSource)
        {
            if (objSource == null)
            {
                return default(TTarget);
            }
            if (objCopyDelegate == null)
            {
                objCopyDelegate = CreateCopyDelegate(typeof(TTarget), typeof(TSource));
            }
            return objCopyDelegate(objSource);
        }


        /// <summary>
        /// 动态创建一个委托并返回
        /// </summary>
        /// <param name="tTarget"></param>
        /// <param name="tSource"></param>
        /// <returns></returns>
        private static CopyDelegate<TTarget, TSource> CreateCopyDelegate(Type tTarget, Type tSource)
        {
            DynamicMethod dynCopyMethod = new DynamicMethod("CopyObject", tTarget, new Type[] { tSource }, typeof(TTarget).Module); //创建一个动态方法
            ILGenerator il = dynCopyMethod.GetILGenerator();
            ConstructorInfo ci = tTarget.GetConstructor(Type.EmptyTypes); //得到目标类的构造方法
            il.DeclareLocal(tTarget);
            il.Emit(OpCodes.Newobj, ci); //创建一个目标类的新实例，并将对象引用（O 类型）推送到计算堆栈上。
            il.Emit(OpCodes.Stloc_0); //从计算堆栈的顶部弹出当前值并将其存储到索引 0 处的局部变量列表中。
            foreach (PropertyInfo pSource in tSource.GetProperties())
            {
                foreach (PropertyInfo pTarget in tTarget.GetProperties())
                {
                    if (pTarget.CanWrite && pSource.Name.ToLower() == pTarget.Name.ToLower() && pSource.PropertyType == pTarget.PropertyType)
                    {
                        il.Emit(OpCodes.Ldloc_0); //将索引 0 处的局部变量加载到计算堆栈上。
                        il.Emit(OpCodes.Ldarg_0); //将索引为 0 的参数加载到计算堆栈上。
                        il.Emit(OpCodes.Callvirt, pSource.GetGetMethod());  //对对象调用后期绑定方法，并且将返回值推送到计算堆栈上。
                        il.Emit(OpCodes.Callvirt, pTarget.GetSetMethod());  //对对象调用后期绑定方法，并且将返回值推送到计算堆栈上。
                        break;
                    }
                }
            }
            il.Emit(OpCodes.Ldloc_0);//将索引 0 处的局部变量加载到计算堆栈上。
            il.Emit(OpCodes.Ret); //从当前方法返回，并将返回值（如果存在）从调用方的计算堆栈推送到被调用方的计算堆栈上。

            return (CopyDelegate<TTarget, TSource>)dynCopyMethod.CreateDelegate(typeof(CopyDelegate<TTarget, TSource>));
        }
    }
}
