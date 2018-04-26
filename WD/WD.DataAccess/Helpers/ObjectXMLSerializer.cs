﻿// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 05-09-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-19-2017
// ***********************************************************************
// <copyright file="ObjectXMLSerializer.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;



// namespace: WD.DataAccess.Helpers
//
// summary:	.


namespace WD.DataAccess.Helpers
{
    
    /// <summary>   Serialization format types. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    

    public enum ObjectXMLSerializer
    {
        /// <summary>
        /// Binary serialization format.
        /// </summary>
        Binary,

        /// <summary>
        /// Document serialization format.
        /// </summary>
        Document
    }

    
    /// <summary>   An object XML serializer. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    ///
    /// <typeparam name="T">    . </typeparam>
    

    public static class ObjectXMLSerializer<T> where T : class // Specify that T must be a class.
    {
        #region Load methods

        
        /// <summary>   Loads an object from an XML file in Document format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="path"> Path of the file to load the object from. </param>
        ///
        /// <returns>   Object loaded from an XML file in Document format. </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load(@"C:\XMLObjects.xml");
        /// </code>
        /// </example>
        

        public static T Load(string path)
        {
            T serializableObject = LoadFromDocumentFormat(null, path, null);
            return serializableObject;
        }

        
        /// <summary>   Loads an object from an XML file using a specified serialized format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="path">             Path of the file to load the object from. </param>
        /// <param name="serializedFormat"> XML serialized format used to load the object. </param>
        ///
        /// <returns>   Object loaded from an XML file using the specified serialized format. </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load(@"C:\XMLObjects.xml", SerializedFormat.Binary);
        /// </code>
        /// </example>
        

        public static T Load(string path, ObjectXMLSerializer serializedFormat)
        {
            T serializableObject = null;

            switch (serializedFormat)
            {
                case ObjectXMLSerializer.Binary:
                    serializableObject = LoadFromBinaryFormat(path, null);
                    break;

                case ObjectXMLSerializer.Document:
                default:
                    serializableObject = LoadFromDocumentFormat(null, path, null);
                    break;
            }

            return serializableObject;
        }

        
        /// <summary>
        /// Loads an object from an XML file in Document format, supplying extra data types to enable
        /// deserialization of custom types within the object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="path">         Path of the file to load the object from. </param>
        /// <param name="extraTypes">   Extra data types to enable deserialization of custom types within
        ///                             the object. </param>
        ///
        /// <returns>   Object loaded from an XML file in Document format. </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load(@"C:\XMLObjects.xml", new Type[] { typeof(MyCustomType) });
        /// </code>
        /// </example>
        

        public static T Load(string path, System.Type[] extraTypes)
        {
            T serializableObject = LoadFromDocumentFormat(extraTypes, path, null);
            return serializableObject;
        }

        
        /// <summary>
        /// Loads an object from an XML file in Document format, located in a specified isolated storage
        /// area.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="fileName">                 Name of the file in the isolated storage area to load
        ///                                         the object from. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to load the object from. </param>
        ///
        /// <returns>
        /// Object loaded from an XML file in Document format located in a specified isolated storage
        /// area.
        /// </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load("XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly());
        /// </code>
        /// </example>
        

        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory)
        {
            T serializableObject = LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
            return serializableObject;
        }

        
        /// <summary>
        /// Loads an object from an XML file located in a specified isolated storage area, using a
        /// specified serialized format.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="fileName">                 Name of the file in the isolated storage area to load
        ///                                         the object from. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to load the object from. </param>
        /// <param name="serializedFormat">         XML serialized format used to load the object. </param>
        ///
        /// <returns>
        /// Object loaded from an XML file located in a specified isolated storage area, using a
        /// specified serialized format.
        /// </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load("XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly(), SerializedFormat.Binary);
        /// </code>
        /// </example>
        

        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, ObjectXMLSerializer serializedFormat)
        {
            T serializableObject = null;

            switch (serializedFormat)
            {
                case ObjectXMLSerializer.Binary:
                    serializableObject = LoadFromBinaryFormat(fileName, isolatedStorageDirectory);
                    break;

                case ObjectXMLSerializer.Document:
                default:
                    serializableObject = LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
                    break;
            }

            return serializableObject;
        }

        
        /// <summary>
        /// Loads an object from an XML file in Document format, located in a specified isolated storage
        /// area, and supplying extra data types to enable deserialization of custom types within the
        /// object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="fileName">                 Name of the file in the isolated storage area to load
        ///                                         the object from. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to load the object from. </param>
        /// <param name="extraTypes">               Extra data types to enable deserialization of custom
        ///                                         types within the object. </param>
        ///
        /// <returns>
        /// Object loaded from an XML file located in a specified isolated storage area, using a
        /// specified serialized format.
        /// </returns>
        ///
        /// <example>
        /// <code>
        /// serializableObject = ObjectXMLSerializer&lt;SerializableObject&gt;.Load("XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly(), new Type[] { typeof(MyCustomType) });
        /// </code>
        /// </example>
        

        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, System.Type[] extraTypes)
        {
            T serializableObject = LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
            return serializableObject;
        }

        #endregion

        #region Save methods

        
        /// <summary>   Saves an object to an XML file in Document format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">   Serializable object to be saved to file. </param>
        /// <param name="path">                 Path of the file to save the object to. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, @"C:\XMLObjects.xml");
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string path)
        {
            SaveToDocumentFormat(serializableObject, null, path, null);
        }

        
        /// <summary>   Saves an object to an XML file using a specified serialized format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">   Serializable object to be saved to file. </param>
        /// <param name="path">                 Path of the file to save the object to. </param>
        /// <param name="serializedFormat">     XML serialized format used to save the object. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, @"C:\XMLObjects.xml", SerializedFormat.Binary);
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string path, ObjectXMLSerializer serializedFormat)
        {
            switch (serializedFormat)
            {
                case ObjectXMLSerializer.Binary:
                    SaveToBinaryFormat(serializableObject, path, null);
                    break;

                case ObjectXMLSerializer.Document:
                default:
                    SaveToDocumentFormat(serializableObject, null, path, null);
                    break;
            }
        }

        
        /// <summary>
        /// Saves an object to an XML file in Document format, supplying extra data types to enable
        /// serialization of custom types within the object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">   Serializable object to be saved to file. </param>
        /// <param name="path">                 Path of the file to save the object to. </param>
        /// <param name="extraTypes">           Extra data types to enable serialization of custom types
        ///                                     within the object. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, @"C:\XMLObjects.xml", new Type[] { typeof(MyCustomType) });
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string path, System.Type[] extraTypes)
        {
            SaveToDocumentFormat(serializableObject, extraTypes, path, null);
        }

        
        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage
        /// area.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">       Serializable object to be saved to file. </param>
        /// <param name="fileName">                 Name of the file in the isolated storage area to save
        ///                                         the object to. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to save the object to. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, "XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly());
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory)
        {
            SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
        }

        
        /// <summary>
        /// Saves an object to an XML file located in a specified isolated storage area, using a
        /// specified serialized format.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">       Serializable object to be saved to file. </param>
        /// <param name="fileName">                 Name of the file in the isolated storage area to save
        ///                                         the object to. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to save the object to. </param>
        /// <param name="serializedFormat">         XML serialized format used to save the object. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, "XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly(), SerializedFormat.Binary);
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, ObjectXMLSerializer serializedFormat)
        {
            switch (serializedFormat)
            {
                case ObjectXMLSerializer.Binary:
                    SaveToBinaryFormat(serializableObject, fileName, isolatedStorageDirectory);
                    break;

                case ObjectXMLSerializer.Document:
                default:
                    SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
                    break;
            }
        }

        
        /// <summary>
        /// Saves an object to an XML file in Document format, located in a specified isolated storage
        /// area, and supplying extra data types to enable serialization of custom types within the
        /// object.
        /// </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">       Serializable object to be saved to file. </param>
        /// <param name="fileName">                 Name of the file in the isolated storage area to save
        ///                                         the object to. </param>
        /// <param name="isolatedStorageDirectory"> Isolated storage area directory containing the XML
        ///                                         file to save the object to. </param>
        /// <param name="extraTypes">               Extra data types to enable serialization of custom
        ///                                         types within the object. </param>
        ///
        /// <example>
        /// <code>
        /// SerializableObject serializableObject = new SerializableObject();
        /// 
        /// ObjectXMLSerializer&lt;SerializableObject&gt;.Save(serializableObject, "XMLObjects.xml", IsolatedStorageFile.GetUserStoreForAssembly(), new Type[] { typeof(MyCustomType) });
        /// </code>
        /// </example>
        

        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, System.Type[] extraTypes)
        {
            SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
        }

        #endregion

        #region Private

        
        /// <summary>   Creates file stream. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        /// <param name="path">                     Path of the file to load the object from. </param>
        ///
        /// <returns>   The new file stream. </returns>
        

        private static FileStream CreateFileStream(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            FileStream fileStream = null;

            if (isolatedStorageFolder == null)
                fileStream = new FileStream(path, FileMode.OpenOrCreate);
            else
                fileStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder);

            return fileStream;
        }

        
        /// <summary>   Loads from binary format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="path">                     Path of the file to load the object from. </param>
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        ///
        /// <returns>   The data that was read from the binary format. </returns>
        

        private static T LoadFromBinaryFormat(string path, IsolatedStorageFile isolatedStorageFolder)
        {
            T serializableObject = null;

            using (FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                serializableObject = binaryFormatter.Deserialize(fileStream) as T;
            }

            return serializableObject;
        }

        
        /// <summary>   Loads from document format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="extraTypes">               Extra data types to enable deserialization of custom
        ///                                         types within the object. </param>
        /// <param name="path">                     Path of the file to load the object from. </param>
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        ///
        /// <returns>   The data that was read from the document format. </returns>
        

        private static T LoadFromDocumentFormat(System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            T serializableObject = null;

            using (TextReader textReader = CreateTextReader(isolatedStorageFolder, path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                serializableObject = xmlSerializer.Deserialize(textReader) as T;

            }

            return serializableObject;
        }

        
        /// <summary>   Creates text reader. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        /// <param name="path">                     Path of the file to load the object from. </param>
        ///
        /// <returns>   The new text reader. </returns>
        

        private static TextReader CreateTextReader(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            TextReader textReader = null;

            if (isolatedStorageFolder == null)
                textReader = new StreamReader(path);
            else
                textReader = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, isolatedStorageFolder));

            return textReader;
        }

        
        /// <summary>   Creates text writer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        /// <param name="path">                     Path of the file to load the object from. </param>
        ///
        /// <returns>   The new text writer. </returns>
        

        private static TextWriter CreateTextWriter(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            TextWriter textWriter = null;

            if (isolatedStorageFolder == null)
                textWriter = new StreamWriter(path);
            else
                textWriter = new StreamWriter(new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder));

            return textWriter;
        }

        
        /// <summary>   Creates XML serializer. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="extraTypes">   Extra data types to enable deserialization of custom types within
        ///                             the object. </param>
        ///
        /// <returns>   The new XML serializer. </returns>
        

        private static XmlSerializer CreateXmlSerializer(System.Type[] extraTypes)
        {
            Type ObjectType = typeof(T);

            XmlSerializer xmlSerializer = null;
           
            if (extraTypes != null)
                xmlSerializer = new XmlSerializer(ObjectType, extraTypes);
            else
                xmlSerializer = new XmlSerializer(ObjectType);

            return xmlSerializer;
        }

        
        /// <summary>   Saves to document format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">       Serializable object to be saved to file. </param>
        /// <param name="extraTypes">               Extra data types to enable serialization of custom
        ///                                         types within the object. </param>
        /// <param name="path">                     Path of the file to save the object to. </param>
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        

        private static void SaveToDocumentFormat(T serializableObject, System.Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using (TextWriter textWriter = CreateTextWriter(isolatedStorageFolder, path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                var xmlns = new System.Xml.Serialization.XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);
                xmlSerializer.Serialize(textWriter, serializableObject, xmlns);
            }
        }

        
        /// <summary>   Saves to binary format. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="serializableObject">       Serializable object to be saved to file. </param>
        /// <param name="path">                     Path of the file to save the object to. </param>
        /// <param name="isolatedStorageFolder">    Pathname of the isolated storage folder. </param>
        

        private static void SaveToBinaryFormat(T serializableObject, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using (FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
            {
               
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, serializableObject);
            }
        }

        #endregion
    }
}
