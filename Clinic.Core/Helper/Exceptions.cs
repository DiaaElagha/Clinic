using System;
using System.Runtime.Serialization;

namespace Clinic.Core.Helper
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, int entityId) : base($"{message} {entityId}")
        {
        }

        public NotFoundException(int entityId) : base($"No item found with id {entityId}")
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message, int duplicateItemId) : base($"{message} {duplicateItemId}")
        {
        }

        public DuplicateItemException(int duplicateItemId) : base($"Duplicate items with id {duplicateItemId}")
        {
        }

        public DuplicateItemException(string message) : base(message)
        {
        }

        public DuplicateItemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
