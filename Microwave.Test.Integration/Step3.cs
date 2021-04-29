using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    class Step3
    {
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ILight _light;
        private StringWriter _stringWriter;
        private ITimer _timer;
        private CookController _sut;

        [SetUp]
        public void Setup()
        {
            _timer = new Timer();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _sut = new CookController(_timer, _display, _powerTube);
        }

        [Test]
        public void TestCookController_Display_OnTimerTick_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _sut.StartCooking(50, 10000);
            Thread.Sleep(3000);

            //_timer.TimeRemaining.Returns(115);

            Assert.That(_stringWriter.ToString().Contains("Display shows:") );
            //&& _stringWriter.ToString().Contains("1:55")
        }
    }
}
