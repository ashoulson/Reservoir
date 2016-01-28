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
  /// <summary>
  /// A shortcut class for quickly creating poolable objects without
  /// having to implement the IPoolable interface.
  /// 
  /// Use as "class MyPoolableThing : Poolable\<MyPoolableThing\>"
  /// </summary>
  public class Poolable<T> : IPoolable<T>
    where T : class, IPoolable<T>, new()
  {
    T INode<T>.Next { get; set; }
    T INode<T>.Previous { get; set; }
    NodeList<T> INode<T>.List { get; set; }
    Pool<T> IPoolable<T>.Pool { get; set; }
    void IPoolable<T>.Initialize() { this.Initialize(); }
    void IPoolable<T>.Reset() { this.Reset(); }

    protected virtual void Initialize()
    {
    }

    protected virtual void Reset()
    {
    }
  }
}
