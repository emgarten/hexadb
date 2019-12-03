using Hexastore.Graph;
using Hexastore.Parser;
using Hexastore.TestCommon;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using RocksDbSharp;
using Xunit;
using Xunit.Abstractions;

namespace Hexastore.Rocks.Test
{
    public class ExportTest
    {
        private readonly ITestOutputHelper _output;

        public ExportTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BasicExport()
        {

            using (var testFolder = new TestFolder())
            {
                var logger = Mock.Of<ILogger<RocksGraphProvider>>();
                var provider = new RocksGraphProvider(logger, testFolder);
                var db = provider.CreateGraph("test", Store.GraphType.Data);
                var json = new JObject(new JProperty("id", "root"), new JProperty("data", new JObject(new JProperty("a", "b"))));
                var graph = TripleConverter.FromJson(json);
                db.Assert(graph);

                foreach (var triple in db.GetTriples())
                {
                    _output.WriteLine($"{triple.Subject}|{triple.Object}");
                }
            }
        }
    }
}
