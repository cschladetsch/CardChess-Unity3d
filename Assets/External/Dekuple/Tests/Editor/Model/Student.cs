using System;
using System.Runtime.InteropServices;
using Dekuple;
using UniRx;

namespace Assets.External.Dekuple.Tests.Editor.Model
{
    public interface IStudent
    {
        IReactiveProperty<Guid> Id { get; }
        IReactiveDictionary<ISubject, int> Scores { get; }
        IUniversity University { get; }
    }

    public interface IUniversity
    {
        string Name { get; }
    }

    public interface ISubject 
    {
        IReactiveProperty<string> Name { get; }
    }

    public class Student : IStudent
    {
        public IReactiveProperty<Guid> Id { get; } = new ReactiveProperty<Guid>();
        public IReactiveDictionary<ISubject, int> Scores { get; } = new ReactiveDictionary<ISubject, int>();

        [Inject] public IUniversity University { get; }
    }

    public class Subject : ISubject
    {
        public IReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
    }

    public class University : IUniversity
    {
        public string Name { get; set; }
    }
}
