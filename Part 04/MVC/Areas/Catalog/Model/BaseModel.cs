using System;
using System.Runtime.Serialization;

namespace MVC.Areas.Catalog.Models
{
    [DataContract]
    public abstract class BaseModel : IEquatable<BaseModel>
    {
        [DataMember]
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseModel);
        }

        public bool Equals(BaseModel other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
