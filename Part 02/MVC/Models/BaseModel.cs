﻿using System.Runtime.Serialization;

namespace MVC.Models
{
    [DataContract]
    public abstract class BaseModel
    {
        [DataMember]
        public int Id { get; set; }
    }
}
