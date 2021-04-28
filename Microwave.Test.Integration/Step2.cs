using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;


namespace Microwave.Test.Integration
{
    class Step2
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
            _timer = Substitute.For<ITimer>();
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
            _stringWriter = new StringWriter();
            _sut = new CookController(_timer,_display,_powerTube);
        }

        [Test]
        public void TestCookController_StartCooking_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _sut.StartCooking(50,1);

            Assert.That(_stringWriter.ToString().Contains("PowerTube works") && _stringWriter.ToString().Contains("50"));

        }

        [Test]
        public void TestCookController_Stop_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _sut.StartCooking(50,1);
            _sut.Stop();

            Assert.That(_stringWriter.ToString().Contains("PowerTube turned off"));

        }

        [Test]
        public void TestCookController_OnTimerTick_CorrectOutput()
        {
            Console.SetOut(_stringWriter);
            _sut.StartCooking(50, 10000);
            Thread.Sleep(3000);

            //Skal vi lave en form for eventargs klasse, fordi det lavede vi sidst, men den er ikke her.
            //_timer.TimerTick += Raise.EventWith(new ElapsedEventArgs());
            //_charger.CurrentValueEvent += _uut.Charging;

            Assert.That(_stringWriter.ToString().Contains("Display shows:"));

        }

    }
}
