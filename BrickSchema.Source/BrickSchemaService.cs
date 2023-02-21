using VDS.RDF.Ontology;
using VDS.RDF.Parsing;
using VDS.RDF;

namespace BrickSchema.Source
{
    public class BrickSchemaService
    {
        private readonly OntologyGraph _brickSchema;

        protected BrickSchemaService()
        {
            _brickSchema = new OntologyGraph();
        }

        private void InitializeBrickGraph()
        {
            UriLoader.Load(_brickSchema, new Uri(@"https://brickschema.org/schema/1.2/Brick.ttl"));
        }

        public IEnumerable<OntologyClass> GetEquipmentClasses()
        {
            return _brickSchema.AllClasses;
        }

        public IEnumerable<IUriNode> GetUriNodes()
        {
            return _brickSchema.Nodes.UriNodes();
        }

        public static BrickSchemaService Create()
        {
            var result = new BrickSchemaService();
            result.InitializeBrickGraph();

            return result;
        }
    }
}
