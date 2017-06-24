using System.Collections.Generic;

namespace Anti_AntiAliasing
{
    public static class Data
    {
        private static string filePath;
        private static List<string> filePaths;
        private static bool pathSet;

        public static string FilePath
        {
            //Return the filePath string
            get
            {
                return filePath;
            }

            //Set the filePath string
            set
            {
                //Do not set if the value is Null or Empty or Whitespaces
                if (string.IsNullOrEmpty(value))
                    return;

                //Do not set if the value is already equal to filePath
                if (filePath == value)
                    return;

                //Set filePath equal to the value
                filePath = value;
            }
        }

        public static List<string> FilePaths
        {
            get
            {
                return filePaths;
            }

            set
            {
                if (filePaths == null || filePaths.Count == 0)
                    return;

                if (filePaths == value)
                    return;

                filePaths = value;
            }
        }

        public static bool PathSet
        {
            //Return the value of the pathSet bool
            get
            {
                return pathSet;
            }

            set
            {
                //If the pathSet bool  is equal to the value, don't set
                if (pathSet == value)
                    return;

                //Set the pathSet variable equal to the value
                pathSet = value;
            }
        }
    }
}