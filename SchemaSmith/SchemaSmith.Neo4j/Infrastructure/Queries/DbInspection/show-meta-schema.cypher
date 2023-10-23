CALL apoc.meta.schema() yield value;
//UNWIND apoc.map.sortedProperties(value) as resourceData
//WITH resourceData[0] as resource, resourceData[1] as data
//UNWIND apoc.map.sortedProperties(data.properties) as property
//WITH resource, data, property[0] as property, property[1] as propData
//RETURN 
//resource, 
//data.type as resourceType,
//property,
//propData.type as propertyType,
//propData.indexed as isIndexed,
//propData.unique as uniqueConstraint,
//propData.existence as existenceConstraint;