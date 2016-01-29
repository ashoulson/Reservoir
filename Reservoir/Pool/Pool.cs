/*
 *  Reservoir - C# Object Pooling and GC-Free Collections
 *  Copyright (c) 2016 - Alexander Shoulson - http://ashoulson.com
 *
 *  This software is provided 'as-is', without any express or implied
 *  warranty. In no event will the authors be held liable for any damages
 *  arising from the use of this software.
 *  Permission is granted to anyone to use this software for any purpose,
 *  including commercial applications, and to alter it and redistribute it
 *  freely, subject to the following restrictions:
 *  
 *  1. The origin of this software must not be misrepresented; you must not
 *     claim that you wrote the original software. If you use this software
 *     in a product, an acknowledgment in the product documentation would be
 *     appreciated but is not required.
 *  2. Altered source versions must be plainly marked as such, and must not be
 *     misrepresented as being the original software.
 *  3. This notice may not be removed or altered from any source distribution.
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Reservoir
{
  public static class Pool
  {
    /// <summary>
    /// Frees an object, returning it to its memory pool. If the object is
    /// already in a NodeList, this function will remove it from that list.
    /// </summary>
    public static void Free<T>(T value)
      where T : class, IPoolable<T>, new()
    {
#if DEBUG
      if (value.List != null)
        throw new InvalidOperationException("Remove nodes before freeing");
#endif
      value.Pool.Deallocate(value);
    }

    public static void FreeAll<T>(NodeList<T> list)
      where T : class, IPoolable<T>, new()
    {

      // We can't do a list append here because we need to call Reset()
      // on each element as it enters the pool
      while(list.Count > 0)
        Pool.Free(list.RemoveLast());
    }

#if DEBUG
    public static bool IsPooled<T>(T value)
      where T : class, IPoolable<T>, new()
    {
      NodeList<T> list = value.List;
      return (list != null) && list.isPoolList;
    }
#endif
  }

  public sealed class Pool<T>
    where T : class, IPoolable<T>, new()
  {
    internal NodeList<T> freeList;

    public Pool()
    {
      this.freeList = new NodeList<T>();
#if DEBUG
      this.freeList.isPoolList = true;
#endif
    }

    public T Allocate()
    {
      T value = null;
      if (this.freeList.Count == 0)
        value = new T();
      else
        // Pull last added element, most likely to still be in cache
        value = this.freeList.RemoveLast();

      value.Pool = this;
      value.Initialize();
      return value;
    }

    internal void Deallocate(T value)
    {
#if DEBUG
      if ((value.Pool != this) || Pool.IsPooled(value))
#else
      if (value.Pool != this)
#endif
        throw new AccessViolationException();

      value.Reset();
      this.freeList.Add(value);
    }
  }
}