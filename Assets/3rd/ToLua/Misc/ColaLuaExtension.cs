﻿using LuaInterface;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对ToLua进行拓展，可以在这里手动注册一些方法
/// </summary>
public static class ColaLuaExtension
{
    private static StringBuilder stringBuilder = new StringBuilder();
    public static readonly int PrintTableDepth = 5;
    private static readonly string indentStr = "  ";
    private static readonly char shortIndentChar = ' ';

    /// <summary>
    /// 外部调用，统一注册
    /// </summary>
    /// <param name="L"></param>
    public static void Register(LuaState L)
    {
        //内部手动管理LuaState上面的堆栈，并实现功能函数与注册
        L.BeginModule(null);
        L.RegFunction("LogFunction", LogFunction);
        L.EndModule();
    }

    /// <summary>
    /// 供Lua端调用的打印方法
    /// </summary>
    /// <param name="L"></param>
    /// <returns></returns>
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    private static int LogFunction(IntPtr L)
    {
        try
        {
            if (LuaDLL.lua_gettop(L) > 1)
            {
                var logTag = LuaDLL.lua_tointeger(L, 1);
                LuaDLL.lua_remove(L, 1);
                return InnerPrint(L, logTag);
            }
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    /// <summary>
    /// 内部对lua的日志进行打印
    /// </summary>
    /// <param name="L"></param>
    /// <param name="traceback"></param>
    /// <returns></returns>
    private static int InnerPrint(IntPtr L, int logTag)
    {

        int n = LuaDLL.lua_gettop(L); //返回栈顶索引（即栈长度）
        for (int i = 1; i <= n; i++)
        {
            if (PrintTableDepth > 0 && LuaDLL.lua_type(L, i) == LuaTypes.LUA_TTABLE)
            {
                InnerPrintTable(L, 0, i);
            }
            else
            {
                if (LuaDLL.lua_isstring(L, i) == 1)
                {
                    stringBuilder.Append(LuaDLL.lua_tostring(L, i));
                }
                else if (LuaDLL.lua_isnil(L, i))
                {
                    stringBuilder.Append("nil");
                }
                else if (LuaDLL.lua_isboolean(L, i))
                {
                    stringBuilder.Append(LuaDLL.lua_toboolean(L, i) ? "true" : "false");
                }
                else
                {
                    IntPtr p = LuaDLL.lua_topointer(L, i);

                    if (p == IntPtr.Zero)
                    {
                        stringBuilder.Append("nil");
                    }
                    else
                    {
                        stringBuilder.Append(LuaDLL.luaL_typename(L, i)).Append(":0x").Append(p.ToString("X"));
                    }
                }

                if (i == n)
                { stringBuilder.AppendLine(); }
                else
                { stringBuilder.Append(indentStr); }
            }
        }

        switch (logTag)
        {
            case (int)LogType.Log:
                Debug.Log(stringBuilder.ToString());
                break;
            case (int)LogType.Warning:
                Debug.LogWarning(stringBuilder.ToString());
                break;
            case (int)LogType.Error:
                Debug.LogError(stringBuilder.ToString());
                break;
            default:
                Debug.Log(stringBuilder.ToString());
                break;
        }

        stringBuilder.Clear();
        return 0;
    }

    /// <summary>
    /// 内部对luaTable进行打印
    /// </summary>
    /// <param name="L"></param>
    /// <param name="traceback"></param>
    /// <returns></returns>
    private static int InnerPrintTable(IntPtr L,int layer,int tbIndex)
    {
        var indent = layer > 0 ? new string(shortIndentChar, layer * shortIndentChar) : string.Empty; //根据表的层，进行相应缩进

        stringBuilder.Append(indent).AppendLine("{");
        LuaDLL.lua_pushnil(L);  /* 一般Push进一个nil作为第一个 key */
        while (LuaDLL.lua_next(L, tbIndex) != 0)
        {
            var keyType = LuaDLL.lua_type(L, -2); //固定写法
            var valType = LuaDLL.lua_type(L, -1); //固定写法

            if (keyType == LuaTypes.LUA_TNUMBER)
            {
                stringBuilder.Append(indent).Append(shortIndentChar, shortIndentChar).AppendFormat("[{0}] = ", LuaDLL.lua_tostring(L, -1));
            }
            else
            {
                stringBuilder.Append(indent).Append(shortIndentChar, shortIndentChar).AppendFormat("{0} = ", LuaDLL.lua_tostring(L, -1));
            }

            if (layer + 1 < PrintTableDepth && valType == LuaTypes.LUA_TTABLE)
            {
                stringBuilder.AppendLine();
                //递归处理
                InnerPrintTable(L, layer + 1, LuaDLL.lua_gettop(L) );
            }
            else
            {
                stringBuilder.AppendLine(LuaDLL.lua_tostring(L, -1));
            }

            /* 移除 'value' ；保留 'key' 做下一次迭代 */
            LuaDLL.lua_pop(L, 1);
        }
        stringBuilder.Append(indent).AppendLine("}");
        return 0;
    }
}