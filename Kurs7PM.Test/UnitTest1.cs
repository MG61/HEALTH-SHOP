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
            ClassHash classHash = new ClassHash();

            string Password = "Vladsapsan228";

            Assert.That(classHash.ValidatePassword(Password, out Password), Is.EqualTo(true));
        }
    }
}