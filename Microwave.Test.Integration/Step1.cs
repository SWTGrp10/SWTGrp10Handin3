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


        [TestCase(1)]
        [TestCase(99)]
        public void TestPowertube_TurnOn_InputBetween1And100CorrectOutput(int input)
        {
            Console.SetOut(_stringWriter);
            _powerTube.TurnOn(input);

            Assert.That(_stringWriter.ToString().Contains("PowerTube works with " + input));
        }

        [TestCase(0)]
        [TestCase(101)]
        public void TestPowertube_TurnOn_InputUnder1AndOver100CorrectOutput(int input)
        {
            Console.SetOut(_stringWriter);

            //Tester at korrekt exception kastes og at den korrekte tekst til exception skrives
            ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(() => _powerTube.TurnOn(input));
            Assert.That(ex.Message.Contains("Must be between 1 and 100 (incl.)"));
        }

        [TestCase(2)]
        [TestCase(99)]
        public void TestPowertube_TurnOn_IsOnIsTrue_CorrectOutput(int input)
        {
            _powerTube.TurnOn(input);
            

            //Tester at korrekt exception kastes og at den korrekte tekst til exception skrives
            ApplicationException ex = Assert.Throws<ApplicationException>(() => _powerTube.TurnOn(input));
            Assert.That(ex.Message.Contains("PowerTube.TurnOn: is already on"));
        }

        [Test]
        public void TestPowertube_TurnOff_IsOnIsTrue_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _powerTube.TurnOn(50);

            _powerTube.TurnOff();
            Assert.That(_stringWriter.ToString().Contains("PowerTube turned off"));
        }

    }
}