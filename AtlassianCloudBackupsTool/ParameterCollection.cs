namespace AtlassianCloudBackupsTool
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a colleciton of paramters and provides methods to extract the values from the them
    /// </summary>
    public class ParameterCollection : Dictionary<string, object>
    {

        public ParameterCollection()
        {
        }

        /// <summary>
        /// Constructor that takes a string array like the one required on the Main entry point for a console app
        /// and converts it for use as a ParameterCollection
        /// </summary>
        /// <param name="args">Parameter array</param>
        public ParameterCollection(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith("/") != true)
                {
                    throw new ArgumentException(arg + " is not a valid parameter.");
                }

                int sepratorLocation = arg.IndexOf(":", StringComparison.Ordinal);

                string argumentProperty;
                object argumentValue;

                if (sepratorLocation != -1)
                {
                    argumentProperty = arg.Substring(0, sepratorLocation).Replace("/", "");
                    argumentValue = arg.Substring(sepratorLocation+1);
                }
                else
                {
                    argumentProperty = arg.Replace("/", "");
                    argumentValue = true;
                }

                Add(argumentProperty, argumentValue);
            }
        }
        /// <summary>
        /// Returns the value of the specified parameter based on the key and type provided
        /// </summary>
        /// <typeparam name="TU">Type to return value as</typeparam>
        /// <param name="property">The key to return the value for</param>
        /// <returns></returns>
        public TU GetParameterValue<TU>(string property)
        {
            object propertyValue;

            TryGetValue(property, out propertyValue);

            if (propertyValue == null)
            {
                return default(TU);
            }

            return (TU)propertyValue;
        }
    }
}
