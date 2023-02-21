// See https://aka.ms/new-console-template for more information
//https://github.com/BrickSchema/Brick/releases/download/nightly/Brick.ttl

using BrickSchema.Source;
using VDS.RDF.Ontology;

var service = BrickSchemaService.Create();

//foreach (var node in service.GetEquipmentClasses())
//{
//    Console.WriteLine(node.Resource);
//}


var equipment = service.GetUriNodes()
    .First(x => x.Uri.AbsoluteUri == @"https://brickschema.org/schema/Brick#Equipment");


//var equipment = service.GetEquipmentClasses()
//    .First(x => x.Resource.GraphUri.AbsoluteUri == @"https://brickschema.org/schema/Brick#Equipment");

Console.WriteLine(equipment.NodeType);

//foreach (var subClass in equipment.).SubClasses)
//{
//    Console.WriteLine(subClass);
//}


