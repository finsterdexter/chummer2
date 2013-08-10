using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chummer;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Chummer2Tests
{
    [TestClass]
    public class ManagerTests
    {
        #region XmlManager Tests
        /// <summary>
        /// Verify that the XmlManager Instance loads correctly.
        /// </summary>
        [TestMethod]
        public void XmlManagerInstanceTest()
        {
            Assert.IsInstanceOfType(XmlManager.Instance, typeof(XmlManager), "XmlManager Instance was not loaded correctly.");
        }

        /// <summary>
        /// Verify that the XmlManager is loading data files correctly by comparing the number of armor nodes in the source file and in the one
        /// compiled by the XmlManager.
        /// </summary>
        [TestMethod]
        public void XmlManagerLoadTest()
        {
            XmlDocument objXmlDocument = XmlManager.Instance.Load("armor.xml");
            XmlDocument objXmlExpected = new XmlDocument();
            objXmlExpected.Load("data/armor.xml");
            Assert.AreEqual(objXmlExpected.SelectNodes("/chummer/armors/armor").Count, objXmlDocument.SelectNodes("/chummer/armors/armor").Count, "armor.xml did not load correctly from the XmlManager.");
        }
        #endregion

        #region LanguageManager Tests
        /// <summary>
        /// Verify that the LanguageManager Instance loads correctly.
        /// </summary>
        [TestMethod]
        public void LanguageManagerInstanceTest()
        {
            Assert.IsInstanceOfType(LanguageManager.Instance, typeof(LanguageManager), "LanguageManager Instance was not loaded correctly.");
        }

        /// <summary>
        /// Verify that each string in the language file was correctly loaded by the LanguageManager by comparing the XML file contents with the value from
        /// the LanguageManager.
        /// </summary>
        [TestMethod]
        public void LanguageManagerLoadTest()
        {
            XmlDocument objXmlDocument = new XmlDocument();
            objXmlDocument.Load("D:\\source\\Chummer2\\Chummer2\\bin\\Debug\\lang\\en-us.xml");

            foreach (XmlNode objNode in objXmlDocument.SelectNodes("/chummer/strings/string"))
                Assert.AreEqual(objNode["text"].InnerText.Replace("\\n", "\n"), LanguageManager.Instance.GetString(objNode["key"].InnerText), "Key " + objNode["key"].InnerText + " not loaded correctly by the LanguageManager.");
        }
        #endregion
    }
}