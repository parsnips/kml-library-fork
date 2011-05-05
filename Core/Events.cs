namespace Core
{
    using System;

    public delegate void GenericEventHandler<T>(object sender, GenericEventArgs<T> e);

    public class GenericEventArgs<T> : EventArgs
    {
        public T Obj;

        public GenericEventArgs(T obj)
        {
            Obj = obj;
        }
    }

    public delegate void GenericEventHandler<A, B>(object sender, GenericEventArgs<A, B> e);

    public class GenericEventArgs<A, B> : EventArgs
    {
        public A ObjA;
        public B ObjB;

        public GenericEventArgs(A objA, B objB)
        {
            ObjA = objA;
            ObjB = objB;
        }
    }
}