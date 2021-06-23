using System;

namespace Inheritance.DataStructure
{
    public class Category: IComparable
    {
        public readonly string Message;
        public readonly MessageType Type;
        public readonly MessageTopic Topic;
        
        public Category(string message, MessageType type, MessageTopic topic)
        {
            Message = message;
            Type = type;
            Topic = topic;
        }

        public override string ToString()
        {
            return Message + "." + Type + "." + Topic;
        }

        public override bool Equals(object obj)
        {
            if (obj is Category other)
                return Equals(other);
            
            return false;
        }

        protected bool Equals(Category other)
        {
            return Message == other.Message && Type == other.Type && Topic == other.Topic;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Message != null ? Message.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ (int) Topic;
                return hashCode;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Category other)
                return CompareTo(other);
            throw new InvalidOperationException();
        }
        
        public int CompareTo(Category other)
        {
            if (other == null) return 0;
            var messageCompare = String.Compare(Message, other.Message, StringComparison.Ordinal);
            if (messageCompare == 0)
            {
                var typeCompare = Type.CompareTo(other.Type);
                if (typeCompare == 0)
                    return Topic.CompareTo(other.Topic);
                else
                    return typeCompare;
            }
            else
                return messageCompare;
        }

        public static bool operator <(Category a, Category b)
            => a.CompareTo(b) < 0;

        public static bool operator >(Category a, Category b)
            => a.CompareTo(b) > 0;
        
        public static bool operator ==(Category a, Category b)
            => Equals(a,b);

        public static bool operator !=(Category a, Category b) 
            => !(a == b);
        
        public static bool operator >=(Category a, Category b)
            => a.CompareTo(b) >= 0;

        public static bool operator <=(Category a, Category b)
            => a.CompareTo(b) <= 0;
    }
}
