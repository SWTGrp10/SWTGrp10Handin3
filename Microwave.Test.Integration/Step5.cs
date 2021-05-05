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
    class Step5
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
            _door = Substitute.For<IDoor>();
            _timer = new Timer();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _cookController = new CookController(_timer, _display, _powerTube);
            _sut = new UserInterface(_buttonPower, _timeButton, _startButton, _door, _display, _light, _cookController);
        }
    }
}
