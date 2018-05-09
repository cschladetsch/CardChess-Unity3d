using System;

namespace App.Model
{
    public interface IHasId
    {
        Guid Id { get; }
    }
}