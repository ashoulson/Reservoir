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
  
  // Your data and functions here
  public int key;
}
```

Example of using the library with this class:

```c#
public class Test
{
  private Pool<Example> pool;
  private NodeList<Example> list;

  void Start()
  {
    this.pool = new Pool<Example>();
    this.list = new NodeList<Example>();

    for (int i = 0; i < 5; i++)
      this.RunTest();
  }

  void RunTest()
  {
    Console.WriteLine("Starting");
	// No GC allocations after the first iteration
    Example a = this.pool.Allocate();
    Example b = this.pool.Allocate();
    Example c = this.pool.Allocate();

    a.key = 0;
    b.key = 1;
    c.key = 2;

    this.list.Add(a);
    this.list.Add(b);
    this.list.Add(c);

	// No GC allocations for these loops
    foreach (Example e in this.list)
      Console.WriteLine(e.key);

    this.list.Remove(b);

    foreach (Example e in this.list)
      Console.WriteLine(e.key);

    this.list.Add(b);
    this.list.RemoveFirst();

    foreach (Example e in this.list)
      Console.WriteLine(e.key);

	// This will automatically remove the nodes from
	// the NodeList so they can be put in the pool
    Pool.Free(a);
    Pool.Free(b);
    Pool.Free(c);
    Console.WriteLine("Ending");
  }
}
```