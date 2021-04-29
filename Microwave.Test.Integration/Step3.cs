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

        [Test]
        public void TestCookController_Display_OnTimerTick_CorrectOutput()
        {
            int ticks = 1;
            
            Console.SetOut(_stringWriter);

            ManualResetEvent pause = new ManualResetEvent(false);

            int ticksGone = 0;

            _timer.TimerTick += (sender, args) =>
            {
                ticksGone++;
                if (ticksGone >= ticks)
                    pause.Set();
            };
            _sut.StartCooking(50, 5000);

            // wait for ticks, only a little longer
            pause.WaitOne(ticks * 1000 + 100);

            //Assert.That(uut.TimeRemaining, Is.EqualTo(5000 - ticks * 1000));
            Assert.That(_stringWriter.ToString().Contains("Display shows:") && _stringWriter.ToString().Contains(Convert.ToString((5000-ticks*1000)/60)));
        }

        [Test]
        public void TestCookController_Moresimple_Display_OnTimerTick_CorrectOutput()
        {
            Console.SetOut(_stringWriter);

            _sut.StartCooking(50, 5000);

            Thread.Sleep(1100);

            Assert.That(_stringWriter.ToString().Contains("Display shows:") && _stringWriter.ToString().Contains("116"));
        }

        [Test]
        public void TestTimer_OnTimerTick_CorrectOutput()
        {
            _sut.StartCooking(50, 1);
            Thread.Sleep(1001);

            Assert.That(_stringWriter.ToString().Contains("Display shows:") && _stringWriter.ToString().Contains("66:40"));
        }
    }
}
