using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiOrleans.Tests.Message
{
    [TestClass]
    public class DeserializerShould
    {
        [TestMethod]
        public void SuccessfullyDeserializeAHeartbeat()
        {
            string source = "{\"cmd\":\"heartbeat\",\"model\":\"gateway\",\"sid\":\"7811dcb06972\",\"short_id\":\"0\",\"token\":\"vYgMNFGfJWk0M22U\",\"data\":\"{\\\"ip\\\":\\\"192.168.2.12\\\"}\"}";
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(new Common.Transmission(source, "127.0.0.1"));

            Assert.IsInstanceOfType(inbound, typeof(Common.Message.Inbound.Heartbeat));

            Common.Message.Inbound.Heartbeat heartbeat = (Common.Message.Inbound.Heartbeat)inbound;

            Assert.AreEqual(heartbeat.Sid, "7811dcb06972");
            Assert.AreEqual(heartbeat.Token, "vYgMNFGfJWk0M22U");
        }

        [TestMethod]
        public void SuccessfullyDeserializeAnIdList()
        {
            string source = "{\"cmd\":\"get_id_list_ack\",\"sid\":\"7811dcb06972\",\"token\":\"L0DI4IiFAvAgInyL\",\"data\":\"[\\\"158d0001a200f5\\\",\\\"158d0001c1cdfb\\\"]\"}";
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(new Common.Transmission(source, "127.0.0.1"));

            Assert.IsInstanceOfType(inbound, typeof(Common.Message.Inbound.IdList));

            Common.Message.Inbound.IdList idList = (Common.Message.Inbound.IdList)inbound;

            Assert.AreEqual(idList.Sid, "7811dcb06972");
            Assert.AreEqual(idList.Token, "L0DI4IiFAvAgInyL");

            CollectionAssert.AreEqual(idList.SubDevices.ToArray(), new [] { "158d0001a200f5", "158d0001c1cdfb" });
        }

        [TestMethod]
        public void SuccessfullyDeserializeAGatewayReading()
        {
            string source = "{\"cmd\":\"read_ack\",\"model\":\"gateway\",\"sid\":\"7811dcb06972\",\"short_id\":0,\"data\":\"{\\\"rgb\\\":0,\\\"illumination\\\":1292,\\\"proto_version\\\":\\\"1.0.9\\\"}\"}";
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(new Common.Transmission(source, "127.0.0.1"));

            Assert.IsInstanceOfType(inbound, typeof(Common.Message.Inbound.Gateway.Reading));

            Common.Message.Inbound.Gateway.Reading reading = (Common.Message.Inbound.Gateway.Reading)inbound;

            Assert.AreEqual(reading.Sid, "7811dcb06972");
            Assert.AreEqual(reading.Rgb, "0");
            Assert.AreEqual(reading.Illumination, 1292);
            Assert.AreEqual(reading.Version, "1.0.9");
        }

        [TestMethod]
        public void SuccessfullyDeserializeADoorSensorReading()
        {
            string source = "{\"cmd\":\"read_ack\",\"model\":\"magnet\",\"sid\":\"158d0001c1cdfb\",\"short_id\":56258,\"data\":\"{\\\"voltage\\\":3115,\\\"status\\\":\\\"open\\\"}\"}";
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(new Common.Transmission(source, "127.0.0.1"));

            Assert.IsInstanceOfType(inbound, typeof(Common.Message.Inbound.DoorSensor.Reading));

            Common.Message.Inbound.DoorSensor.Reading reading = (Common.Message.Inbound.DoorSensor.Reading)inbound;

            Assert.AreEqual(reading.Sid, "158d0001c1cdfb");
            Assert.AreEqual(reading.Voltage, 3115);
            Assert.AreEqual(reading.Status, Common.Message.Inbound.DoorSensor.Status.Open);
        }

        [TestMethod]
        public void SuccessfullyDeserializeADoorSensorReport()
        {
            string source = "{\"cmd\":\"report\",\"model\":\"magnet\",\"sid\":\"158d0001c1cdfb\",\"short_id\":56258,\"data\":\"{\\\"status\\\":\\\"open\\\"}\"}";
            Common.Message.IInbound inbound = Common.Message.Deserializer.Default.Deserialize(new Common.Transmission(source, "127.0.0.1"));

            Assert.IsInstanceOfType(inbound, typeof(Common.Message.Inbound.DoorSensor.Report));

            Common.Message.Inbound.DoorSensor.Report report = (Common.Message.Inbound.DoorSensor.Report)inbound;

            Assert.AreEqual(report.Sid, "158d0001c1cdfb");
            Assert.AreEqual(report.Status, Common.Message.Inbound.DoorSensor.Status.Open);
        }
    }
}
