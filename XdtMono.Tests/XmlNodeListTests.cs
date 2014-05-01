using System;
using System.Xml;
using NUnit.Framework;

namespace XdtMono.Tests
{
	[TestFixture]
	public class XmlNodeListTests
	{
		[Test]
		public void RemoveNodeFromRootShouldNotChangeXmlNodeList ()
		{
			string xml = 
@"<root>
	<child id='1'/>
	<child id='2'/>
	<child id='3'/>
</root>";
			var doc = new XmlDocument ();
			doc.LoadXml (xml);
			XmlNodeList children = doc.SelectNodes ("//child");
			XmlNode firstChild = children [0];
			int childrenCountBeforeRemove = children.Count;

			doc.DocumentElement.RemoveChild (firstChild);

			Assert.AreEqual (3, children.Count);
		}
	}
}

