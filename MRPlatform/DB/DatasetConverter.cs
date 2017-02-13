/*
 * Code converted from post on CodeProject: https://www.codeproject.com/Articles/13352/NET-DataSet-to-ADODB-Recordset-Conversion
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace MRPlatform.DB
{
    class DataSetConverter
    {
        public DataSetConverter()
        {
            // Class constructor
        }

        public string GetADORecordset(DataSet dataSet, string databaseName)
        {
            // Create memory stream to contain the XML
            MemoryStream mStream = new MemoryStream();

            // Create XMLWriter object to writed formatted XML to memory stream
            XmlTextWriter xWriter = new XmlTextWriter(mStream, Encoding.UTF8);

            // Additional formatting for XML
            xWriter.Indentation = 8;
            xWriter.Formatting = Formatting.Indented;

            // CAll this method to write the ADO Namespaces
            WriteADONamespaces(xWriter);

            // Call this method to write the ADO Recordset schema
            WriteSchemaElement(dataSet, databaseName, xWriter);

            // Call this method to transform the data portion of the DataSet object
            TransformData(dataSet, xWriter);

            // Flush all input to XmlWriter
            xWriter.Flush();

            //Prepare the return value
            mStream.Position = 0;

            byte[] buffer = new byte[mStream.Length];
            mStream.Read(buffer, 0, Convert.ToInt32(mStream.Length));

            UTF8Encoding textConverter = new UTF8Encoding();
            return textConverter.GetString(buffer);
        }

        private void WriteADONamespaces(XmlTextWriter xWriter)
        {
            xWriter.WriteStartElement("", "xml", "");
            xWriter.WriteAttributeString("xmlns", "s", null, "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            xWriter.WriteAttributeString("xmlns", "dt", null, "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
            xWriter.WriteAttributeString("xmlns", "rs", null, "urn:schemas-microsoft-com:rowset");
            xWriter.WriteAttributeString("xmlns", "z", null, "#RowsetSchema");
            xWriter.Flush();
        }

        private void WriteSchemaElement(DataSet dataSet, string databaseName, XmlTextWriter xWriter)
        {
            // Write element Schema
            xWriter.WriteStartElement("s", "Schema", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            xWriter.WriteAttributeString("id", "RowsetSchema");

            // Write element ElementType
            xWriter.WriteStartElement("s", "ElementType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
            xWriter.WriteAttributeString("name", "", "row");
            xWriter.WriteAttributeString("content", "", "eltOnly");
            xWriter.WriteAttributeString("rs", "updatable", "urn:schemas-microsoft-com:rowset", "true");

            WriteSchema(dataSet, databaseName, xWriter);
        }

        private void WriteSchema(DataSet dataSet, string databaseName, XmlTextWriter xWriter)
        {
            int i = 1;

            foreach(DataColumn dc in dataSet.Tables[0].Columns)
            {
                dc.ColumnMapping = MappingType.Attribute;

                xWriter.WriteStartElement("s", "AttributeType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
                xWriter.WriteAttributeString("name", "", dc.ToString());
                xWriter.WriteAttributeString("rs", "number", "urn:schemas-microsoft-com:rowset", i.ToString());
                xWriter.WriteAttributeString("rs", "baseCatalog", "urn:schemas-microsoft-com:rowset", databaseName);
                xWriter.WriteAttributeString("rs", "baseTable", "urn:schemas-microsoft-com:rowset", dc.Table.TableName.ToString());
                xWriter.WriteAttributeString("rs", "keycolumn", "urn:schemas-microsoft-com:rowset", dc.Unique.ToString());
                xWriter.WriteAttributeString("rs", "autoincrement", "urn:schemas-microsoft-com:rowset", dc.AutoIncrement.ToString());
                
                // Write child element
                xWriter.WriteStartElement("s", "datatype", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
                xWriter.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", GetDataType(dc.DataType.ToString()));
                xWriter.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", dc.MaxLength.ToString());
                xWriter.WriteAttributeString("rs", "maybenull", "urn:schemas-microsoft-com:rowset", dc.AllowDBNull.ToString());
                xWriter.WriteEndElement();

                xWriter.WriteEndElement();
                xWriter.Flush();

                // Increment column counter
                i += 1;
            }
        }

        private string GetDataType(string dataType)
        {
            switch(dataType)
            {
                case "System.Int32":
                    return "int";
                case "System.Int16":
                    return "int";
                case "System.Integer":
                    return "int";
                case "System.DateTime":
                    return "dateTime.iso8601tz";
                case "System.Byte[]":
                    return "bin.hex";
                case "System.Boolean":
                    return "boolean";
                case "System.Guid":
                    return "guid";
                default:
                    return "string";
            }
        }

        private void TransformData(DataSet dataSet, XmlTextWriter xWriter)
        {
            // Loop through DataSet and add to XML
            xWriter.WriteStartElement("", "rs:data", "");

            // Loop through each row
            for(int i = 0; i < dataSet.Tables[0].Rows.Count - 1; i++)
            {
                // Write start element for the row
                xWriter.WriteStartElement("", "Z:row", "");

                // Loop through each column in the row
                for(int k = 0; k < dataSet.Tables[0].Columns.Count - 1; k++)
                {
                    // Write the attribute that describes this column and its value
                    if(dataSet.Tables[0].Columns[i].DataType.ToString() == "System.Byte[]")
                    {
                        // Binary data must be properly encoded (bin.hex)
                        if(!Convert.IsDBNull(dataSet.Tables[0].Rows[i].ItemArray[]))
                    }
                }
            }

            foreach(DataRow row in dataSet.Tables[0].Rows)
            {
                xWriter.WriteStartElement("", "Z:row", "");

                foreach(DataColumn col in dataSet.Tables[0].Columns)
                {
                    if(col.DataType.ToString() == "System.Byte[]")
                    {
                        if(!Convert.IsDBNull(row[col.ColumnName]))
                        {
                            xWriter.WriteAttributeString(col.ColumnName, DataToBinHex(Encoding.UTF8.GetBytes(col.ColumnName)));
                        }
                    }
                    else if(!Convert.IsDBNull(row[col.ColumnName]))
                    {
                        xWriter.WriteAttributeString(col.ColumnName, row[col].ToString());
                    }
                }
            }
        }

        private string DataToBinHex(byte[] data)
        {
            return BitConverter.ToString(data);
        }
    }
}
