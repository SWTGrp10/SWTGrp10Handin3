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

        [SetUp]
        public void Setup()
        {
            _output = new Output();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _light = new Light(_output);
        }

        [Test]
        public void Test_TurnOn_CorrectOutput()
        {
            _light.TurnOn();
        }


    }
}