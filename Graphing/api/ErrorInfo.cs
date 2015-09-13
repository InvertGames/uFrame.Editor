using System;
using Invert.Data;

namespace Invert.Core.GraphDesigner
{
    public class ErrorInfo
    {
        protected bool Equals(ErrorInfo other)
        {
            return string.Equals(Identifier, other.Identifier) && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ErrorInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Identifier != null ? Identifier.GetHashCode() : 0)*397) ^ (Message != null ? Message.GetHashCode() : 0);
            }
        }
        public IDataRecord Record { get; set; }
        public string Identifier { get; set; }
        public string Message { get; set; }
        public Action AutoFix { get; set; }
        public ValidatorType Siverity { get; set; }
    }
}