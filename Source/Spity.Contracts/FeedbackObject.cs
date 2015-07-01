using System;
using System.Runtime.Serialization;

namespace Spity.Contracts
{
    [DataContract]
    public sealed class FeedbackObject
    {
        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}
