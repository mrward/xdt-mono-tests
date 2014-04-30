using System;
using System.Xml;
using NUnit.Framework;
using Microsoft.Web.XmlTransform;
using System.IO;
using System.Text;

namespace XdtMono.Tests
{
	[TestFixture]
	public class RemoveTransformTests
	{
		[Test]
		public void TransformShouldRemoveExistingXmlElements ()
		{
			string input =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
 <runtime>
  <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
    <dependentAssembly>
      <assemblyIdentity name=""System.Web.WebPages"" publicKeyToken=""31bf3856ad364e35""/>
      <bindingRedirect oldVersion=""0.0.0.0-0.0.0.0"" newVersion=""0.0.0.0""/>
    </dependentAssembly>
  </assemblyBinding>
 </runtime>
</configuration>
";

			string xdt = 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
 <runtime>
    <assemblyBinding xmlns=""urn:schemas-microsoft-com:asm.v1"">
      <dependentAssembly xdt:Transform=""Remove""
          xdt:Locator=""Condition(./_defaultNamespace:assemblyIdentity/@name='System.Web.WebPages')"" >
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>";

			string output = RunTransform (input, xdt);

			StringAssert.DoesNotContain ("WebPages", output);
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

