using System;

namespace Exceptions
{
    public class NotLoadedContentRequestedException : Exception
    {
        public NotLoadedContentRequestedException()
        {
        }
        
        public NotLoadedContentRequestedException(string message) : base(message)
        {
        } 

        public NotLoadedContentRequestedException(string message, Exception exception) : base(message, exception)
        {
        }

    }
}