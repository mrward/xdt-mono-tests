using System;
using NUnit.Framework;
using System.Xml;

namespace XdtMono.Tests
{
	[TestFixture]
	public class XmlAttributeTests
	{
		[Test]
		public void SetPrefixToNull ()
		{
			var doc = new XmlDocument ();
			doc.LoadXml ("<root attr='' />");
			XmlAttribute attribute = doc.DocumentElement.Attributes [0];

			Assert.DoesNotThrow (() => attribute.Prefix = null);

			Assert.AreEqual (String.Empty, attribute.Prefix);
		}
	}
}

