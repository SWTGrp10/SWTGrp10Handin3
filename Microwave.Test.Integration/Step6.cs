using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    class Step6
    {
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ILight _light;
        private StringWriter _stringWriter;
        private ITimer _timer;
        private CookController _cookController;
        private IUserInterface _sut;
        private IButton _buttonPower;
        private IButton _timeButton;
        private IButton _startButton;
        private IDoor _door;

        [SetUp]
        public void Setup()
        {
            _buttonPower = new Button();
            _timeButton = new Button();
            _startButton = new Button();
            _door = new Door();
            _timer = Substitute.For<ITimer>();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _cookController = new CookController(_timer, _display, _powerTube);
            _sut = new UserInterface(_buttonPower, _timeButton, _startButton, _door, _display, _light, _cookController);
        }

        [Test]
        public void TestUserInterface_DoorOpen_OnDoorOpen()
        {
            Console.SetOut(_stringWriter);
            // Also checks if TimeButton is subscribed
            _door.Open();
            // Now in SetPower


            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("on"));
        }

        [Test]
        public void TestUserInterface_DoorClosed_OnDoorClosed()
        {
            Console.SetOut(_stringWriter);
            // Also checks if TimeButton is subscribed
            _door.Open();
            _door.Close();
            // Now in SetPower


            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("off"));
        }

    }
}
