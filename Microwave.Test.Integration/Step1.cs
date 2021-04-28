using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class Step1
    {
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ILight _light;
        private StringWriter _stringWriter;

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
        }

        [Test]
        public void TestLight_TurnOn_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _light.TurnOn();

            Assert.That(_stringWriter.ToString().Contains("on") && _stringWriter.ToString().Contains("Light"));
        }

        [Test]
        public void TestLight_TurnOff_CorrectOutput()
        {
            _light.TurnOn();
            Console.SetOut(_stringWriter);
            _light.TurnOff();

            Assert.That(_stringWriter.ToString().Contains("off") && _stringWriter.ToString().Contains("Light"));
        }

        [TestCase(20,15)]
        [TestCase(1,45)]
        public void TestDisplay_ShowTime_CorrectOutput(int min, int sec)
        {
            Console.SetOut(_stringWriter);
            _display.ShowTime(min, sec);

            Assert.That(_stringWriter.ToString().Contains(min.ToString()) && _stringWriter.ToString().Contains(sec.ToString()));
        }

        [TestCase(20)]
        [TestCase(1)]
        public void TestDisplay_ShowPower_CorrectOutput(int power)
        {
            Console.SetOut(_stringWriter);
            _display.ShowPower(power);

            Assert.That(_stringWriter.ToString().Contains(power.ToString()) && _stringWriter.ToString().Contains("W"));
        }

        [Test]
        public void TestDisplay_Clear_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _display.Clear();

            Assert.That(_stringWriter.ToString().Contains("cleared"));
        }
    }
}