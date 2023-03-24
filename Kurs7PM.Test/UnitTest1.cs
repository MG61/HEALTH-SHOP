using System;
using System.Net;
using System.Reflection;
using Kurs7PM.API.Models;


namespace Kurs7PM.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestValidationPassword()
        {
            TestVaid testVaid = new TestVaid();

            string Password = "Ironman15#";

            Assert.That(testVaid.ValidatePassword(Password, out Password), Is.EqualTo(true));
        }
    }
}