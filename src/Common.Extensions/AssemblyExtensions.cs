// MIT-License
// Copyright BridgingIT GmbH - All Rights Reserved
// Use of this source code is governed by an MIT-style license that can be
// found in the LICENSE file at https://github.com/bridgingit/bitdevkit/license

namespace BridgingIT.DevKit.Common;

using System;
using System.Linq;
using System.Reflection;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> SafeGetTypes(this IEnumerable<Assembly> assemblies)
    {
        if (assemblies is null)
        {
            return[];
        }

        return assemblies.SelectMany(SafeGetTypes);
    }

    public static IEnumerable<Type> SafeGetTypes(this Assembly assembly)
    {
        if (assembly is null)
        {
            return[];
        }

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null);
        }
    }

    public static IEnumerable<Type> SafeGetTypes<TInterface>(this IEnumerable<Assembly> assemblies)
    {
        return SafeGetTypes(assemblies, typeof(TInterface));
    }

    public static IEnumerable<Type> SafeGetTypes<TInterface>(this Assembly assembly)
    {
        return SafeGetTypes(assembly, typeof(TInterface));
    }

    public static IEnumerable<Type> SafeGetTypes(this IEnumerable<Assembly> assemblies, Type @interface)
    {
        if (assemblies is null || @interface is null)
        {
            return[];
        }

        return assemblies.SelectMany(t => SafeGetTypes(t, @interface));
    }

    public static IEnumerable<Type> SafeGetTypes(this Assembly assembly, Type @interface)
    {
        if (assembly is null || @interface is null)
        {
            return[];
        }

        try
        {
            return assembly.GetTypes().Where(t => t != null && t.ImplementsInterface(@interface));
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null && t.ImplementsInterface(@interface));
        }
    }
}