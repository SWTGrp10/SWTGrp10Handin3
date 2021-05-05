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
    class Step4
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
            _buttonPower = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _timer = Substitute.For<ITimer>();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _cookController = new CookController(_timer, _display, _powerTube);
            _sut = new UserInterface(_buttonPower,_timeButton,_startButton, _door,_display,_light,_cookController);
        }

        [Test]
        public void TestUserInterface_LightOn_CorrectOutput()
        {
            Console.SetOut(_stringWriter);

            // Also checks if TimeButton is subscribed
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("on"));
        }

        [Test]
        public void TestUserInterface_LightOff_CorrectOutput()
        {
            Console.SetOut(_stringWriter);

            // Also checks if TimeButton is subscribed
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("off"));
        }

        [Test]
        public void TestUserInterface_CookController_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            _startButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("PowerTube works") && _stringWriter.ToString().Contains("50"));
        }

        [Test]
        public void TestUserInterface_DisplayShowPower_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            

            Assert.That(_stringWriter.ToString().Contains("Display shows") && _stringWriter.ToString().Contains("W"));
        }

        [Test]
        public void TestUserInterface_DisplayShowTime_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("Display shows") && _stringWriter.ToString().Contains(":"));
        }

        [Test]
        public void TestUserInterface_LightOnWhenStartIsPressed_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Now in SetPower
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            _startButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("on"));
        }

        [Test]
        public void TestUserInterface_StartCooking_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Now in SetPower
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            _startButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            Assert.That(_stringWriter.ToString().Contains("PowerTube works with"));
        }

        [Test]
        public void TestUserInterface_DisplayClear_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _door.Opened += Raise.EventWith(this, EventArgs.Empty);
            _door.Closed += Raise.EventWith(this, EventArgs.Empty);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Now in SetPower
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            _startButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _sut.CookingIsDone();

            Assert.That(_stringWriter.ToString().Contains("Display cleared"));
        }

        [Test]
        public void TestUserInterface_CookingIsDone_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _buttonPower.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            _startButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking
            _sut.CookingIsDone();

            Assert.That(_stringWriter.ToString().Contains("Light") && _stringWriter.ToString().Contains("off"));
        }

    }
}
