using System;
using System.Xml;
using NUnit.Framework;
using Microsoft.Web.XmlTransform;
using System.IO;
using System.Text;

namespace XdtMono.Tests
{
	[TestFixture]
	public class SetAttributesTransformTests
	{
		[Test]
		public void TransformShouldSetAttribute ()
		{
			string input =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
	<connectionStrings>
		<!-- Example connection to a SQL Server Database on localhost. -->
		<add name=""MyDB""
		     connectionString=""""
		     providerName=""System.Data.SqlClient""/>
	</connectionStrings>
	<appSettings>
		<add key=""Setting1"" value=""Very""/>
		<add key=""Setting2"" value=""Easy""/>
 	</appSettings>
</configuration>
";

			string xdt = 
@"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
	<connectionStrings>
		<add name=""MyDB""
			connectionString=""value for the deployed Web.config file""
			xdt:Transform=""SetAttributes"" xdt:Locator=""Match(name)""/>
		<add name=""AWLT"" connectionString=""newstring""
			providerName=""newprovider""
			xdt:Transform=""Insert"" />
	</connectionStrings>
</configuration>";

			string output = RunTransform (input, xdt);

			StringAssert.Contains ("deployed Web.config file", output);
		}

		string RunTransform (string input, string xdt)
		{
			using (var transformation = new XmlTransformation (xdt, isTransformAFile: false, logger: null)) {
				using (var document = new XmlTransformableDocument ()) {
					document.PreserveWhitespace = true;

					document.Load (new StringReader (input));

					bool succeeded = transformation.Apply(document);
					if (succeeded) {
						var writer = new StringWriter ();
						document.Save (writer);
						return writer.ToString ();
					}
					return null;
				}
			}
		}
	}
}

