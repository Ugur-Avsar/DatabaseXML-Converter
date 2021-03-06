// <copyright file="DatabaseConnectionTest.cs">Copyright ©  2016</copyright>
using System;
using System.Collections.Generic;
using System.Data;
using DatabaseXMLConverter;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;

namespace DatabaseXMLConverter.Tests
{
    [TestClass]
    public class DatabaseConnectionTest
    {
        [TestMethod, ExpectedException(typeof(MySqlException))]
        public void ConnectionTest()
        {
            DatabaseConnection.Connect("localhost", "world", "root", "ich bin ein ungültiges Passwort");
        }

        [TestMethod]
        public void ConnectionTest2()
        {
            DatabaseConnection.Connect("localhost", "world", "root", "Ugur1995");
        }
    }
}
