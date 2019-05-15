using ATCVWorksPlugin;
using NUnit.Framework;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace Tests
{
    public class VWorksPluginTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void MetaDataIsSerializedCorrectly()
        {
            VWorksXml.Velocity11 actual = new VWorksXml.Velocity11();

            actual.Version = "1.0";
            actual.File = "MetaData";

            actual.MetaData = new VWorksXml.MetaDataElement
            {
                Commands = new VWorksXml.CommandElement[0],
                Device = new VWorksXml.DeviceElement(),
                Versions = new VWorksXml.Version[0]
            };

            #region Device
            // Add Device Info
            actual.MetaData.Device.MiscAttributes = 0;
            actual.MetaData.Device.PreferredTab = "Plate Handling";
            actual.MetaData.Device.Name = "PlateLoc";
            actual.MetaData.Device.Description = "Velocity11 PlateLoc Sealer";

            // Add Parameter
            var param = new VWorksXml.Parameter
            {
                Name = "Profile",
                Type = 2,
                Style = 0
            };
            actual.MetaData.Device.Parameters = new VWorksXml.Parameter[] { param };

            // Add Location
            var loc = new VWorksXml.Location
            {
                Name = "Stage",
                Type = 1,
                Offset = 0,
                Group = 0
            };
            actual.MetaData.Device.Locations = new VWorksXml.Location[] { loc };

            // Add Storage Dimensions
            actual.MetaData.Device.StorageDimensions = new VWorksXml.StorageDimensionsElement();
            actual.MetaData.Device.StorageDimensions.DirectStorageAccess = 0;
            #endregion

            #region Version
            // Add Version Element
            var ver = new VWorksXml.Version();
            ver.Name = "PlateLoc";
            ver.VersionAttribute = "3.0.0";
            ver.Date = "April 3, 2006";
            ver.Company = "ABC Company";
            ver.Author = "Joe Smith";
            actual.MetaData.Versions = new VWorksXml.Version[] { ver };
            #endregion

            #region Commands
            var command = new VWorksXml.CommandElement();
            command.Name = "Seal";
            command.Description = "Seal a plate";
            command.Editor = 2;
            command.Compiler = 0;

            var sealTime = new VWorksXml.Parameter();
            sealTime.Name = "Seal Time";
            sealTime.Type = 12;
            sealTime.Style = 0;
            sealTime.Value = "1.2";
            sealTime.Units = "s";
            sealTime.Ranges = new VWorksXml.Range[]
            {
                new VWorksXml.Range() { Value = "0.5" },
                new VWorksXml.Range() { Value = "12" }
            };

            var sealTemperature = new VWorksXml.Parameter();
            sealTemperature.Name = "Seal Temperature";
            sealTemperature.Type = 8;
            sealTemperature.Style = 0;
            sealTemperature.Value = "170";
            sealTemperature.Units = "C";
            sealTemperature.Ranges = new VWorksXml.Range[]
            {
                new VWorksXml.Range() { Value = "20" },
                new VWorksXml.Range() { Value = "235" }
            };

            command.Parameters = new VWorksXml.Parameter[]
            {
                sealTime,
                sealTemperature
            };

            actual.MetaData.Commands = new VWorksXml.CommandElement[] { command };
            #endregion
            
            // Load Test XML File
            string dir = @"..\..\..\TestData\GetMetaData.xml";

            VWorksXml.Velocity11 expectedMetaData;
            XmlSerializer readData = new XmlSerializer(typeof(VWorksXml.Velocity11));
            FileStream stream = new FileStream(dir, FileMode.Open);
            expectedMetaData = (VWorksXml.Velocity11)readData.Deserialize(stream);

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(expectedMetaData.SerializeObject());
            string fPath = "C:/Users/tlee.CALICOLABS/Desktop/";
            string fname = "expectedMetaData.xml";
            xdoc.Save(fPath + fname);

            xdoc = new XmlDocument();
            xdoc.LoadXml(actual.SerializeObject());
            xdoc.Save(fPath + "actualMetaData.xml");

            Assert.AreEqual(expectedMetaData.SerializeObject(), actual.SerializeObject());
        }

        [Test]
        public void StringCommandIsDeserializedCorrectly()
        {
            VWorksXml.Velocity11 actual = new VWorksXml.Velocity11();
            

            actual.Version = "1.0";
            actual.File = "MetaData";
            actual.md5sum = "9e0e9dcbdbb460a7d444cba2a2d0a474";

            var command = new VWorksXml.CommandElement();
            command.Compiler = 0;
            command.Description = "Execute a method";
            command.Editor = 2;
            command.Name = "Execute method";
            command.NextTaskToExecute = 1;
            command.ProtocolName = "Protocol File - 1";
            command.RequiresRefresh = 0;
            command.TaskRequiresLocation = 1;
            command.VisibleAvailability = 1;

            VWorksXml.Parameter param = new VWorksXml.Parameter()
            {
                Description = "Method name",
                Name = "Method name",
                Scriptable = 1,
                Style = 0,
                Type = 2,
                Value = "a.lmeth"
            };

            VWorksXml.Range range = new VWorksXml.Range();
            range.Value = "a.lmeth";
            param.Ranges = new VWorksXml.Range[1] { range };
            command.Parameters = new VWorksXml.Parameter[1] { param };

            VWorksXml.Value v = new VWorksXml.Value();
            v.LocationValue = "Location";
            command.Locations = new VWorksXml.Value[1] { v };

            actual.Command = command;

            // Load Test XML File
            string dir = @"..\..\..\TestData\Command.xml";
            VWorksXml.Velocity11 expectedMetaData;
            XmlSerializer readData = new XmlSerializer(typeof(VWorksXml.Velocity11));
            FileStream stream = new FileStream(dir, FileMode.Open);
            expectedMetaData = (VWorksXml.Velocity11)readData.Deserialize(stream);

            var test = new VWorksXml.Velocity11
            {
                MetaData = new VWorksXml.MetaDataElement
                {
                    Commands = new VWorksXml.CommandElement[]
                    {
                        command
                    }
                }
            };

            var test_Str = test.SerializeObject();

            Assert.AreEqual(expectedMetaData.md5sum, actual.md5sum);
            Assert.AreEqual(expectedMetaData.File, actual.File);
            Assert.AreEqual(expectedMetaData.Version, actual.Version);
            Assert.AreEqual(expectedMetaData.MetaData, actual.MetaData);
            Assert.AreEqual(expectedMetaData.SerializeObject(), actual.SerializeObject());
        }
    }
}