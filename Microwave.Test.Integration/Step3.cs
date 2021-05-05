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
        private StringWriter _stringWriter;
        private ITimer _timer;
        private CookController _sut;
        private IUserInterface _UI;

        [SetUp]
        public void Setup()
        {
            _timer = new Timer();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _stringWriter = new StringWriter();
            _UI = Substitute.For<IUserInterface>();
            _sut = new CookController(_timer, _display, _powerTube, _UI);

        }


        [Test]
        public void TestCookController__Display_OnTimerTick_CorrectTime()
        {
            Console.SetOut(_stringWriter);

            _sut.StartCooking(50, 100);

            Thread.Sleep(1100);

            Assert.That(_stringWriter.ToString().Contains("Display shows:") && _stringWriter.ToString().Contains("01:39"));
        }


        [Test]
        public void TestCookController__UserInterface_OnTimerTick_CallsUICookingIsDone()
        {
            _sut.StartCooking(50, 2);

            Thread.Sleep(2100);

            _UI.Received().CookingIsDone();
        }
    }
}
