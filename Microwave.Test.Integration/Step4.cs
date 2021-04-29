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
    class Step4
    {
        private IOutput _output;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ILight _light;
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
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _sut = new CookController(_timer, _display, _powerTube);
            _UI = Substitute.For<IUserInterface>();
        }
    }
}
