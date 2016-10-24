using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public class Exceptions
    {
        [Serializable]
        public class FileAlreadyExistsException : Exception
        {
            public FileAlreadyExistsException(string message) : base(message)
            {
            }

            protected FileAlreadyExistsException(System.Runtime.Serialization.SerializationInfo info, 
                                                 System.Runtime.Serialization.StreamingContext context)
                                                 : base (info, context)
            {
            }
        }

        [Serializable]
        public class DirectoryAlreadyExistsException : Exception
        {
            public DirectoryAlreadyExistsException(string message)
                : base(message)
            {
            }

            protected DirectoryAlreadyExistsException(System.Runtime.Serialization.SerializationInfo info,
                                                      System.Runtime.Serialization.StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
