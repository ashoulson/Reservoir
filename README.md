**Reservoir: C# Object Pooling and GC-Free Collections**

Alexander Shoulson, Ph.D. - http://ashoulson.com

---

Provides support for in-place linked lists with GC-free iteration, and for memory-pooled objects. Example poolable class:

```c#
public class Example : IPoolable<Example>
{
  #region IPoolable Members
  Pool<Example> IPoolable<Example>.Pool { get; set; }
  bool IPoolable<Example>.IsPooled { get; set; }

  Example INode<Example>.Next { get; set; }
  Example INode<Example>.Previous { get; set; }
  NodeList<Example> INode<Example>.List { get; set; }

  void IPoolable<Example>.Initialize()
  {
    // Called when the object is allocated from the pool
  }

  void IPoolable<Example>.Reset()
  {
    // Called when the object is put back in the pool
  }
  #endregion
}
```